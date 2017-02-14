using System.Web.Mvc;

namespace Bikewale.Controllers.Mobile.NewLaunches
{
    public class NewLaunchesController : Controller
    {
        public ActionResult Index(ushort pageNumber)
        {
            return View();
        }

        public ActionResult Index(uint makeId, ushort pageNumber)
        {
            return View();
        }

        public ActionResult Index(string launchYear, ushort pageNumber)
        {
            return View();
        }

        /// <summary>
        /// Route for new bike launches models
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        [Route("m/newlaunches/models/")]
        public ActionResult Models(uint makeId, string launchYear, ushort pageNumber)
        {
            return View("~/views/m/newlaunches/index.cshtml");
        }

        [Route("m/newlaunches/makes/")]
        public ActionResult Makes(bool showCount)
        {
            return PartialView();
        }

        [Route("m/newlaunches/years/")]
        public ActionResult Years()
        {
            return PartialView();
        }

        [Route("m/newlaunches/yearwise/make/{makeId}/")]
        public ActionResult YearwiseLaunches(int makeId)
        {
            return PartialView();
        }
    }
}