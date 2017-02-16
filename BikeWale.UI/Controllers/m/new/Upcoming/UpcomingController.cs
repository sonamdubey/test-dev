using Bikewale.Interfaces.BikeData.UpComing;
using System.Web.Mvc;

namespace Bikewale.Controllers.Mobile.Upcoming
{
    public class UpcomingController : Controller
    {
        private readonly IUpcomingBL _upcoming = null;

        public UpcomingController(IUpcomingBL upcoming)
        {
            _upcoming = upcoming;
        }
        [Route("m/newlaunches/")]
        public ActionResult Index()
        {

            //ViewBag.UpComingList = _upcoming.GetUpComingBike();
            return View("~/views/m/newlaunches/index.cshtml");
        }
    }
}