using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS;
using Bikewale.Entities.Location;
using Bikewale.Entities.Pager;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Pager;
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
    public class IndexPage
    {
        #region Variables for dependency injection
        private readonly ICMSCacheContent _Cache = null;
        private readonly IPager _objPager = null;
        private readonly IUpcoming _upcoming = null;
        private readonly IBikeModels<BikeModelEntity, int> _bikeModels = null;
        #endregion

        #region Page level variables
        private const int _pageSize = 10, _pagerSlotSize = 5;
        private int curPageNo = 1;
        public StatusCodes status;
        #endregion

        #region Public properties
        public bool IsMobile { get; set; }
        #endregion

        #region Constructor
        public IndexPage(ICMSCacheContent Cache, IPager objPager, IUpcoming upcoming, IBikeModels<BikeModelEntity, int> bikeModels)
        {
            _Cache = Cache;
            _objPager = objPager;
            _upcoming = upcoming;
            _bikeModels = bikeModels;
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
        }

        /// <summary>
        /// Created By:- Subodh Jain 23 March 2017
        /// Summary:- Fetching data about feature article
        /// </summary>
        /// <returns></returns>
        public IndexFeatureVM GetData(int widgetTopCount)
        {
            IndexFeatureVM objIndex = new IndexFeatureVM();
            GetFeaturesList(objIndex);
            BindLinkPager(objIndex);
            BindPageMetas(objIndex);
            BindWidget(objIndex, widgetTopCount);
            CreatePrevNextUrl(objIndex);
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
                ErrorClass objErr = new ErrorClass(err, "Bikewale.Models.Features.IndexPage.GetFeaturesList");
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
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.Features.IndexPage.BindLinkPager");
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
                objPage.PageMetaTags.Title = string.Format("Features - Stories, Specials & Travelogues | BikeWale");
                objPage.PageMetaTags.Description = string.Format("Features section of BikeWale brings specials, stories, travelogues and much more.");
                objPage.PageMetaTags.Keywords = string.Format("features, stories, travelogues, specials, drives.");
                objPage.PageMetaTags.CanonicalUrl = string.Format("{0}/features/{1}", BWConfiguration.Instance.BwHostUrl, (curPageNo > 1 ? string.Format("page/{0}/", curPageNo) : ""));
                objPage.PageMetaTags.AlternateUrl = string.Format("{0}/m/features/{1}", BWConfiguration.Instance.BwHostUrl, (curPageNo > 1 ? string.Format("page/{0}/", curPageNo) : ""));
           
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.Features.IndexPage.BindPageMetas");
            }
        }
        /// <summary>
        /// Created By : Subodh Jain 31 March 2017
        /// Summary    : Bind Widgets
        /// </summary>
        private void BindWidget(IndexFeatureVM objIndex,int topCount)
        {
            try
            {
                GlobalCityAreaEntity currentCityArea = GlobalCityArea.GetGlobalCityArea();
                uint CityId = 0;
                if (currentCityArea != null)
                    CityId = currentCityArea.CityId;
                UpcomingBikesWidget objUpcomingBikes = new UpcomingBikesWidget(_upcoming);
                objUpcomingBikes.Filters = new UpcomingBikesListInputEntity();
                objUpcomingBikes.Filters.StartIndex = 1;
                objUpcomingBikes.Filters.EndIndex = topCount;
                objUpcomingBikes.SortBy = EnumUpcomingBikesFilter.Default;

                objIndex.UpcomingBikes = objUpcomingBikes.GetData();


                objIndex.UpcomingBikes.WidgetHeading = "Upcoming bikes";
                objIndex.UpcomingBikes.WidgetHref = "/upcoming-bikes/";
                objIndex.UpcomingBikes.WidgetLinkTitle = "Upcoming Bikes in India";

                MostPopularBikesWidget objPopularBikes = new MostPopularBikesWidget(_bikeModels, EnumBikeType.All, false, false);
                objPopularBikes.TopCount = topCount;
                objPopularBikes.CityId = CityId;


                objIndex.MostPopularBikes = objPopularBikes.GetData();
                objIndex.MostPopularBikes.WidgetHeading = "Popular bikes";
                objIndex.MostPopularBikes.WidgetHref = "/best-bikes-in-india/";
                objIndex.MostPopularBikes.WidgetLinkTitle = "Best Bikes in India";
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "Bikewale.Models.Features.IndexPage.BindWidget");

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
                if (curPageNo == 1 )
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
        #endregion
    }
}