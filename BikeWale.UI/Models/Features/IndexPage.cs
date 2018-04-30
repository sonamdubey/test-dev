using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS;
using Bikewale.Entities.EditorialWidgets;
using Bikewale.Entities.Location;
using Bikewale.Entities.Pager;
using Bikewale.Entities.Schema;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Pager;
using Bikewale.Models.EditorialPages;
using Bikewale.Notifications;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Web;
namespace Bikewale.Models
{
    /// <summary>
    /// Created By :- Subodh Jain 31 March 2017
    /// Summary :- Model For Index Page
    /// </summary>
    public class IndexPage : EditorialBasePage
    {
        #region Variables for dependency injection
        private readonly ICMSCacheContent _Cache = null;
        private readonly IPager _objPager = null;
        private readonly IUpcoming _upcoming = null;
        private readonly IBikeModels<BikeModelEntity, int> _bikeModels = null;
        private readonly IBikeMakesCacheRepository _bikeMakesCacheRepository = null;
        private readonly IBikeModelsCacheRepository<int> _models = null;
        private readonly IBikeSeries _series;
        private uint CityId = 0;
        #endregion

        #region Page level variables
        private const int _pageSize = 10, _pagerSlotSize = 5;
        private int curPageNo = 1;
        private uint _totalPagesCount;
        public StatusCodes status;
        #endregion

        #region Public properties
        public bool IsMobile { get; set; }
        #endregion

        #region Constructor
        public IndexPage(ICMSCacheContent Cache, IPager objPager, IUpcoming upcoming, IBikeModels<BikeModelEntity, int> bikeModels, IBikeMakesCacheRepository bikeMakesCacheRepository, IBikeModelsCacheRepository<int> models, IBikeSeries series)
            : base(bikeMakesCacheRepository, models, bikeModels, upcoming, series)
        {
            _Cache = Cache;
            _objPager = objPager;
            _upcoming = upcoming;
            _bikeModels = bikeModels;
            _bikeMakesCacheRepository = bikeMakesCacheRepository;
            _models = models;
            _series = series;
            ProcessQueryString();
        }
        #endregion

        #region Functions

        /// <summary>
        /// Created by : Aditi Srivastava on 3 Apr 2017
        /// Summary    : Process query string
        /// </summary>
        private void ProcessQueryString()
        {
            var request = HttpContext.Current.Request;
            var queryString = request != null ? request.QueryString : null;

            if (queryString != null)
            {

                if (!string.IsNullOrEmpty(queryString["pn"]))
                {
                    string _pageNo = queryString["pn"];
                    if (!string.IsNullOrEmpty(_pageNo))
                    {
                        int.TryParse(_pageNo, out curPageNo);
                    }
                }
            }
            GlobalCityAreaEntity currentCityArea = GlobalCityArea.GetGlobalCityArea();
            if (currentCityArea != null)
                CityId = currentCityArea.CityId;
        }

        /// <summary>
        /// Created By:- Subodh Jain 23 March 2017
        /// Summary:- Fetching data about feature article
        /// Modified by : Snehal Dange on 29th Nov 2017
        /// Descritpion : Added ga for page
        /// </summary>
        /// <returns></returns>
        public IndexFeatureVM GetData(int widgetTopCount)
        {
            IndexFeatureVM objIndex = new IndexFeatureVM();
            GetFeaturesList(objIndex);

            _totalPagesCount = (uint)_objPager.GetTotalPages((int)objIndex.TotalArticles, _pageSize);

            BindLinkPager(objIndex);
            BindPageMetas(objIndex);
            BindWidget(objIndex);
            CreatePrevNextUrl(objIndex);
            objIndex.Page = Entities.Pages.GAPages.Editorial_List_Page;
            return objIndex;
        }

        /// <summary>
        /// Written By : Subodh Jain 23 March 2017
        /// Summary    : Get features list
        /// </summary>
        public void GetFeaturesList(IndexFeatureVM objIndex)
        {
            try
            {
                int _startIndex = 0, _endIndex = 0;

                _objPager.GetStartEndIndex(_pageSize, curPageNo, out _startIndex, out _endIndex);

                IList<EnumCMSContentType> categorList = new List<EnumCMSContentType>();
                categorList.Add(EnumCMSContentType.Features);
                categorList.Add(EnumCMSContentType.SpecialFeature);
                string _featuresCategoryId = CommonApiOpn.GetContentTypesString(categorList);

                var _objFeaturesList = _Cache.GetArticlesByCategoryList(_featuresCategoryId, _startIndex, _endIndex, 0, 0);

                if (_objFeaturesList != null && _objFeaturesList.Articles.Count > 0)
                {

                    objIndex.ArticlesList = _objFeaturesList.Articles;
                    objIndex.TotalArticles = _objFeaturesList.RecordCount;
                    objIndex.StartIndex = (uint)_startIndex;
                    objIndex.EndIndex = (uint)_endIndex;
                    status = StatusCodes.ContentFound;
                }
                else
                {
                    status = StatusCodes.ContentNotFound;
                }
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, "Bikewale.Models.Features.IndexPage.GetFeaturesList");
            }
        }

        /// <summary>
        /// Created By : Subodh Jain 31 March 2017
        /// Summary    : Bind link pager
        /// </summary>
        private void BindLinkPager(IndexFeatureVM objIndex)
        {
            try
            {
                objIndex.PagerEntity = new PagerEntity();
                objIndex.PagerEntity.BaseUrl = (IsMobile ? "/m/features/" : "/features/");
                objIndex.PagerEntity.PageNo = curPageNo;
                objIndex.PagerEntity.PagerSlotSize = _pagerSlotSize;
                objIndex.PagerEntity.PageUrlType = "page/";
                objIndex.PagerEntity.TotalResults = (int)objIndex.TotalArticles;
                objIndex.PagerEntity.PageSize = _pageSize;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.Features.IndexPage.BindLinkPager");
            }
        }

        /// <summary>
        /// Created By:- Subodh Jain 23 March 2017
        /// Summary:- Fetching Meta Tags
        /// </summary>
        /// <returns></returns>
        private void BindPageMetas(IndexFeatureVM objPage)
        {

            try
            {
                objPage.PageMetaTags.Title = "Features - Stories, Specials & Travelogues | BikeWale";
                objPage.PageMetaTags.Description = "Features section of BikeWale brings specials, stories, travelogues and much more.";
                objPage.PageMetaTags.Keywords = "features, stories, travelogues, specials, drives.";
                objPage.PageMetaTags.CanonicalUrl = string.Format("{0}/features/{1}", BWConfiguration.Instance.BwHostUrl, (curPageNo > 1 ? string.Format("page/{0}/", curPageNo) : ""));
                objPage.PageMetaTags.AlternateUrl = string.Format("{0}/m/features/{1}", BWConfiguration.Instance.BwHostUrl, (curPageNo > 1 ? string.Format("page/{0}/", curPageNo) : ""));

                if (curPageNo > 1)
                {
                    objPage.PageMetaTags.Description = string.Format("Page {0} of {1} - {2}", curPageNo, _totalPagesCount, objPage.PageMetaTags.Description);
                    objPage.PageMetaTags.Title = string.Format("Page {0} of {1} - {2}", curPageNo, _totalPagesCount, objPage.PageMetaTags.Title);
                }
                SetBreadcrumList(objPage);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.Features.IndexPage.BindPageMetas");
            }
        }
        /// <summary>
        /// Created By : Subodh Jain 31 March 2017
        /// Summary    : Bind Widgets
        /// Modified by sajal Gupta on 05-12-2017
        /// Desc : Adderd multui tab widget
        /// Modified by : Sanskar Gupta on 22 Jan 2018
        /// Description : Added Newly Launched feature
        /// </summary>
        private void BindWidget(IndexFeatureVM objIndex)
        {
            try
            {
                SetAdditionalVariables();
                objIndex.PageWidgets = base.GetEditorialWidgetData(EnumEditorialPageType.Listing);
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, "Bikewale.Models.Features.IndexPage.BindWidget");

            }
        }

        /// <summary>
        /// Created By : Subodh Jain 31 March 2017
        /// Summary    : Bind previous and next url
        /// </summary>
        private void CreatePrevNextUrl(IndexFeatureVM objData)
        {
            string _mainUrl = String.Format("{0}{1}page/", BWConfiguration.Instance.BwHostUrl, objData.PagerEntity.BaseUrl);
            string prevPageNumber = string.Empty, nextPageNumber = string.Empty;
            int totalPages = _objPager.GetTotalPages((int)objData.TotalArticles, _pageSize);
            if (totalPages > 1)
            {
                if (curPageNo == 1)
                {
                    nextPageNumber = "2";
                    objData.PageMetaTags.NextPageUrl = string.Format("{0}{1}/", _mainUrl, nextPageNumber);
                }
                else if (curPageNo == totalPages)
                {
                    prevPageNumber = Convert.ToString(curPageNo - 1);
                    objData.PageMetaTags.PreviousPageUrl = string.Format("{0}{1}/", _mainUrl, prevPageNumber);
                }
                else
                {
                    prevPageNumber = Convert.ToString(curPageNo - 1);
                    objData.PageMetaTags.PreviousPageUrl = string.Format("{0}{1}/", _mainUrl, prevPageNumber);
                    nextPageNumber = Convert.ToString(curPageNo + 1);
                    objData.PageMetaTags.NextPageUrl = string.Format("{0}{1}/", _mainUrl, nextPageNumber);
                }
            }
        }

        /// <summary>
        /// Created By :Snehal Dange on 8th Nov 2017
        /// Description : Function to create page level schema for breadcrum
        /// </summary>
        private void SetBreadcrumList(IndexFeatureVM objIndex)
        {
            IList<BreadcrumbListItem> BreadCrumbs = new List<BreadcrumbListItem>();
            string bikeUrl;
            bikeUrl = string.Format("{0}/", Utility.BWConfiguration.Instance.BwHostUrl);
            ushort position = 1;
            if (IsMobile)
            {
                bikeUrl += "m/";
            }

            BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, bikeUrl, "Home"));

            BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position, null, "Features"));

            objIndex.BreadcrumbList.BreadcrumListItem = BreadCrumbs;

        }

        /// <summary>
        /// Created by  :   Sumit Kate on 26 Apr 2018
        /// Description :   Set basic flags to get the editorial widgets
        /// </summary>        
        private void SetAdditionalVariables()
        {
            EditorialWidgetEntity editorialWidgetData = new EditorialWidgetEntity
            {
                IsMobile = IsMobile,
                CityId = CityId
            };
            base.SetAdditionalData(editorialWidgetData);
        }
        #endregion
    }
}