using Bikewale.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bikewale.Controllers
{
    public class PopUpController : Controller
    {
        // GET: PopUp
        [Route("m/popup/AMP/")]
        public ActionResult Index(string queryString)
        {
            PoupCityAreaVM objPopupCityAreaVM = new PoupCityAreaVM();

            return View("~/Views/m/Shared/_PopupWidget.cshtml", objPopupCityAreaVM);
        }
    }
}