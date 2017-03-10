using Bikewale.Entities.BikeData.NewLaunched;
using Bikewale.Interfaces.BikeData.NewLaunched;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Models.Shared;
using Bikewale.Utility;
using System.Web.Mvc;

namespace Bikewale.Controllers.Desktop.Scooters
{
    public class ScootersController : Controller
    {
        private readonly INewBikeLaunchesBL _newLaunches = null;
        private readonly IBikeModels<BikeModelEntity, int> _models = null;
        public ScootersController(IBikeModels<BikeModelEntity,int> models,INewBikeLaunchesBL newLaunches)
        {
            _newLaunches = newLaunches;
            _models = models;
        }

        [Route("scooters/")]
        public ActionResult Index()
        {
            PopulateNewlaunch();
            PopulatePopularScooters();
            return View("~/views/scooters/index.cshtml");
        }
        /// <summary>
        /// Created by : Aditi Srivastava on 9 Mar 2017
        /// Summary    : get list of popular scooters
        /// </summary>
        private void PopulatePopularScooters()
        {
            uint cityId = GlobalCityArea.GetGlobalCityArea().CityId;
            uint topCount = 9;
            PopularScootersList objScooters = new PopularScootersList();
            objScooters.PopularScooters = _models.GetMostPopularScooters(topCount, cityId);
            
            objScooters.PQSourceId = 86;
            objScooters.PageCatId = 62;
            ViewBag.popularScooters = objScooters;
        }
        [Route("m/scooters/")]
        public ActionResult MIndex()
        {
            PopulateNewlaunch();
            MPopulatePopularScooters();
            return View("~/views/m/scooters/index.cshtml");
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
        [Route("scooters/make/")]
        public ActionResult BikesByMake()
        {
            return View("~/views/scooters/bikesbymake.cshtml");
        }

        
        /// <summary>
        /// Created by : Aditi Srivastava on 9 Mar 2017
        /// Summary    : get list of popular scooters
        /// </summary>
        private void MPopulatePopularScooters()
        {
            uint cityId = GlobalCityArea.GetGlobalCityArea().CityId;
            uint topCount = 9;
            PopularScootersList objScooters = new PopularScootersList();
            objScooters.PopularScooters = _models.GetMostPopularScooters(topCount, cityId);

            objScooters.PQSourceId = 87;
            objScooters.PageCatId = 63;
            ViewBag.popularScooters = objScooters;
        }
        [Route("m/scooters/make/")]
        public ActionResult MBikesByMake()
        {
            return View("~/views/m/scooters/bikesbymake.cshtml");
        }

    }
}