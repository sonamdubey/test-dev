using Bikewale.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bikewale.Controllers
{
    public class UpcomingBikesController : Controller
    {
        // GET: UpcomingBikes
        [Route("upcomingbikes/")]
        public ActionResult Index()
        {
            ModelBase m = new ModelBase();
            return View(m);
        }
        
        // GET: UpcomingBikes
        [Route("m/upcomingbikes/")]
        public ActionResult Index_Mobile()
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