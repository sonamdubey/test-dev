using Carwale.BL.Finance;
using Carwale.Entity.Finance;
using Carwale.Entity.Enum;
using Carwale.Notifications;
using Carwale.Interfaces.Finance;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using System;
using System.Web.Http;
using System.Linq;
using Carwale.DTOs.Finance;

namespace Carwale.Service.Controllers.Insurance
{
    public class ClientFinanceController : ApiController
    {
        private readonly IUnityContainer _unityContainer;
        protected Clients requestClient = 0;
        protected Platform platform;

        public ClientFinanceController(IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
        }
        /// <summary>
        /// Api to submit Finance Lead
        /// </summary>
        /// <param name="request"></param>
        /// <param name="FinanceLead"></param>
        /// <returns></returns>
        [HttpPost,Route("api/finance/quote/")]
        public IHttpActionResult SubmitLead([FromBody] FinanceLead inputs)
        {
            try
            {
                if (Request.Headers.Contains("clientid"))
                {
                    Enum.TryParse<Clients>(Request.Headers.GetValues("clientid").First(), out requestClient);
                    inputs.ClientId = requestClient;
                }
                else
                {
                    return BadRequest("clientid is missing");
                }

                if (Request.Headers.Contains("sourceid"))
                {
                    Enum.TryParse<Platform>(Request.Headers.GetValues("sourceid").First(), out platform);
                    inputs.PlatformId = platform;
                }
                else
                {
                    return BadRequest("sourceid is missing");
                }

                if ((requestClient != Clients.HDFC) && (requestClient != Clients.Axis))            
                {
                    return BadRequest("invalid Client");
                }
              
                //DECIDE CLIENT N SEND LEAD 
                var _financeAdapter = _unityContainer.Resolve<IFinanceAdapter>("generic");
                var data = _financeAdapter.Get(inputs);                        
                return Ok(data);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "ClientFinance.SubmitLead()" + (inputs == null ? "NULL" : JsonConvert.SerializeObject(inputs)));
                objErr.LogException();
            }
            return InternalServerError();
        }

        [HttpGet, Route("api/finance/checkeligibility")]
        public IHttpActionResult UserEligiblityLoanParams([FromUri]LoanEligibilityRequestEntity input)
        {
            LoanParams objLoanParams = new LoanParams();

            try
            {
                AutoMapper.Mapper.CreateMap<LoanParams, HdfcLoanParams>();
                var _financeRepo = _unityContainer.Resolve<IFinance<FinanceLead, ClientResponseDto>>("Hdfc");
                objLoanParams = _financeRepo.IsEligibleForLoan(input);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "UserEligiblityLoanParams");
                objErr.LogException();
            }

            return Ok(AutoMapper.Mapper.Map<LoanParams, HdfcLoanParams>(objLoanParams));
        }

        [HttpGet, Route("api/finance/eligibility")]
        public IHttpActionResult UserEligiblity([FromUri]LoanEligibilityRequestEntity input)
        {
            try
            {
                var _financeRepo = _unityContainer.Resolve<IFinanceOperations>();
                var isEligible = _financeRepo.CheckEligibility(input);
                return Ok(isEligible);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CheckEligibility");
                objErr.LogException();
            }
            return InternalServerError();
        }
    }
}
