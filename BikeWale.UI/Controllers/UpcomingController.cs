using Bikewale.Interfaces.BikeData.NewLaunched;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Models;
using System.Web.Mvc;

namespace Bikewale.Controllers
{
    public class UpcomingController : Controller
    {
        private IUpcoming _upcoming = null;
        private readonly INewBikeLaunchesBL _newLaunches = null;
        public UpcomingController(IUpcoming upcoming, INewBikeLaunchesBL newLaunches)
        {
            _upcoming = upcoming;
            _newLaunches = newLaunches;
        }
        // GET: UpcomingBikes
        [Route("upcomingbikes/")]
        public ActionResult Index()
        {
            UpcomingPageModel objData = new UpcomingPageModel(10, _upcoming, _newLaunches);
            UpcomingPageVM vm = objData.GetData();
            return View(vm);
        }

        // GET: UpcomingBikes
        [Route("m/upcomingbikes/")]
        public ActionResult Index_Mobile()
        {
            ModelBase m = new ModelBase();
            return View(m);
        }

        // GET: UpcomingBikes by Make
        [Route("m/upcomingbikes/make/")]
        public ActionResult UpcomingBikesByMake_Mobile()
        {
            ModelBase m = new ModelBase();
            return View(m);
        }
    }
}