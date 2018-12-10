using Carwale.Utility;
using Carwale.DTOs.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Carwale.UI.Common;

namespace Carwale.UI.Controllers.Security
{
    public class SecurityController : Controller
    {
        // GET: Security
        public ActionResult Index()
        {
            return View("~/Views/Security/Security.cshtml");
        }

        [HttpPost]
        public ActionResult ValidateReCaptcha()
        {
            string gRecaptchaResponse = Request["g-recaptcha-response"];
            const string reCaptchaVerificationUrl = "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}";
            string gRecaptchaValidationResponse = string.Empty;
            using (WebClient webClient = new WebClient())
            {
                gRecaptchaValidationResponse = webClient.DownloadString(string.Format(reCaptchaVerificationUrl, CWConfiguration.CwRecaptchaSecretKey, gRecaptchaResponse));
            }

            ReCaptchaResponse response = JsonConvert.DeserializeObject<ReCaptchaResponse>(gRecaptchaValidationResponse);

            if (response.Success)
            {
                CookiesCustomers.IsReCaptchaVerified = true;
                Response.Redirect("/m/");
            }

            return View("~/Views/Security/Security.cshtml");
        }
    }
}