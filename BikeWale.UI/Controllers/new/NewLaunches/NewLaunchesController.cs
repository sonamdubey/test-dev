using Bikewale.Interfaces.BikeData.NewLaunched;
using System.Web.Mvc;

namespace Bikewale.Controllers.Desktop.NewLaunches
{
    public class NewLaunchesController : Controller
    {
        private readonly INewBikeLaunchesBL _newLaunches = null;

        public NewLaunchesController(INewBikeLaunchesBL newLaunches)
        {
            _newLaunches = newLaunches;
        }

        [Route("newlaunches/")]
        public ActionResult Index()
        {
            ViewBag.Years = _newLaunches.YearList();
            return View("~/views/newlaunches/index.cshtml");
        }

        [Route("newlaunches/make/{makeMaskingName}/")]
        public ActionResult Index(string makeMaskingName, ushort? pageNumber)
        {
            return View("~/views/newlaunches/bikesbymake.cshtml");
        }
    }
}