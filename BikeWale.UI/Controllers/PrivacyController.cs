using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bikewale.Controllers
{
    public class PrivacyController : Controller
    {
        [HttpGet]
        public ActionResult PrivacyPolicy()
        {
            return View("~/Views/Shared/_PrivacyPolicy.cshtml");
        }

        [HttpGet]
        public ActionResult VisitorAgreement()
        {
            return View("~/Views/Shared/_VisitorAgreement.cshtml");
        }
    }
}