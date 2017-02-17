using Bikewale.Entities.BikeData;
using Bikewale.Entities.BikeData.NewLaunched;
using Bikewale.Entities.Pager;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.NewLaunched;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace Bikewale.Controllers.Mobile.NewLaunches
{
    public class NewLaunchesController : Controller
    {
        private readonly INewBikeLaunchesBL _newLaunches = null;
        private readonly IBikeMakesCacheRepository<int> _objMakeCache = null;
        private readonly IBikeMakes<BikeMakeEntity, int> _objMakeRepo = null;
        private string nextUrl, prevUrl;

        public NewLaunchesController(INewBikeLaunchesBL newLaunches, IBikeMakesCacheRepository<int> objMakeCache)
        {
            _newLaunches = newLaunches;
            _objMakeCache = objMakeCache;
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
            ViewBag.Makes = makes;
            if (makes != null && makes.Count() > 0)
            {
                ViewBag.TopMakes = makes.Take(TopCount);
                ViewBag.OtherMakes = makes.Count() > TopCount ? makes.Skip(TopCount).OrderBy(m => m.Make.MakeName) : null;
            }
            ViewBag.Years = _newLaunches.YearList();
            ViewBag.Description = "Check out the latest bikes in India. Explore the recently launched bikes of Honda, Bajaj, Hero, Royal Enfield and other major brands.";
            ViewBag.Title = "New Bike Launches | Latest Bikes in India- BikeWale";
            ViewBag.Keywords = string.Format("new bikes {0}, new bike launches in {1}, just launched bikes, new bike arrivals, bikes just got launched", DateTime.Today.AddDays(-1).Year, DateTime.Today.Year);
            ViewBag.canonical = string.Format("{0}/new-bike-launches/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrlForJs);

            ViewBag.pager = new PagerEntity()
            {
                PageNo = ViewBag.PageNumber,
                PageSize = ViewBag.PageSize,
                PagerSlotSize = 5,
                BaseUrl = "/m/new-bike-launches/",
                PageUrlType = "page/",
                TotalResults = (int)(ViewBag.Bikes != null ? ViewBag.Bikes.TotalCount : 0)
            };
            Paging.CreatePrevNextUrl((int)ViewBag.Bikes.TotalCount, "/m/new-bike-launches/", (int)ViewBag.PageNumber, ref prevUrl, ref nextUrl);
            ViewBag.relPrevPageUrl = prevUrl;
            ViewBag.relNextPageUrl = nextUrl;
            return View("~/views/m/newlaunches/index.cshtml");
        }

        [Route("m/newlaunches/make/{maskingName}/")]
        public ActionResult BikeByMake(string maskingName, ushort? pageNumber)
        {
            ViewBag.PageNumber = (int)(pageNumber.HasValue ? pageNumber : 1);
            ViewBag.PageSize = 10;
            MakeMaskingResponse objResponse = _objMakeCache.GetMakeMaskingResponse(maskingName);

            var objFilters = new InputFilter()
            {
                PageNo = ViewBag.PageNumber,
                PageSize = ViewBag.PageSize,
                Make = objResponse.MakeId
            };

            var objBikes = _newLaunches.GetBikes(objFilters);
            ViewBag.Bikes = objBikes;
            ViewBag.Years = _newLaunches.YearList();

            ViewBag.MakeName = "";
            ViewBag.MakeId = objResponse.MakeId;

            if (objBikes != null && objBikes.TotalCount > 0)
            {
                ViewBag.MakeName = objBikes.Bikes.First().Make.MakeName;
            }
            else
            {
                BikeMakeEntityBase objMake = _objMakeRepo.GetMakeDetails(objResponse.MakeId);
                ViewBag.MakeName = objMake.MakeName;
            }

            ViewBag.Description = string.Format("Check out the latest {0} bikes in India. Know more about prices, mileage, colors, specifications, and dealers of recently launched {0} bikes.", ViewBag.MakeName.ToLower());
            ViewBag.Title = string.Format("{0} Bike Launches| Latest {0} Bikes in India- BikeWale", ViewBag.MakeName);
            ViewBag.Keywords = string.Format("new {2} bikes {0}, new {2} bike launches in {1}, just launched {2} bikes, new {2} bike arrivals, {2} bikes just got launched", DateTime.Today.AddDays(-1).Year, DateTime.Today.Year, ViewBag.MakeName.ToLower());
            ViewBag.canonical = string.Format("{0}/new-{1}-bike-launches/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrlForJs, objResponse.MaskingName);

            ViewBag.pager = new PagerEntity()
            {
                PageNo = ViewBag.PageNumber,
                PageSize = ViewBag.PageSize,
                PagerSlotSize = 5,
                BaseUrl = string.Format("/m/new-{0}-bikes-launches/", maskingName),
                PageUrlType = "page/",
                TotalResults = (int)(objBikes != null ? objBikes.TotalCount : 0)
            };

            Paging.CreatePrevNextUrl((int)objBikes.TotalCount, string.Format("/m/new-{0}-bike-launches/", maskingName), (int)ViewBag.PageNumber, ref prevUrl, ref nextUrl);
            ViewBag.relPrevPageUrl = prevUrl;
            ViewBag.relNextPageUrl = nextUrl;

            return View("~/views/m/newlaunches/bikesbymake.cshtml");
        }

        [Route("m/newlaunches/year/{launchYear}/")]
        public ActionResult BikeByYear(string launchYear, ushort? pageNumber)
        {
            ViewBag.PageNumber = (int)(pageNumber.HasValue ? pageNumber : 1);
            ViewBag.PageSize = 10;

            var objFilters = new InputFilter()
            {
                PageNo = ViewBag.PageNumber,
                PageSize = ViewBag.PageSize,
                YearLaunch = Convert.ToUInt32(launchYear)
            };

            ViewBag.Bikes = _newLaunches.GetBikes(objFilters);
            IEnumerable<BikesCountByMakeEntityBase> makes = _newLaunches.GetMakeList();
            ViewBag.Makes = makes;

            ViewBag.launchYear = launchYear;

            ViewBag.Description = string.Format("Check out the latest bikes launched in {0}. Know more about prices, mileage, colors, specifications, and dealers of new bikes launched in {0}.", launchYear);
            ViewBag.Title = string.Format("Bike Launches in {0} | Latest Bikes launched in {0}- BikeWale", launchYear);
            ViewBag.Keywords = string.Format("new bikes {0}, new bike launches in {1}, just launched bikes, new bike arrivals, bikes just got launched", Convert.ToUInt32(launchYear) - 1, launchYear);
            ViewBag.canonical = string.Format("{0}/new-bike-launches-in-{1}/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrlForJs, launchYear);

            ViewBag.pager = new PagerEntity()
            {
                PageNo = ViewBag.PageNumber,
                PageSize = ViewBag.PageSize,
                PagerSlotSize = 5,
                BaseUrl = string.Format("/m/new-bike-launches-in-{0}/", launchYear),
                PageUrlType = "page/",
                TotalResults = (int)(ViewBag.Bikes != null ? ViewBag.Bikes.TotalCount : 0)
            };

            Paging.CreatePrevNextUrl((int)ViewBag.Bikes.TotalCount, string.Format("/m/new-bike-launches-in-{0}/", launchYear), (int)ViewBag.PageNumber, ref prevUrl, ref nextUrl);
            ViewBag.relPrevPageUrl = prevUrl;
            ViewBag.relNextPageUrl = nextUrl;

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
            return View("~/views/m/shared/_newlaunchedbymakewithoutcount.cshtml");
        }

        [Route("m/newlaunches/makes/")]
        public ActionResult Makes(uint? makeId)
        {
            if (makeId != null && makeId.HasValue)
            {
                ViewBag.BrandCountList = _newLaunches.GetMakeList(makeId.Value).Take(9);
            }
            else
            {
                ViewBag.BrandCountList = _newLaunches.GetMakeList().Take(9);
            }
            return PartialView("~/Views/m/Shared/_NewLaunchedByMakeWithCount.cshtml", ViewBag.BrandCountList);
        }

        /// <summary>
        /// modified by : Sajal Gupta on 16-02-2017
        /// Description : skip year from list which is present.
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        [Route("m/newlaunches/years/")]
        public ActionResult Years(int? year)
        {
            IEnumerable<BikesCountByYearEntityBase> objYears = _newLaunches.YearList();

            if (year != null && year.HasValue)
                objYears = objYears.Where(x => x.Year != year.Value);

            return PartialView("~/views/m/shared/_newlaunchedbyyear.cshtml", objYears);
        }

        [Route("m/newlaunches/yearwise/make/{makeId}/")]
        public ActionResult YearwiseLaunches(int makeId)
        {
            return PartialView();
        }

    }
}