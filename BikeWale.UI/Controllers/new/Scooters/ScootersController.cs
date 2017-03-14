using Bikewale.Entities.BikeData;
using Bikewale.Entities.BikeData.NewLaunched;
using Bikewale.Entities.Compare;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.NewLaunched;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.Compare;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Bikewale.Controllers.Desktop.Scooters
{
    public class ScootersController : Controller
    {
        private readonly INewBikeLaunchesBL _newLaunches = null;
        private readonly IBikeMakes<BikeMakeEntity, int> _objMakeRepo = null;
        private readonly IUpcoming _upcoming = null;
        private readonly IBikeCompareCacheRepository _compareScooters = null;
        private readonly IBikeMakesCacheRepository<int> _objMakeCache = null;
        private readonly IBikeModels<BikeModelEntity, int> _objBikeModel = null;
        public ScootersController(INewBikeLaunchesBL newLaunches, IUpcoming upcoming, IBikeCompareCacheRepository compareScooters, IBikeMakesCacheRepository<int> objMakeCache, IBikeModels<BikeModelEntity, int> objBikeModel, IBikeMakes<BikeMakeEntity, int> objMakeRepo)
        {
            _newLaunches = newLaunches;
            _upcoming = upcoming;
            _compareScooters = compareScooters;
            _objMakeCache = objMakeCache;
            _objBikeModel = objBikeModel;
            _objMakeRepo = objMakeRepo;
        }

        [Route("scooters/")]
        public ActionResult Index()
        {
            PopulateNewlaunch();
            UpcomingScooters();
            CompareScootersList();

            return View("~/views/scooters/index.cshtml");
        }
        [Route("m/scooters/")]
        public ActionResult MIndex()
        {
            PopulateNewlaunch();
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

            }
            ViewBag.ScootersList = ScootersList;
            string versionList = string.Join(",", ScootersList.Select(m => m.objVersion.VersionId));
            ICollection<SimilarCompareBikeEntity> similarBikeList = BindSimilarBikes(versionList);
            ViewBag.similarBikeList = similarBikeList;
            return View("~/views/scooters/bikesbymake.cshtml");
        }
        [Route("m/scooters/make/{makemaskingname}/")]
        public ActionResult MBikesByMake(string makemaskingname)
        {
            IEnumerable<MostPopularBikesBase> ScootersList = null;
            ViewBag.MakeName = "";
            MakeMaskingResponse objResponse = _objMakeCache.GetMakeMaskingResponse(makemaskingname);
            if (objResponse != null)
            {
                ScootersList = BindPopularScooters(objResponse.MakeId);
                BikeMakeEntityBase objMake = _objMakeRepo.GetMakeDetails(objResponse.MakeId);
                ViewBag.MakeName = objMake.MakeName;

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