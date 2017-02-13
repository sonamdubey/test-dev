using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bikewale.Controllers.Mobile.NewLaunches
{
    public class NewLaunchesController : Controller
    {
        [Route("m/newlaunches/")]
        public ActionResult Index()
        {
            return View();
        }
    }
}