using Bikewale.Entities.BikeData.NewLaunched;
using Bikewale.Filters;
using Bikewale.Interfaces.BikeData.NewLaunched;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace Bikewale.Controllers.Desktop.NewLaunches
{
    public class DNewLaunchesController : Controller
    {
        private readonly INewBikeLaunchesBL _newLaunches = null;

        public DNewLaunchesController(INewBikeLaunchesBL newLaunches)
        {
            _newLaunches = newLaunches;
        }
        /// <summary>
        /// Modified By :- Subodh Jain 15 Feb 2017
        /// Summary:- Added make widget changes for landing page
        /// </summary>
        /// <param name="pageNumber"></param>
        [Route("newlaunches/")]
        [DeviceDetection]
        public ActionResult Index()
        {
            int TopCount = 10;
            IEnumerable<BikesCountByMakeEntityBase> makes = _newLaunches.GetMakeList();
            if (makes != null && makes.Count() > 0)
            {
                ViewBag.TopMakes = makes.Take(TopCount);
                ViewBag.OtherMakes = makes.Count() > TopCount ? makes.Skip(TopCount).OrderBy(m => m.Make.MakeName) : null;
            }
            ViewBag.Years = _newLaunches.YearList();
            return View("~/views/newlaunches/index.cshtml");
        }

        [Route("newlaunches/make/{makeMaskingName}/")]
        [DeviceDetection]
        public ActionResult Index(string makeMaskingName, ushort? pageNumber)
        {
            return View("~/views/newlaunches/bikesbymake.cshtml");
        }

        [Route("newlaunches/makes/")]
        [DeviceDetection]
        public ActionResult Makes(uint? makeId)
        {
            if (makeId != null && makeId.HasValue)
            {
                ViewBag.BrandCountList = (_newLaunches.GetMakeList(makeId.Value).Take(9));
            }
            else
            {
                ViewBag.BrandCountList = (_newLaunches.GetMakeList().Take(9));
            }
            return PartialView("~/Views/Shared/_NewLaunchedByMakeWithCount.cshtml", ViewBag.BrandCountList);
        }

        [Route("newlaunches/year/{launchYear}/")]
        [DeviceDetection]
        public ActionResult bikesByYear(string launchYear, ushort? pageNumber)
        {
            return View("~/views/newlaunches/bikesbyyear.cshtml");
        }

        /// <summary>
        /// modified by : Sajal Gupta on 16-02-2017
        /// Description : skip year from list which is present.
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        [Route("newlaunches/years/")]
        public ActionResult Years(int? year)
        {
            IEnumerable<BikesCountByYearEntityBase> objYears = _newLaunches.YearList();

            if (year != null && year.HasValue)
                objYears = objYears.Where(x => x.Year != year.Value);

            return PartialView("~/Views/Shared/_NewLaunchedByYear.cshtml", objYears);
        }
    }
}
