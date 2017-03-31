using Bikewale.BAL.MVC.UI;
using Bikewale.Common;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.PriceQuote;
using Bikewale.Entities.ServiceCenters;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.NewLaunched;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.Compare;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.ServiceCenter;
using Bikewale.Models;
using Bikewale.Models.Shared;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace Bikewale.Controllers
{
    /// <summary>
    /// Created by  :   Sumit Kate on 30 Mar 2017
    /// Description :   Scooters Controller
    /// </summary>
    public class ScootersController : Controller
    {
        private readonly INewBikeLaunchesBL _newLaunches = null;

        private readonly IBikeModels<BikeModelEntity, int> _models = null;
        private readonly IBikeMakesCacheRepository<int> _objMakeCache = null;
        private readonly IBikeModels<BikeModelEntity, int> _objBikeModel = null;
        private readonly IBikeMakes<BikeMakeEntity, int> _objMakeRepo = null;
        private readonly IUpcoming _upcoming = null;
        private readonly IBikeCompareCacheRepository _compareScooters = null;
        private readonly IDealerCacheRepository _dealerCache = null;
        private readonly IServiceCenter _serviceCenter = null;
        public ScootersController(IBikeMakes<BikeMakeEntity, int> objMakeRepo, IBikeModels<BikeModelEntity, int> models, INewBikeLaunchesBL newLaunches, IUpcoming upcoming, IBikeCompareCacheRepository compareScooters, IDealerCacheRepository dealerCache, IBikeMakesCacheRepository<int> objMakeCache, IBikeModels<BikeModelEntity, int> objBikeModel, IBikeMakes<BikeMakeEntity, int> objMakeRepor, IServiceCenter serviceCenter)
        {
            _newLaunches = newLaunches;
            _models = models;
            _upcoming = upcoming;
            _compareScooters = compareScooters;
            _objMakeCache = objMakeCache;
            _objBikeModel = objBikeModel;
            _dealerCache = dealerCache;
            _serviceCenter = serviceCenter;
            _objMakeRepo = objMakeRepo;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 30 Mar 2017
        /// Description :   Returns view model Desktop Scooter landing page
        /// </summary>
        /// <returns></returns>
        [Route("scooters/")]
        [Bikewale.Filters.DeviceDetection]
        public ActionResult Index()
        {
            ScootersIndexPageModel model = new ScootersIndexPageModel(
                _objMakeRepo, _models, _newLaunches, _upcoming, _compareScooters);
            model.BrandTopCount = 10;
            model.PqSource = PQSourceEnum.Desktop_Scooters_Landing_Check_on_road_price;
            return View(model.GetData());
        }

        /// <summary>
        /// Modified by :   Sumit Kate on 30 Mar 2017
        /// Description :   Returns view model Mobile Scooter landing page
        /// </summary>
        /// <returns></returns>
        [Route("m/scooters/")]
        public ActionResult Index_Mobile()
        {
            ScootersIndexPageModel model = new ScootersIndexPageModel(
                _objMakeRepo, _models, _newLaunches, _upcoming, _compareScooters);
            model.BrandTopCount = 6;
            model.PqSource = PQSourceEnum.Mobile_Scooters_Landing_Check_on_road_price;
            return View(model.GetData());
        }
        /// <summary>
        /// Created By :- Subodh Jain 09 March 2017
        /// Summary :- Populate Upcoming scooters
        /// </summary>
        private void UpcomingScooters()
        {
            var objFiltersUpcoming = new Bikewale.Entities.BikeData.UpcomingBikesListInputEntity()
            {
                EndIndex = 9,
                StartIndex = 1,
                BodyStyleId = 5
            };
            var sortBy = Bikewale.Entities.BikeData.EnumUpcomingBikesFilter.Default;
            IEnumerable<UpcomingBikeEntity> objUpcomingBikes = _upcoming.GetModels(objFiltersUpcoming, sortBy);
            ViewBag.UpcomingBikes = objUpcomingBikes;

        }

        /// <summary>
        /// Created By :- Subodh Jain 17 March 2017
        /// Summary :- Added statuscode check
        /// </summary>
        /// <param name="makeMaskingName"></param>
        /// <returns></returns>
        [Route("scooters/make/{makeMaskingName}/")]
        [Bikewale.Filters.DeviceDetection]
        public ActionResult BikesByMake(string makeMaskingName)
        {

            try
            {
                MakeMaskingResponse objResponse = _objMakeCache.GetMakeMaskingResponse(makeMaskingName);
                if (objResponse != null && objResponse.StatusCode == 200)
                {
                    ViewBag.PageCatId = 8;
                    ViewBag.showServiceCenter = false;
                    ViewBag.showServiceWidget = false;
                    ViewBag.showDealerWidget = false;
                    ViewBag.CityId = GlobalCityArea.GetGlobalCityArea().CityId;
                    ViewBag.CityName = GlobalCityArea.GetGlobalCityArea().City;
                    IEnumerable<MostPopularBikesBase> ScootersList = null;
                    ViewBag.MakeMaskingName = makeMaskingName;
                    ScootersList = BindPopularScooters(objResponse.MakeId);
                    BikeMakeEntityBase objMake = _objMakeRepo.GetMakeDetails(objResponse.MakeId);
                    ViewBag.MakeName = objMake.MakeName;
                    ViewBag.MakeId = objResponse.MakeId;
                    UpcomingScooters();
                    DealerShowrooms(ViewBag.CityId, objResponse.MakeId, Convert.ToUInt16(ViewBag.CityId > 0 ? 3 : 6));
                    ServiceCenters(ViewBag.CityId, (int)objResponse.MakeId, 3);
                    BikeDescriptionEntity scooterSynopis = _objMakeCache.GetScooterMakeDescription(objResponse.MakeId);
                    ViewBag.Synopsis = scooterSynopis;
                    ViewBag.ScootersList = ScootersList;
                    string versionList = string.Join(",", ScootersList.Select(m => m.objVersion.VersionId));
                    ICollection<SimilarCompareBikeEntity> similarBikeList = BindSimilarBikes(versionList);
                    ViewBag.similarBikeList = similarBikeList;
                    ViewBag.PageMetaTags = new ScootersHelper().CreateMakeWiseMetaTags(true, makeMaskingName, ViewBag.MakeName);
                    return View("~/views/scooters/bikesbymake.cshtml");
                }

                else if (objResponse.StatusCode == 301)
                {
                    return RedirectPermanent(Request.RawUrl.Replace(makeMaskingName, objResponse.MaskingName));
                }
                else
                {
                    return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
                }
            }
            catch (Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, "ScootersController.BikesByMake");
                return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
            }

        }

        /// <summary>
        /// Created By :- Subodh Jain 17 March 2017
        /// Summary :- Added statuscode check
        /// </summary>
        /// <param name="makeMaskingName"></param>
        /// <returns></returns>
        [Route("m/scooters/make/{makemaskingname}/")]
        public ActionResult BikesByMake_Mobile(string makeMaskingName)
        {

            try
            {
                MakeMaskingResponse objResponse = _objMakeCache.GetMakeMaskingResponse(makeMaskingName);
                if (objResponse != null && objResponse.StatusCode == 200)
                {
                    ViewBag.PageCatId = 8;
                    ViewBag.showServiceCenter = false;
                    ViewBag.showServiceWidget = false;
                    ViewBag.showDealerWidget = false;
                    ViewBag.CityId = GlobalCityArea.GetGlobalCityArea().CityId;
                    ViewBag.CityName = GlobalCityArea.GetGlobalCityArea().City;
                    IEnumerable<MostPopularBikesBase> ScootersList = null;
                    ViewBag.MakeMaskingName = makeMaskingName;
                    ScootersList = BindPopularScooters(objResponse.MakeId);
                    BikeMakeEntityBase objMake = _objMakeRepo.GetMakeDetails(objResponse.MakeId);
                    ViewBag.MakeName = objMake.MakeName;
                    ViewBag.MakeId = objResponse.MakeId;
                    UpcomingScooters();
                    DealerShowrooms(ViewBag.CityId, objResponse.MakeId, 6);
                    ServiceCenters(ViewBag.CityId, (int)objResponse.MakeId, 9);
                    BikeDescriptionEntity scooterSynopis = _objMakeCache.GetScooterMakeDescription(objResponse.MakeId);
                    ViewBag.Synopsis = scooterSynopis;
                    ViewBag.ScootersList = ScootersList;
                    string versionList = string.Join(",", ScootersList.Select(m => m.objVersion.VersionId));
                    ICollection<SimilarCompareBikeEntity> similarBikeList = BindSimilarBikes(versionList);
                    ViewBag.similarBikeList = similarBikeList;
                    ViewBag.PageMetaTags = new ScootersHelper().CreateMakeWiseMetaTags(false, makeMaskingName, ViewBag.MakeName);
                    return View("~/views/m/scooters/bikesbymake.cshtml");
                }
                else if (objResponse.StatusCode == 301)
                {
                    return RedirectPermanent(Request.RawUrl.Replace(makeMaskingName, objResponse.MaskingName));
                }
                else
                {
                    return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
                }
            }
            catch (Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, "ScootersController.BikesByMake");
                return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
            }

        }

        [Route("scooters/otherBrands/")]
        public ActionResult OtherBrands(uint makeId)
        {
            ScooterBrands scooters = new ScooterBrands();
            IEnumerable<BikeMakeEntityBase> otherBrand = scooters.GetOtherScooterBrands(_objMakeCache, makeId, 9);
            return View("~/views/shared/_otherbrands.cshtml", otherBrand);
        }

        [Route("m/scooters/otherBrands/")]
        public ActionResult OtherBrands_Mobile(uint makeId)
        {
            ScooterBrands scooters = new ScooterBrands();
            IEnumerable<BikeMakeEntityBase> otherBrand = scooters.GetOtherScooterBrands(_objMakeCache, makeId, 9);
            return View("~/views/m/shared/_otherbrands.cshtml", otherBrand);
        }

        /// <summary>
        /// Created By :- Subodh Jain 10 March 2017
        /// Summary :- Bind similar bike list 
        /// </summary>
        private ICollection<SimilarCompareBikeEntity> BindSimilarBikes(string versionList)
        {
            int cityId = (int)GlobalCityArea.GetGlobalCityArea().CityId;
            ushort topCount = 1;
            return _compareScooters.GetSimilarCompareBikes(versionList, topCount, cityId);
        }
        /// <summary>
        /// Created By :- Subodh Jain 10 March 2017
        /// Summary :- Bind popular bike list 
        /// </summary>
        private IEnumerable<MostPopularBikesBase> BindPopularScooters(uint makeId)
        {
            return _objBikeModel.GetMostPopularScooters(makeId);
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 10 Mar 2017
        /// Summary    : To fetch dealer showroom info
        /// </summary>
        private void DealerShowrooms(uint cityId, uint makeId, int topCount)
        {
            ShowroomsVM objShowrooms = new ShowroomsVM();
            if (cityId > 0)
            {
                objShowrooms.dealers = _dealerCache.GetDealerByMakeCity(cityId, makeId, 0);
                if (objShowrooms.dealers != null && objShowrooms.dealers.Dealers != null && objShowrooms.dealers.Dealers.Count() > 0)
                {
                    ViewBag.MakeName = objShowrooms.dealers.MakeName;
                    ViewBag.MakeMaskingName = objShowrooms.dealers.MakeMaskingName;
                    ViewBag.CityMaskingName = objShowrooms.dealers.CityMaskingName;
                    ViewBag.CityName = objShowrooms.dealers.CityName;
                    objShowrooms.dealers.Dealers = objShowrooms.dealers.Dealers.Take(topCount);
                    ViewBag.showDealerWidget = true;
                }
            }
            else
            {
                objShowrooms.dealerServiceCenter = _dealerCache.GetPopularCityDealer(makeId, (uint)topCount);
            }
            if (objShowrooms.dealerServiceCenter != null)
            {
                ViewBag.showDealerWidget = (objShowrooms.dealerServiceCenter.TotalDealerCount > 0 || objShowrooms.dealerServiceCenter.TotalServiceCenterCount > 0);
                ViewBag.showServiceCenter = (objShowrooms.dealerServiceCenter.TotalServiceCenterCount > 0);
            }
            ViewBag.objShowrooms = objShowrooms;
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 10 Mar 2017
        /// Summary    : To fetch service center info
        /// </summary>
        private void ServiceCenters(uint cityId, int makeId, int topCount)
        {
            ServiceCenterData objServiceCenter = _serviceCenter.GetServiceCentersByCity(cityId, makeId);
            IEnumerable<ServiceCenterDetails> ServiceCenterList = null;
            if (objServiceCenter != null && objServiceCenter.ServiceCenters != null && objServiceCenter.ServiceCenters.Count() > 0)
            {
                ServiceCenterList = objServiceCenter.ServiceCenters.Take(topCount);

                if (objServiceCenter.Count > 0)
                    ViewBag.showServiceWidget = true;

                ViewBag.serviceCenters = ServiceCenterList;
            }
        }

    }
}