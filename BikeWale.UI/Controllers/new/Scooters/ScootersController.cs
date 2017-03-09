using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bikewale.Controllers.Desktop.Scooters
{
    public class ScootersController : Controller
    {
        // GET: Scooters
        [Route("scooters/")]
        public ActionResult Index()
        {
            return View("~/views/scooters/index.cshtml");
        }
    }
}