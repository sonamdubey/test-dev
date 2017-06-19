using Bikewale.Common;
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Bikewale.Entities.Compare;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.Compare;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.ServiceCenter;
using Bikewale.Models.CompareBikes;
using Bikewale.Models.ServiceCenters;
using Bikewale.Utility;
using System;
using System.Linq;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Videos;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 31-Mar-2017
    /// Model for scooters make page
    /// Modified by : Aditi Srivastava on 5 June 2017
    /// Summary     : Added BL instance instead of cache for comparison carousel
    /// </summary>
    public class ScootersMakePageModel
    {
        private readonly IBikeMakes<BikeMakeEntity, int> _bikeMakes = null;
        private readonly IBikeModels<BikeModelEntity, int> _bikeModels = null;
        private readonly IUpcoming _upcoming = null;
        private readonly IBikeCompare _compareScooters = null;
        private readonly IBikeMakesCacheRepository<int> _objMakeCache = null;
        private readonly IDealerCacheRepository _objDealerCache = null;
        private readonly IServiceCenter _objService = null;
        private readonly ICMSCacheContent _articles = null;
        private readonly IVideos _videos = null;


        public StatusCodes status;
        public MakeMaskingResponse objResponse;
        private uint _makeId;
        private string _makeName, _makeMaskingName;
        public string redirectUrl;
        /// <summary>
        /// Created by  :   Sumit Kate on 30 Mar 2017
        /// Description :   Constructor to initialize the member variables
        /// </summary>
        public ScootersMakePageModel(
            string makeMaskingName,
            IBikeMakes<BikeMakeEntity, int> bikeMakes,
            IBikeModels<BikeModelEntity, int> bikeModels,
            IUpcoming upcoming,
            IBikeCompare compareScooters,
            IBikeMakesCacheRepository<int> objMakeCache,
            IDealerCacheRepository objDealerCache,
            IServiceCenter objServices,
            ICMSCacheContent articles,
            IVideos videos 
            )
        {
            _makeMaskingName = makeMaskingName;
            _bikeMakes = bikeMakes;
            _bikeModels = bikeModels;
            _upcoming = upcoming;
            _compareScooters = compareScooters;
            _objMakeCache = objMakeCache;
            _objDealerCache = objDealerCache;
            ProcessQuery(makeMaskingName);
            _objService = objServices;
            _articles = articles;
            _videos = videos;
        }

        public uint CityId { get { return GlobalCityArea.GetGlobalCityArea().CityId; } }
        public ushort BrandTopCount { get; set; }
        public PQSourceEnum PqSource { get; set; }
        public CompareSources CompareSource { get; set; }

        /// <summary>
        /// Created by  :   Sumit Kate on 30 Mar 2017
        /// Description :   Returns the Scooters Index Page view model
        /// </summary>
        /// <returns></returns>
        public ScootersMakePageVM GetData()
        {
            ScootersMakePageVM objViewModel = null;
            try
            {
                objViewModel = new ScootersMakePageVM();
                CityEntityBase cityEntity = null;
                var cityBase = GlobalCityArea.GetGlobalCityArea();
                if (cityBase != null && cityBase.CityId > 0)
                {
                    cityEntity = new CityHelper().GetCityById(cityBase.CityId);
                    objViewModel.Location = cityEntity.CityName;
                    objViewModel.LocationMasking = cityEntity.CityMaskingName;
                }
                else
                {
                    objViewModel.Location = "India";
                    objViewModel.LocationMasking = "india";
                }
                objViewModel.PageCatId = 8;
                objViewModel.Make = _bikeMakes.GetMakeDetails(_makeId);
                if (objViewModel.Make != null)
                {
                    _makeName = objViewModel.Make.MakeName;
                }
                BindPageMetaTags(objViewModel.PageMetaTags, objViewModel.Make);
                objViewModel.Description = _objMakeCache.GetScooterMakeDescription(objResponse.MakeId);
                objViewModel.Scooters = _bikeModels.GetMostPopularScooters(_makeId);
                BindUpcomingBikes(objViewModel);
                BindDealersServiceCenters(objViewModel, cityEntity);
                BindOtherScooterBrands(objViewModel, _makeId, 9);
                BindCompareScootes(objViewModel,CompareSource);
                BindEditorialWidget(objViewModel);
                SetFlags(objViewModel, CityId);
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass er = new Bikewale.Notifications.ErrorClass(ex, "ScootersIndexPageModel.GetData()");
            }
            return objViewModel;
        }

        /// <summary>
        /// Modified by : Aditi Srivastava on 25 Apr 2017
        /// Summary  :  Moved the comparison logic to common model
        /// Modified by : Aditi Srivastava on 27 Apr 2017
        /// Summary  : Added source for comparisons
        /// </summary>
        private void BindCompareScootes(ScootersMakePageVM objViewModel, CompareSources CompareSource)
        {
            try
            {
                string versionList = string.Join(",", objViewModel.Scooters.Select(m => m.objVersion.VersionId));
                PopularModelCompareWidget objCompare = new PopularModelCompareWidget(_compareScooters, 1, CityId, versionList);
                objViewModel.SimilarCompareScooters = objCompare.GetData();
                objViewModel.SimilarCompareScooters.CompareSource = CompareSource;
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass er = new Bikewale.Notifications.ErrorClass(ex, "ScootersIndexPageModel.BindCompareScootes()");
            }
        }

        private void BindOtherScooterBrands(ScootersMakePageVM objViewModel, uint _makeId, int topCount)
        {
            var scooterBrand = _objMakeCache.GetScooterMakes();
            objViewModel.OtherBrands = scooterBrand.Where(x => x.MakeId != _makeId).Take(topCount);
        }

        /// <summary>
        /// Modified by : Aditi Srivastava on 15 June 2017
        /// Summary     : Added flags for editorial section (News, expert reviews and videos)
        /// </summary>
        private void SetFlags(ScootersMakePageVM objData, uint cityId)
        {
            if (objData != null)
            {

                objData.IsScooterDataAvailable = objData.Scooters != null && objData.Scooters.Count() > 0;
                objData.IsCompareDataAvailable = objData.SimilarCompareScooters != null && objData.SimilarCompareScooters.CompareBikes != null && objData.SimilarCompareScooters.CompareBikes.Count() > 0;
                objData.IsUpComingBikesAvailable = objData.UpcomingScooters != null && objData.UpcomingScooters != null && objData.UpcomingScooters.UpcomingBikes != null && objData.UpcomingScooters.UpcomingBikes.Count() > 0;
                objData.IsDealerAvailable = objData.Dealers != null && objData.Dealers.Dealers != null && objData.Dealers.Dealers.Count() > 0;
                objData.IsServiceDataAvailable = objData.ServiceCenters != null && objData.ServiceCenters.ServiceCentersList != null && objData.ServiceCenters.ServiceCentersList.Count() > 0;
                objData.IsDealerServiceDataAvailable = cityId > 0 && (objData.IsDealerAvailable || objData.IsServiceDataAvailable);
                objData.IsDealerServiceDataInIndiaAvailable = cityId == 0 && objData.DealersServiceCenter != null && objData.DealersServiceCenter.DealerServiceCenters != null && objData.DealersServiceCenter.DealerServiceCenters.DealerDetails != null && objData.DealersServiceCenter.DealerServiceCenters.DealerDetails.Count() > 0;
                objData.DealerServiceTitle = cityId == 0 ? "Dealers & Service Centers" : String.Format("{0}{1}", objData.IsDealerAvailable ? "Dealers" : "", objData.IsServiceDataAvailable ? " & Service Centers" : "");
                objData.IsNewsAvailable = objData.News != null && objData.News.ArticlesList != null && objData.News.ArticlesList.Count() > 0;
                objData.IsExpertReviewsAvailable = objData.News != null && objData.ExpertReviews.ArticlesList != null && objData.ExpertReviews.ArticlesList.Count() > 0;
                objData.IsVideosAvailable = objData.Videos != null && objData.Videos.VideosList != null && objData.Videos.VideosList.Count() > 0;
                objData.IsMakeTabsDataAvailable = (objData.Description != null && objData.Description.FullDescription.Length > 0 || objData.IsDealerServiceDataAvailable || objData.IsDealerServiceDataInIndiaAvailable || objData.IsNewsAvailable || objData.IsExpertReviewsAvailable || objData.IsVideosAvailable);
            }

        }

        /// <summary>
        /// Binds the dealers service centers.
        /// </summary>
        /// <param name="objVM">The object vm.</param>
        /// <param name="cityEntity">The city entity.</param>
        private void BindDealersServiceCenters(ScootersMakePageVM objVM, CityEntityBase cityEntity)
        {
            try
            {
                if (cityEntity != null && cityEntity.CityId > 0)
                {
                    var dealerData = new DealerCardWidget(_objDealerCache, cityEntity.CityId, _makeId);
                    dealerData.TopCount = 3;
                    objVM.Dealers = dealerData.GetData();
                    objVM.ServiceCenters = new ServiceCentersCard(_objService, 3, _makeId, cityEntity.CityId).GetData();
                }
                else
                {
                    objVM.DealersServiceCenter = new DealersServiceCentersIndiaWidgetModel(_makeId, _makeName, _makeMaskingName, _objDealerCache).GetData();
                }
            }
            catch (Exception ex)
            {
                ErrorClass er = new ErrorClass(ex, "ScootersMakePageModel.BindDealersServiceCenters()");
            }

        }

        /// <summary>
        /// Binds the page meta tags.
        /// </summary>
        /// <param name="pageMetaTags">The page meta tags.</param>
        /// <param name="make">The make.</param>
        private void BindPageMetaTags(PageMetaTags pageMetaTags, BikeMakeEntityBase make)
        {
            try
            {
                pageMetaTags.CanonicalUrl = string.Format("https://www.bikewale.com/{0}-scooters/", make.MaskingName);
                pageMetaTags.AlternateUrl = string.Format("https://www.bikewale.com/m/{0}-scooters/", make.MaskingName);
                pageMetaTags.Keywords = string.Format("{0} Scooter, {0} Scooty, Scooter {0}, Scooty {0}, Scooters, Scooty", make.MakeName);
                pageMetaTags.Description = string.Format("Check {0} Scooty prices in India. Know more about new and upcoming {0} scooters, their prices, performance and mileage.", make.MakeName);
                pageMetaTags.Title = string.Format("{0} Scooters in India | Scooty Prices, Mileage & Images - BikeWale", make.MakeName);
            }
            catch (Exception ex)
            {
                ErrorClass er = new ErrorClass(ex, "ScootersMakePageModel.BindPageMetaTags()");
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
                objResponse = _objMakeCache.GetMakeMaskingResponse(makeMaskingName);
                if (objResponse != null)
                {
                    status = (StatusCodes)objResponse.StatusCode;
                    if (objResponse.StatusCode == 200)
                    {
                        _makeId = objResponse.MakeId;
                    }
                    else if (objResponse.StatusCode == 301)
                    {
                        status = StatusCodes.RedirectPermanent;
                    }
                    else
                    {
                        status = StatusCodes.ContentNotFound;
                    }
                }
                else
                {
                    status = StatusCodes.ContentNotFound;
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, string.Format("ScootersMakePageModel.ProcessQuery() makeMaskingName:{0}", makeMaskingName));
            }
        }

        /// <summary>
        /// Binds the upcoming bikes.
        /// </summary>
        /// <param name="objData">The object data.</param>
        private void BindUpcomingBikes(ScootersMakePageVM objData)
        {
            UpcomingBikesWidget objUpcoming = new UpcomingBikesWidget(_upcoming);
            objUpcoming.Filters = new Bikewale.Entities.BikeData.UpcomingBikesListInputEntity()
            {
                PageSize = 9,
                PageNo = 1,
                BodyStyleId = 5
            };
            objUpcoming.SortBy = EnumUpcomingBikesFilter.Default;
            objData.UpcomingScooters = objUpcoming.GetData();
        }
        /// <summary>
        /// Created by : Aditi Srivastava on 15 June 2017
        /// Summary    : Bind make scooter related editorial content
        /// </summary>
        private void BindEditorialWidget(ScootersMakePageVM objData)
        {
            RecentNews objNews = new RecentNews(2, _makeId, _makeName, _makeMaskingName, string.Format("News about {0} Scooters", _makeName), _articles);
            objNews.IsScooter = true;
            objData.News = objNews.GetData();
           
            RecentExpertReviews objReviews = new RecentExpertReviews(2, _makeId, _makeName, _makeMaskingName, _articles, string.Format("{0} Reviews", _makeName));
            objReviews.IsScooter = true;
            objData.ExpertReviews = objReviews.GetData();

            RecentVideos objVideos = new RecentVideos(1, 2, _makeId, _makeName, _makeMaskingName, _videos);
            objVideos.IsScooter = true;
            objData.Videos = objVideos.GetData();
        }
    }
}