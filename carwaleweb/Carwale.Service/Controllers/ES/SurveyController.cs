using Carwale.Notifications;
using Carwale.Entity.ES;
using Newtonsoft.Json;
using System.Web.Http;
using System;
using Carwale.Interfaces.ES;

namespace Carwale.Service.Controllers.ES
{
    public class SurveyController : ApiController
    {
        private readonly ISurveyRepository _surveyRepo;
        private ISurveyBL _surveyBL;

        public SurveyController(ISurveyRepository surveyRepo, ISurveyBL surveyBL)
        {
            _surveyRepo = surveyRepo;
            _surveyBL = surveyBL;
        }

        [Route("api/survey/")]
        [HttpPost]
        public IHttpActionResult SubmitSurvey([FromBody] ESSurveyCustomerResponse customer, [FromUri] string cwcCookie)
        {
            try
            {
                if (customer.CampaignId > 0)
                {
                    var response = _surveyBL.SaveSurveyData(customer, cwcCookie);
                    return Ok(response);
                }
                else
                {
                    return BadRequest("campaign id is missing");
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "SurveyController.SubmitSurvey()" + (customer == null ? "NULL" : JsonConvert.SerializeObject(customer)));
                objErr.LogException();
            }
            return InternalServerError();
        }

        [HttpGet, Route("api/iselectricuser")]
        public IHttpActionResult ElectricCars(string cwcCookie)
        {
            try
            {
                if (cwcCookie != null)
                {
                    var result = _surveyBL.ElectricCars(cwcCookie);
                    return Ok(result);
                }
                else
                {
                    return BadRequest("cwcCookie is missing");
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "SurveyController.ElectricCars()");
                objErr.LogException();
            }
            return InternalServerError();
        }
    }
}
