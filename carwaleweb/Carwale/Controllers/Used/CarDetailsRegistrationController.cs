using Carwale.Entity.Classified.CarDetails;
using Carwale.Interfaces.Stock;
using Carwale.UI.ViewModels.Used.Reports;
using System;
using System.Web;
using System.Web.Mvc;

namespace Carwale.UI.Controllers.Used
{
    public class CarDetailsRegistrationController : Controller
    {
        private readonly IStockBL _stockBL;
        public CarDetailsRegistrationController(IStockBL stockBL)
        {
            _stockBL = stockBL;
        }
        [Route("used/certificationreport/")]
        public ActionResult Index(CarRegistrationReportViewModel carRegistrationReportViewModel)
        {
            if (carRegistrationReportViewModel != null && !string.IsNullOrWhiteSpace(carRegistrationReportViewModel.RegistrationNumber))
            {
                string detailsPageUrl = _stockBL.GetDetailsPageUrlFromRegistrationNumber(carRegistrationReportViewModel.RegistrationNumber);
                if (!string.IsNullOrWhiteSpace(detailsPageUrl))
                {
                    return Redirect($"{detailsPageUrl}#certificationReport");
                }
                else
                {
                    ModelState.AddModelError("RegNo", "Please enter correct car registration number");
                }
            }
            return View("~/Views/m/Used/CarRegistrationReport.cshtml",carRegistrationReportViewModel);
        }

    }
}