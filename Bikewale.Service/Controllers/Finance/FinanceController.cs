using Bikewale.Entities.Finance.CapitalFirst;
using Bikewale.Interfaces.Finance;
using Bikewale.Interfaces.Finance.CapitalFirst;
using Bikewale.Notifications;
using System;
using System.Web.Http;

namespace Bikewale.Service.Controllers
{
    public class FinanceController : ApiController
    {
        private readonly IFinanceRepository _objRepository = null;
        private readonly ICapitalFirst _objICapitalFirst = null;
        public FinanceController(IFinanceRepository objRepository, ICapitalFirst objICapitalFirst)
        {
            _objRepository = objRepository;
            _objICapitalFirst = objICapitalFirst;

        }
        /// <summary>
        /// Created By :- Subodh Jain on 24 july 2017
        /// Summary :- Banner SaveBannerBasicDetails
        /// </summary>
        [HttpPost, Route("api/finance/savepersonaldetails/")]
        public IHttpActionResult SavePersonalDetails(PersonalDetails objDetails)
        {
            try
            {
                _objRepository.SavePersonalDetails(objDetails);

                return Ok();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Service.Controllers.SavePersonalDetails");

                return InternalServerError();
            }


        }


        /// <summary>
        /// Created By :- Subodh Jain on 24 july 2017
        /// Summary :- Banner SaveBannerBasicDetails
        /// </summary>
        [HttpPost, Route("api/finance/saveemployedetails/")]
        public IHttpActionResult SaveEmployeDetails(EmployeDetails objDetails)
        {
            try
            {
                _objRepository.SaveEmployeDetails(objDetails);

                return Ok();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Service.Controllers.SaveEmployeDetails");

                return InternalServerError();
            }


        }

        /// <summary>
        /// Created by  :   Sumit Kate on 11 Sep 2017
        /// Description :   Finance Lead API for Capital First
        /// This API will be called by CarTrade to push Capital First Loan Voucher details and Capital First Agent Details
        /// </summary>
        /// <param name="ctLeadId"></param>
        /// <param name="requestBody">Any valid JSON format</param>
        /// <returns></returns>
        [HttpPost, Route("api/finance/capitalfirst/{ctLeadId}/")]
        public IHttpActionResult SaveCapitalFirstVoucherDetails(string ctLeadId, [FromBody]string requestBody)
        {
            if (!String.IsNullOrEmpty(ctLeadId) && !String.IsNullOrEmpty(requestBody))
            {
                try
                {
                    string message = _objICapitalFirst.SaveVoucherDetails(ctLeadId, requestBody);
                    return Ok(message);
                }
                catch (Exception ex)
                {
                    ErrorClass objErr = new ErrorClass(ex, String.Format("Bikewale.Service.Controllers.FinanceController.SaveCapitalFirstVoucherDetails({0},{1})", ctLeadId, requestBody));
                    return InternalServerError(new Exception("Server error has occured."));
                }
            }
            else
            {
                return BadRequest("Invalid Request");
            }
        }
    }
}
