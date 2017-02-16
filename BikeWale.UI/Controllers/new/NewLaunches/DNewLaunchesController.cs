using Bikewale.Entities.BikeData.NewLaunched;
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
        public ActionResult Index(string makeMaskingName, ushort? pageNumber)
        {
            return View("~/views/newlaunches/bikesbymake.cshtml");
        }

        [Route("newlaunches/makes/")]
        public ActionResult Makes(uint? makeId)
        {
            if (makeId != null && makeId.HasValue)
            {
                ViewBag.BrandCountList = (_newLaunches.GetMakeList(makeId.Value).Take(9);
                return PartialView("~/Views/Shared/_NewLaunchedByBrand.cshtml");
            }
            else
                return View();
        }

        [Route("newlaunches/year/{launchYear}/")]
        public ActionResult bikesByYear(string launchYear, ushort? pageNumber)
        {
            return View("~/views/newlaunches/bikesbyyear.cshtml");
        }
    }
}
