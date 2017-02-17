using Bikewale.Entities.BikeData;
using Bikewale.Entities.BikeData.NewLaunched;
using Bikewale.Entities.Location;
using Bikewale.Entities.Pager;
using Bikewale.Filters;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.NewLaunched;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace Bikewale.Controllers.Desktop.NewLaunches
{
    public class DNewLaunchesController : Controller
    {
        private readonly INewBikeLaunchesBL _newLaunches = null;
        private readonly IBikeMakesCacheRepository<int> _objMakeCache = null;
        private readonly IBikeMakes<BikeMakeEntity, int> _objMakeRepo = null;
        private GlobalCityAreaEntity _objLocation = GlobalCityArea.GetGlobalCityArea();

        public DNewLaunchesController(INewBikeLaunchesBL newLaunches, IBikeMakesCacheRepository<int> objMakeCache)
        {
            _newLaunches = newLaunches;
            _objMakeCache = objMakeCache;
        }
        /// <summary>
        /// Modified By :- Subodh Jain 15 Feb 2017
        /// Summary:- Added make widget changes for landing page
        /// </summary>
        /// <param name="pageNumber"></param>
        [Route("newlaunches/")]
        [DeviceDetection]
        public ActionResult Index(ushort? pageNumber)
        {
            ViewBag.PageNumber = (int)(pageNumber.HasValue ? pageNumber : 1);
            ViewBag.PageSize = 15;

            var objFilters = new InputFilter()
            {
                PageNo = ViewBag.PageNumber,
                PageSize = ViewBag.PageSize
            };
            ViewBag.Bikes = _newLaunches.GetBikes(objFilters);
            int TopCount = 10;
            IEnumerable<BikesCountByMakeEntityBase> makes = _newLaunches.GetMakeList();
            ViewBag.Makes = makes;
            if (makes != null && makes.Count() > 0)
            {
                ViewBag.TopMakes = makes.Take(TopCount);
                ViewBag.OtherMakes = makes.Count() > TopCount ? makes.Skip(TopCount).OrderBy(m => m.Make.MakeName) : null;
            }
            ViewBag.pageHeading = "Latest bikes in India";
            ViewBag.Years = _newLaunches.YearList();

            var objFiltersUpcoming = new Bikewale.Entities.BikeData.UpcomingBikesListInputEntity()
            {
                EndIndex = 9,
                StartIndex = 1
            };
            var sortBy = Bikewale.Entities.BikeData.EnumUpcomingBikesFilter.Default;
            ViewBag.Filters = objFiltersUpcoming;
            ViewBag.SortBy = sortBy;

            ViewBag.Description = "Check out the latest bikes in India. Explore the recently launched bikes of Honda, Bajaj, Hero, Royal Enfield and other major brands.";
            ViewBag.Title = "New Bike Launches | Latest Bikes in India- BikeWale";
            ViewBag.Keywords = string.Format("new bikes {0}, new bike launches in {1}, just launched bikes, new bike arrivals, bikes just got launched", DateTime.Today.AddDays(-1).Year, DateTime.Today.Year);
            ViewBag.canonical = string.Format("{0}/new-bike-launches/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrlForJs);
            ViewBag.alternate = string.Format("{0}/m/new-bike-launches/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrlForJs);
            ViewBag.pager = new PagerEntity()
            {
                PageNo = ViewBag.PageNumber,
                PageSize = ViewBag.PageSize,
                PagerSlotSize = 5,
                BaseUrl = "/new-bike-launches/",
                PageUrlType = "page/",
                TotalResults = (int)(ViewBag.Bikes != null ? ViewBag.Bikes.TotalCount : 0)
            };
            ViewBag.location = _objLocation;
            string prevUrl = string.Empty, nextUrl = string.Empty;
            Paging.CreatePrevNextUrl((int)ViewBag.Bikes.TotalCount, "/new-bike-launches/", (int)ViewBag.PageNumber, ref nextUrl, ref prevUrl);
            ViewBag.relPrevPageUrl = prevUrl;
            ViewBag.relNextPageUrl = nextUrl;

            return View("~/views/newlaunches/index.cshtml");
        }

        /// <summary>
        /// Modified By :- Subodh Jain 17 Feb 2017
        /// Summary ;- Added upcoming widget
        /// </summary>
        /// <param name="launchYear"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        [Route("newlaunches/make/{maskingName}/")]
        [DeviceDetection]
        public ActionResult BikeByMake(string maskingName, ushort? pageNumber)
        {
            ViewBag.PageNumber = (int)(pageNumber.HasValue ? pageNumber : 1);
            ViewBag.PageSize = 15;
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
            ViewBag.pageHeading = string.Format("Latest {0} bikes in India", ViewBag.MakeName);
            ViewBag.Description = string.Format("Check out the latest {0} bikes in India. Know more about prices, mileage, colors, specifications, and dealers of recently launched {0} bikes.", ViewBag.MakeName.ToLower());
            ViewBag.Title = string.Format("{0} Bike Launches| Latest {0} Bikes in India- BikeWale", ViewBag.MakeName);
            ViewBag.Keywords = string.Format("new {2} bikes {0}, new {2} bike launches in {1}, just launched {2} bikes, new {2} bike arrivals, {2} bikes just got launched", DateTime.Today.AddDays(-1).Year, DateTime.Today.Year, ViewBag.MakeName.ToLower());
            ViewBag.canonical = string.Format("{0}/new-{1}-bike-launches/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrlForJs, objResponse.MaskingName);
            ViewBag.alternate = string.Format("{0}/m/new-{1}-bike-launches/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrlForJs, objResponse.MaskingName);
            ViewBag.pager = new PagerEntity()
            {
                PageNo = ViewBag.PageNumber,
                PageSize = ViewBag.PageSize,
                PagerSlotSize = 5,
                BaseUrl = string.Format("/new-{0}-bike-launches/", maskingName),
                PageUrlType = "page/",
                TotalResults = (int)(objBikes != null ? objBikes.TotalCount : 0)
            };
            ViewBag.location = _objLocation;
            string prevUrl = string.Empty, nextUrl = string.Empty;
            Paging.CreatePrevNextUrl((int)objBikes.TotalCount, string.Format("/new-{0}-bike-launches/", maskingName), (int)ViewBag.PageNumber, ref nextUrl, ref prevUrl);
            ViewBag.relPrevPageUrl = prevUrl;
            ViewBag.relNextPageUrl = nextUrl;
            var objFiltersUpcoming = new Bikewale.Entities.BikeData.UpcomingBikesListInputEntity()
            {
                MakeId = (int)objResponse.MakeId,
                EndIndex = 9,
                StartIndex = 1
            };
            var sortBy = Bikewale.Entities.BikeData.EnumUpcomingBikesFilter.Default;
            ViewBag.Filters = objFiltersUpcoming;
            ViewBag.SortBy = sortBy;
            return View("~/views/newlaunches/bikesbymake.cshtml");
        }

        /// <summary>
        /// Modified By :- Subodh Jain 17 Feb 2017
        /// Summary ;- Added upcoming widget
        /// </summary>
        /// <param name="launchYear"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        [Route("newlaunches/year/{launchYear}/")]
        public ActionResult BikeByYear(string launchYear, ushort? pageNumber)
        {
            ViewBag.PageNumber = (int)(pageNumber.HasValue ? pageNumber : 1);
            ViewBag.PageSize = 15;

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
            ViewBag.pageHeading = string.Format("Latest bikes in India- {0}", launchYear);
            ViewBag.Description = string.Format("Check out the latest bikes launched in {0}. Know more about prices, mileage, colors, specifications, and dealers of new bikes launched in {0}.", launchYear);
            ViewBag.Title = string.Format("Bike Launches in {0} | Latest Bikes launched in {0}- BikeWale", launchYear);
            ViewBag.Keywords = string.Format("new bikes {0}, new bike launches in {1}, just launched bikes, new bike arrivals, bikes just got launched", Convert.ToUInt32(launchYear) - 1, launchYear);
            ViewBag.canonical = string.Format("{0}/new-bike-launches-in-{1}/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrlForJs, launchYear);
            ViewBag.alternate = string.Format("{0}/m/new-bike-launches-in-{1}/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrlForJs, launchYear);
            ViewBag.pager = new PagerEntity()
            {
                PageNo = ViewBag.PageNumber,
                PageSize = ViewBag.PageSize,
                PagerSlotSize = 5,
                BaseUrl = string.Format("/new-bike-launches-in-{0}/", launchYear),
                PageUrlType = "page/",
                TotalResults = (int)(ViewBag.Bikes != null ? ViewBag.Bikes.TotalCount : 0)
            };
            ViewBag.location = _objLocation;
            string prevUrl = string.Empty, nextUrl = string.Empty;
            Paging.CreatePrevNextUrl((int)ViewBag.Bikes.TotalCount, string.Format("/new-bike-launches-in-{0}/", launchYear), (int)ViewBag.PageNumber, ref nextUrl, ref prevUrl);
            ViewBag.relPrevPageUrl = prevUrl;
            ViewBag.relNextPageUrl = nextUrl;
            var objFiltersUpcoming = new Bikewale.Entities.BikeData.UpcomingBikesListInputEntity()
            {
                EndIndex = 9,
                StartIndex = 1
            };
            var sortBy = Bikewale.Entities.BikeData.EnumUpcomingBikesFilter.Default;
            ViewBag.Filters = objFiltersUpcoming;
            ViewBag.SortBy = sortBy;
            return View("~/views/newlaunches/bikesbyyear.cshtml");
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
