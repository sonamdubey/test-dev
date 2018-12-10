using System.Web.Mvc;

namespace Carwale.UI.Controllers.m.PriceQuote
{
    public class QuotationPageLandingController : Controller
    {
        [Route("quotation/landing/")]
        public ActionResult Index()
        {
            return View("~/Views/m/New/PriceQuote/PriceQuoteLanding.cshtml");
        }
    }
}