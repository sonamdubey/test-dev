/// <summary>
/// Created By : Pratibha Verma on 31st January 2018
/// Description  : Controller method to render Partial views PrivacyPolicy and VisitorAgreement
/// </summary>
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