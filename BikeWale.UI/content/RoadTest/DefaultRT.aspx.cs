using Bikewale.BAL.BikeData;
using Bikewale.BAL.EditCMS;
using Bikewale.BAL.Pager;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.CMS;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.Controls;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.Location;
using Bikewale.Entities.Pager;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.EditCMS;
using Bikewale.Interfaces.Pager;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Bikewale.Content
{
    //Modified By : Ashwini Todkar on 29 Sept 2014
    //Modified code to retrieve roadtest from api

    /// <summary>
    /// Modified By : Sushil Kumar on 10th Nov 2016
    /// Description : Bind most popular bikes widget for edit cms
    /// </summary>
    public class DefaultRT : System.Web.UI.Page
    {
        protected UpcomingBikesMinNew ctrlUpcoming;
        protected Repeater rptRoadTest;
        protected HtmlGenericControl alertObj;
        protected Bikewale.Mobile.Controls.LinkPagerControl ctrlPager;
        protected string nextUrl = string.Empty, prevUrl = string.Empty, makeName = string.Empty, modelName = string.Empty, makeMaskingName = string.Empty, modelMaskingName = string.Empty;
        private const int _pageSize = 10;
        private int _pageNo = 1;
        private const int _pagerSlotSize = 5;
        private bool _isContentFound = true;
        protected MostPopularBikesMin ctrlPopularBikes;
        string makeId = string.Empty, modelId = string.Empty;
        protected string startIndex, endIndex, totalArticles;

        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
        }

        /// <summary>
        /// Modified By : Sushil Kumar on 10th Nov 2016
        /// Description : Bind most popular bikes widget for edit cms
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Load(object sender, EventArgs e)
        {
            // Modified By :Lucky Rathore on 12 July 2016.
            Form.Action = Request.RawUrl;
            //code for device detection added by Ashwini Todkar
            // Modified By :Ashish Kamble on 5 Feb 2016
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];
            DeviceDetection dd = new DeviceDetection(originalUrl);
            dd.DetectDevice();

            CommonOpn op = new CommonOpn();

            if (!IsPostBack)
            {

                ProcessQS();
                GetRoadTestList(makeId, modelId, makeName, modelName);
            }

        }

        private void ProcessQS()
        {
            if (Request["pn"] != null && Request.QueryString["pn"] != "")
            {
                string _pageNumber = Request.QueryString["pn"];
                if (CommonOpn.CheckId(_pageNumber) == true)
                    _pageNo = Convert.ToInt32(_pageNumber);
            }

            makeName = Request.QueryString["make"];
            makeMaskingName = makeName;
            if (!String.IsNullOrEmpty(makeName))
            {
                MakeMaskingResponse objResponse = null;

                try
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IBikeMakesCacheRepository<int>, BikeMakesCacheRepository<BikeMakeEntity, int>>()
                              .RegisterType<ICacheManager, MemcacheManager>()
                              .RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>()
                             ;
                        var objCache = container.Resolve<IBikeMakesCacheRepository<int>>();

                        objResponse = objCache.GetMakeMaskingResponse(makeMaskingName);
                    }
                }
                catch (Exception ex)
                {
                    Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, Request.ServerVariables["URL"] + "ParseQueryString");
                    objErr.SendMail();
                    Response.Redirect("pageNotFound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
                finally
                {
                    if (objResponse != null)
                    {
                        if (objResponse.StatusCode == 200)
                        {
                            makeId = Convert.ToString(objResponse.MakeId);
                        }
                        else if (objResponse.StatusCode == 301)
                        {
                            CommonOpn.RedirectPermanent(Request.RawUrl.Replace(makeMaskingName, objResponse.MaskingName));
                        }
                        else
                        {
                            Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
                            this.Page.Visible = false;
                        }
                    }
                    else
                    {
                        Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        this.Page.Visible = false;
                    }
                }


                using (IUnityContainer container1 = new UnityContainer())
                {
                    container1.RegisterType<IBikeMakesCacheRepository<int>, BikeMakesCacheRepository<BikeMakeEntity, int>>()
                            .RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>()
                            .RegisterType<ICacheManager, MemcacheManager>();
                    var _objMakeCache = container1.Resolve<IBikeMakesCacheRepository<int>>();
                    BikeMakeEntityBase objMMV = _objMakeCache.GetMakeDetails(Convert.ToUInt32(makeId));
                    makeName = objMMV.MakeName;
                }


            }

            if (!String.IsNullOrEmpty(makeId))
            {
                modelMaskingName = Request.QueryString["model"];
                if (!String.IsNullOrEmpty(modelMaskingName))
                {
                    ModelMaskingResponse objResponse = null;
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IBikeMaskingCacheRepository<BikeModelEntity, int>, BikeModelMaskingCache<BikeModelEntity, int>>()
                                 .RegisterType<ICacheManager, MemcacheManager>()
                                 .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>()
                                ;
                        var objCache = container.Resolve<IBikeMaskingCacheRepository<BikeModelEntity, int>>();
                        objResponse = objCache.GetModelMaskingResponse(modelMaskingName);
                        if (objResponse != null && objResponse.StatusCode == 200)
                        {
                            modelId = objResponse.ModelId.ToString();
                            modelName = Request.QueryString["model"];
                            using (IUnityContainer modelContainer = new UnityContainer())
                            {
                                modelContainer.RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>();
                                IBikeModels<BikeModelEntity, int> objClient = modelContainer.Resolve<IBikeModels<BikeModelEntity, int>>();
                                BikeModelEntity bikemodelEnt = objClient.GetById(Convert.ToInt32(modelId));
                                modelName = bikemodelEnt.ModelName;

                            }
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
            }
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 26 Sept 2014
        /// Summary    : method to get roadtest list and total roadtest count from carwale api
        /// </summary>
        private void GetRoadTestList(string makeId, string modelId, string makeName, string modelName)
        {
            try
            {
                // get pager instance
                IPager objPager = GetPager();
                int _startIndex = 0, _endIndex = 0;
                objPager.GetStartEndIndex(_pageSize, _pageNo, out _startIndex, out _endIndex);
                CMSContent _objRoadTestList = null;
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IArticles, Articles>()
                            .RegisterType<ICMSCacheContent, CMSCacheRepository>()
                            .RegisterType<ICacheManager, MemcacheManager>();
                    ICMSCacheContent _cache = container.Resolve<ICMSCacheContent>();

                    _objRoadTestList = _cache.GetArticlesByCategoryList(Convert.ToString((int)EnumCMSContentType.RoadTest), _startIndex, _endIndex, string.IsNullOrEmpty(makeId) ? 0 : Convert.ToInt32(makeId), string.IsNullOrEmpty(modelId) ? 0 : Convert.ToInt32(modelId));

                    if (_objRoadTestList != null && _objRoadTestList.Articles.Count > 0)
                    {

                        rptRoadTest.DataSource = _objRoadTestList.Articles;
                        rptRoadTest.DataBind();

                        BindLinkPager(objPager, Convert.ToInt32(_objRoadTestList.RecordCount), makeName, modelName);
                        BindPageWidgets();
                        totalArticles = Format.FormatPrice(_objRoadTestList.RecordCount.ToString());
                        startIndex = Format.FormatPrice(_startIndex.ToString());
                        endIndex = Format.FormatPrice(_endIndex > _objRoadTestList.RecordCount ? _objRoadTestList.RecordCount.ToString() : _endIndex.ToString());
                    }
                    else
                    {
                        alertObj.Visible = true;
                        alertObj.InnerText = "Sorry! No road tests have been carried out for the selected bike.";
                        _isContentFound = false;
                    }
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"] + "GetRoadTestList");
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
        /// Written By : Ashwini Todkar on 24 Sept 2014
        /// PopulateWhere to bind link pager control 
        /// </summary>
        /// <param name="objPager"> Pager instance </param>
        /// <param name="recordCount"> total news available</param>
        private void BindLinkPager(IPager objPager, int recordCount, string makeName, string modelName)
        {
            PagerOutputEntity _pagerOutput = null;
            PagerEntity _pagerEntity = null;
            string _baseUrl = "/expert-reviews/";

            try
            {
                if (!String.IsNullOrEmpty(modelName))
                    _baseUrl = string.Format("/{0}-bikes/{1}/expert-reviews/", makeName, modelName);
                else if (!String.IsNullOrEmpty(makeName))
                    _baseUrl = string.Format("/{0}-bikes/expert-reviews/", makeName);

                _pagerEntity = new PagerEntity();
                _pagerEntity.BaseUrl = _baseUrl;
                _pagerEntity.PageNo = _pageNo; //Current page number
                _pagerEntity.PagerSlotSize = _pagerSlotSize; // 5 links on a page
                _pagerEntity.PageUrlType = "page/";
                _pagerEntity.TotalResults = recordCount; //total News count
                _pagerEntity.PageSize = _pageSize;        //No. of news to be displayed on a page

                _pagerOutput = objPager.GetPager<PagerOutputEntity>(_pagerEntity);

                // for RepeaterPager
                ctrlPager.PagerOutput = _pagerOutput;
                ctrlPager.CurrentPageNo = _pageNo;
                ctrlPager.TotalPages = objPager.GetTotalPages(recordCount, _pageSize);
                ctrlPager.BindPagerList();

                //For SEO
                //CreatePrevNextUrl(linkPager.TotalPages,_baseUrl);
                prevUrl = String.IsNullOrEmpty(_pagerOutput.PreviousPageUrl) ? "" : "https://www.bikewale.com" + _pagerOutput.PreviousPageUrl;
                nextUrl = String.IsNullOrEmpty(_pagerOutput.NextPageUrl) ? "" : "https://www.bikewale.com" + _pagerOutput.NextPageUrl;
            }
            catch (Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + "BindLinkPager");
                objErr.SendMail();
            }
        }


        /// <summary>
        /// Created by : Aditi Srivastava on 8 Nov 2016
        /// Summary  : Bind upcoming bikes list
        /// </summary>
        private void BindPageWidgets()
        {

            try
            {
                GlobalCityAreaEntity currentCityArea = GlobalCityArea.GetGlobalCityArea();

                ctrlPopularBikes.totalCount = 4;
                ctrlPopularBikes.CityId = Convert.ToInt32(currentCityArea.CityId);
                ctrlPopularBikes.cityName = currentCityArea.City;

                ctrlUpcoming.sortBy = (int)EnumUpcomingBikesFilter.Default;
                ctrlUpcoming.pageSize = 9;
                ctrlUpcoming.topCount = 4;

                if (!string.IsNullOrEmpty(makeId) && makeId != "0")
                {
                    ctrlPopularBikes.makeId = Convert.ToInt32(makeId);
                    ctrlPopularBikes.makeMasking = makeMaskingName;
                    ctrlPopularBikes.makeName = makeName;

                    ctrlUpcoming.makeName = makeName;
                    ctrlUpcoming.makeMaskingName = makeMaskingName;
                    ctrlUpcoming.MakeId = Convert.ToInt32(makeId);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + "BindPageWidgets");
                objErr.SendMail();
            }
        }
    }
}