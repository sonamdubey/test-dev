using Carwale.DTOs.LeadForm;
using Carwale.Entity.Campaigns;
using Carwale.Interfaces.LeadForm;
using System.Web.Http;
using System.Web.Mvc;
using Carwale.Utility;

namespace Carwale.UI.Controllers.m.DealerLeadForm
{
    public class DealerLeadFormController : Controller
    {
        private readonly ILeadFormAdapter _leadformAdapter;
        public DealerLeadFormController(ILeadFormAdapter leadformAdapter)
        {
            _leadformAdapter = leadformAdapter;
        }
        [System.Web.Mvc.Route("dealerleadform")]
        public ActionResult Index([FromUri] DealerLeadFormInput campaignInput)
        {
            if (System.Web.HttpContext.Current.Request.Headers["IMEI"] != null)
            {
                CookieManager.SetCookieByValue("CWC", System.Web.HttpContext.Current.Request.Headers["IMEI"], 2);
            }
            LeadFormDto leadFormDetail = null;
            if (campaignInput != null)
            {
                if (campaignInput.PlatformId == 0)
                {
                    campaignInput.PlatformId = 74;
                }

                leadFormDetail = _leadformAdapter.GetDealerLeadFormDetails(campaignInput);
            }
            return View("~/Views/m/LeadForm/DealerLeadForm.cshtml", leadFormDetail);

        }
	}
}