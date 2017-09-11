using Bikewale.Entities.Finance.CapitalFirst;
using Bikewale.Interfaces.Finance;
using System.Linq;
using Bikewale.Notifications;
using System;
using System.Web.Http;
using Bikewale.Entities.Dealer;

namespace Bikewale.Service.Controllers
{
    public class FinanceController : ApiController
    {
      
        private readonly ICapitalFirst _objICapitalFirst = null;
        public FinanceController( ICapitalFirst objICapitalFirst)
        {
           
            _objICapitalFirst = objICapitalFirst;

        }
        /// <summary>
        /// Created By :- Subodh Jain on 24 july 2017
        /// Summary :- Banner SaveBannerBasicDetails
        /// </summary>
        [HttpPost, Route("api/finance/savepersonaldetails/")]
        public IHttpActionResult SavePersonalDetails([FromBody] PersonalDetails objDetails)
        {
            try
            {
                string Utmz = Request.Headers.Contains("utma") ? Request.Headers.GetValues("utma").FirstOrDefault() : String.Empty;
              string Utma = Request.Headers.Contains("utmz") ? Request.Headers.GetValues("utmz").FirstOrDefault() : String.Empty;
                objDetails.objLead = Newtonsoft.Json.JsonConvert.DeserializeObject<ManufacturerLeadEntity>(objDetails.objLeadJson);
                _objICapitalFirst.SavePersonalDetails(objDetails, Utmz, Utma);

                return Ok();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Service.Controllers.SavePersonalDetails");

                return InternalServerError();
            }


        }


        /// <summary>
        /// Created By :- Subodh Jain on 11 september 2017
        /// Summary :- Banner SaveBannerBasicDetails
        /// </summary>
        [HttpPost, Route("api/finance/saveemployedetails/")]
        public IHttpActionResult SaveEmployeDetails([FromBody] PersonalDetails objDetails)
        {
            try
            {
             string message=   _objICapitalFirst.SaveEmployeDetails(objDetails);

                return Ok(message);
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
