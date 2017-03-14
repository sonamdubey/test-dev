using Bikewale.BAL.MVC.UI;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.BikeData.NewLaunched;
using Bikewale.Entities.Compare;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.NewLaunched;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.Compare;
using Bikewale.Models.Shared;
using Bikewale.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Bikewale.Controllers.Desktop.Scooters
{
    public class ScootersController : Controller
    {
        private readonly INewBikeLaunchesBL _newLaunches = null;
        public readonly IBikeMakesCacheRepository<int> _IScooterCache;
        private readonly IBikeModels<BikeModelEntity, int> _models = null;
        private readonly IBikeMakesCacheRepository<int> _objMakeCache = null;
        private readonly IBikeModels<BikeModelEntity, int> _objBikeModel = null;
        private readonly IBikeMakes<BikeMakeEntity, int> _objMakeRepo = null;
        private readonly IUpcoming _upcoming = null;
        private readonly IBikeCompareCacheRepository _compareScooters = null;
        public ScootersController(IBikeModels<BikeModelEntity, int> models, INewBikeLaunchesBL newLaunches, IUpcoming upcoming, IBikeCompareCacheRepository compareScooters, IBikeMakesCacheRepository<int> IScooter, IBikeMakesCacheRepository<int> objMakeCache, IBikeModels<BikeModelEntity, int> objBikeModel, IBikeMakes<BikeMakeEntity, int> objMakeRepo)
        {
            _newLaunches = newLaunches;
            _models = models;
            _upcoming = upcoming;
            _compareScooters = compareScooters;
            _IScooterCache = IScooter;
            _objMakeCache = objMakeCache;
            _objBikeModel = objBikeModel;
            _objMakeRepo = objMakeRepo;
        }

        [Route("scooters/")]
        public ActionResult Index()
        {
            PopulateNewlaunch();
            PopularScooters();
            UpcomingScooters();
            CompareScootersList();

            return View("~/views/scooters/index.cshtml");
        }

        [Route("m/scooters/")]
        public ActionResult MIndex()
        {
            PopulateNewlaunch();
            MPopularScooters();
            UpcomingScooters();
            CompareScootersList();
            return View("~/views/m/scooters/index.cshtml");
        }
        /// <summary>
        /// Created By :- Subodh Jain 10 March 2017
        /// Summary :- Populate Compare ScootersList
        /// </summary>
        private void CompareScootersList()
        {
            uint topcount = 4;
            IEnumerable<TopBikeCompareBase> topScootersCompares = _compareScooters.ScooterCompareList(topcount);
            ViewBag.TopScootersCompares = topScootersCompares;
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
        /// Created By :- Subodh Jain 09 March 2017
        /// Summary :- Populate New launchs
        /// </summary>
        private void PopulateNewlaunch()
        {
            var filters = new InputFilter()
            {
                PageSize = 9,
                BodyStyle = 5
            };
            NewLaunchedBikeResult objNewLaunchesBikes = _newLaunches.GetBikes(filters);
            ViewBag.NewLaunchesList = objNewLaunchesBikes;
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 9 Mar 2017
        /// Summary    : get list of popular scooters
        /// </summary>
        private void PopularScooters()
        {
            uint cityId = GlobalCityArea.GetGlobalCityArea().CityId;
            uint topCount = 9;
            PopularScootersList objScooters = new PopularScootersList();
            objScooters.PopularScooters = _models.GetMostPopularScooters(topCount, cityId);

            objScooters.PQSourceId = (int)PQSourceEnum.Desktop_Scooters_Landing_Check_on_road_price;
            objScooters.PageCatId = (int)BikeInfoPageSource.ScootersLandingPage_Desktop;
            ViewBag.popularScooters = objScooters;
        }

        [Route("scooters/make/{makemaskingname}/")]
        public ActionResult BikesByMake(string makemaskingname)
        {
            IEnumerable<MostPopularBikesBase> ScootersList = null;
            ViewBag.MakeName = "";
            MakeMaskingResponse objResponse = _objMakeCache.GetMakeMaskingResponse(makemaskingname);
            if (objResponse != null)
            {
                ScootersList = BindPopularScooters(objResponse.MakeId);
                BikeMakeEntityBase objMake = _objMakeRepo.GetMakeDetails(objResponse.MakeId);
                ViewBag.MakeName = objMake.MakeName;
                ViewBag.MakeId = objResponse.MakeId;
            }
            ViewBag.ScootersList = ScootersList;
            string versionList = string.Join(",", ScootersList.Select(m => m.objVersion.VersionId));
            ICollection<SimilarCompareBikeEntity> similarBikeList = BindSimilarBikes(versionList);
            ViewBag.similarBikeList = similarBikeList;
            return View("~/views/scooters/bikesbymake.cshtml");
        }

        [Route("scooters/brands/")]
        public ActionResult Brands()
        {
            ScooterBrands scooters = new ScooterBrands();
            BrandWidget brands = scooters.GetScooterBrands(_IScooterCache, 10);
            return View("~/views/shared/_brands.cshtml", brands);
        }

        [Route("m/scooters/brands/")]
        public ActionResult BrandsMobile()
        {
            ScooterBrands scooters = new ScooterBrands();
            BrandWidget brands = scooters.GetScooterBrands(_IScooterCache, 6);
            return View("~/views/m/shared/_brands.cshtml", brands);
        }


        [Route("scooters/otherBrands/")]
        public ActionResult OtherBrands(uint makeId)
        {
            ScooterBrands scooters = new ScooterBrands();
            IEnumerable<BikeMakeEntityBase> otherBrand = scooters.GetOtherScooterBrands(_IScooterCache, makeId, 9);
            return View("~/views/shared/_otherbrands.cshtml", otherBrand);
        }

        [Route("m/scooters/otherBrands/")]
        public ActionResult OtherBrandsMobile(uint makeId)
        {
            ScooterBrands scooters = new ScooterBrands();
            IEnumerable<BikeMakeEntityBase> otherBrand = scooters.GetOtherScooterBrands(_IScooterCache, makeId, 9);
            return View("~/views/m/shared/_otherbrands.cshtml", otherBrand);
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 9 Mar 2017
        /// Summary    : get list of popular scooters
        /// </summary>
        private void MPopularScooters()
        {
            uint cityId = GlobalCityArea.GetGlobalCityArea().CityId;
            uint topCount = 9;
            PopularScootersList objScooters = new PopularScootersList();
            objScooters.PopularScooters = _models.GetMostPopularScooters(topCount, cityId);

            objScooters.PQSourceId = (int)PQSourceEnum.Mobile_Scooters_Landing_Check_on_road_price;
            objScooters.PageCatId = (int)BikeInfoPageSource.ScootersLandingPage_Mobile;
            ViewBag.popularScooters = objScooters;
        }
        [Route("m/scooters/make/{makemaskingname}/")]
        public ActionResult MBikesByMake(string makemaskingname)
        {
            IEnumerable<MostPopularBikesBase> ScootersList = null;
            ViewBag.MakeName = "";
            ViewBag.MakeId = 0;
            MakeMaskingResponse objResponse = _objMakeCache.GetMakeMaskingResponse(makemaskingname);
            if (objResponse != null)
            {
                ScootersList = BindPopularScooters(objResponse.MakeId);
                BikeMakeEntityBase objMake = _objMakeRepo.GetMakeDetails(objResponse.MakeId);
                ViewBag.MakeName = objMake.MakeName;
                ViewBag.MakeId = objResponse.MakeId;

            }
            ViewBag.ScootersList = ScootersList;
            string versionList = string.Join(",", ScootersList.Select(m => m.objVersion.VersionId));
            ICollection<SimilarCompareBikeEntity> similarBikeList = BindSimilarBikes(versionList);
            ViewBag.similarBikeList = similarBikeList;
            return View("~/views/m/scooters/bikesbymake.cshtml");
        }
        /// <summary>
        /// Created By :- Subodh Jain 10 March 2017
        /// Summary :- Bind similar bike list 
        /// </summary>
        private ICollection<SimilarCompareBikeEntity> BindSimilarBikes(string versionList)
        {

            return _compareScooters.ScooterCompareList(versionList, 1, 1);
        }
        /// <summary>
        /// Created By :- Subodh Jain 10 March 2017
        /// Summary :- Bind popular bike list 
        /// </summary>
        private IEnumerable<MostPopularBikesBase> BindPopularScooters(uint makeId)
        {
            return _objBikeModel.GetMostPopularScooters(makeId);
        }
    }
}