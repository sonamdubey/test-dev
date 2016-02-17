using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Bikewale.Common;
using Bikewale.Controls;
using Bikewale.Memcache;
using System.Net.Http;
using System.Configuration;
using System.Net.Http.Headers;
using Bikewale.Interfaces.Pager;
using Bikewale.Entities.CMS;
using Bikewale.Entities.CMS.Articles;
using Microsoft.Practices.Unity;
using Bikewale.BAL.Pager;
using Bikewale.Entities.Pager;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Cache.Core;
using Bikewale.Cache.BikeData;
using Bikewale.DAL.BikeData;

namespace Bikewale.Content
{
    //Modified By : Ashwini Todkar on 29 Sept 2014
    //Modified code to retrieve roadtest from api

    public class DefaultRT : System.Web.UI.Page
    {
        protected Repeater rptRoadTest;
        protected MakeModelSearch MakeModelSearch;
        protected HtmlGenericControl alertObj;
        protected LinkPagerControl linkPager;

        protected string nextUrl = string.Empty ,prevUrl = string.Empty;

        private const int _pageSize = 10;
        private int _pageNo = 1;
        private const int _pagerSlotSize = 5;
        private bool _isContentFound = true;

        protected override void OnInit(EventArgs e)
        {         
            base.Load += new EventHandler(Page_Load);
        }

        private void Page_Load(object sender, EventArgs e)
        {
            //code for device detection added by Ashwini Todkar
            // Modified By :Ashish Kamble on 5 Feb 2016
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];

            DeviceDetection dd = new DeviceDetection(originalUrl);
            dd.DetectDevice();


            CommonOpn op = new CommonOpn();
            Trace.Warn("current url :" + HttpContext.Current.Request.Url);
            if (!IsPostBack)
            {
                string _makeName = string.Empty , _makeId = string.Empty , _modelId = string.Empty , _modelName = string.Empty;

                ProcessQS(out _makeName, out _makeId, out _modelId, out _modelName);
                GetRoadTestList(_makeId, _modelId, _makeName , _modelName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_makeName"></param>
        /// <param name="_makeId"></param>
        /// <param name="_modelId"></param>
        /// <param name="_modelName"></param>
        private void ProcessQS(out string _makeName, out string _makeId, out string _modelId, out string _modelName)
        {
            if (Request["pn"] != null && Request.QueryString["pn"] != "")
            {
                string _pageNumber = Request.QueryString["pn"];
                if (CommonOpn.CheckId(_pageNumber) == true)
                    _pageNo = Convert.ToInt32(_pageNumber);
            }

            _makeName = string.Empty;
            _makeId = string.Empty;
            _modelId = string.Empty;
            _modelName = string.Empty;


            _makeName = Request.QueryString["make"];

            if (!String.IsNullOrEmpty(_makeName))
                _makeId = MakeMapping.GetMakeId(_makeName.ToLower());

            if (!String.IsNullOrEmpty(_makeId))
            {
                Trace.Warn("model ;" + Request.QueryString["model"]);
                if (!String.IsNullOrEmpty(Request.QueryString["model"]))
                {
                    ModelMaskingResponse objResponse = null;

                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IBikeMaskingCacheRepository<BikeModelEntity, int>, BikeModelMaskingCache<BikeModelEntity, int>>()
                                 .RegisterType<ICacheManager, MemcacheManager>()
                                 .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>()
                                ;
                        var objCache = container.Resolve<IBikeMaskingCacheRepository<BikeModelEntity, int>>();

                        objResponse = objCache.GetModelMaskingResponse(Request.QueryString["model"]);

                        if (objResponse != null && objResponse.StatusCode == 200)
                        {
                            _modelId = objResponse.ModelId.ToString();
                            _modelName = Request.QueryString["model"];
                        }
                        else
                        {
                            if (objResponse.StatusCode == 301)
                            {
                                //redirect permanent to new page 
                                CommonOpn.RedirectPermanent(Request.RawUrl.Replace(Request.QueryString["model"], objResponse.MaskingName));
                            }
                            else
                            {
                                Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                                HttpContext.Current.ApplicationInstance.CompleteRequest();
                                this.Page.Visible = false;
                            }
                        }
                    }

                    //ModelMapping mm = new ModelMapping();
                    //_modelId = mm.GetModelId(Request.QueryString["model"]);
                    //_modelName = Request.QueryString["model"].ToString();
                    //Trace.Warn("Model Name : " + _modelName);
                }
            }
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 26 Sept 2014
        /// Summary    : method to get roadtest list and total roadtest count from carwale api
        /// </summary>
        private async void GetRoadTestList(string makeId, string modelId , string makeName, string modelName )
        {
            try
            {
                // get pager instance
                IPager objPager = GetPager();

                int _startIndex = 0, _endIndex = 0, _roadtestCategoryId = (int)EnumCMSContentType.RoadTest;

                objPager.GetStartEndIndex(_pageSize, _pageNo, out _startIndex, out _endIndex);

                string _apiUrl = string.Empty;

                if (String.IsNullOrEmpty(makeId))
                {
                    _apiUrl = "webapi/article/listbycategory/?applicationid=2&categoryidlist=" + _roadtestCategoryId + "&startindex=" + _startIndex + "&endindex=" + _endIndex;
                }
                else
                {
                    if (String.IsNullOrEmpty(modelId))
                    {
                        _apiUrl = "webapi/article/listbycategory/?applicationid=2&categoryidlist=" + _roadtestCategoryId + "&startindex=" + _startIndex + "&endindex=" + _endIndex + "&makeid=" + makeId;
                        MakeModelSearch.MakeId = makeId;
                    }
                    else
                    {
                        _apiUrl = "webapi/article/listbycategory/?applicationid=2&categoryidlist=" + _roadtestCategoryId + "&startindex=" + _startIndex + "&endindex=" + _endIndex + "&makeid=" + makeId + "&modelid=" + modelId;
                        MakeModelSearch.MakeId = makeId;
                        MakeModelSearch.ModelId = modelId;
                    }
                }

                CMSContent _objRoadTestList = null;

                using(Utility.BWHttpClient objClient = new Utility.BWHttpClient())
                {
                    _objRoadTestList = await objClient.GetApiResponse<CMSContent>(Utility.APIHost.CW, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, _objRoadTestList);
                }
                
                if (_objRoadTestList != null)
                {
                    if (_objRoadTestList.Articles.Count > 0)
                    {
                        BindRoadtest(_objRoadTestList);
                        BindLinkPager(objPager, Convert.ToInt32(_objRoadTestList.RecordCount), makeName, modelName);
                    }
                    else
                        _isContentFound = false;
                }
                else
                {
                    alertObj.Visible = true;
                    alertObj.InnerText = "Sorry! No road tests have been carried out for the selected bike.";
                }
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                if (!_isContentFound)
                {
                    Response.Redirect("/pagenotfound.aspx", true);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
        }

        //PopulateWhere to create Pager instance
        private IPager GetPager()
        {
            IPager _objPager = null;
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IPager, Pager>();
                _objPager = container.Resolve<IPager>();
            }
            return _objPager;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_objRoadtestList"></param>
        private void BindRoadtest(CMSContent _objRoadtestList)
        {
            rptRoadTest.DataSource = _objRoadtestList.Articles;
            rptRoadTest.DataBind();
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 24 Sept 2014
        /// PopulateWhere to bind link pager control 
        /// </summary>
        /// <param name="objPager"> Pager instance </param>
        /// <param name="recordCount"> total news available</param>
        private void BindLinkPager(IPager objPager, int recordCount , string makeName , string modelName)
        {
            PagerOutputEntity _pagerOutput = null;
            PagerEntity _pagerEntity = null;
            string _baseUrl = "/road-tests/";

            try
            {
                if (!String.IsNullOrEmpty(modelName))
                      _baseUrl = "/" + makeName + "-bikes/" + modelName + "/road-tests/";
                else if (!String.IsNullOrEmpty(makeName))
                    _baseUrl = "/" + makeName + "-bikes" + "/road-tests/";

                _pagerEntity = new PagerEntity();
                _pagerEntity.BaseUrl = _baseUrl;
                _pagerEntity.PageNo = _pageNo; //Current page number
                _pagerEntity.PagerSlotSize = _pagerSlotSize; // 5 links on a page
                _pagerEntity.PageUrlType = "page/";
                _pagerEntity.TotalResults = recordCount; //total News count
                _pagerEntity.PageSize = _pageSize;        //No. of news to be displayed on a page

                _pagerOutput = objPager.GetPager<PagerOutputEntity>(_pagerEntity);

                // for RepeaterPager
                linkPager.PagerOutput = _pagerOutput;
                linkPager.CurrentPageNo = _pageNo;
                linkPager.TotalPages = objPager.GetTotalPages(recordCount, _pageSize);
                linkPager.BindPagerList();

                //For SEO
                //CreatePrevNextUrl(linkPager.TotalPages,_baseUrl);
                prevUrl = String.IsNullOrEmpty(_pagerOutput.PreviousPageUrl) ? "" : "http://www.bikewale.com" + _pagerOutput.PreviousPageUrl;
                nextUrl = String.IsNullOrEmpty(_pagerOutput.NextPageUrl) ? "" : "http://www.bikewale.com" + _pagerOutput.NextPageUrl;       
            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        // Commented By : Ashwini Todkar on 25 Sept
        //Not more required 
        //private void CreatePrevNextUrl(int totalPages, string baseUrl)
        //{
        //    baseUrl =  " http://www.bikewale.com" +  baseUrl +  "page/";

        //    string prevPageNumber = string.Empty, nextPageNumber = string.Empty;

        //    if (_pageNo == 1)    //if page is first page
        //    {
        //        nextPageNumber = "2";
        //        nextUrl = baseUrl + nextPageNumber + "/";
        //    }
        //    else if (_pageNo == totalPages)    //if page is last page
        //    {
        //        prevPageNumber = (_pageNo - 1).ToString();
        //        prevUrl = baseUrl + prevPageNumber + "/";
        //    }
        //    else
        //    {          //for middle pages
        //        prevPageNumber = (_pageNo - 1).ToString();
        //        prevUrl = baseUrl + prevPageNumber + "/";
        //        nextPageNumber = (_pageNo + 1).ToString();
        //        nextUrl = baseUrl + nextPageNumber + "/";
        //    }
        //}

      


        //private void CreatePrevNextUrl()
        //{
        //    throw new NotImplementedException();
        //}

        //private void FillRoadTestDetails()
        //{
        //    throw new NotImplementedException();
        //}

        //private void FetchRoadTests(string makeId, string makeName, string modelId, string modelName)
        //{
        //    throw new NotImplementedException();
        //}

        //private void CreatePrevNextUrl()
        //{
        //    //Trace.Warn("indide create url");
        //    string mainUrl = "http://www.bikewale.com/road-tests/page/";
        //    string prevPageNumber = string.Empty, nextPageNumber = string.Empty;

        //    if (pageNumber == string.Empty || pageNumber == "1")    //if page is first page
        //    {
        //        nextPageNumber = "2";
        //        nextUrl = mainUrl + nextPageNumber + "/";
        //    }
        //    else if (int.Parse(pageNumber) == rpgRoadTest.totalPages)    //if page is last page
        //    {
        //        prevPageNumber = (int.Parse(pageNumber) - 1).ToString();
        //        prevUrl = mainUrl + prevPageNumber + "/";
        //    }
        //    else
        //    {          //for middle pages
        //        prevPageNumber = (int.Parse(pageNumber) - 1).ToString();
        //        prevUrl = mainUrl + prevPageNumber + "/";
        //        nextPageNumber = (int.Parse(pageNumber) + 1).ToString();
        //        nextUrl = mainUrl + nextPageNumber + "/";
        //    }
        //}

        //private void FetchRoadTests(string makeId, string makeName, string modelId, string modelName)
        //{
        //    Trace.Warn("Parameters  : " + makeId + makeName + modelId + modelName);
        //    Trace.Warn("Inside fetch function");
        //    SelectClause = " CB.Id AS BasicId, CB.AuthorName, CB.Description, CB.DisplayDate, CB.Views, CB.Title, CB.Url, CEI.IsMainImage, CEI.HostURL, CEI.ImagePathThumbnail, CEI.ImagePathLarge, Cmo.Name As ModelName, Cma.Name As MakeName, SC.Name As SubCategory ";
        //    FromClause = " Con_EditCms_Basic AS CB With(NoLock) Join Con_EditCms_Bikes CC With(NoLock) On CC.BasicId = CB.Id And CC.IsActive = 1 Join BikeModels Cmo With(NoLock) On Cmo.ID = CC.ModelId Join BikeMakes Cma With(NoLock) On Cma.ID = CC.MakeId Left Join Con_EditCms_Images CEI With(NoLock) On CEI.BasicId = CB.Id And CEI.IsMainImage = 1 And CEI.IsActive = 1 "
        //                 + " Left Join Con_EditCms_BasicSubCategories BSC With(NoLock) On BSC.BasicId = CB.Id Left Join Con_EditCms_SubCategories SC With(NoLock) On SC.Id = BSC.SubCategoryId And SC.IsActive = 1";
        //    WhereClause = " CB.CategoryId = @CategoryId AND CB.IsActive = 1 AND CB.IsPublished = 1 AND CC.MakeId = @MakeId";
        //    if (modelId != string.Empty)
        //    {
        //        WhereClause += " AND CC.ModelId = @ModelId";
        //    }
        //    OrderByClause = " DisplayDate Desc ";
        //    RecordCntQry = " Select Count(CB.Id) From " + FromClause + " Where " + WhereClause;
        //    BaseUrl = "/" + makeName + "-bikes/";
            
        //    //NavUrl = makeName + "-bikes";
                       
        //    //Trace.Warn("Model NAME : " + modelName);
        //    if (modelId != "")
        //    {
        //        BaseUrl += modelName + "/";
        //    }

        //    BaseUrl += "road-tests/";
        //    //NavUrl += modelName + "road-tests/";

        //    //Trace.Warn("model name " + modelName);
        //    //Trace.Warn("Nav Url" + NavUrl);
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.Parameters.Add("@CategoryId", SqlDbType.BigInt).Value = "8";// Default value for Articles on Road Tests
        //    cmd.Parameters.Add("@MakeId", SqlDbType.BigInt).Value = makeId;
        //    MakeModelSearch.MakeId = makeId;
        //    if (modelId != string.Empty)
        //    {
        //        cmd.Parameters.Add("@ModelId", SqlDbType.BigInt).Value = modelId;
        //        MakeModelSearch.ModelId = modelId;
        //    }
        //    BindData(cmd);
        //}

        //private void FillRoadTestDetails()
        //{
        //    SelectClause = " CB.Id AS BasicId, CB.AuthorName, CB.Description, CB.DisplayDate, CB.Views, CB.Title, CB.Url, CEI.IsMainImage, CEI.HostURL, CEI.ImagePathThumbnail, CEI.ImagePathLarge, Cmo.Name As ModelName, Cma.Name As MakeName, SC.Name As SubCategory ";
        //    FromClause = " Con_EditCms_Basic AS CB With(NoLock) Join Con_EditCms_Bikes CC With(NoLock) On CC.BasicId = CB.Id And CC.IsActive = 1 Join BikeModels Cmo With(NoLock) On Cmo.ID = CC.ModelId Join BikeMakes Cma With(NoLock) On Cma.ID = CC.MakeId Left Join Con_EditCms_Images CEI With(NoLock) On "
        //                 + " CEI.BasicId = CB.Id And CEI.IsMainImage = 1 And CEI.IsActive = 1 Left Join Con_EditCms_BasicSubCategories BSC With(NoLock) On BSC.BasicId = CB.Id Left Join Con_EditCms_SubCategories SC With(NoLock) On SC.Id = BSC.SubCategoryId And SC.IsActive = 1";
        //    WhereClause = " CB.CategoryId = @CategoryId AND CB.IsActive = 1 AND CB.IsPublished = 1";
        //    OrderByClause = " DisplayDate Desc ";
        //    RecordCntQry = " Select Count(CB.Id) From " + FromClause + " Where " + WhereClause;
        //    BaseUrl = "/road-tests/";
        //    Trace.Warn("Base Url"+BaseUrl);
        //    //Trace.Warn("Nav Url" + NavUrl);
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.Parameters.Add("@CategoryId", SqlDbType.BigInt).Value = "8";// Default value for Articles on Road Tests

        //    BindData(cmd);
        //}

        //void BindData(SqlCommand cmd)
        //{
        //    try
        //    {
        //        if (pageNumber != string.Empty)
        //            rpgRoadTest.CurrentPageIndex = Convert.ToInt32(pageNumber);

        //        //form the base url. 
        //        string qs = Request.ServerVariables["QUERY_STRING"];
        //        int recordCount;

        //        rpgRoadTest.BaseUrl = BaseUrl;
        //        rpgRoadTest.SelectClause = SelectClause;
        //        rpgRoadTest.FromClause = FromClause;
        //        rpgRoadTest.WhereClause = WhereClause;
        //        rpgRoadTest.OrderByClause = OrderByClause;
        //        rpgRoadTest.RecordCountQuery = RecordCntQry;
        //        rpgRoadTest.CmdParamQ = cmd;	//pass the parameter values for the query
        //        rpgRoadTest.CmdParamR = cmd.Clone();	//pass the parameter values for the record count                
        //        //initialize the grid, and this will also bind the repeater               
        //        rpgRoadTest.InitializeGrid();
        //        recordCount = rpgRoadTest.RecordCount;
        //        Trace.Warn("recordCount: " + recordCount.ToString());
        //        if (recordCount == 0)
        //        {
        //            alertObj.Visible = true;
        //            alertObj.InnerText = "Sorry! No road tests have been carried out for the selected bike.";
        //        }
        //        else
        //        {
        //            alertObj.Visible = false;
        //        }
        //        Trace.Warn("BaseURL" + BaseUrl);
        //        //Trace.Warn("Nav Url" + NavUrl);
        //    }
        //    catch (Exception err)
        //    {
        //        Trace.Warn(err.Message);
        //        ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
        //        objErr.SendMail();
        //    }
        //}        
    }
}