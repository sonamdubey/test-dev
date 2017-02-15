using Bikewale.Entities.BikeData.NewLaunched;
using Bikewale.Interfaces.BikeData.NewLaunched;
using System;
using System.Web.Mvc;

namespace Bikewale.Controllers.Mobile.NewLaunches
{
    public class NewLaunchesController : Controller
    {
        private readonly INewBikeLaunchesBL _newLaunches = null;

        public NewLaunchesController(INewBikeLaunchesBL newLaunches)
        {
            _newLaunches = newLaunches;
        }

        [Route("m/newlaunches/")]
        public ActionResult Index(ushort? pageNumber)
        {
            var objFilters = new InputFilter()
            {
                PageNo = (int)(pageNumber.HasValue ? pageNumber : 1),
                PageSize = 10
            };
            ViewBag.Bikes = _newLaunches.GetBikes(objFilters);
            ViewBag.Description = "Check out the latest bikes in India. Explore the recently launched bikes of Honda, Bajaj, Hero, Royal Enfield and other major brands.";
            ViewBag.Title = "New Bike Launches| Latest Bikes in India- BikeWale";
            ViewBag.Keywords = string.Format("new bikes {0}, new bike launches in {1}, just launched bikes, new bike arrivals, bikes just got launched", DateTime.Today.AddDays(-1).Year, DateTime.Today.Year);

            return View("~/views/m/newlaunches/index.cshtml");
        }

        [Route("m/newlaunches/make/{makeId}/")]
        public ActionResult Index(uint makeId, ushort? pageNumber)
        {
            return View();
        }

        [Route("m/newlaunches/year/{launchYear}/")]
        public ActionResult Index(string launchYear, ushort? pageNumber)
        {
            return View();
        }

        /// <summary>
        /// Route for new bike launches models
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        [Route("m/newlaunches/models/")]
        public ActionResult Models()
        {
            return View("~/views/m/shared/_newlaunchedbymake.cshtml");
        }

        [Route("m/newlaunches/makes/")]
        public ActionResult Makes(bool showCount)
        {
            return PartialView("~/views/m/shared/_newlaunchedbymake.cshtml");
        }

        [Route("m/newlaunches/years/")]
        public ActionResult Years()
        {
            return PartialView("~/views/m/shared/_newlaunchedbyyear.cshtml");
        }

        [Route("m/newlaunches/yearwise/make/{makeId}/")]
        public ActionResult YearwiseLaunches(int makeId)
        {
            return PartialView();
        }
    }
}