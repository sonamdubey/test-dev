using Bikewale.DTO.Finance;
using Bikewale.Entities.Dealer;
using Bikewale.Entities.Finance.CapitalFirst;
using Bikewale.Interfaces.Finance;
using Bikewale.Interfaces.MobileVerification;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.Finance;
using System;
using System.Linq;
using System.Web.Http;

namespace Bikewale.Service.Controllers
{
    public class FinanceController : ApiController
    {

        private readonly ICapitalFirst _objICapitalFirst = null;
        private readonly IMobileVerificationRepository _mobileVerRespo = null;
        private readonly IMobileVerificationCache _mobileVerCacheRepo = null;
        public FinanceController(ICapitalFirst objICapitalFirst, IMobileVerificationRepository mobileVerRespo, IMobileVerificationCache mobileVerCacheRepo)
        {

            _objICapitalFirst = objICapitalFirst;
            _mobileVerRespo = mobileVerRespo;
            _mobileVerCacheRepo = mobileVerCacheRepo;

        }
        /// <summary>
        /// Created By :- Subodh Jain on 24 july 2017
        /// Summary :- Banner SaveBannerBasicDetails
        /// </summary>
        [HttpPost, Route("api/finance/savepersonaldetails/")]
        public IHttpActionResult SavePersonalDetails([FromBody] PersonalDetails objDetails, DTO.PriceQuote.PQSources source)
        {
            try
            {
                string Utma = Request.Headers.Contains("utma") ? Request.Headers.GetValues("utma").FirstOrDefault() : String.Empty;
                string Utmz = Request.Headers.Contains("utmz") ? Request.Headers.GetValues("utmz").FirstOrDefault() : String.Empty;
                objDetails.objLead = Newtonsoft.Json.JsonConvert.DeserializeObject<ManufacturerLeadEntity>(objDetails.objLeadJson);

                var leadResponse = _objICapitalFirst.SavePersonalDetails(objDetails, Utmz, Utma, (ushort)source);
                var dto = FinanceMapper.Convert(leadResponse);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Service.Controllers.SavePersonalDetails");

                return InternalServerError();
            }


        }


        /// <summary>
        /// Created By :- Subodh Jain on 11 september 2017
        /// Summary :- Banner SaveBannerBasicDetails
        /// </summary>
        [HttpPost, Route("api/finance/saveemployedetails/")]
        public IHttpActionResult SaveEmployeDetails([FromBody] PersonalDetails objDetails, DTO.PriceQuote.PQSources source)
        {
            try
            {
                objDetails.objLead = Newtonsoft.Json.JsonConvert.DeserializeObject<ManufacturerLeadEntity>(objDetails.objLeadJson);
                string Utma = Request.Headers.Contains("utma") ? Request.Headers.GetValues("utma").FirstOrDefault() : String.Empty;
                string Utmz = Request.Headers.Contains("utmz") ? Request.Headers.GetValues("utmz").FirstOrDefault() : String.Empty;

                var leadResponse = _objICapitalFirst.SaveEmployeDetails(objDetails, Utmz, Utma, (ushort)source);
                var dto = FinanceMapper.Convert(leadResponse);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Service.Controllers.SaveEmployeDetails");

                return InternalServerError();
            }


        }

        /// <summary>
        /// Created by  :   Sumit Kate on 11 Sep 2017
        /// Description :   Finance Lead API for Capital First
        /// This API will be called by CarTrade to push Capital First Loan Voucher details and Capital First Agent Details
        /// </summary>
        /// <param name="ctLeadId"></param>
        /// <param name="voucher">Capital First Voucher details</param>
        /// <returns></returns>
        [APISecurity.IPAuthorization]
        [HttpPost, Route("api/finance/capitalfirst/{ctLeadId}/")]
        public IHttpActionResult SaveCapitalFirstVoucherDetails(string ctLeadId, [FromBody]CapitalFirstVoucherDTO voucher)
        {
            if (!String.IsNullOrEmpty(ctLeadId) && voucher != null)
            {
                try
                {
                    CapitalFirstVoucherEntityBase entity = FinanceMapper.Convert(voucher);
                    string message = _objICapitalFirst.SaveVoucherDetails(ctLeadId, entity);
                    return Ok(message);
                }
                catch (Exception ex)
                {
                    ErrorClass.LogError(ex, String.Format("Bikewale.Service.Controllers.FinanceController.SaveCapitalFirstVoucherDetails({0},{1})", ctLeadId, Newtonsoft.Json.JsonConvert.SerializeObject(voucher)));
                    return InternalServerError(new Exception("Server error has occured."));
                }
            }
            else
            {
                return BadRequest("Invalid Request");
            }
        }

        /// <summary>
        /// Created By :- Subodh Jain on 24 july 2017
        /// Summary :- Banner SaveBannerBasicDetails
        /// </summary>
        [HttpPost, Route("api/finance/verifymobile/otp/{otp}/")]
        public IHttpActionResult VerifyMobile(string otp, [FromBody] PersonalDetails objDetails, DTO.PriceQuote.PQSources source)
        {
            try
            {
                bool mobileVerified = _mobileVerRespo.VerifyMobileVerificationCode(objDetails.MobileNumber, otp, string.Empty);
                if (mobileVerified)
                {
                    objDetails.objLead = Newtonsoft.Json.JsonConvert.DeserializeObject<ManufacturerLeadEntity>(objDetails.objLeadJson);
                    _objICapitalFirst.PushLeadinCTandAutoBiz(objDetails, (ushort)source);


                }

                return Ok(mobileVerified);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Service.Controllers.SavePersonalDetails");

                return InternalServerError();
            }


        }
    }
}
