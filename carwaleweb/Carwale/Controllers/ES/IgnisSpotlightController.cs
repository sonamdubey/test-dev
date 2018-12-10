using Carwale.UI.ClientBL;
using Carwale.UI.Common;
using Carwale.UI.Filters;
using System.Web.Mvc;

namespace Carwale.UI.Controllers.ES
{
	public class IgnisSpotlightController : Controller
    {
        [Route("customize-your-ignis")]
        public ActionResult Index()
        {
            if (DeviceDetectionManager.IsMobile(this.HttpContext))
            {
                ViewBag.isMobile = true;
            }
            return View("~/Views/ES/IgnisSpotlight.cshtml");
        }
    }
}