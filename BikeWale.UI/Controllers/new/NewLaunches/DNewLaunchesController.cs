﻿using Bikewale.Entities.BikeData;
using Bikewale.Entities.BikeData.NewLaunched;
using Bikewale.Entities.Location;
using Bikewale.Entities.Pager;
using Bikewale.Filters;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.NewLaunched;
using Bikewale.Interfaces.BikeData.UpComing;
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
        private readonly IUpcoming _upcoming = null;
        private GlobalCityAreaEntity _objLocation = GlobalCityArea.GetGlobalCityArea();

        public DNewLaunchesController(INewBikeLaunchesBL newLaunches, IBikeMakesCacheRepository<int> objMakeCache, IUpcoming upcoming)
        {
            _newLaunches = newLaunches;
            _objMakeCache = objMakeCache;
            _upcoming = upcoming;
        }
        /// <summary>
        /// Modified By :- Subodh Jain 15 Feb 2017
        /// Summary:- Added make widget changes for landing page
        /// Modified By :- Subodh Jain 20 March 2017
        /// Summary :- Changed title and description for page
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
                CityId = _objLocation.CityId,
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
            ViewBag.pageHeading = string.Format("Latest bikes in India - {0}", DateTime.Now.Year);

            var objFiltersUpcoming = new Bikewale.Entities.BikeData.UpcomingBikesListInputEntity()
            {
                EndIndex = 9,
                StartIndex = 1
            };
            var sortBy = Bikewale.Entities.BikeData.EnumUpcomingBikesFilter.Default;
            IEnumerable<UpcomingBikeEntity> objUpcomingBikes = _upcoming.GetModels(objFiltersUpcoming, sortBy);
            ViewBag.UpcomingBikes = objUpcomingBikes;
            ViewBag.Description = string.Format("Check out the latest bikes in India. Explore the bikes launched in {0}. Know more about prices, mileage,colors, specifications, and dealers of new bikes launches in {0}.", DateTime.Today.Year);
            ViewBag.Title = string.Format("New Bike Launches in {0} | Latest Bikes in India - BikeWale", DateTime.Today.Year);
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
            int pages = (int)(ViewBag.Bikes.TotalCount / ViewBag.PageSize);

            if ((ViewBag.Bikes.TotalCount % ViewBag.PageSize) > 0)
                pages += 1;
            string prevUrl = string.Empty, nextUrl = string.Empty;
            Paging.CreatePrevNextUrl(pages, "/new-bike-launches/", (int)ViewBag.PageNumber, ref nextUrl, ref prevUrl);
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
            ViewBag.location = _objLocation;
            ViewBag.MakeMaskingName = maskingName;
            var objFilters = new InputFilter()
            {
                PageNo = ViewBag.PageNumber,
                PageSize = ViewBag.PageSize,
                Make = objResponse.MakeId,
                CityId = _objLocation.CityId
            };

            var objBikes = _newLaunches.GetBikes(objFilters);
            ViewBag.Bikes = objBikes;

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
            ViewBag.Title = string.Format("{0} Bike Launches | Latest {0} Bikes in India- BikeWale", ViewBag.MakeName);
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


            int pages = (int)(ViewBag.Bikes.TotalCount / ViewBag.PageSize);

            if ((ViewBag.Bikes.TotalCount % ViewBag.PageSize) > 0)
                pages += 1;

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
            IEnumerable<UpcomingBikeEntity> objUpcomingBikes = _upcoming.GetModels(objFiltersUpcoming, sortBy);
            ViewBag.UpcomingBikes = objUpcomingBikes;
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

    }
}
