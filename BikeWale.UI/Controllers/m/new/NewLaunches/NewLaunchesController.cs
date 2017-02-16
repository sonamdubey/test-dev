using Bikewale.Entities.BikeData.NewLaunched;
using Bikewale.Entities.Pager;
using Bikewale.Interfaces.BikeData.NewLaunched;
using System;
using System.Collections.Generic;
using System.Linq;
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
        /// <summary>
        /// Modified By :- Subodh Jain 15 Feb 2017
        /// Summary:- Added make widget changes for landing page
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        [Route("m/newlaunches/")]
        public ActionResult Index(ushort? pageNumber)
        {
            ViewBag.PageNumber = (int)(pageNumber.HasValue ? pageNumber : 1);
            ViewBag.PageSize = 10;

            var objFilters = new InputFilter()
            {
                PageNo = ViewBag.PageNumber,
                PageSize = ViewBag.PageSize
            };
            ViewBag.Bikes = _newLaunches.GetBikes(objFilters);
            int TopCount = 6;
            IEnumerable<BikesCountByMakeEntityBase> makes = _newLaunches.GetMakeList();
            if (makes != null && makes.Count() > 0)
            {
                ViewBag.TopMakes = makes.Take(TopCount);
                ViewBag.OtherMakes = makes.Count() > TopCount ? makes.Skip(TopCount).OrderBy(m => m.Make.MakeName) : null;
            }
            ViewBag.Years = _newLaunches.YearList();
            ViewBag.Description = "Check out the latest bikes in India. Explore the recently launched bikes of Honda, Bajaj, Hero, Royal Enfield and other major brands.";
            ViewBag.Title = "New Bike Launches | Latest Bikes in India- BikeWale";
            ViewBag.Keywords = string.Format("new bikes {0}, new bike launches in {1}, just launched bikes, new bike arrivals, bikes just got launched", DateTime.Today.AddDays(-1).Year, DateTime.Today.Year);

            ViewBag.pager = new PagerEntity()
            {
                PageNo = ViewBag.PageNumber,
                PageSize = ViewBag.PageSize,
                PagerSlotSize = 5,
                BaseUrl = "/m/new-bikes-launches/",
                PageUrlType = "page/",
                TotalResults = (int)(ViewBag.Bikes != null ? ViewBag.Bikes.TotalCount : 0)
            };

            return View("~/views/m/newlaunches/index.cshtml");
        }

        [Route("m/newlaunches/make/{maskingName}/")]
        public ActionResult bikesByMake(string maskingName, ushort? pageNumber)
        {
            return View("~/views/m/newlaunches/bikesbymake.cshtml");
        }

        [Route("m/newlaunches/year/{launchYear}/")]
        public ActionResult bikesByYear(string launchYear, ushort? pageNumber)
        {
            return View("~/views/m/newlaunches/bikesbyyear.cshtml");
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
        public ActionResult Makes(uint? makeId)
        {
            if (makeId != null && makeId.HasValue)
            {
                ViewBag.BrandCountList = (_newLaunches.GetMakeList((uint)makeId)).Take(9);
                return PartialView("~/Views/m/Shared/_NewLaunchedByBrand.cshtml");
            }
            else
                return View();
        }

        [Route("m/newlaunches/years/")]
        public ActionResult Years()
        {
            IEnumerable<BikesCountByYearEntityBase> objYears = _newLaunches.YearList();
            return PartialView("~/views/m/shared/_newlaunchedbyyear.cshtml", objYears);
        }

        [Route("m/newlaunches/yearwise/make/{makeId}/")]
        public ActionResult YearwiseLaunches(int makeId)
        {
            return PartialView();
        }
    }
}