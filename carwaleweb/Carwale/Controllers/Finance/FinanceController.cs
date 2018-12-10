using Carwale.DTOs.Finance;
using Carwale.Entity.Enum;
using Carwale.Interfaces.Finance;
using Carwale.UI.ClientBL;
using Carwale.Utility;
using System.Configuration;
using System.Web;
using System.Web.Mvc;

namespace Carwale.UI.Controllers
{
    public class FinanceController : Controller
    {
        private readonly IFinanceCacheRepository _financeCache;

        public FinanceController(IFinanceCacheRepository financeCache)
        {
            _financeCache = financeCache;
        }
        // GET: Default
        [Route("finance")]
        public ActionResult Index()
        {
            
            ViewBag.hideNav = !string.IsNullOrEmpty(HttpContext.Request.Params.Get("platformid"));

            if (DeviceDetectionManager.IsMobile(this.HttpContext))
            {
                ViewBag.isMobile = true;
            }
            
            var model = new FinancePageDTO();
            int clientId = (int)Clients.HDFC;
            model.CompanyList = _financeCache.GetFinanceCompanyList(clientId);
            return View("~/Views/Finance/Index.cshtml", model);
        }

        [Route("finance/axis-carloan/")]
        public ActionResult AxisController()
        {

            ViewBag.hideNav = !string.IsNullOrEmpty(HttpContext.Request.Params.Get("platformid"));

            if (DeviceDetectionManager.IsMobile(this.HttpContext))
            {
                ViewBag.isMobile = true;
            }

            return View("~/Views/Finance/Axis.cshtml");
        }
    }
}