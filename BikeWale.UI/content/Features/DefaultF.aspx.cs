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
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Interfaces.Pager;
using Bikewale.Entities.CMS;
using Microsoft.Practices.Unity;
using Bikewale.BAL.Pager;
using Bikewale.Entities.Pager;
using System.Threading.Tasks;
using Bikewale.Utility;

namespace Bikewale.Content
{
    /// <summary>
    /// Modified By : Ashwini Todkar on 25 Sept 2014
    /// Retrieved features from carwale web api
    /// </summary>
    public class DefaultF : System.Web.UI.Page
    {
        protected Repeater rptFeatures;
        protected LinkPagerControl linkPager;
        protected string prevUrl = string.Empty, nextUrl = string.Empty;

        private int _pageNo = 1;
        private const int _pageSize = 10, _pagerSlotSize = 5;
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

            if (Request["pn"] != null && Request.QueryString["pn"] != "")
            {
                if (CommonOpn.CheckId(Request.QueryString["pn"]) == true)
                    _pageNo = Convert.ToInt32(Request.QueryString["pn"]);
            }

            GetFeaturesList();
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 25 Sept 2014
        /// Summary    : method to fetch features list and total features count from carwale api
        /// </summary>      
        private async void GetFeaturesList()
        {
            try
            {
                // get pager instance
                IPager objPager = GetPager();

                int _startIndex = 0, _endIndex = 0;// _featuresCategoryId = (int)EnumCMSContentType.Features;

                List<EnumCMSContentType> categorList = new List<EnumCMSContentType>();
                categorList.Add(EnumCMSContentType.Features);
                categorList.Add(EnumCMSContentType.SpecialFeature);
                string _featuresCategoryId = CommonApiOpn.GetContentTypesString(categorList);

                objPager.GetStartEndIndex(_pageSize, _pageNo, out _startIndex, out _endIndex);
                CMSContent _objFeaturesList = null;
                string _apiUrl = "webapi/article/listbycategory/?applicationid=2&categoryidlist=" + _featuresCategoryId + "&startindex=" + _startIndex + "&endindex=" + _endIndex;
                // Send HTTP GET requests 

                using(Utility.BWHttpClient objClient = new Utility.BWHttpClient())
                {
                    _objFeaturesList = await objClient.GetApiResponse<CMSContent>(Utility.APIHost.CW, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, _objFeaturesList);
                }                

                if (_objFeaturesList != null)
                {
                    if (_objFeaturesList.Articles.Count > 0)
                    {
                        BindFeatures(_objFeaturesList);
                        BindLinkPager(objPager, Convert.ToInt32(_objFeaturesList.RecordCount));
                    }
                    else
                        _isContentFound = false;
                    
                }
                else
                {
                    _isContentFound = false;
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
                if(!_isContentFound)
                    Response.Redirect("/pagenotfound.aspx", true);
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
        /// Written By : Ashwini Todkar on 24 Sept 2014
        /// PopulateWhere to bind link pager control 
        /// </summary>
        /// <param name="objPager"> Pager instance </param>
        /// <param name="recordCount"> total news available</param>
        private void BindLinkPager(IPager objPager, int recordCount)
        {
            PagerOutputEntity _pagerOutput = null;
            PagerEntity _pagerEntity = null;

            try
            {
                _pagerEntity = new PagerEntity();
                _pagerEntity.BaseUrl = "/features/";
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
                //CreatePrevNextUrl(linkPager.TotalPages);
                prevUrl =  String.IsNullOrEmpty(_pagerOutput.PreviousPageUrl) ? "" : "http://www.bikewale.com" + _pagerOutput.PreviousPageUrl;
                nextUrl = String.IsNullOrEmpty(_pagerOutput.NextPageUrl) ? "" : "http://www.bikewale.com" + _pagerOutput.NextPageUrl;       
            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        private void BindFeatures(CMSContent _objFeaturesList)
        {
            rptFeatures.DataSource = _objFeaturesList.Articles;
            rptFeatures.DataBind();
        }

        // Commented By : Ashwini Todkar on 25 Sept
        //Not more required 
        //private void CreatePrevNextUrl(int totalPages)
        //{
        //    string _mainUrl = "http://www.bikewale.com/features/page/";
        //    string prevPageNumber = string.Empty, nextPageNumber = string.Empty;

        //    if (_pageNo == 1)    //if page is first page
        //    {
        //        nextPageNumber = "2";
        //        nextUrl = _mainUrl + nextPageNumber + "/";
        //    }
        //    else if (_pageNo == totalPages)    //if page is last page
        //    {
        //        prevPageNumber = (_pageNo - 1).ToString();
        //        prevUrl = _mainUrl + prevPageNumber + "/";
        //    }
        //    else
        //    {          //for middle pages
        //        prevPageNumber = (_pageNo - 1).ToString();
        //        prevUrl = _mainUrl + prevPageNumber + "/";
        //        nextPageNumber = (_pageNo + 1).ToString();
        //        nextUrl = _mainUrl + nextPageNumber + "/";
        //    }
        //}


        // Commented By : Ashwini Todkar on 25 Sept
        //Not more required as all details getting from carwale api  

        //protected RepeaterPager rpgFeatures;
        //protected Repeater rptFeatures;
        //protected DropDownList ddlUsedCarLocations, ddlMake;
        //protected MakeModelSearch MakeModelSearch;       

        //protected string SelectClause = string.Empty, FromClause = string.Empty, WhereClause = string.Empty,
        //                     OrderByClause = string.Empty, RecordCntQry = string.Empty, BaseUrl = string.Empty;

        //private string pageNumber = string.Empty;
        //protected override void OnInit(EventArgs e)
        //{
        //    rptFeatures = (Repeater)rpgFeatures.FindControl("rptFeatures");
        //    base.Load += new EventHandler(Page_Load);
        //}
        //private void Page_Load(object sender, EventArgs e)
        //{
        //    //code for device detection added by Ashwini Todkar
        //    DeviceDetection dd = new DeviceDetection(Request.ServerVariables["HTTP_X_ORIGINAL_URL"].ToString());
        //    dd.DetectDevice();

        //    CommonOpn op = new CommonOpn();

        //    if (Request["pn"] != null && Request.QueryString["pn"] != "")
        //    {
        //        if (CommonOpn.CheckId(Request.QueryString["pn"]) == true)
        //            pageNumber = Request.QueryString["pn"];
        //        Trace.Warn("pn: " + Request.QueryString["pn"]);
        //    }
        //    Trace.Warn("pageNumber: " + pageNumber);
        //    if (!IsPostBack)
        //    {
        //        FillFeaturesDetails();
        //    }
        //}        

        //private void FillFeaturesDetails()
        //{

        //    SelectClause = " CB.Id AS BasicId, CB.AuthorName, CB.Description, CB.DisplayDate, CB.Views, CB.Title, CB.Url, CEI.HostURL, CEI.ImagePathThumbnail, CEI.ImagePathLarge ";
        //    FromClause = " Con_EditCms_Basic AS CB With(NoLock) Left Join Con_EditCms_Images CEI With(NoLock) On CEI.BasicId = CB.Id And CEI.IsMainImage = 1 And CEI.IsActive = 1 ";
        //    WhereClause = " CB.CategoryId = @CategoryId AND CB.IsActive = 1 AND CB.IsPublished = 1";
        //    OrderByClause = " DisplayDate Desc ";
        //    RecordCntQry = " Select Count(CB.Id) From " + FromClause + " Where " + WhereClause;
        //    BaseUrl = "/features/";

        //    SqlCommand cmd = new SqlCommand();
        //    cmd.Parameters.Add("@CategoryId", SqlDbType.BigInt).Value = "6";// Default value for Articles on Features

        //    BindData(cmd);
        //}

        //void BindData(SqlCommand cmd)
        //{
        //    try
        //    {
        //        if (pageNumber != string.Empty)
        //            rpgFeatures.CurrentPageIndex = Convert.ToInt32(pageNumber);

        //        //form the base url. 
        //        string qs = Request.ServerVariables["QUERY_STRING"];
        //        int recordCount;

        //        rpgFeatures.BaseUrl = BaseUrl;

        //        rpgFeatures.SelectClause = SelectClause;
        //        rpgFeatures.FromClause = FromClause;
        //        rpgFeatures.WhereClause = WhereClause;
        //        rpgFeatures.OrderByClause = OrderByClause;
        //        rpgFeatures.RecordCountQuery = RecordCntQry;
        //        rpgFeatures.CmdParamQ = cmd;	//pass the parameter values for the query
        //        rpgFeatures.CmdParamR = cmd.Clone();	//pass the parameter values for the record count                
        //        //initialize the grid, and this will also bind the repeater               
        //        rpgFeatures.InitializeGrid();
        //        recordCount = rpgFeatures.RecordCount;
        //        Trace.Warn("recordCount: " + recordCount.ToString());                
        //        Trace.Warn("BaseURL" + BaseUrl);
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