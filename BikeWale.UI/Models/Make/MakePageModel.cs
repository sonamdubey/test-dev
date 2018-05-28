using Bikewale.BAL.ApiGateway.Entities.BikeData;
using Bikewale.Common;
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.BikeData.NewLaunched;
using Bikewale.Entities.CMS;
using Bikewale.Entities.Compare;
using Bikewale.Entities.Filters;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Entities.Pages;
using Bikewale.Entities.Schema;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.NewLaunched;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Compare;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.Filters;
using Bikewale.Interfaces.ServiceCenter;
using Bikewale.Interfaces.Used;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Interfaces.Videos;
using Bikewale.Models.BikeSeries;
using Bikewale.Models.CompareBikes;
using Bikewale.Models.Images;
using Bikewale.Models.Make;
using Bikewale.Models.BikeSeries;
using Bikewale.Models.UserReviews;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 27-Mar-2017
    /// Model for make page
    /// Modified by : Aditi Srivastava on 5 June 2017
    /// Summary     : Added BL instance instead of cache for comaprison carousel
    /// Modified by : Ashutosh Sharma on 29 Oct 2017
    /// Description : Added property IsAmpPage.
    /// Modified By :Snehal Dange on 21st Nov 2017
    /// Description: Added IUserReviewsCache _cacheUserReviews
    /// Modified By : Deepak Israni on 6th Feb 2018
    /// Description : Added TopCountNews property
    /// Modified by : Sanskar Gupta on 07 Feb 2018
    /// Description : Added INewBikeLaunchesBL
    /// </summary>
    public class MakePageModel
    {
        private readonly string _makeMaskingName;
        private uint _makeId;
        private readonly IBikeModelsCacheRepository<int> _bikeModelsCache;
        private readonly IBikeMakesCacheRepository _bikeMakesCache;
        private readonly ICMSCacheContent _articles = null;
        private readonly ICMSCacheContent _expertReviews = null;
        private readonly IVideos _videos = null;
        private readonly IUsedBikeDetailsCacheRepository _cachedBikeDetails = null;
        private readonly IBikeModels<BikeModelEntity, int> _objModelEntity = null;
        private readonly IDealerCacheRepository _cacheDealers = null;
        private readonly IUpcoming _upcoming = null;
        private readonly IBikeCompare _compareBikes = null;
        private readonly IServiceCenter _objSC;
        private readonly IUserReviewsCache _cacheUserReviews;
        private readonly INewBikeLaunchesBL _newLaunchesBL;
        private readonly IPageFilters _pageFilters;
        private readonly IBikeSeries _bikeSeries;
        private uint _makeCategoryId;
        private bool _newMakePageV1Status;
        private bool _oldMakePageV1Status;
        public StatusCodes Status { get; set; }
        public MakeMaskingResponse objResponse { get; set; }
        public string RedirectUrl { get; set; }
        public CompareSources CompareSource { get; set; }
        public bool IsMobile { get; set; }
        public bool IsAmpPage { get; set; }
        private CityEntityBase cityBase = null;
        public uint TopCountNews { get; set; }
        public uint TopCountExpertReviews { get; set; }

        private readonly String _adPath_Mobile_Old = "/1017752/Bikewale_Mobile_Make";
        private readonly String _adId_Mobile_Old = "1444028878952";
        private readonly String _adPath_Mobile_New = "/1017752/Bikewale_Mobile_Make";
        private readonly String _adId_Mobile_New = "1519729632700";
        private readonly String _adPath_Desktop = "/1017752/Bikewale_Make";
        private readonly String _adId_Desktop = "1516179232964";


        public MakePageModel(string makeMaskingName, IBikeModels<BikeModelEntity, int> objModelEntity, IBikeModelsCacheRepository<int> bikeModelsCache, IBikeMakesCacheRepository bikeMakesCache, ICMSCacheContent articles, ICMSCacheContent expertReviews, IVideos videos, IUsedBikeDetailsCacheRepository cachedBikeDetails, IDealerCacheRepository cacheDealers, IUpcoming upcoming, IBikeCompare compareBikes, IServiceCenter objSC, IUserReviewsCache cacheUserReviews, INewBikeLaunchesBL newLaunchesBL, IPageFilters pageFilters, IBikeSeries bikeSeries)
        {
            this._makeMaskingName = makeMaskingName;
            this._bikeModelsCache = bikeModelsCache;
            this._bikeMakesCache = bikeMakesCache;
            this._articles = articles;
            this._expertReviews = expertReviews;
            this._videos = videos;
            this._cachedBikeDetails = cachedBikeDetails;
            this._cacheDealers = cacheDealers;
            this._upcoming = upcoming;
            this._compareBikes = compareBikes;
            this._objSC = objSC;
            this._cacheUserReviews = cacheUserReviews;
            this._newLaunchesBL = newLaunchesBL;
            _objModelEntity = objModelEntity;
            this._pageFilters = pageFilters;
            ProcessQuery(this._makeMaskingName);
            this._bikeSeries = bikeSeries;
        }

        /// <summary>
        /// Gets the data for homepage
        /// Modified by : Ashutosh Sharma on 05 Oct 2017
        /// Description : Replaced call to method 'GetMostPopularBikesByMake' with 'GetMostPopularBikesByMakeWithCityPrice' to get city price when city is selected.
        /// Modified by : Vivek Singh Tomar on 11th Oct 2017
        /// Summary : Removed unnecessary arguments from BindDealerSserviceData which was required for fetchin service center details
        /// Modified by sajal Gupta on 06-11-2017
        /// Descriptition :  Chaged default sorting of bikes on page for particuaklar makes
        /// Modified by : Ashutosh Sharma on 27 Oct 2017
        /// Description : Added call to BindAmpJsTags.
        /// Modified by : Snehal Dange on 21st Nov 2017
        /// Description : Added BindUserReviews() method.
        /// Modified by: Snehal Dange on 23rd Nov 2017
        /// Description : Added BindMakeFooterCategoriesandPriceWidget() method
        /// Modified by sajal Gupta on 24-11-2017
        /// Descriptition :  Added BikeCityPopup
        /// Modified BY: Snehal Dange on 23rd Nov 2017
        /// Description: Added IsFooterDescriptionAvailable ,IsPriceListingAvailable checks
        /// Modified by : Snehal Dange on 17th Jan 2018
        /// Description: Added BindResearchMoreMakeWidget()
        /// Modified by: Deepak Israni on 30th Jan 2018
        /// Description: Removed ShowCheckOnRoadpriceBtn property
        /// </summary>         
        /// <returns>
        /// Created by : Sangram Nandkhile on 25-Mar-2017 
        /// Modified by : Rajan Chauhan on 3 Jan 2017
        /// Description : Bind MakeId to objData
        /// Modified by : Sanskar Gupta on 07 Feb 2018
        /// Descritpion : Added logic to fetch Newly Launched Bikes (within a period of 10 days) for Mobile Make page.
        /// Modified by : Snehal Dange on 20th Feb 2018
        /// Description : Added BindPageFilters();
        /// Modified by : Sanskar Gupta on 12 March 2018
        /// Description : Added `BindEMICalculator()`
        /// </returns>
        public MakePageVM GetData()
        {
            MakePageVM objData = new MakePageVM();

            try
            {
                #region Variable initialization

                uint cityId = 0;
                string cityName = string.Empty, cityMaskingName = string.Empty;

                GlobalCityAreaEntity location = GlobalCityArea.GetGlobalCityArea();
                objData.City = location;
                if (location != null && location.CityId > 0)
                {
                    cityId = location.CityId;
                    cityName = location.City;
                    var cityEntity = new CityHelper().GetCityById(cityId);
                    cityMaskingName = cityEntity != null ? cityEntity.CityMaskingName : string.Empty;
                    objData.Location = cityName;
                    objData.LocationMasking = cityMaskingName;
                    cityBase = new CityEntityBase()
                    {
                        CityId = cityId,
                        CityMaskingName = cityMaskingName,
                        CityName = cityName
                    };
                }
                else
                {
                    objData.Location = "India";
                    objData.LocationMasking = "india";
                }

                #endregion

                objData.Bikes = _objModelEntity.GetMostPopularBikesByMakeWithCityPrice((int)_makeId, cityId);

                if (objData.Bikes != null && objData.Bikes.Count() > 5)
                {
                    objData.TopPopularBikes = objData.Bikes.OrderBy(x => x.BikePopularityIndex).Take(4);
                }

                BikeMakeEntityBase makeBase = _bikeMakesCache.GetMakeDetails(_makeId);
                objData.BikeDescription = _bikeMakesCache.GetMakeDescription(_makeId);
                objData.SelectedSortingId = 0;
                objData.SelectedSortingText = "Price: Low to High";

                if (makeBase != null)
                {
                    objData.MakeMaskingName = makeBase.MaskingName;
                    objData.MakeName = makeBase.MakeName;
                    objData.MakeId = makeBase.MakeId;
                }

                if (!string.IsNullOrEmpty(BWConfiguration.Instance.PopularityOrderForMake) && BWConfiguration.Instance.PopularityOrderForMake.Split(',').Contains(_makeId.ToString()))
                {
                    objData.Bikes = objData.Bikes.OrderBy(x => x.BikePopularityIndex);
                    objData.SelectedSortingId = 1;
                    objData.SelectedSortingText = "Popular";
                }
                
                BindUpcomingBikes(objData);
                BindPageMetaTags(objData, objData.Bikes, makeBase);
                BindCompareBikes(objData, CompareSource, cityId);
                BindDealerServiceData(objData, cityId);
                BindCMSContent(objData);
                objData.UsedModels = BindUsedBikeByModel(_makeId, cityId);
                BindDiscontinuedBikes(objData);
                BindOtherMakes(objData);
                BindUserReviews(objData);
                BindMakeFooterCategoriesandPriceWidget(objData);
                BindEMICalculator(objData);
                objData.Page = GAPages.Make_Page;
                objData.BikeCityPopup = new PopUp.BikeCityPopup()
                {
                    ApiUrl = "/api/v2/DealerCity/?makeId=" + _makeId,
                    PopupShowButtonMessage = "Show showrooms",
                    PopupSubHeading = "See Showrooms in your city!",
                    FetchDataPopupMessage = "Fetching showrooms for ",
                    RedirectUrl = string.Format("/dealer-showrooms/{0}/", _makeMaskingName),
                    IsCityWrapperPresent = 1
                };
                BindModelPhotos(objData);
                BindShowroomPopularCityWidget(objData);
                BindResearchMoreMakeWidget(objData);
                GetEMIDetails(objData);
                BindExpertReviewCount(objData.ExpertReviews);
                BindSeriesLinkages(objData, cityId);

                if (!IsMobile)
                {
                    BindAdSlots(objData);
                }

                if (objData.Bikes != null && objData.Bikes.Count() > 6)
                {
                    BindPageFilters(objData);
                }
                BindNewBikeSearchPopupData(objData);
                #region Set Visible flags

                if (objData != null)
                {
                    objData.IsUpComingBikesAvailable = objData.UpcomingBikes != null && objData.UpcomingBikes.UpcomingBikes != null && objData.UpcomingBikes.UpcomingBikes.Any();
                    objData.IsNewsAvailable = objData.News != null && objData.News.ArticlesList != null && objData.News.ArticlesList.Any();
                    objData.IsExpertReviewsAvailable = objData.ExpertReviews != null && objData.ExpertReviews.ArticlesList != null && objData.ExpertReviews.ArticlesList.Any();
                    objData.IsVideosAvailable = objData.Videos != null && objData.Videos.VideosList != null && objData.Videos.VideosList.Any();
                    objData.IsUsedModelsBikeAvailable = objData.UsedModels != null && objData.UsedModels.UsedBikeModelList != null && objData.UsedModels.UsedBikeModelList.Any();

                    objData.IsDealerAvailable = objData.Dealers != null && objData.Dealers.Dealers != null && objData.Dealers.Dealers.Any();
                    objData.IsDealerServiceDataAvailable = cityId > 0 && objData.IsDealerAvailable;
                    objData.IsDealerServiceDataInIndiaAvailable = cityId == 0 && objData.DealersServiceCenter != null && objData.DealersServiceCenter.DealerServiceCenters != null && objData.DealersServiceCenter.DealerServiceCenters.DealerDetails != null && objData.DealersServiceCenter.DealerServiceCenters.DealerDetails.Any();
                    objData.IsUserReviewsAvailable = (objData.PopularBikesUserReviews != null && objData.PopularBikesUserReviews.BikesReviewsList != null && objData.PopularBikesUserReviews.BikesReviewsList.Any() && objData.PopularBikesUserReviews.BikesReviewsList.FirstOrDefault().MostRecent != null);

                    objData.IsMakeTabsDataAvailable = (objData.BikeDescription != null && objData.BikeDescription.FullDescription.Length > 0 || objData.IsNewsAvailable ||
                        objData.IsExpertReviewsAvailable || objData.IsVideosAvailable || objData.IsUsedModelsBikeAvailable || objData.IsDealerServiceDataAvailable || objData.IsDealerServiceDataInIndiaAvailable || objData.IsUserReviewsAvailable);

                    objData.IsFooterDescriptionAvailable = objData.SubFooter != null && objData.SubFooter.FooterContent != null && objData.SubFooter.FooterContent.FooterDescription != null && objData.SubFooter.FooterContent.FooterDescription.Any();

                    objData.IsPriceListingAvailable = objData.IsFooterDescriptionAvailable && objData.SubFooter.FooterContent.ModelPriceList != null && objData.SubFooter.FooterContent.ModelPriceList.Any();

                }


                if (IsMobile)
                {
                    BindNewLaunchedWidget(objData);
                }

                if (IsAmpPage)
                {
                    BindAmpJsTags(objData);
                }
                #endregion
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("MakePageModel.GetData() => MakeId: {0}", _makeId));
            }

            return objData;
        }

        /// <summary>
        /// Modified by : Rajan Chauhan on 19 Feb 2018
        /// Description : Changed BikeModelPhotos to accept ImageWidgetVM
        /// </summary>
        /// <param name="objData"></param>
        private void BindModelPhotos(MakePageVM objData)
        {
            try
            {
                ImageCarausel imageCarausel = new ImageCarausel(_makeId, 6, 7, EnumBikeBodyStyles.AllBikes, _objModelEntity);
                objData.BikeModelsPhotos = imageCarausel.GetData();
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.Models.MakePhotosPage.BindModelPhotos : BindModelPhotos({0})", objData));
            }
        }
        /// <summary>
        /// Created by : Ashutosh Sharma on 27 Oct 2017
        /// Description : Method to bind required JS for AMP page.
        /// </summary>
        /// <param name="objData"></param>
        private void BindAmpJsTags(MakePageVM objData)
        {
            try
            {
                objData.AmpJsTags = new Entities.Models.AmpJsTags();
                objData.AmpJsTags.IsAccordion = true;
                objData.AmpJsTags.IsAd = true;
                objData.AmpJsTags.IsAnalytics = true;
                objData.AmpJsTags.IsBind = true;
                objData.AmpJsTags.IsCarousel = true;
                objData.AmpJsTags.IsSidebar = true;

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("BindAmpJsTags_{0}", objData));
            }
        }

        /// <summary>
        /// Created by Sajal on 24-11-2017
        /// Desc : Widget Bind Showroom Popular City 
        /// </summary>
        /// <param name="objMakePage"></param>
        private void BindShowroomPopularCityWidget(MakePageVM objMakePage)
        {
            DealersServiceCentersIndiaWidgetVM objData = new DealersServiceCentersIndiaWidgetVM();
            try
            {
                uint topCount = 8;
                objData.DealerServiceCenters = _cacheDealers.GetPopularCityDealer(_makeId, topCount);
                objData.MakeMaskingName = objMakePage.MakeMaskingName;
                objData.MakeName = objMakePage.MakeName;
                objData.CityCardTitle = "showrooms in";
                objData.CityCardLink = "dealer-showrooms";
                objData.IsServiceCenterPage = false;
                objMakePage.DealersServiceCenterPopularCities = objData;
                if (objData.DealerServiceCenters.DealerDetails.Any())
                {
                    objMakePage.DealersServiceCenterPopularCities.DealerServiceCenters.DealerDetails = objMakePage.DealersServiceCenterPopularCities.DealerServiceCenters.DealerDetails.Where(m => !m.CityId.Equals(cityBase != null ? cityBase.CityId : 0)).ToList();
                }
                objData.IsIndiaCardNeeded = true;
            }
            catch (System.Exception ex)
            {

                ErrorClass er = new ErrorClass(ex, "ServiceCenterDetailsPage.BindShowroomPopularCityWidget");
            }

        }

        /// <summary>
        /// Created by  :   Sumit Kate on 24 Aug 2017
        /// Description :   Bind Other Make list
        /// Modifiwed by Sajal Gupta on 15-11-2017
        /// Dewsc : Added makecategory sorting logic
        /// Modified by: Snehal Dange on 14th Dec 2017
        /// Desc: Added logic for cardtext and PageLinkFormat
        /// </summary>
        /// <param name="objData"></param>
        private void BindOtherMakes(MakePageVM objData)
        {
            try
            {
                IEnumerable<BikeMakeEntityBase> makes = _bikeMakesCache.GetMakesByType(EnumBikeType.New);

                var popularBrandsList = Utility.BikeFilter.FilterMakesByCategory(_makeId, makes);

                if (popularBrandsList != null && popularBrandsList.Any())
                {
                    var otherMakes = new OtherMakesVM();
                    otherMakes.Makes = popularBrandsList.Take(9);
                    objData.OtherMakes = otherMakes;

                    if (objData.OtherMakes != null)
                    {
                        objData.OtherMakes.CardText = "bike";
                        objData.OtherMakes.PageLinkFormat = "/{0}-bikes/";
                        objData.OtherMakes.PageTitleFormat = "{0} Bikes";
                    }


                }
                if (makes != null && _makeId > 0)
                {
                    BikeMakeEntityBase makeObj = makes.FirstOrDefault(x => x.MakeId == _makeId);
                    if (makeObj != null)
                    {
                        _makeCategoryId = makeObj.MakeCategoryId;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("MakePageModel.BindOtherMakes() => MakeId: {0}", _makeId));
            }
        }

        private UsedBikeModelsWidgetVM BindUsedBikeByModel(uint makeId, uint cityId)
        {
            UsedBikeModelsWidgetVM UsedBikeModel = new UsedBikeModelsWidgetVM();
            try
            {

                UsedBikeModelsWidgetModel objUsedBike = new UsedBikeModelsWidgetModel(9, _cachedBikeDetails);
                if (makeId > 0)
                    objUsedBike.makeId = makeId;
                if (cityId > 0)
                    objUsedBike.cityId = cityId;
                UsedBikeModel = objUsedBike.GetData();
            }
            catch (Exception ex)
            {

                ErrorClass.LogError(ex, "MakePageModel.BindUsedBikeByModel()");
            }

            return UsedBikeModel;

        }

        /// <summary>
        /// Modified by : Vivek Singh Tomar on 11th Oct 2017
        /// Summary : Removed call for getting service centers details
        /// </summary>
        /// <param name="objData"></param>
        /// <param name="cityId"></param>
        private void BindDealerServiceData(MakePageVM objData, uint cityId)
        {
            if (cityId > 0)
            {
                var dealerData = new DealerCardWidget(_cacheDealers, cityId, _makeId);
                dealerData.TopCount = 3;
                objData.Dealers = dealerData.GetData();
            }
            else
            {
                objData.DealersServiceCenter = new DealersServiceCentersIndiaWidgetModel(_makeId, objData.MakeName, _makeMaskingName, _cacheDealers).GetData();
            }
        }

        private void BindUpcomingBikes(MakePageVM objData)
        {
            UpcomingBikesWidget objUpcoming = new UpcomingBikesWidget(_upcoming);
            objUpcoming.Filters = new Bikewale.Entities.BikeData.UpcomingBikesListInputEntity()
            {
                PageSize = 9,
                PageNo = 1,
                MakeId = (int)this._makeId
            };
            objUpcoming.SortBy = Bikewale.Entities.BikeData.EnumUpcomingBikesFilter.Default;
            objData.UpcomingBikes = objUpcoming.GetData();
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 24 Apr 2017
        /// Summary  :  Function to bind popular comparison carousel
        /// Modified by : Aditi Srivastava on 27 Apr 2017
        /// Summary  : Added source for comparisons
        /// </summary>
        private void BindCompareBikes(MakePageVM objViewModel, CompareSources compareSource, uint cityId)
        {
            try
            {
                string versionList = string.Join(",", objViewModel.Bikes.OrderBy(m => m.BikePopularityIndex).Select(m => m.objVersion.VersionId).Take(9));
                PopularModelCompareWidget objCompare = new PopularModelCompareWidget(_compareBikes, 1, cityId, versionList);
                objViewModel.CompareSimilarBikes = objCompare.GetData();
                objViewModel.IsCompareBikesAvailable = (objViewModel.CompareSimilarBikes != null && objViewModel.CompareSimilarBikes.CompareBikes != null && objViewModel.CompareSimilarBikes.CompareBikes.Any());
                objViewModel.CompareSimilarBikes.CompareSource = compareSource;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "MakePageModel.BindCompareBikes");
            }
        }

        /// <summary>
        /// Modified By : Deepak Israni on 8th Feb 2018
        /// Description : Moved binding of Recent Expert Reviews to another function
        /// </summary>
        /// <param name="objData"></param>
        private void BindCMSContent(MakePageVM objData)
        {

            objData.News = new RecentNews(TopCountNews, _makeId, objData.MakeName, _makeMaskingName, string.Format("{0} News", objData.MakeName), _articles).GetData();

            BindRecentExpertReviews(objData);

            if (IsMobile)
            {
                objData.Videos = new RecentVideos(1, 2, _makeId, objData.MakeName, _makeMaskingName, _videos).GetData();
            }
            else
            {
                objData.Videos = new RecentVideos(1, 4, _makeId, objData.MakeName, _makeMaskingName, _videos).GetData();
            }

        }

        /// <summary>
        /// Created By : Deepak Israni on 8th Feb 2018
        /// Description : To bind the Recent Expert Reviews along with the type of reviews.
        /// </summary>
        /// <param name="objData"></param>
        private void BindRecentExpertReviews(MakePageVM objData)
        {
            RecentExpertReviews objExpertReviews = new RecentExpertReviews(TopCountExpertReviews, _makeId, objData.MakeName, _makeMaskingName, _expertReviews, string.Format("{0} Reviews", objData.MakeName));

            List<EnumCMSContentType> categoryList = new List<EnumCMSContentType>
                    {
                        EnumCMSContentType.RoadTest
                    };
            List<EnumCMSContentSubCategoryType> subCategoryList = new List<EnumCMSContentSubCategoryType>
                    {
                        EnumCMSContentSubCategoryType.Road_Test,
                        EnumCMSContentSubCategoryType.First_Drive,
                        EnumCMSContentSubCategoryType.Long_Term_Report
                    };
            objData.ExpertReviews = objExpertReviews.GetData(categoryList, subCategoryList);
        }


        private void BindDiscontinuedBikes(MakePageVM objData)
        {
            objData.DiscontinuedBikes = _bikeMakesCache.GetDiscontinuedBikeModelsByMake(_makeId);
            objData.IsDiscontinuedBikeAvailable = objData.DiscontinuedBikes != null && objData.DiscontinuedBikes.Any();

            if (objData.IsDiscontinuedBikeAvailable)
            {
                foreach (var bike in objData.DiscontinuedBikes)
                {
                    bike.Href = string.Format("/{0}-bikes/{1}/", _makeMaskingName, bike.ModelMasking);
                    bike.BikeName = string.Format("{0} {1}", objData.MakeName, bike.ModelName);
                }
            }
        }

        /// Modified by :- Subodh Jain 19 june 2017
        /// Summary :- Added Target Make
        /// Modified By : Sushil Kumar on 23rd Aug 2017
        /// Description : Added null check for min and max price
        /// Modified by : Snehal Dange on 11th Jan 2017
        /// Decription: Changed meta title and description
        private void BindPageMetaTags(MakePageVM objData, IEnumerable<MostPopularBikesBase> objModelList, BikeMakeEntityBase objMakeBase)
        {
            long minPrice = 0;
            long MaxPrice = 0;
            IEnumerable<MostPopularBikesBase> objTopModelList = null;
            IList<string> topBikeList = null;
            int topModelCount = 4;
            string topModelsName = null;
            try
            {
                topBikeList = new List<string>();
                if (objModelList != null && objModelList.Any())
                {
                    if (objModelList.Any())
                    {
                        minPrice = objModelList.Min(bike => bike.VersionPrice);
                        MaxPrice = objModelList.Max(bike => bike.VersionPrice);
                    }
                    if (objModelList.Count() < topModelCount)
                    {
                        topModelCount = objModelList.Count();
                    }

                    objTopModelList = objModelList.OrderBy(x => x.BikePopularityIndex).Take(topModelCount);

                    if (objTopModelList != null)
                    {
                        foreach (var objBike in objTopModelList.Take(topModelCount - 1))
                        {
                            topBikeList.Add(objBike.BikeName);
                        }

                        if (objTopModelList.Last() != null)
                        {
                            topModelsName = string.Format("{0} and {1}", string.Join(",", topBikeList), objTopModelList.Last().BikeName);
                        }
                    }

                }
                objData.PageMetaTags.Title = string.Format("{0} Bikes in India- {0} New Bikes Prices, Specs, & Images - BikeWale", objData.MakeName);

                objData.PageMetaTags.Description = string.Format("{0} has a total of {1} models. The top 4 {0} models are- {2}. BikeWale offers history, prices, specs, and images for all {0} models in India.{3}", objData.MakeName, objModelList.Count(), topModelsName, (objData.UpcomingBikes.UpcomingBikes.Count() > 0 ? string.Format("There are {0} {1} upcoming models as well.", objData.UpcomingBikes.UpcomingBikes.Count(), objData.MakeName) : ""));

                objData.PageMetaTags.CanonicalUrl = string.Format("{0}/{1}-bikes/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrl, _makeMaskingName);
                objData.PageMetaTags.AlternateUrl = string.Format("{0}/m/{1}-bikes/", BWConfiguration.Instance.BwHostUrl, _makeMaskingName);
                objData.PageMetaTags.AmpUrl = string.Format("{0}/m/{1}-bikes/amp/", BWConfiguration.Instance.BwHostUrl, _makeMaskingName);
                objData.PageMetaTags.Keywords = string.Format("{0}, {0} Bikes , {0} Bikes prices, {0} Bikes reviews, {0} Images, new {0} Bikes", objData.MakeName);
                objData.AdTags.TargetedMakes = objData.MakeName;
                objData.Page_H1 = string.Format("{0} Bikes", objData.MakeName);
                objData.ReturlUrl = string.Format("/m/{0}-bikes/", objData.MakeMaskingName);

                SetBreadcrumList(ref objData);

                CheckCustomPageMetas(objData, objMakeBase);

                SetPageJSONLDSchema(objData, objMakeBase);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "MakePageModel.BindPageMetaTags()");
            }

        }

        /// <summary>
        /// Created By  : Sushil Kumar on 6th Nov Sep 2017
        /// Description : Added breadcrum and webpage schema and added brand schema
        /// </summary>
        private void SetPageJSONLDSchema(MakePageVM objPageMeta, BikeMakeEntityBase objMakeBase)
        {
            WebPage webpage = SchemaHelper.GetWebpageSchema(objPageMeta.PageMetaTags, objPageMeta.BreadcrumbList);

            if (webpage != null)
            {
                if (objMakeBase != null)
                {
                    Brand brand = new Brand();
                    brand.Logo = Image.GetPathToShowImages(objMakeBase.LogoUrl, objMakeBase.HostUrl, "0x0");
                    brand.Url = string.Format("{0}/{1}-bikes/", BWConfiguration.Instance.BwHostUrl, objMakeBase.MaskingName);
                    brand.Name = objMakeBase.MakeName;
                    brand.Description = objPageMeta.BikeDescription != null ? objPageMeta.BikeDescription.SmallDescription : objPageMeta.PageMetaTags.Description;
                    objPageMeta.PageMetaTags.PageSchemaJSON = SchemaHelper.JsonSerialize(brand);
                }
                objPageMeta.PageMetaTags.SchemaJSON = SchemaHelper.JsonSerialize(webpage);
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar on 12th Sep 2017
        /// Description : Function to create page level schema for breadcrum
        /// Modified by : Snehal Dange on 27th Dec 2017
        /// Description: Added 'new bikes' in breadcrumb
        /// </summary>
        private void SetBreadcrumList(ref MakePageVM objData)
        {
            IList<BreadcrumbListItem> BreadCrumbs = new List<BreadcrumbListItem>();
            string url = string.Format("{0}/", Utility.BWConfiguration.Instance.BwHostUrl);
            ushort position = 1;
            if (IsMobile)
            {
                url += "m/";
            }

            BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, url, "Home"));
            url = string.Format("{0}new-bikes-in-india/", url);
            BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, url, "New Bikes"));
            BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position, null, objData.Page_H1));


            objData.BreadcrumbList.BreadcrumListItem = BreadCrumbs;

        }


        /// <summary>
        /// Created By : Sushil Kumar on 13th Aug 2017
        /// Description : Function to check and set custom page metas and summary for the page
        /// </summary>
        /// <param name="objData"></param>
        /// <param name="objMakeBase"></param>
        private void CheckCustomPageMetas(MakePageVM objData, BikeMakeEntityBase objMakeBase)
        {
            try
            {
                if (objMakeBase != null && objMakeBase.Metas != null)
                {
                    var metas = objMakeBase.Metas.FirstOrDefault(m => m.PageId == (int)(IsMobile ? BikewalePages.Mobile_MakePage : BikewalePages.Desktop_MakePage));
                    if (metas != null)
                    {
                        if (!string.IsNullOrEmpty(metas.Title))
                        {
                            objData.PageMetaTags.Title = metas.Title;
                        }
                        if (!string.IsNullOrEmpty(metas.Description))
                        {
                            objData.PageMetaTags.Description = metas.Description;
                        }
                        if (!string.IsNullOrEmpty(metas.Keywords))
                        {
                            objData.PageMetaTags.Keywords = metas.Keywords;
                        }
                        if (!string.IsNullOrEmpty(metas.Heading))
                        {
                            objData.Page_H1 = metas.Heading;
                        }
                        if (!string.IsNullOrEmpty(metas.Summary))
                        {
                            if (objData.BikeDescription == null)
                            {
                                objData.BikeDescription = new BikeDescriptionEntity();
                            }

                            objData.BikeDescription.FullDescription = objData.BikeDescription.SmallDescription = metas.Summary;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("MakePageModel.CheckCustomPageMetas() makeId:{0}", _makeId));
            }
        }

        /// <summary>
        /// Created By:- Sangram Nandkhile on 29-Mar-2017 
        /// Summary:- Process the input query
        /// </summary>
        /// <returns></returns>
        private void ProcessQuery(string makeMaskingName)
        {
            try
            {
                objResponse = _bikeMakesCache.GetMakeMaskingResponse(makeMaskingName);
                if (objResponse != null)
                {
                    Status = (StatusCodes)objResponse.StatusCode;
                    if (objResponse.StatusCode == 200)
                    {
                        _makeId = objResponse.MakeId;
                    }
                    else if (objResponse.StatusCode == 301)
                    {
                        RedirectUrl = HttpContext.Current.Request.RawUrl.Replace(_makeMaskingName, objResponse.MaskingName);
                        Status = StatusCodes.RedirectPermanent;
                    }
                    else
                    {
                        Status = StatusCodes.ContentNotFound;
                    }
                }
                else
                {
                    Status = StatusCodes.ContentNotFound;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("MakePageModel.ProcessQuery() makeMaskingName:{0}", makeMaskingName));
            }
        }

        /// <summary>
        /// Created By:Snehal Dange on 20th Nov 2017
        /// Description: Get most recent and helpful reviews by make
        /// </summary>
        /// <param name="objData"></param>
        private void BindUserReviews(MakePageVM objData)
        {
            try
            {
                if (_makeId > 0 && objData != null && _cacheUserReviews != null)
                {
                    objData.PopularBikesUserReviews = new BikesWithReviewsByMakeVM();
                    if (objData.PopularBikesUserReviews != null)
                    {
                        objData.PopularBikesUserReviews.BikesReviewsList = _cacheUserReviews.GetBikesWithReviewsByMake(_makeId);
                        if (!IsMobile)
                        {
                            objData.PopularBikesUserReviews.BikesReviewsList = objData.PopularBikesUserReviews.BikesReviewsList.Take(4);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, string.Format("MakePageModel.BindUserReviews() makeId:{0}", _makeId));
            }
        }

        /// <summary>
        /// Created By: Snehal Dange on 23rd Nov 2017
        /// Description: Created BindMakeFooterCategoriesandPriceWidget() to bind SubFooter on make page
        /// </summary>
        /// <param name="objData"></param>
        private void BindMakeFooterCategoriesandPriceWidget(MakePageVM objData)
        {
            try
            {
                if (_makeId > 0 && objData != null && _bikeMakesCache != null)
                {
                    objData.SubFooter = new MakeFooterCategoriesandPriceVM();
                    if (objData.SubFooter != null)
                    {
                        objData.SubFooter.FooterContent = _bikeMakesCache.GetMakeFooterCategoriesandPrice(_makeId);
                        if (objData.SubFooter.FooterContent != null && objData.SubFooter.FooterContent.ModelPriceList != null && objData.SubFooter.FooterContent.ModelPriceList.Any())
                        {
                            objData.SubFooter.Make = objData.SubFooter.FooterContent.ModelPriceList.First().Make;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, string.Format("MakePageModel.BindMakeFooterCategoriesandPriceWidget() makeId:{0}", _makeId));
            }
        }

        /// <summary>
        /// Created by :  Snehal Dange on 17th Jan 2018
        /// Description: Method to bind research more about make widget data
        /// </summary>
        /// <param name="objData"></param>
        private void BindResearchMoreMakeWidget(MakePageVM objData)
        {

            try
            {
                if (_makeId > 0 && objData != null)
                {
                    objData.ResearchMoreMakeWidget = new ResearchMoreAboutMakeVM();

                    if (objData.ResearchMoreMakeWidget != null)
                    {
                        if (cityBase != null && cityBase.CityId > 0)
                        {
                            objData.ResearchMoreMakeWidget.WidgetObj = _bikeMakesCache.ResearchMoreAboutMakeByCity(_makeId, cityBase.CityId);
                            if (objData.ResearchMoreMakeWidget.WidgetObj != null)
                            {
                                objData.ResearchMoreMakeWidget.WidgetObj.City = cityBase;
                            }

                        }
                        else
                        {
                            objData.ResearchMoreMakeWidget.WidgetObj = _bikeMakesCache.ResearchMoreAboutMake(_makeId);
                        }
                        if (objData.ResearchMoreMakeWidget.WidgetObj != null)
                        {
                            objData.ResearchMoreMakeWidget.WidgetObj.ShowServiceCenterLink = true;
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, string.Format("MakePageModel.BindResearchMoreMakeWidget() makeId:{0} , cityId:{1}", _makeId, (cityBase != null ? cityBase.CityId.ToString() : "0")));
            }
        }

        /// <summary>
        /// Created by : Snehal Dange on 1st Feb 2018
        /// Descritpion: Method created to set emi details for bikelist models
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        private void GetEMIDetails(MakePageVM objData)
        {

            try
            {
                if (objData != null && objData.Bikes != null && objData.Bikes.Any())
                {
                    foreach (var bike in objData.Bikes)
                    {
                        if (bike != null)
                        {
                            if (bike.OnRoadPrice > 0)
                            {
                                bike.EMIDetails = EMICalculation.SetDefaultEMIDetails((uint)bike.OnRoadPrice);
                            }
                            else
                            {
                                bike.EMIDetails = EMICalculation.SetDefaultEMIDetails((uint)bike.OnRoadPriceMumbai);
                            }
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("MakePageModel.GetEMIDetails: Make : {0} , City : {1}", _makeId, cityBase.CityId));
            }

        }
        /// <summary>
        /// Created by : Sanskar Gupta on 09 Feb 2018
        /// Description : Method to bind NewLaunchedWidget
        /// </summary>
        /// <param name="objData"></param>
        private void BindNewLaunchedWidget(MakePageVM objData)
        {
            try
            {
                if (objData != null)
                {
                    InputFilter inputFilter = new InputFilter();
                    inputFilter.Days = 10;
                    inputFilter.Make = _makeId;

                    if (cityBase != null)
                    {
                        inputFilter.CityId = cityBase.CityId;
                    }

                    NewLaunchedWidgetVM NewLaunchedWidget = new NewLaunchedWidgetVM();
                    NewLaunchedWidget.Bikes = _newLaunchesBL.GetNewLaunchedBikesListByMakeAndDays(inputFilter);

                    objData.NewLaunchedWidget = NewLaunchedWidget;

                    if (objData.NewLaunchedWidget != null)
                    {
                        objData.NewLaunchedWidget.City = cityBase;
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("BindNewLaunchedWidget_MakeId_{0}_CityId_{1}", _makeId, cityBase.CityId));
            }
        }

        /// <summary>
        /// Created By : Deepak Israni on 9th Feb 2018
        /// Description : To bind the number of models with expert reviews and total number of expert reviews on VM
        /// </summary>
        /// <param name="objData"></param>
        private void BindExpertReviewCount(RecentExpertReviewsVM expertReviews)
        {
            try
            {
                ExpertReviewCountEntity ercEntity = _bikeMakesCache.GetExpertReviewCountByMake(_makeId);
                expertReviews.ModelCount = ercEntity.ModelCount;
                expertReviews.ExpertReviewCount = ercEntity.ExpertReviewCount;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("BindExpertReviewCount_MakeId_{0}", _makeId));
            }
        }
        /// <summary>
        /// Created by  : Sanskar Gupta on 12 March 2018
        /// Description : Function to initialize EMICalculator
        /// </summary>
        /// <param name="objMakePage"></param>
        private void BindEMICalculator(MakePageVM objMakePage)
        {
            objMakePage.EMICalculator = new EMICalculatorVM();
        }
        /// <summary>
        /// Created by : Snehal Dange on 20th Feb 2018
        /// Description: Method created to get relevant filters for a particular make (according to min values of budget,                           displacements and mileage)
        /// Modified by : Ashutosh Sharma on 29 Mar 2018
        /// Description: Made changes due to change in Specs features entites.
        /// </summary>
        /// <param name="objData"></param>
        private void BindPageFilters(MakePageVM objData)
        {
            try
            {
                CustomInputFilters objInputFilters = null;
                objData.PageFilters = new FilterPageEntity();
                objInputFilters = new CustomInputFilters();
                if (objData.Bikes != null)
                {
                    float minDisplacement = Single.MaxValue, tempMinDisplacement,displacementValue;
                    ushort minMileage = UInt16.MaxValue, tempMinMileage,mileageValue;
                    long minExShowroomPrice = Int64.MaxValue, tempExShowroomPrice;
                    IEnumerable<SpecsItem> minSpecList;
                    foreach (var bike in objData.Bikes)
                    {
                        minSpecList = bike.MinSpecsList;
                        if (minSpecList != null)
                        {
                            tempMinDisplacement = Single.TryParse(minSpecList.SingleOrDefault(s => s.Id == (int)EnumSpecsFeaturesItems.Displacement).Value, out displacementValue) ? displacementValue : 0;
                            minDisplacement = tempMinDisplacement > 0 && minDisplacement > tempMinDisplacement ? tempMinDisplacement : minDisplacement;
                            tempMinMileage = (UInt16.TryParse(minSpecList.SingleOrDefault(s => s.Id == (int)EnumSpecsFeaturesItems.FuelEfficiencyOverall).Value, out mileageValue)) ? mileageValue : Convert.ToUInt16(0);
                            minMileage = tempMinMileage > 0 && minMileage > tempMinMileage ? tempMinMileage : minMileage;
                        }
                        tempExShowroomPrice = bike.ExShowroomPrice;
                        minExShowroomPrice = tempExShowroomPrice > 0 && minExShowroomPrice > tempExShowroomPrice ? tempExShowroomPrice : minExShowroomPrice;
                    }
                    objInputFilters.MinMileage = minMileage != UInt16.MaxValue ? minMileage : (ushort)0;
                    objInputFilters.MinPrice = minExShowroomPrice != Int64.MaxValue ? minExShowroomPrice : 0;
                    objInputFilters.MinDisplacement = minDisplacement <= Single.MaxValue && minDisplacement >= Single.MaxValue ? 0 : minDisplacement;
                    objInputFilters.MakeCategoryId = _makeCategoryId;
                    objData.PageFilters.FilterResults = _pageFilters.GetRelevantPageFilters(objInputFilters).ToList();
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("MakePageModel.BindPageFilters_MakeId_{0}", _makeId));
            }
        }

        private void BindNewBikeSearchPopupData(MakePageVM objData)
        {
            try
            {
                if (objData != null)
                {
                    objData.NewBikeSearchPopup = new NewBikeSearch.NewBikeSearchPopupVM();
                    objData.NewBikeSearchPopup.HasFilteredBikes = true;
                    objData.NewBikeSearchPopup.HasOtherRecommendedBikes = true;
                    objData.NewBikeSearchPopup.MakeId = _makeId;
                    objData.NewBikeSearchPopup.MakeName = objData.MakeName;
                    if (cityBase != null)
                    {
                        objData.NewBikeSearchPopup.CityId = cityBase.CityId;
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("MakePageModel.BindNewBikeSearchPopupData_MakeId_{0}", _makeId));
            }
        }

        /// <summary>
        /// Created By : Deepak Israni on 20 March 2018
        /// Description: Overload of GetData function to Bind different ad slots with old and new Make page.
        /// Modified by : Snehal Dange on 30th April 2018
        /// Description: Added MakeABTestCookie to get abTestValues
        /// </summary>
        /// <param name="isNew"></param>
        /// <returns></returns>
        public MakePageVM GetData(MakeABTestCookie abTestValues)
        {
            bool isNew = false;
            if (abTestValues != null)
            {
                isNew = abTestValues.IsNewPage;
                _newMakePageV1Status = abTestValues.NewMakePageV1Status;
                _oldMakePageV1Status = abTestValues.OldMakePageV1Status;
            }
            MakePageVM objData = GetData();
            BindAdSlots(objData, isNew);
            return objData;
        }

        /// <summary>
        /// Created By : Deepak Israni on 20 March 2018
        /// Description: Method to bind ad slots to mobile make page.
        /// </summary>
        /// <param name="objData"></param>
        private void BindAdSlots(MakePageVM objData, bool isNewPage)
        {
            if (IsMobile)
            {
                if (!isNewPage)
                {
                    AdTags adTagsObj = objData.AdTags;
                    adTagsObj.AdPath = _adPath_Mobile_Old;
                    adTagsObj.AdId = _adId_Mobile_Old;
                    adTagsObj.Ad_320x50 = true;
                    adTagsObj.Ad300x250_Bottom = true;
                    adTagsObj.Ad_300x250BTF = _oldMakePageV1Status && objData.Bikes.Count() > 5;


                    IDictionary<string, AdSlotModel> ads = new Dictionary<string, AdSlotModel>();

                    NameValueCollection adInfo = new NameValueCollection();
                    adInfo["adId"] = _adId_Mobile_Old;
                    adInfo["adPath"] = _adPath_Mobile_Old;

                    if (adTagsObj.Ad_320x50)
                    {
                        ads.Add(String.Format("{0}-0", _adId_Mobile_Old), GoogleAdsHelper.SetAdSlotProperties(adInfo, ViewSlotSize.ViewSlotSizes[AdSlotSize._320x50], 0, 320, AdSlotSize._320x50, "Top", true));
                    }
                    if (adTagsObj.Ad300x250_Bottom)
                    {
                        ads.Add(String.Format("{0}-16", _adId_Mobile_Old), GoogleAdsHelper.SetAdSlotProperties(adInfo, ViewSlotSize.ViewSlotSizes[AdSlotSize._300x250], 16, 300, AdSlotSize._300x250, "Bottom"));
                    }
                    if (adTagsObj.Ad_300x250BTF)
                    {
                        ads.Add(String.Format("{0}-1", _adId_Mobile_Old), GoogleAdsHelper.SetAdSlotProperties(adInfo, ViewSlotSize.ViewSlotSizes[AdSlotSize._300x250], 1, 300, AdSlotSize._300x250, "BTF"));
                    }

                    objData.AdSlots = ads;
                }
                else
                {
                    AdTags adTagsObj = objData.AdTags;
                    adTagsObj.AdId = _adId_Mobile_New;
                    adTagsObj.AdPath = _adPath_Mobile_New;
                    if (!_newMakePageV1Status)
                    {
                        adTagsObj.Ad_320x100_Top = true;
                    }

                    adTagsObj.Ad_300x250_Top = true;
                    adTagsObj.Ad_300x250_Middle = true;
                    adTagsObj.Ad_300x250_Bottom = true;

                    objData.AdTags = adTagsObj;

                    IDictionary<string, AdSlotModel> ads = new Dictionary<string, AdSlotModel>();

                    NameValueCollection adInfo = new NameValueCollection();
                    adInfo["adId"] = _adId_Mobile_New;
                    adInfo["adPath"] = _adPath_Mobile_New;


                    if (!_newMakePageV1Status && adTagsObj.Ad_320x100_Top)
                    {
                        ads.Add(String.Format("{0}-3", _adId_Mobile_New), GoogleAdsHelper.SetAdSlotProperties(adInfo, ViewSlotSize.ViewSlotSizes[AdSlotSize._320x100], 3, 320, AdSlotSize._320x100, "Top", true));
                    }
                    if (adTagsObj.Ad_300x250_Top)
                    {
                        ads.Add(String.Format("{0}-1", _adId_Mobile_New), GoogleAdsHelper.SetAdSlotProperties(adInfo, ViewSlotSize.ViewSlotSizes[AdSlotSize._300x250], 1, 300, AdSlotSize._300x250, "Top"));
                    }
                    if (adTagsObj.Ad_300x250_Middle)
                    {
                        ads.Add(String.Format("{0}-2", _adId_Mobile_New), GoogleAdsHelper.SetAdSlotProperties(adInfo, ViewSlotSize.ViewSlotSizes[AdSlotSize._300x250], 2, 300, AdSlotSize._300x250, "Middle"));
                    }

                    if (adTagsObj.Ad_300x250_Bottom)
                    {
                        ads.Add(String.Format("{0}-0", _adId_Mobile_New), GoogleAdsHelper.SetAdSlotProperties(adInfo, ViewSlotSize.ViewSlotSizes[AdSlotSize._300x250], 0, 300, AdSlotSize._300x250, "Bottom"));
                    }

                    objData.AdSlots = ads;
                }
            }
        }

        /// <summary>
        /// Created By : Deepak Israni on 26 March 2018
        /// Description: Method to bind ad slots to desktop make page.
        /// </summary>
        /// <param name="objData"></param>
        private void BindAdSlots(MakePageVM objData)
        {
            AdTags adTagsObj = objData.AdTags;
            adTagsObj.AdPath = _adPath_Desktop;
            adTagsObj.AdId = _adId_Desktop;
            adTagsObj.Ad_Model_ATF_300x250 = true;
            adTagsObj.Ad_Model_BTF_300x250 = true;
            adTagsObj.Ad_Top_300x250 = true;
            adTagsObj.Ad_970x90Bottom = true;
            adTagsObj.Ad_970x90Top = true;

            IDictionary<string, AdSlotModel> ads = new Dictionary<string, AdSlotModel>();

            NameValueCollection adInfo = new NameValueCollection();
            adInfo["adId"] = _adId_Desktop;
            adInfo["adPath"] = _adPath_Desktop;

            if (adTagsObj.Ad_Model_ATF_300x250)
            {
                ads.Add(String.Format("{0}-9", _adId_Desktop), GoogleAdsHelper.SetAdSlotProperties(adInfo, ViewSlotSize.ViewSlotSizes[AdSlotSize._300x250], 9, 300, AdSlotSize._300x250, "ATF", true));
            }
            if (adTagsObj.Ad_Model_BTF_300x250)
            {
                ads.Add(String.Format("{0}-11", _adId_Desktop), GoogleAdsHelper.SetAdSlotProperties(adInfo, ViewSlotSize.ViewSlotSizes[AdSlotSize._300x250], 11, 300, AdSlotSize._300x250, "BTF"));
            }
            if (adTagsObj.Ad_Top_300x250)
            {
                ads.Add(String.Format("{0}-17", _adId_Desktop), GoogleAdsHelper.SetAdSlotProperties(adInfo, ViewSlotSize.ViewSlotSizes[AdSlotSize._300x250], 17, 300, AdSlotSize._300x250, "Top", true));
            }
            if (adTagsObj.Ad_970x90Bottom)
            {
                ads.Add(String.Format("{0}-5", _adId_Desktop), GoogleAdsHelper.SetAdSlotProperties(adInfo, ViewSlotSize.ViewSlotSizes[AdSlotSize._970x90 + "_C"], 5, 970, AdSlotSize._970x90, "Bottom"));
            }
            if (adTagsObj.Ad_970x90Top)
            {
                ads.Add(String.Format("{0}-19", _adId_Desktop), GoogleAdsHelper.SetAdSlotProperties(adInfo, ViewSlotSize.ViewSlotSizes[AdSlotSize._970x90 + "_C"], 19, 970, AdSlotSize._970x90, "Top", true));
            }

            objData.AdSlots = ads;
        }


        /// <summary>
        /// Created By : Deepak Israni on 16 April 2018
        /// Description: To bind the series linkage widget on the page.
        /// </summary>
        /// <param name="objData"></param>
        /// <param name="cityId"></param>
        private void BindSeriesLinkages(MakePageVM objData, uint cityId)
        {
            try
            {
                objData.SeriesLinkages = new MakeSeriesSlugVM
                {
                    MakeName = objData.MakeName,
                    MakeMaskingName = objData.MakeMaskingName
                };
                objData.SeriesLinkages.MakeSeriesList = _bikeSeries.GetMakeSeries(_makeId, cityId);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("MakePageModel.BindSeriesLinkages_MakeId_{0}_CityId_{1}", _makeId, cityId));
            }
        }
    }
}