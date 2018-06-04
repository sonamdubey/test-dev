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

                var leadResponse = _objICapitalFirst.SaveLead(objDetails, Utmz, Utma, (ushort)source);
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
        /// Created By :- Subodh Jain on 24 july 2017
        /// Summary :- Banner SaveBannerBasicDetails
        /// </summary>
        [HttpPost, Route("api/finance/verifymobile/otp/{otp}/")]
        public IHttpActionResult VerifyMobile(string otp, [FromBody] PersonalDetails objDetails, DTO.PriceQuote.PQSources source)
        {
            try
            {
                string utma = Request.Headers.Contains("utma") ? Request.Headers.GetValues("utma").FirstOrDefault() : String.Empty;
                string utmz = Request.Headers.Contains("utmz") ? Request.Headers.GetValues("utmz").FirstOrDefault() : String.Empty;
                bool mobileVerified = _mobileVerRespo.VerifyMobileVerificationCode(objDetails.MobileNumber, otp, string.Empty);
                if (mobileVerified)
                {
                    objDetails.objLead = Newtonsoft.Json.JsonConvert.DeserializeObject<ManufacturerLeadEntity>(objDetails.objLeadJson);
                    var leadResponse = _objICapitalFirst.SaveLead(objDetails, utmz, utma, (ushort)source);
                    var dto = FinanceMapper.Convert(leadResponse);
                    return Ok(dto);

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
