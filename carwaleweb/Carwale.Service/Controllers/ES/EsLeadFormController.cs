using Carwale.Entity.ES;
using Carwale.Interfaces.ES;
using Carwale.Notifications;
using Newtonsoft.Json;
using System;
using System.Web.Http;

namespace Carwale.Service.Controllers.ES
{

    public class EsLeadFormController : ApiController
    {
        private readonly IEsLeadFormRepository _customerDataRepo;

        public EsLeadFormController(IEsLeadFormRepository customerDataRepo)
        {
            _customerDataRepo = customerDataRepo;
        }
        [HttpPost, Route("api/leadform/")]
        public IHttpActionResult SubmitLeadForm(EsLeadFormResponse customerResponse)
        {
            try
            {
                return Ok(_customerDataRepo.SubmitEsLeadFormData(customerResponse));
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "SurveyController.LeadFormController()" + (customerResponse == null ? "NULL" : JsonConvert.SerializeObject(customerResponse)));
                objErr.LogException();
            }
            return InternalServerError();
        }

    }
}
