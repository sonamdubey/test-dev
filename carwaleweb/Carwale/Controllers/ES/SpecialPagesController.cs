using Carwale.UI.ClientBL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Carwale.UI.Controllers.ES
{
    public class SpecialPagesController : Controller
    {
        [Route("specials/volkswagen-polo/")]
        public ActionResult VwPoloSpecial()
        {
            ViewBag.isMobile = false;
            if (DeviceDetectionManager.IsMobile(this.HttpContext))
            {
                ViewBag.isMobile = true;
            }

            return View("~/Views/ES/VWPoloSpotlight.cshtml");
        }

        [Route("specials/volkswagen-vento/")]
        public ActionResult VwVentoSpecial()
        {
            ViewBag.isMobile = false;
            if (DeviceDetectionManager.IsMobile(this.HttpContext))
            {
                ViewBag.isMobile = true;
            }

            return View("~/Views/ES/VWVentoSpotlight.cshtml");
        }

        [Route("specials/volkswagen-ameo/"), Route("ameospecials")]
        public ActionResult VwAmeoSpecial()
        {
            ViewBag.isMobile = false;
            if (DeviceDetectionManager.IsMobile(this.HttpContext))
            {
                ViewBag.isMobile = true;
            }

            return View("~/Views/ES/VWAmeoSpotlight.cshtml");
        }
    }
}