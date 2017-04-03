using Bikewale.Common;
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.Compare;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.ServiceCenter;
using Bikewale.Models.ServiceCenters;
using Bikewale.Utility;
using System;
using System.Linq;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 31-Mar-2017
    /// Model for scooters make page
    /// </summary>
    public class ScootersMakePageModel
    {
        private readonly IBikeMakes<BikeMakeEntity, int> _bikeMakes = null;
        private readonly IBikeModels<BikeModelEntity, int> _bikeModels = null;
        private readonly IUpcoming _upcoming = null;
        private readonly IBikeCompareCacheRepository _compareScooters = null;
        private readonly IBikeMakesCacheRepository<int> _objMakeCache = null;
        private readonly IDealerCacheRepository _objDealerCache = null;
        private readonly IServiceCenter _objService = null;



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
            IBikeCompareCacheRepository compareScooters,
            IBikeMakesCacheRepository<int> objMakeCache,
            IDealerCacheRepository objDealerCache,
            IServiceCenter objServices
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
        }

        public uint CityId { get { return GlobalCityArea.GetGlobalCityArea().CityId; } }
        public ushort BrandTopCount { get; set; }
        public PQSourceEnum PqSource { get; set; }

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
                BindCompareScootes(objViewModel);
                SetFlags(objViewModel, CityId);
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass er = new Bikewale.Notifications.ErrorClass(ex, "ScootersIndexPageModel.GetData()");
            }
            return objViewModel;
        }

        private void BindCompareScootes(ScootersMakePageVM objViewModel)
        {
            try
            {
                string versionList = string.Join(",", objViewModel.Scooters.Select(m => m.objVersion.VersionId));
                var compareBikes = _compareScooters.GetSimilarCompareBikes(versionList, 4, (int)CityId);
                objViewModel.SimilarCompareScooters = new ScooterComparesVM();
                objViewModel.SimilarCompareScooters.Bikes = compareBikes.Take(4).ToList();
                objViewModel.SimilarCompareScooters.MakeName = _makeName;
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


        private void SetFlags(ScootersMakePageVM objData, uint cityId)
        {
            if (objData != null)
            {

                objData.IsScooterDataAvailable = objData.Scooters != null && objData.Scooters.Count() > 0;
                objData.IsCompareDataAvailable = objData.SimilarCompareScooters != null && objData.SimilarCompareScooters.Bikes != null && objData.SimilarCompareScooters.Bikes.Count > 0;
                objData.IsUpComingBikesAvailable = objData.UpcomingScooters != null && objData.UpcomingScooters != null && objData.UpcomingScooters.UpcomingBikes != null && objData.UpcomingScooters.UpcomingBikes.Count() > 0;
                objData.IsDealerAvailable = objData.Dealers != null && objData.Dealers.Dealers != null && objData.Dealers.Dealers.Count() > 0;
                objData.IsServiceDataAvailable = objData.ServiceCenters != null && objData.ServiceCenters.ServiceCentersList != null && objData.ServiceCenters.ServiceCentersList.Count() > 0;
                objData.IsDealerServiceDataAvailable = cityId > 0 && (objData.IsDealerAvailable || objData.IsServiceDataAvailable);
                objData.IsDealerServiceDataInIndiaAvailable = cityId == 0 && objData.DealersServiceCenter != null && objData.DealersServiceCenter.DealerServiceCenters != null && objData.DealersServiceCenter.DealerServiceCenters.DealerDetails != null && objData.DealersServiceCenter.DealerServiceCenters.DealerDetails.Count() > 0;

                objData.IsMakeTabsDataAvailable = (objData.Description != null && objData.Description.FullDescription.Length > 0 || objData.IsDealerServiceDataAvailable || objData.IsDealerServiceDataInIndiaAvailable);
                objData.DealerServiceTitle = cityId == 0 ? "Dealers & Service Centers" : String.Format("{0}{1}", objData.IsDealerAvailable ? "Dealers" : "", objData.IsServiceDataAvailable ? " & Service Centers" : "");
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
                EndIndex = 9,
                StartIndex = 1
            };
            objUpcoming.SortBy = Bikewale.Entities.BikeData.EnumUpcomingBikesFilter.Default;
            objData.UpcomingScooters = objUpcoming.GetData();
        }
    }
}