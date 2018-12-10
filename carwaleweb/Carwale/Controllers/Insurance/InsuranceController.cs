using Carwale.UI.Filters;
using System.Configuration;
using System.Web.Mvc;

namespace Carwale.Controllers
{
    public class InsuranceController : Controller
    {
       
        protected string insuranceThankYouMsg = ConfigurationManager.AppSettings["InsuranceThankYouMsg"] ?? "";

       

        [DeviceDetectionFilter]
        public ActionResult Index()
        {
            ViewBag.insuranceThankYouMsg = insuranceThankYouMsg;
            return View();
        }        
    }
}