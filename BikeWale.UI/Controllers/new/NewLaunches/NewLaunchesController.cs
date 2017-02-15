using Bikewale.Interfaces.BikeData.NewLaunched;
using System.Linq;
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

        [Route("newlaunches/makes/")]
        public ActionResult Makes(uint? makeId)
        {
            if (makeId != null && makeId.HasValue)
            {
                ViewBag.BrandCountList = (_newLaunches.GetMakeList((uint)makeId)).Take(6);
                return PartialView("~/Views/Shared/_NewLaunchedByBrand.cshtml");
            }
            else
                return View();
        }
    }
}