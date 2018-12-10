using Carwale.UI.ClientBL;
using Carwale.UI.Filters;
using System.Web.Mvc;
using System.Web.SessionState;

namespace Carwale.UI.Controllers
{
    [SessionState(SessionStateBehavior.Disabled)]
    public class EMICalculatorController : Controller
    {

        [Route("finance/emicalculator/")]
        public ActionResult Index()
        {
            if (DeviceDetectionManager.IsMobile(this.HttpContext))
            {
                ViewBag.isMobile = true;
            }
            return View("~/Views/Finance/EmiCalculator.cshtml");
        }

        [Route("finance/emicompare/")]
        public ActionResult Allphoto()
        {
            if (DeviceDetectionManager.IsMobile(this.HttpContext))
            {
                ViewBag.isMobile = true;
            }
            return View("~/Views/Finance/EmiComparison.cshtml");
        }
        [Route("finance/emiviewchart/")]
        public ActionResult EmiChart()
        {
            if (DeviceDetectionManager.IsMobile(this.HttpContext))
            {
                ViewBag.isMobile = true;
            }
            return View("~/Views/Finance/EmiViewChart.cshtml");
        }

    }
}