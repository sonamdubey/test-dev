using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Entities.Pager;
using Bikewale.Entities.PriceQuote;
using Bikewale.Entities.Schema;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Pager;
using Bikewale.Models.Scooters;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Web;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by: Vivek Singh Tomar on 17th Aug 2017
    /// Summary: Model for expert reviews for scooters landing page
    /// </summary>
    public class ScooterExpertReviewsPage
    {
        #region Variables for dependency injection
        private readonly ICMSCacheContent _cmsCache = null;
        private readonly IPager _pager;
        private readonly IBikeModels<BikeModelEntity, int> _bikeModels = null;
        private readonly IBikeMakesCacheRepository _bikeMakesCacheRepository = null;
        #endregion

        #region Page level variables
        private const int pageSize = 10, pagerSlotSize = 5;
        private int curPageNo = 1;
        private uint _totalPagesCount;
        string make = string.Empty;
        private uint MakeId, CityId, pageCatId = 0;
        public string redirectUrl { get; set; }
        public StatusCodes status { get; set; }
        private GlobalCityAreaEntity currentCityArea;
        private BikeMakeEntityBase objMake = null;
        private EnumBikeType bikeType = EnumBikeType.Scooters;
        private bool showCheckOnRoadCTA = false;
        private PQSourceEnum pqSource = 0;
        private int widgetTopCount = 4;
        #endregion

        #region Public properties
        public bool IsMobile { get; set; }
        #endregion

        #region Constructor
        public ScooterExpertReviewsPage(ICMSCacheContent cmsCache, IPager pager, IBikeModelsCacheRepository<int> models, IBikeModels<BikeModelEntity, int> bikeModels, IBikeMakesCacheRepository bikeMakesCacheRepository)
        {
            _cmsCache = cmsCache;
            _pager = pager;
            _bikeModels = bikeModels;
            _bikeMakesCacheRepository = bikeMakesCacheRepository;
            ProcessQueryString();
        }
        #endregion

        #region Functions
        /// <summary>
        /// Created by : Vivek Singh Tomar on 17th Aug 2017
        /// Summary    : Function to get the expert reviews landing page data
        /// </summary>
        public ScooterExpertReviewsPageVM GetData()
        {
            ScooterExpertReviewsPageVM objData = new ScooterExpertReviewsPageVM();

            try
            {
                if (objMake != null)
                    objData.Make = objMake;

                int _startIndex = 0, _endIndex = 0;
                _pager.GetStartEndIndex(pageSize, curPageNo, out _startIndex, out _endIndex);

                objData.StartIndex = _startIndex;
                objData.EndIndex = _endIndex;

                objData.Articles = _cmsCache.GetArticlesByCategoryList(Convert.ToString((int)EnumCMSContentType.RoadTest), _startIndex, _endIndex, Convert.ToString((int)EnumBikeBodyStyles.Scooter), (int)MakeId);

                _totalPagesCount = (uint)_pager.GetTotalPages((int)objData.Articles.RecordCount, pageSize);

                if (objData.Articles != null && objData.Articles.RecordCount > 0)
                {
                    status = StatusCodes.ContentFound;
                    objData.StartIndex = _startIndex;
                    objData.EndIndex = _endIndex > objData.Articles.RecordCount ? Convert.ToInt32(objData.Articles.RecordCount) : _endIndex;
                    BindLinkPager(objData);
                    SetPageMetas(objData);
                    CreatePrevNextUrl(objData);
                    SetBreadcrumList(objData);
                    GetWidgetData(objData, widgetTopCount);
                    objData.Page = Entities.Pages.GAPages.Editorial_List_Page;
                }
                else
                {
                    status = StatusCodes.ContentNotFound;
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "Bikewale.Models.ScooterExpertReviewsPage.GetData");
            }

            return objData;
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 17th Aug 2017
        /// Summary    : Process query string for expert reviews page
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
                make = queryString["make"];

                ProcessMakeMaskingName(request, make);
            }
        }

        /// <summary>
        /// Created by  :  Vivek Singh Tomar on 17th Aug 2017
        /// Summary     :  Processes Make masking name
        /// </summary>
        private void ProcessMakeMaskingName(HttpRequest request, string make)
        {
            MakeMaskingResponse makeResponse = null;
            if (!string.IsNullOrEmpty(make))
            {
                makeResponse = _bikeMakesCacheRepository.GetMakeMaskingResponse(make);
            }
            if (makeResponse != null)
            {
                if (makeResponse.StatusCode == 200)
                {
                    MakeId = makeResponse.MakeId;
                    objMake = _bikeMakesCacheRepository.GetMakeDetails(MakeId);
                }
                else if (makeResponse.StatusCode == 301)
                {
                    status = StatusCodes.RedirectPermanent;
                    redirectUrl = request.RawUrl.Replace(make, makeResponse.MaskingName);
                }
                else
                {
                    status = StatusCodes.ContentNotFound;
                }
            }
        }

        /// <summary>
        /// Created by  :  Vivek Singh Tomar on 17th Aug 2017
        /// Summary     :  Set page metas and headings
        /// </summary>
        private void SetPageMetas(ScooterExpertReviewsPageVM objData)
        {
            objData.PageMetaTags.CanonicalUrl = string.Format("{0}{1}{2}", BWConfiguration.Instance.BwHostUrlForJs, UrlFormatter.FormatScootersExpertReviewUrl(make), (curPageNo > 1 ? string.Format("page/{0}/", curPageNo) : ""));
            objData.PageMetaTags.AlternateUrl = string.Format("{0}/m{1}{2}", BWConfiguration.Instance.BwHostUrlForJs, UrlFormatter.FormatScootersExpertReviewUrl(make), (curPageNo > 1 ? string.Format("page/{0}/", curPageNo) : ""));
            if (MakeId > 0)
            {
                objData.PageMetaTags.Title = string.Format("{0} Scooter Expert Reviews | Reviews on {0} scooters - BikeWale", objMake.MakeName);
                objData.PageMetaTags.Description = string.Format("Read reviews on {0} scooters from BikeWale experts. Know more about comparison tests and first ride reviews of {0} scooters on BikeWale", objMake.MakeName);
                objData.PageMetaTags.Keywords = string.Format("{0} Scooter expert reviews, {0} scooter reviews, {0} scooty expert reviews, {0} scooty reviews", objMake.MakeName);
                objData.PageH1 = string.Format("{0} Scooters Expert Reviews", objMake.MakeName);
                objData.AdTags.TargetedMakes = objMake.MakeName;
            }
            else
            {
                objData.PageMetaTags.Title = " Scooter Expert Reviews | Comparison Test & First Ride Reviews on scooters - BikeWale";
                objData.PageMetaTags.Description = " Read reviews on scooters from BikeWale experts. Know more about comparison tests and first ride reviews about scooters";
                objData.PageMetaTags.Keywords = "scooter reviews, scooty reviews, auto experts, expert reviews scooters, scooty expert reviews";
                objData.PageH1 = string.Format("Expert Reviews on Scooters");
            }
            if (!IsMobile)
            {
                objData.PageMetaTags.FBTitle = objData.PageMetaTags.Title;
                objData.PageMetaTags.FBImage = BWConfiguration.Instance.BikeWaleLogo;
            }

            if (curPageNo > 1)
            {
                objData.PageMetaTags.Description = string.Format("Page {0} of {1} - {2}", curPageNo, _totalPagesCount, objData.PageMetaTags.Description);
                objData.PageMetaTags.Title = string.Format("Page {0} of {1} - {2}", curPageNo, _totalPagesCount, objData.PageMetaTags.Title);
            }
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 17th Aug 2017
        /// Summary    : Get view model for page widgets
        /// Modified by Sajal Gupta on 24-08-2017
        /// description : Added Popular Scooter Brands widget
        /// </summary>
        private void GetWidgetData(ScooterExpertReviewsPageVM objData, int topCount)
        {
            try
            {
                currentCityArea = GlobalCityArea.GetGlobalCityArea();
                if (currentCityArea != null)
                    CityId = currentCityArea.CityId;

                MostPopularBikesWidget objPopularBikes = new MostPopularBikesWidget(_bikeModels, bikeType, showCheckOnRoadCTA, false, pqSource, pageCatId, MakeId);
                objPopularBikes.TopCount = topCount;
                objPopularBikes.CityId = CityId;
                objData.MostPopularScooters = objPopularBikes.GetData();
                if (MakeId > 0 && objMake != null)
                {
                    objData.MostPopularScooters.WidgetHeading = string.Format("Popular {0} Scooters", objMake.MakeName);
                    objData.MostPopularScooters.WidgetHref = string.Format("/{0}-scooters/", objMake.MaskingName);
                    objData.MostPopularScooters.WidgetLinkTitle = string.Format("{0} Scooters", objMake.MakeName);
                }
                else
                {
                    objData.MostPopularScooters.WidgetHeading = "Popular Scooters";
                    objData.MostPopularScooters.WidgetHref = "/best-scooters-in-india/";
                    objData.MostPopularScooters.WidgetLinkTitle = "Best Scooters in India";
                }

                PopularScooterBrandsWidget objPopularScooterBrands = new PopularScooterBrandsWidget(_bikeMakesCacheRepository);
                objPopularScooterBrands.TopCount = 4;

                if (MakeId > 0)
                {
                    objPopularScooterBrands.SkipMakeId = MakeId;
                    objData.PopularScooterBrandsWidgetHeading = "Other scooter brands";
                }
                else
                    objData.PopularScooterBrandsWidgetHeading = "Popular scooter brands";

                objData.PopularScooterMakesWidget = objPopularScooterBrands.GetData();
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "Bikewale.Models.ScooterExpertReviewsPage.GetWidgetData");
            }
        }

        /// <summary>
        /// Created By : Vivek Singh Tomar on 17th Aug 2017
        /// Summary    : Bind link pager
        /// </summary>
        private void BindLinkPager(ScooterExpertReviewsPageVM objData)
        {
            try
            {
                objData.PagerEntity = new PagerEntity();
                objData.PagerEntity.BaseUrl = string.Format("{0}{1}", (IsMobile ? "/m" : ""), UrlFormatter.FormatScootersExpertReviewUrl(make));
                objData.PagerEntity.PageNo = curPageNo;
                objData.PagerEntity.PagerSlotSize = pagerSlotSize;
                objData.PagerEntity.PageUrlType = "page/";
                objData.PagerEntity.TotalResults = (int)objData.Articles.RecordCount;
                objData.PagerEntity.PageSize = pageSize;
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "Bikewale.Models.ScooterExpertReviewsPage.BindLinkPager");
            }
        }
        /// <summary>
        /// Created By : Vivek Singh Tomar on 17th Aug 2017
        /// Summary    : Create previous and next page urls
        /// </summary>
        /// <param name="objData"></param>
        private void CreatePrevNextUrl(ScooterExpertReviewsPageVM objData)
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

        /// <summary>
        /// Created By : Snehal Dange on 10th Nov 2017
        /// Description : Function to create page level schema for breadcrum
        /// </summary>
        private void SetBreadcrumList(ScooterExpertReviewsPageVM objPageVM)
        {
            try
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


                BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, string.Format("{0}scooters/", bikeUrl), "Scooters"));

                if (objPageVM.Make != null && objPageVM.Make.MakeId > 0)
                {
                    bikeUrl = string.Format("{0}{1}-scooters/", bikeUrl, objPageVM.Make.MaskingName);
                    BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position, bikeUrl, string.Format("{0} Scooters", objPageVM.Make.MakeName)));
                }
                BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position, null, "Expert Reviews"));

                if (objPageVM != null)
                {
                    objPageVM.BreadcrumbList.BreadcrumListItem = BreadCrumbs;
                }

            }
            catch (Exception ex)
            {

                Bikewale.Notifications.ErrorClass.LogError(ex, "Bikewale.Models.Videos.ScooterVideos.SetBreadcrumList()");
            }
        #endregion
        }
    }
}