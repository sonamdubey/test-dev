using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using Microsoft.Practices.Unity;
using Bikewale.Entities.CMS;
using Bikewale.Interfaces.CMS;
using Bikewale.BAL.CMS;
using Bikewale.Common;
using Bikewale.Interfaces.Pager;
using Bikewale.BAL.Pager;
using Bikewale.Entities.Pager;
using Bikewale.Mobile.Controls;
using Bikewale.Memcache;
using System.Net.Http;
using System.Configuration;
using System.Net.Http.Headers;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Cache.BikeData;
using Bikewale.DAL.BikeData;

namespace Bikewale.Content
{
    /// <summary>
    /// Created By : Ashwini Todkar on 21 May 2014
    /// </summary>
    public class RoadTest : System.Web.UI.Page
    {
        protected Repeater rptRoadTest;
        protected ListPagerControl listPager;
        protected HtmlSelect ddlMakes,ddlModels;

        protected int totalPages = 0;
        private const int _pageSize = 10;
        protected string prevPageUrl = String.Empty, nextPageUrl = String.Empty, modelId = string.Empty, makeId = string.Empty;
        private bool _isContentFound = true;
        int _curPageNo = 1;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ProcessQueryString();
             
                GetRoadTestList();
                BindMakes();
                AutoFill();
            }
        }

        private async void GetRoadTestList()
        {
            try
            {
                string _cwHostUrl = ConfigurationManager.AppSettings["cwApiHostUrl"];

                // get pager instance
                IPager objPager = GetPager();

                int _startIndex = 0, _endIndex = 0, _roadtestCategoryId = (int)EnumCMSContentType.RoadTest;

                objPager.GetStartEndIndex(_pageSize, _curPageNo, out _startIndex, out _endIndex);

                string _apiUrl = string.Empty;

                if (String.IsNullOrEmpty(makeId))
                {
                    _apiUrl = "webapi/article/listbycategory/?applicationid=" + ConfigurationManager.AppSettings["applicationId"] + "&categoryidlist=" + _roadtestCategoryId + "&startindex=" + _startIndex + "&endindex=" + _endIndex;
                }
                else
                {
                    if (String.IsNullOrEmpty(modelId))
                    {
                        _apiUrl = "webapi/article/listbycategory/?applicationid=" + ConfigurationManager.AppSettings["applicationId"] + "&categoryidlist=" + _roadtestCategoryId + "&startindex=" + _startIndex + "&endindex=" + _endIndex + "&makeid=" + makeId;
                    }
                    else
                    {
                        _apiUrl = "webapi/article/listbycategory/?applicationid=" + ConfigurationManager.AppSettings["applicationId"] + "&categoryidlist=" + _roadtestCategoryId + "&startindex=" + _startIndex + "&endindex=" + _endIndex + "&makeid=" + makeId + "&modelid=" + modelId;
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
                        BindLinkPager(objPager, Convert.ToInt32(_objRoadTestList.RecordCount));
                    }
                    else
                    {
                        _isContentFound = false;
                    }
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
                    Response.Redirect("/m/pagenotfound.aspx", true);
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
        /// Written By : Ashwini Todkar on 24 Sept 2014
        /// PopulateWhere to bind link pager control 
        /// </summary>
        /// <param name="objPager"> Pager instance </param>
        /// <param name="recordCount"> total news available</param>
        private void BindLinkPager(IPager objPager, int recordCount)
        {
            PagerOutputEntity _pagerOutput = null;
            PagerEntity _pagerEntity = null;
            string _baseUrl = "/m/road-tests/";

            try
            {
                if (!String.IsNullOrEmpty(makeId) && !String.IsNullOrEmpty(modelId))
                    _baseUrl = Request.QueryString["make"] + "-bikes/" + Request.QueryString["model"] + "/road-tests/";
                else if (!String.IsNullOrEmpty(Request.QueryString["make"]))
                    _baseUrl = "/m/" + Request.QueryString["make"] + "-bikes/" + "/road-tests/";

                _pagerEntity = new PagerEntity();
                _pagerEntity.BaseUrl = _baseUrl;
                _pagerEntity.PageNo = _curPageNo; //Current page number
                _pagerEntity.PagerSlotSize = objPager.GetTotalPages(recordCount, _pageSize); // 5 links on a page
                _pagerEntity.PageUrlType = "page/";
                _pagerEntity.TotalResults = recordCount; //total News count
                _pagerEntity.PageSize = _pageSize;        //No. of news to be displayed on a page

                _pagerOutput = objPager.GetPager<PagerOutputEntity>(_pagerEntity);
              
                // for RepeaterPager
                listPager.PagerOutput = _pagerOutput;
                listPager.CurrentPageNo = _curPageNo;
                listPager.TotalPages = objPager.GetTotalPages(recordCount, _pageSize);
                listPager.BindPageNumbers();

                //For SEO            
                prevPageUrl = _pagerOutput.PreviousPageUrl;
                nextPageUrl = _pagerOutput.NextPageUrl;
            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        private void BindRoadtest(CMSContent _objRoadtestList)
        {
            rptRoadTest.DataSource = _objRoadtestList.Articles;
            rptRoadTest.DataBind();
        }


        //protected void LoadRoadTest()
        //{
        //    try
        //    {
        //        using (var container = new UnityContainer())
        //        {
        //            int recordCount = 0, startIndex = 0, endIndex = 0, pageSize = 10;

        //            // Get road test object
        //            container.RegisterType<ICMSContentRepository<CMSContentListEntity, CMSPageDetailsEntity>, CMSData<CMSContentListEntity, CMSPageDetailsEntity>>();
        //            ICMSContentRepository<CMSContentListEntity, CMSPageDetailsEntity> repository = container.Resolve<ICMSContentRepository<CMSContentListEntity, CMSPageDetailsEntity>>(new ResolverOverride[] { new ParameterOverride("contentType", EnumCMSContentType.RoadTest) });

        //            // Get Pager object
        //            container.RegisterType<IPager, Pager>();
        //            IPager objPager = container.Resolve<IPager>();
                    
        //            objPager.GetStartEndIndex(pageSize, curPageNo, out startIndex, out endIndex);

        //            ContentFilter filter = new ContentFilter();

        //            if (!String.IsNullOrEmpty(modelId))
        //            {
        //                filter.ModelId = Convert.ToInt16(modelId);
        //            }
        //            else if (!String.IsNullOrEmpty(makeId))
        //            {
        //                filter.MakeId = Convert.ToInt16(makeId);
        //            }
                    
        //            // Get content list
        //            IList<CMSContentListEntity> objContentList = repository.GetContentList(startIndex, endIndex, out recordCount, filter);

        //            totalPages = objPager.GetTotalPages(recordCount, pageSize);


        //            //if current page number exceeded the total pages count i.e. the page is not available 
        //            if (curPageNo > 0 && curPageNo <= totalPages)
        //            {
        //                if (objContentList != null)
        //                {
        //                    rptRoadTest.DataSource = objContentList;
        //                    rptRoadTest.DataBind();
        //                }

        //                PagerEntity pagerEntity = new PagerEntity();
        //                if(!String.IsNullOrEmpty(makeId))
        //                    pagerEntity.BaseUrl = "/m/" + Request.QueryString["make"] + "-bikes/road-tests/";
        //                else
        //                    pagerEntity.BaseUrl = "/m/road-tests/";
        //                pagerEntity.PageNo = curPageNo;
        //                pagerEntity.PagerSlotSize = totalPages;
        //                pagerEntity.PageUrlType = "page/";
        //                pagerEntity.TotalResults = recordCount;
        //                pagerEntity.PageSize = pageSize;

        //                PagerOutputEntity pagerOutput = objPager.GetPager<PagerOutputEntity>(pagerEntity);

        //                listPager.PagerOutput = pagerOutput;
        //                listPager.TotalPages = totalPages;
        //                listPager.CurrentPageNo = curPageNo;

        //                //get next and prev page links for SEO
        //                prevPageUrl = pagerOutput.PreviousPageUrl;
        //                nextPageUrl = pagerOutput.NextPageUrl;
        //            }
        //            else
        //            {
        //                Response.Redirect("/m/pagenotfound.aspx", false);
        //                HttpContext.Current.ApplicationInstance.CompleteRequest();
        //                this.Page.Visible = false;
        //            }
        //        }
        //    }
        //    catch (SqlException err)
        //    {
        //        Trace.Warn("Error = " + err.Message);
        //        ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
        //        objErr.SendMail();
        //    }
        //    catch (Exception err)
        //    {
        //        Trace.Warn("Error = " + err.Message);
        //        ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
        //        objErr.SendMail();
        //    }
        //}

        private void BindMakes()
        {
            try
            {
                MakeModelVersion mmv = new MakeModelVersion();

                ddlMakes.DataSource = mmv.GetMakes("ROADTEST");
                ddlMakes.DataValueField = "Value";
                ddlMakes.DataTextField = "Text";
                ddlMakes.DataBind();
                ddlMakes.Items.Insert(0, (new ListItem("--Select Make--", "0")));

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        void AutoFill()
        {
            MakeModelVersion mmv = new MakeModelVersion();
            try
            {
                HttpContext.Current.Trace.Warn("AUTO FILL");
                if (makeId != "" && makeId != "-1")
                {
                    ddlMakes.SelectedIndex = ddlMakes.Items.IndexOf(ddlMakes.Items.FindByValue(makeId + '_' + Request.QueryString["make"].ToString()));
                    ddlModels.Disabled = false;
                    ddlModels.DataSource = mmv.GetModelsWithMappingName(makeId, "ROADTEST");
                    ddlModels.DataTextField = "Text";
                    ddlModels.DataValueField = "Value";
                    ddlModels.DataBind();
                    ListItem item = new ListItem("--Select Model--", "0");
                    ddlModels.Items.Insert(0, item);

                    if (modelId != "" && modelId != "-1")
                    {
                        ddlModels.SelectedIndex = ddlModels.Items.IndexOf(ddlModels.Items.FindByValue(modelId + '_' + Request.QueryString["model"].ToString()));
                    }
                }
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        private void ProcessQueryString()
        {
            if (!String.IsNullOrEmpty(Request.QueryString["model"]))
            {
                //ModelMapping objMapping = new ModelMapping();

                //modelId = objMapping.GetModelId(Request.QueryString["model"].ToLower());

                //if (String.IsNullOrEmpty(modelId))
                //{
                //    Response.Redirect("/m/pagenotfound.aspx", false);
                //    HttpContext.Current.ApplicationInstance.CompleteRequest();
                //    this.Page.Visible = false;
                //}

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
                        modelId = objResponse.ModelId.ToString();
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

            }

            if (!String.IsNullOrEmpty(Request.QueryString["make"]))
            {
                makeId = MakeMapping.GetMakeId(Request.QueryString["make"].ToLower());

                if (String.IsNullOrEmpty(makeId))
                {
                    Response.Redirect("/m/pagenotfound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }

            if (!String.IsNullOrEmpty(Request.QueryString["pn"]))
            {
                if (!Int32.TryParse(Request.QueryString["pn"], out _curPageNo))
                    _curPageNo = 1;
                else
                    _curPageNo = Convert.ToInt32(Request.QueryString["pn"]);
            }
        }


    }
}