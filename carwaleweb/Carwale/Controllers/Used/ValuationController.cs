using Carwale.Entity.Classified;
using Carwale.Entity.Classified.CarValuation;
using Carwale.Interfaces.Classified.CarValuation;
using Carwale.UI.ClientBL;
using Carwale.UI.Filters.ActionFilters;
using System.Web.Mvc;

namespace Carwale.UI.Controllers.Used
{
    public class ValuationController : Controller
    {
        private readonly IValuationBL _valuationBL;
        public ValuationController(IValuationBL valuationBL)
        {
            _valuationBL = valuationBL;
        }

        [Route("used/carvaluation/")]
        [ResponsiveViewHeaders]
        public ActionResult Form()
        {
            if (DeviceDetectionManager.IsMobile(this.HttpContext))
            {
                return new FilePathResult("~/used/valuation/index.html", "text/html");
            }
            else
            {
                return View(new WebFormView(this.ControllerContext, "~/used/valuation/default.aspx"));
            }
        }

        [Route("used/valuation/v1/report/")]
        [Route("m/used/valuation/v1/report/")]
        public ActionResult Report(short year, int car, int city, int kms, int? askingPrice = null, UsedCarOwnerTypes owner = UsedCarOwnerTypes.NA, string ratingText = null,bool isSellingIndex=true)
        {
            ValuationReport report = _valuationBL.GetValuationReport(year, car, city, kms, owner, isSellingIndex);
            if (report != null)
            {
                report.SellerAskingPrice = askingPrice;
                report.DealerRatingText = ratingText;
            }
            return (DeviceDetectionManager.IsMobile(this.HttpContext))
                  ? PartialView("~/Views/Shared/m/Used/_ValuationReport.cshtml", report)
                  : PartialView("~/Views/Shared/Used/_ValuationReport.cshtml", report);
        }

    }
}