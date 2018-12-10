using Carwale.UI.ClientBL;
using Carwale.UI.Common;
using Carwale.UI.Filters;
using System.Web.Mvc;

namespace Carwale.UI.Controllers.ES
{
    public class MahindraTUVController : Controller
    {

        [Route("tuv300specials")]
        public ActionResult Index()
        {
            ViewBag.isMobile = false;
            if (DeviceDetectionManager.IsMobile(this.HttpContext))
            {
                ViewBag.isMobile = true;
            }
            return View("~/Views/ES/MahindraTUVSpotlight.cshtml");
        }
    }
}