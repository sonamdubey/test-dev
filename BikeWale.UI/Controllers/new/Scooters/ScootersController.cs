using Bikewale.Entities.BikeData;
using Bikewale.Entities.BikeData.NewLaunched;
using Bikewale.Interfaces.BikeData.NewLaunched;
using Bikewale.Interfaces.BikeData.UpComing;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Bikewale.Controllers.Desktop.Scooters
{
    public class ScootersController : Controller
    {
        private readonly INewBikeLaunchesBL _newLaunches = null;
        private readonly IUpcoming _upcoming = null;
        public ScootersController(INewBikeLaunchesBL newLaunches, IUpcoming upcoming)
        {
            _newLaunches = newLaunches;
            _upcoming = upcoming;
        }

        [Route("scooters/")]
        public ActionResult Index()
        {
            PopulateNewlaunch();
            Upcoming();
            return View("~/views/scooters/index.cshtml");
        }
        [Route("m/scooters/")]
        public ActionResult MIndex()
        {
            PopulateNewlaunch();
            Upcoming();
            return View("~/views/m/scooters/index.cshtml");
        }
        /// <summary>
        /// Created By :- Subodh Jain 09 March 2017
        /// Summary :- Populate Upcoming scooters
        /// </summary>
        private void Upcoming()
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
        [Route("scooters/make/")]
        public ActionResult BikesByMake()
        {
            return View("~/views/scooters/bikesbymake.cshtml");
        }
    }
}