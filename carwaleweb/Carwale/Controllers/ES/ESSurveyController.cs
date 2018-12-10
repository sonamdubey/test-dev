using Carwale.Entity.ES;
using Carwale.Interfaces.ES;
using Carwale.UI.ClientBL;
using Carwale.UI.Common;
using Carwale.Utility;
using System.Configuration;
using System.Web.Mvc;

namespace Carwale.UI.Controllers.ES
{
    public class ESSurveyController : Controller
    {        
        private readonly int defaultCampaignId = CustomParser.parseIntObject(ConfigurationManager.AppSettings["ESSurveyCampaignId"]);
        private ISurveyCache _surveyCache;
        private ISurveyBL _surveyBL;

        public ESSurveyController(ISurveyCache surveyCache, ISurveyBL surveyBL)
        {
            _surveyCache = surveyCache;            
            _surveyBL = surveyBL;
        }

        [Route("carwale-survey/")]
        public ActionResult Index()
        {            
            if (DeviceDetectionManager.IsMobile(this.HttpContext))
            {
                ViewBag.isMobile = true;
            }
            ViewBag.CampaignId = defaultCampaignId;

            var surveyCampaignsData = _surveyCache.GetSurveyQuestions(defaultCampaignId);

            return View("~/Views/ES/ESSurvey.cshtml", surveyCampaignsData);
        }

        [Route("carwale-luxury-survey/")]
        public ActionResult SurveyCampaign()
        {
            int campaignId = 6;  //harcoded for Volvo 
            if (DeviceDetectionManager.IsMobile(this.HttpContext))
            {
                ViewBag.isMobile = true;
            }
            ViewBag.CampaignId = campaignId;

            var surveyCampaignsData = _surveyCache.GetSurveyQuestions(campaignId);

            if (surveyCampaignsData == null || surveyCampaignsData.Campaign.Id <= 0 || surveyCampaignsData.Questions.Count == 0)
            {
                return RedirectPermanent("/");
            }

			return View("~/Views/ES/VolvoSurvey.cshtml", surveyCampaignsData);
        }

        [HttpPost, Route("survey/submit/{campaignId}/")]
        public void SaveUserResult([System.Web.Http.FromBody]ESSurveyCustomerResponse Customer, [System.Web.Http.FromUri] int campaignId)
        {
            string cwcCookie = CurrentUser.CWC;
            Customer.CampaignId = campaignId;
            _surveyBL.SaveSurveyData(Customer, cwcCookie);
            Response.Redirect("/");
        }

        [Route("the-great-indian-hatchback-of-2016")]
        public ActionResult Result()
        {
            if (DeviceDetectionManager.IsMobile(this.HttpContext))
            {
                ViewBag.isMobile = true;
            }
            ViewBag.CampaignId = defaultCampaignId;

            var surveyCampaignsData = _surveyCache.GetSurveyQuestions(1);

            return View("~/Views/ES/ESSurveyResult.cshtml", surveyCampaignsData);
        }

	    }
}