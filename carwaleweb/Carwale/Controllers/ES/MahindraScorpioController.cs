using Carwale.UI.ClientBL;
using Carwale.UI.Common;
using Carwale.UI.Filters;
using System.Web.Mvc;

namespace Carwale.UI.Controllers.ES
{
    public class MahindraScorpioController : Controller
    {

        [Route("scorpiospecials")]
        public ActionResult Index()
        {
            ViewBag.isMobile = false;
            if (DeviceDetectionManager.IsMobile(this.HttpContext))
            {
                ViewBag.isMobile = true;
            }
            return View("~/Views/ES/MahindraScorpioSpotlight.cshtml");
        }
    }
}