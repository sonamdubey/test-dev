using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS;
using Bikewale.Entities.Location;
using Bikewale.Entities.Pager;
using Bikewale.Entities.PriceQuote;
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
    /// Created by : Aditi Srivastava on 31 Mar 2017
    /// Summary    : Model to fetch data for Bike care listing page
    /// </summary>
    public class BikeCareIndexPage
    {
        #region Variables for dependency injection
        private readonly ICMSCacheContent _cmsCache = null;
        private readonly IPager _pager;
        private readonly IUpcoming _upcoming = null;
        private readonly IBikeModels<BikeModelEntity, int> _bikeModels = null;
        #endregion

        #region Page level variables
        private const int pageSize = 10, pagerSlotSize = 5;
        private uint CityId, pageCatId = 0;
        private int curPageNo = 1;
        public string redirectUrl;
        public StatusCodes status;
        private GlobalCityAreaEntity currentCityArea;
        private EnumBikeType bikeType = EnumBikeType.All;
        private bool showCheckOnRoadCTA = false;
        private PQSourceEnum pqSource = 0;
        #endregion

        #region Public members
        public bool IsMobile { get; set; }
        #endregion

        #region Constructor
        public BikeCareIndexPage(ICMSCacheContent cmsCache, IPager pager, IUpcoming upcoming, IBikeModels<BikeModelEntity, int> bikeModels)
        {
            _cmsCache = cmsCache;
            _pager = pager;
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
        /// Created by : Aditi Srivastava on 31 Mar 2017
        /// Summary    : Populate page view model 
        /// </summary>
        public BikeCareIndexPageVM GetData(int widgetTopCount)
        {
            BikeCareIndexPageVM objData = new BikeCareIndexPageVM();
            try
            {
                int _startIndex = 0, _endIndex = 0;
                _pager.GetStartEndIndex(pageSize, curPageNo, out _startIndex, out _endIndex);

                objData.StartIndex = _startIndex;
                objData.EndIndex = _endIndex;

                IList<EnumCMSContentType> categorList = new List<EnumCMSContentType>();
                categorList.Add(EnumCMSContentType.TipsAndAdvices);
                string _contentType = CommonApiOpn.GetContentTypesString(categorList);


                objData.Articles = _cmsCache.GetArticlesByCategoryList(_contentType, _startIndex, _endIndex, 0, 0);
                if (objData.Articles != null && objData.Articles.RecordCount > 0)
                {
                    status = StatusCodes.ContentFound;
                    objData.StartIndex = _startIndex;
                    objData.EndIndex = _endIndex > objData.Articles.RecordCount ? Convert.ToInt32(objData.Articles.RecordCount) : _endIndex;
                    BindLinkPager(objData);
                    SetPageMetas(objData);
                    CreatePrevNextUrl(objData);
                    GetWidgetData(objData,widgetTopCount);
                }
                else
                    status = StatusCodes.ContentNotFound;
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.BikeCareIndexPage.GetData");
            }
            return objData;
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 1 Apr 2017
        /// Summary    : Bind pager
        /// </summary>
        private void BindLinkPager(BikeCareIndexPageVM objData)
        {
            try
            {
                objData.PagerEntity = new PagerEntity();
                objData.PagerEntity.BaseUrl = (IsMobile ? "/m/bike-care/" : "/bike-care/");
                objData.PagerEntity.PageNo = curPageNo;
                objData.PagerEntity.PagerSlotSize = pagerSlotSize;
                objData.PagerEntity.PageUrlType = "page/";
                objData.PagerEntity.TotalResults = (int)objData.Articles.RecordCount;
                objData.PagerEntity.PageSize = pageSize;
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.BikeCareIndexPage.BindLinkPager");
            }
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 1 Apr 2017
        /// Summary    : Set page metas
        /// </summary>
        private void SetPageMetas(BikeCareIndexPageVM objData)
        {
            try
            {
                objData.PageMetaTags.Title = string.Format("Bike Care | Maintenance Tips from Bike Experts - BikeWale");
                objData.PageMetaTags.Description = string.Format("BikeWale brings you maintenance tips from the bike experts to help you keep your bike in good shape. Read through these maintenance tips to learn more about your bike maintenance");
                objData.PageMetaTags.Keywords = string.Format("Bike maintenance, bike common issues, bike common problems, Maintaining bikes, bike care");
                objData.PageMetaTags.CanonicalUrl = string.Format("{0}/bike-care/{1}", BWConfiguration.Instance.BwHostUrl, (curPageNo > 1 ? string.Format("page/{0}/", curPageNo) : ""));
                objData.PageMetaTags.AlternateUrl = string.Format("{0}/m/bike-care/{1}", BWConfiguration.Instance.BwHostUrl, (curPageNo > 1 ? string.Format("page/{0}/", curPageNo) : ""));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.BikeCareIndexPage.SetPageMetas"); 
            }
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 1 Apr 2017
        /// Summary    : Create prev and next page urls for SEO
        /// </summary>
        private void CreatePrevNextUrl(BikeCareIndexPageVM objData)
        {
            try
            {
                string _mainUrl = String.Format("{0}{1}page/", BWConfiguration.Instance.BwHostUrl, objData.PagerEntity.BaseUrl);
                string prevPageNumber = string.Empty, nextPageNumber = string.Empty;
                int totalPages = _pager.GetTotalPages((int)objData.Articles.RecordCount, pageSize);
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
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.BikeCareIndexPage.CreatePrevNextUrl");
            }
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 1 Apr 2017
        /// Summary    : Get data to populate widget view model
        /// </summary>
        private void GetWidgetData(BikeCareIndexPageVM objData,int topCount)
        {
            try
            {
                currentCityArea = GlobalCityArea.GetGlobalCityArea();
                if (currentCityArea != null)
                    CityId = currentCityArea.CityId;

                MostPopularBikesWidget objPopularBikes = new MostPopularBikesWidget(_bikeModels, bikeType, showCheckOnRoadCTA, false, pqSource, pageCatId, 0);
                objPopularBikes.TopCount = topCount;
                objPopularBikes.CityId = CityId;
                objData.MostPopularBikes = objPopularBikes.GetData();
                objData.MostPopularBikes.WidgetHeading = "Popular bikes";
                objData.MostPopularBikes.WidgetHref = "/best-bikes-in-india/";
                objData.MostPopularBikes.WidgetLinkTitle = "Best Bikes in India";


                UpcomingBikesWidget objUpcomingBikes = new UpcomingBikesWidget(_upcoming);
                objUpcomingBikes.Filters = new UpcomingBikesListInputEntity();
                objUpcomingBikes.Filters.PageNo = 1;
                objUpcomingBikes.Filters.PageSize = topCount;
                objUpcomingBikes.SortBy = EnumUpcomingBikesFilter.Default;
                objData.UpcomingBikes = objUpcomingBikes.GetData();
                objData.UpcomingBikes.WidgetHeading = "Upcoming bikes";
                objData.UpcomingBikes.WidgetHref = "/upcoming-bikes/";
                objData.UpcomingBikes.WidgetLinkTitle = "Upcoming Bikes in India";
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.BikeCareIndexPage.GetWidgetData");
            }
        }
        #endregion

    }
}