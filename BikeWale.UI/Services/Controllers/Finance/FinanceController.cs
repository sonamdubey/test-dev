using Bikewale.DTO.Finance.BajajAuto;
using Bikewale.Entities.Dealer;
using Bikewale.Entities.Finance.BajajAuto;
using Bikewale.Entities.Finance.CapitalFirst;
using Bikewale.Interfaces.Finance;
using Bikewale.Interfaces.Finance.BajajAuto;
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
        private readonly IBajajAuto _bajajAuto;
        public FinanceController(ICapitalFirst objICapitalFirst, IMobileVerificationRepository mobileVerRespo, IMobileVerificationCache mobileVerCacheRepo, IBajajAuto bajajAuto)
        {
            _objICapitalFirst = objICapitalFirst;
            _mobileVerRespo = mobileVerRespo;
            _mobileVerCacheRepo = mobileVerCacheRepo;
            _bajajAuto = bajajAuto;

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

        #region BajajAuto
        [HttpPost, Route("api/finance/bajaj/basicdetails/")]
        public IHttpActionResult SaveBajajAutoBasicDetails([FromBody] UserDetails userDetails)
        {
            try
            {
                if (userDetails == null && string.IsNullOrEmpty(userDetails.MobileNumber) && string.IsNullOrEmpty(userDetails.EmailId))
                {
                    return BadRequest();
                }
                return Ok(_bajajAuto.SaveBasicDetails(userDetails));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Service.Controllers.SaveBajajAutoBasicDetails");
                return InternalServerError();
            }
        }
        [HttpPost, Route("api/finance/bajaj/employeedetails/")]
        public IHttpActionResult SaveBajajAutoEmployeeDetails([FromBody] UserDetails userDetails)
        {
            try
            {
                if (userDetails != null && userDetails.BajajAutoId > 0 && userDetails.VersionId > 0 && userDetails.PinCodeId > 0 && userDetails.EmploymentType > 0)
                {
                    return Ok(_bajajAuto.SaveEmployeeDetails(userDetails));
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Service.Controllers.SaveBajajAutoEmployeeDetails");
                return InternalServerError();
            }
        }
        [HttpPost, Route("api/finance/bajaj/otherdetails/")]
        public IHttpActionResult SaveBajajAutoOtherDetails([FromBody] UserDetails userDetails, DTO.PriceQuote.PQSources source)
        {
            try
            {
                if (userDetails == null && userDetails.BajajAutoId == 0 && string.IsNullOrEmpty(userDetails.MobileNumber) && string.IsNullOrEmpty(userDetails.EmailId))
                {
                    return BadRequest();
                }
                userDetails.ManufacturerLead = Newtonsoft.Json.JsonConvert.DeserializeObject<ManufacturerLeadEntity>(userDetails.LeadJson);
                string utma = Request.Headers.Contains("utma") ? Request.Headers.GetValues("utma").FirstOrDefault() : String.Empty;
                string utmz = Request.Headers.Contains("utmz") ? Request.Headers.GetValues("utmz").FirstOrDefault() : String.Empty;
                LeadResponse leadResponse = _bajajAuto.SaveOtherDetails(userDetails, utmz, utma, (ushort)source);
                BajajAutoLeadResponseDto dto = FinanceMapper.Convert(leadResponse);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Service.Controllers.SaveBajajAutoOtherDetails");
                return InternalServerError();
            }
        } 
        #endregion
    }
}
