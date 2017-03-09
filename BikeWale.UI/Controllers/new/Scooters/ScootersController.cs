using Bikewale.Entities.BikeData.NewLaunched;
using Bikewale.Interfaces.BikeData.NewLaunched;
using System.Web.Mvc;

namespace Bikewale.Controllers.Desktop.Scooters
{
    public class ScootersController : Controller
    {
        private readonly INewBikeLaunchesBL _newLaunches = null;
        public ScootersController(INewBikeLaunchesBL newLaunches)
        {
            _newLaunches = newLaunches;
        }

        [Route("scooters/")]
        public ActionResult Index()
        {
            PopulateNewlaunch();
            return View("~/views/scooters/index.cshtml");
        }
        [Route("m/scooters/")]
        public ActionResult MIndex()
        {
            PopulateNewlaunch();
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
    }
}