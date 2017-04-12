﻿using Bikewale.Filters;
using Bikewale.Interfaces.BikeData.NewLaunched;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Models;
using System.Web.Mvc;

namespace Bikewale.Controllers
{
    public class UpcomingController : Controller
    {
        private IUpcoming _upcoming = null;
        private readonly INewBikeLaunchesBL _newLaunches = null;
        public UpcomingController(IUpcoming upcoming, INewBikeLaunchesBL newLaunches)
        {
            _upcoming = upcoming;
            _newLaunches = newLaunches;
        }
        // GET: UpcomingBikes
        [Route("upcomingbikes/")]
        [DeviceDetection]
        public ActionResult Index(ushort? pageNumber)
        {
            UpcomingPageModel objData = null;
            if (pageNumber.HasValue)
            {
                objData = new UpcomingPageModel(_upcoming, (ushort)pageNumber, _newLaunches);
            }
            else
            {
                objData = new UpcomingPageModel(_upcoming, 1, _newLaunches);
            }
            UpcomingPageVM objVM = objData.GetData();
            return View(objVM);
        }

        // GET: UpcomingBikes
        [Route("m/upcomingbikes/")]
        public ActionResult Index_Mobile(ushort? pageNumber)
        {
            UpcomingPageModel objData = null;
            if (pageNumber.HasValue)
            {
                objData = new UpcomingPageModel(_upcoming, (ushort)pageNumber, _newLaunches);
            }
            else
            {
                objData = new UpcomingPageModel(_upcoming, 1, _newLaunches);
            }
            UpcomingPageVM objVM = objData.GetData();
            return View(objVM);
        }

        // GET: UpcomingBikes by Make
        [Route("upcomingbikes/make/")]
        public ActionResult UpcomingBikesByMake()
        {
            ModelBase m = new ModelBase();
            return View(m);
        }

        // GET: UpcomingBikes by Make
        [Route("m/upcomingbikes/make/")]
        public ActionResult UpcomingBikesByMake_Mobile()
        {
            ModelBase m = new ModelBase();
            return View(m);
        }
    }
}