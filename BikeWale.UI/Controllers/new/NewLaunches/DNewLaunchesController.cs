﻿using Bikewale.Entities.BikeData;
using Bikewale.Entities.BikeData.NewLaunched;
using Bikewale.Entities.Pager;
using Bikewale.Filters;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.NewLaunched;
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

            ViewBag.pager = new PagerEntity()
            {
                PageNo = ViewBag.PageNumber,
                PageSize = ViewBag.PageSize,
                PagerSlotSize = 5,
                BaseUrl = "/new-bikes-launches/",
                PageUrlType = "page/",
                TotalResults = (int)(ViewBag.Bikes != null ? ViewBag.Bikes.TotalCount : 0)
            };

            return View("~/views/newlaunches/index.cshtml");
        }

        [Route("newlaunches/make/{makeMaskingName}/")]
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

            ViewBag.Description = string.Format("Check out the latest {0} bikes in India. Know more about prices, mileage, colors, specifications, and dealers of recently launched {0} bikes.", ViewBag.MakeName.ToLower());
            ViewBag.Title = string.Format("{0} Bike Launches| Latest {0} Bikes in India- BikeWale", ViewBag.MakeName);
            ViewBag.Keywords = string.Format("new {2} bikes {0}, new {2} bike launches in {1}, just launched {2} bikes, new {2} bike arrivals, {2} bikes just got launched", DateTime.Today.AddDays(-1).Year, DateTime.Today.Year, ViewBag.MakeName.ToLower());

            ViewBag.pager = new PagerEntity()
            {
                PageNo = ViewBag.PageNumber,
                PageSize = ViewBag.PageSize,
                PagerSlotSize = 5,
                BaseUrl = string.Format("/new-{0}-bike-launches/", maskingName),
                PageUrlType = "page/",
                TotalResults = (int)(objBikes != null ? objBikes.TotalCount : 0)
            };

            return View("~/views/newlaunches/bikesbymake.cshtml");
        }


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

            ViewBag.Description = string.Format("Check out the latest bikes launched in {0}. Know more about prices, mileage, colors, specifications, and dealers of new bikes launched in {0}.", launchYear);
            ViewBag.Title = string.Format("Bike Launches in {0} | Latest Bikes launched in {0}- BikeWale", launchYear);
            ViewBag.Keywords = string.Format("new bikes {0}, new bike launches in {1}, just launched bikes, new bike arrivals, bikes just got launched", Convert.ToUInt32(launchYear) - 1, launchYear);

            ViewBag.pager = new PagerEntity()
            {
                PageNo = ViewBag.PageNumber,
                PageSize = ViewBag.PageSize,
                PagerSlotSize = 5,
                BaseUrl = string.Format("/new-bike-launches-in-{0}/", launchYear),
                PageUrlType = "page/",
                TotalResults = (int)(ViewBag.Bikes != null ? ViewBag.Bikes.TotalCount : 0)
            };

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
            return PartialView("~/Views/Shared/_NewLaunchedByBrand.cshtml");
        }


        [Route("newlaunches/years/")]
        public ActionResult Years()
        {
            IEnumerable<BikesCountByYearEntityBase> objYears = _newLaunches.YearList();
            return PartialView("~/Views/Shared/_NewLaunchedByYear.cshtml", objYears);
        }
    }
}
