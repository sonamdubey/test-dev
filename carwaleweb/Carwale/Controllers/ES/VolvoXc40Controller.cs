using Carwale.UI.ClientBL;
using Carwale.UI.Common;
using Carwale.UI.Filters;
using System.Web.Mvc;

namespace Carwale.UI.Controllers.Es
{
    public class VolvoXc40Controller : Controller
    {
        [Route("specials/volvo-xc40")]
        public ActionResult Index(bool isApp = false)
        {
            ViewBag.isMobile = false;
            if (DeviceDetectionManager.IsMobile(this.HttpContext))
            {
                ViewBag.isMobile = true;
            }
            ViewBag.isApp = isApp;
            return View("~/Views/ES/VolvoXc40.cshtml");
        }
    }
}