
using Bikewale.BAL.BikeData;
using Bikewale.BAL.EditCMS;
using Bikewale.BAL.Pager;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.CMS;
using Bikewale.Cache.Core;
using Bikewale.Common;
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
using Bikewale.Mobile.Controls;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Bikewale.BindViewModels.Webforms.EditCMS
{
    /// <summary>
    /// Modified By : Sajal Gupta on 30-01-2017
    /// Description : Common logic to bind expert-reviews listing page 
    /// </summary>
    public class RoadTestListing : System.Web.UI.Page
    {
        private IPager objPager = null;
        protected DropDownList ddlMakes;
        protected HtmlSelect ddlModels;
        public int startIndex = 0, endIndex = 0, totalrecords;
        protected int totalPages = 0;
        private const int _pageSize = 10, _pagerSlotSize = 5;
        public string prevPageUrl = String.Empty, nextPageUrl = String.Empty, modelId = string.Empty, makeId = string.Empty, makeName = string.Empty, modelName = string.Empty, makeMaskingName = string.Empty, modelMaskingName = string.Empty;
        public bool isContentFound = true;
        int _curPageNo = 1;
        HttpRequest page = HttpContext.Current.Request;
        public bool pageNotFound;
        public IList<ArticleSummary> articlesList;

        public RoadTestListing()
        {
            ProcessQueryString();
            GetRoadTestList();
            BindMakes();
            AutoFill();
        }

        /// <summary>
        /// Created By : Aditi Srivastava on 16 Nov 2016
        /// Summary    : To inject pagination widget
        /// </summary>
        private void GetRoadTestList()
        {
            try
            {
                // get pager instance
                IPager objPager = GetPager();

                int _startIndex = 0, _endIndex = 0;
                objPager.GetStartEndIndex(_pageSize, _curPageNo, out _startIndex, out _endIndex);
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

                        if (_objRoadTestList != null)
                        {
                            articlesList = _objRoadTestList.Articles;
                        }
                        totalrecords = Convert.ToInt32(_objRoadTestList.RecordCount);

                    }
                    else
                    {
                        isContentFound = false;
                    }
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
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

        private void BindMakes()
        {
            try
            {
                MakeModelVersion mmv = new MakeModelVersion();
                mmv.GetMakes(EnumBikeType.RoadTest, ref ddlMakes);

            }
            catch (Exception ex)
            {
                Bikewale.Common.ErrorClass objErr = new Bikewale.Common.ErrorClass(ex, Request.ServerVariables["URL"]);
            }
        }

        void AutoFill()
        {
            MakeModelVersion mmv = new MakeModelVersion();
            try
            {
                HttpContext.Current.Trace.Warn("AUTO FILL");
                if (makeId != "" && makeId != "-1" && ddlModels != null)
                {
                    if (ddlMakes != null)
                    {
                        ddlMakes.SelectedIndex = ddlMakes.Items.IndexOf(ddlMakes.Items.FindByValue(makeId + '_' + Request.QueryString["make"].ToString()));
                    }
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
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
            }
        }

        /// <summary>
        /// Modified By : Sajal Gupta on 27-01-2017
        /// Description : Return page found status    
        /// </summary>
        /// <returns></returns>

        private void ProcessQueryString()
        {
            try
            {
                modelMaskingName = HttpContext.Current.Request.QueryString["model"];
                if (!String.IsNullOrEmpty(modelMaskingName))
                {
                    ModelMaskingResponse objResponse = null;

                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IBikeMaskingCacheRepository<BikeModelEntity, int>, BikeModelMaskingCache<BikeModelEntity, int>>()
                                 .RegisterType<ICacheManager, MemcacheManager>()
                                 .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>();
                        var objCache = container.Resolve<IBikeMaskingCacheRepository<BikeModelEntity, int>>();
                        objResponse = objCache.GetModelMaskingResponse(modelMaskingName);
                        if (objResponse != null && objResponse.StatusCode == 200)
                        {
                            modelId = objResponse.ModelId.ToString();
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
                                CommonOpn.RedirectPermanent(Request.RawUrl.Replace(HttpContext.Current.Request.QueryString["model"], objResponse.MaskingName));
                            }
                            else
                            {
                                pageNotFound = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, Request.ServerVariables["URL"] + "ProcessQueryString");
                pageNotFound = true;
            }

            makeMaskingName = HttpContext.Current.Request.QueryString["make"];
            if (!String.IsNullOrEmpty(makeMaskingName))
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
                    pageNotFound = true;
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
                            pageNotFound = true;
                        }
                    }
                    else
                    {
                        pageNotFound = true;
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
                if (String.IsNullOrEmpty(makeId))
                {
                    pageNotFound = true;
                }
            }

            if (!String.IsNullOrEmpty(HttpContext.Current.Request.QueryString["pn"]))
            {
                if (!Int32.TryParse(HttpContext.Current.Request.QueryString["pn"], out _curPageNo))
                    _curPageNo = 1;
                else
                    _curPageNo = Convert.ToInt32(HttpContext.Current.Request.QueryString["pn"]);
            }

        }

        /// <summary>
        ///  Created by : Aditi Srivastava on 18 Nov 2016
        /// Summary     : Create pagination
        /// </summary>
        /// <param name="_ctrlPager"></param>
        public void BindLinkPager(LinkPagerControl _ctrlPager)
        {
            objPager = GetPager();
            objPager.GetStartEndIndex(_pageSize, _curPageNo, out startIndex, out endIndex);
            PagerOutputEntity _pagerOutput = null;
            PagerEntity _pagerEntity = null;
            int recordCount = totalrecords;
            string _baseUrl = RemoveTrailingPage(page.RawUrl.ToLower());

            try
            {
                GetStartEndIndex(_pageSize, _curPageNo, out startIndex, out endIndex, recordCount);

                _pagerEntity = new PagerEntity();
                _pagerEntity.BaseUrl = _baseUrl;
                _pagerEntity.PageNo = _curPageNo; //Current page number
                _pagerEntity.PagerSlotSize = _pagerSlotSize; // 5 links on a page
                _pagerEntity.PageUrlType = "page/";
                _pagerEntity.TotalResults = (int)recordCount; //total News count
                _pagerEntity.PageSize = _pageSize;  //No. of news to be displayed on a page
                _pagerOutput = objPager.GetPager<PagerOutputEntity>(_pagerEntity);

                // for RepeaterPager

                _ctrlPager.PagerOutput = _pagerOutput;
                _ctrlPager.CurrentPageNo = _curPageNo;
                _ctrlPager.TotalPages = objPager.GetTotalPages((int)recordCount, _pageSize);
                _ctrlPager.BindPagerList();

                //For SEO

                prevPageUrl = String.IsNullOrEmpty(_pagerOutput.PreviousPageUrl) ? string.Empty : "https://www.bikewale.com" + _pagerOutput.PreviousPageUrl;
                nextPageUrl = String.IsNullOrEmpty(_pagerOutput.NextPageUrl) ? string.Empty : "https://www.bikewale.com" + _pagerOutput.NextPageUrl;
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "BikeCareModels.BindLinkPager");
            }
        }
        /// <summary>
        /// Created By : Aditi Srivastava on 18 Nov 2016
        /// Summary    : Set current page's start and end index of articles
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="currentPageNo"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="totalCount"></param>
        public void GetStartEndIndex(int pageSize, int currentPageNo, out int startIndex, out int endIndex, int totalCount)
        {
            startIndex = 0;
            endIndex = 0;
            endIndex = currentPageNo * pageSize;
            startIndex = (endIndex - pageSize) + 1;
            if (totalCount < endIndex)
                endIndex = totalCount;
        }
        /// <summary>
        /// Created By : Aditi Srivastava on 18 Nov 2016
        /// Summary    : Remove trailing page from link
        /// </summary>
        /// <param name="rawUrl"></param>
        /// <returns></returns>
        public string RemoveTrailingPage(string rawUrl)
        {
            string retUrl = rawUrl;
            if (rawUrl.Contains("/page/"))
            {
                string[] urlArray = rawUrl.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                retUrl = string.Format("/{0}/", string.Join("/", urlArray.Take(urlArray.Length - 2).ToArray()));
            }
            return retUrl;
        }
    }
}