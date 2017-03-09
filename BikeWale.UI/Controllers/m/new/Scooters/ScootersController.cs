using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bikewale.Controllers.Mobile
{
    public class ScootersController : Controller
    {
        // GET: Scooters
        [Route("m/scooters/")]
        public ActionResult Index()
        {
            return View("~/views/m/scooters/index.cshtml");
        }

        [Route("m/scooters/make/")]
        public ActionResult BikesByMake()
        {
            return View("~/views/m/scooters/bikesbymake.cshtml");
        }
    }
}