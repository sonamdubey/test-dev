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
using Bikewale.Entities.Pager;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.EditCMS;
using Bikewale.Interfaces.Pager;
using Bikewale.Memcache;
using Microsoft.Practices.Unity;
using System;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Bikewale.Content
{
    //Modified By : Ashwini Todkar on 29 Sept 2014
    //Modified code to retrieve roadtest from api

    public class DefaultRT : System.Web.UI.Page
    {
        protected Repeater rptRoadTest;
        //protected MakeModelSearch MakeModelSearch;
        protected HtmlGenericControl alertObj;
        protected LinkPagerControl linkPager;
        protected string nextUrl = string.Empty, prevUrl = string.Empty, makeName = string.Empty, modelName = string.Empty, makeMaskingName = string.Empty, modelMaskingName = string.Empty;
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
            Trace.Warn("current url :" + HttpContext.Current.Request.Url);
            if (!IsPostBack)
            {
                string _makeName = string.Empty, _makeId = string.Empty, _modelId = string.Empty, _modelName = string.Empty;
                ProcessQS(out _makeName, out _makeId, out _modelId, out _modelName);
                GetRoadTestList(_makeId, _modelId, _makeName, _modelName);
            }
        }

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
            makeMaskingName = _makeName;
            if (!String.IsNullOrEmpty(_makeName))
            {
                _makeId = MakeMapping.GetMakeId(_makeName.ToLower());
                using (IUnityContainer container1 = new UnityContainer())
                {
                    container1.RegisterType<IBikeMakesCacheRepository<int>, BikeMakesCacheRepository<BikeMakeEntity, int>>()
                            .RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>()
                            .RegisterType<ICacheManager, MemcacheManager>();
                    var _objMakeCache = container1.Resolve<IBikeMakesCacheRepository<int>>();
                    BikeMakeEntityBase objMMV = _objMakeCache.GetMakeDetails(Convert.ToUInt32(_makeId));
                    makeName = objMMV.MakeName;
                }


            }

            if (!String.IsNullOrEmpty(_makeId))
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
                            _modelId = objResponse.ModelId.ToString();
                            _modelName = Request.QueryString["model"];
                            using (IUnityContainer modelContainer = new UnityContainer())
                            {
                                modelContainer.RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>();
                                IBikeModels<BikeModelEntity, int> objClient = modelContainer.Resolve<IBikeModels<BikeModelEntity, int>>();
                                BikeModelEntity bikemodelEnt = objClient.GetById(Convert.ToInt32(_modelId));
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

                        BindRoadtest(_objRoadTestList);
                        BindLinkPager(objPager, Convert.ToInt32(_objRoadTestList.RecordCount), makeName, modelName);

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
    }
}