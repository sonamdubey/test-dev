using AutoMapper;
using Carwale.DAL.ApiGateway;
using Carwale.DAL.ApiGateway.ApiGatewayHelper;
using Carwale.DTOs.CustomerVerification;
using Carwale.DTOs.CustomerVerification.Validators;
using Carwale.Entity.CustomerVerification;
using Carwale.Entity.Vernam;
using Carwale.Interfaces.CustomerVerification;
using Carwale.Interfaces.Otp;
using Carwale.Notifications.Logs;
using Carwale.Service.Filters;
using Carwale.Utility;
using FluentValidation;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using VerNam.ProtoClass;

namespace Carwale.Service.Controllers.CustomerVerification
{
    public class VerificationController : ApiController
    {
        protected readonly ICustomerVerification _customerVerification;
        private readonly ICustomerVerificationRepository _customerVerificationRepo;
        private readonly IOtpLogic _otpLogic;
        private Carwale.Entity.Enum.Platform platform = Carwale.Entity.Enum.Platform.CarwaleDesktop;
        private readonly IApiGatewayCaller _apiGatewayCaller;
        private readonly IApiGatewayAdapter<RequestData, bool, GrpcBool> _verifyOtpGatewayAdapter;
        private readonly IApiGatewayAdapter<RequestData, bool, VerNam.ProtoClass.GrpcBool> _apiGatewayInitiateOtpAdapter;
        private readonly IApiGatewayAdapter<RequestData, string, VerNam.ProtoClass.GrpcString> _apiGatewayInitiateMissedCallVerificationAdapter;
        private readonly IApiGatewayAdapter<RequestData, bool, VerNam.ProtoClass.GrpcBool> _apiGatewayIsVerifiedForDeviceAdapter;
        private static readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore
        };
        private static readonly AbstractValidator<InitiateVerificationDto> _initiateMobileOtpDtoValidator = new InitiateMobileOtpDtoValidator();

        public VerificationController
            (
             ICustomerVerification customerVerification
            , ICustomerVerificationRepository customerVerificationRepo
            , IOtpLogic otpLogic
            , IApiGatewayCaller apiGatewayCaller
            , [Dependency("InitiateOtp")]IApiGatewayAdapter<RequestData, bool, VerNam.ProtoClass.GrpcBool> apiGatewayInitiateOtpAdapter
            , [Dependency("VerifyOtp")]IApiGatewayAdapter<RequestData, bool, GrpcBool> verifyOtpGatewayAdapter
            , [Dependency("IsVerifiedForDevice")]IApiGatewayAdapter<RequestData, bool, VerNam.ProtoClass.GrpcBool> apiGatewayIsVerifiedForDeviceAdapter
            , [Dependency("InitiateMissedCallVerification")]IApiGatewayAdapter<RequestData, string, VerNam.ProtoClass.GrpcString> apiGatewayInitiateMissedCallVerificationAdapter
            )
        {
            _customerVerification = customerVerification;
            _customerVerificationRepo = customerVerificationRepo;
            _otpLogic = otpLogic;
            _apiGatewayCaller = apiGatewayCaller;
            _apiGatewayInitiateOtpAdapter = apiGatewayInitiateOtpAdapter;
            _verifyOtpGatewayAdapter = verifyOtpGatewayAdapter;
            _apiGatewayIsVerifiedForDeviceAdapter = apiGatewayIsVerifiedForDeviceAdapter;
            _apiGatewayInitiateMissedCallVerificationAdapter = apiGatewayInitiateMissedCallVerificationAdapter;
        }

        [HttpGet, Route("api/mobile/verify/{mobileNumber:regex(^[6-9][0-9]{9}$)}")]
        public IHttpActionResult VerifyMobile(string mobileNumber, string emailId = "")
        {
            if (Request.Headers.Contains("sourceId"))
            {
                Enum.TryParse<Carwale.Entity.Enum.Platform>(Request.Headers.GetValues("sourceId").First(), out platform);
            }

            var response = _customerVerification.IsMobileVerified(mobileNumber, emailId, platform);

            Mapper.CreateMap<MobileVerificationReponseEntity, MobileVerificationReponseDto>();

            var responseDto = Mapper.Map<MobileVerificationReponseEntity, MobileVerificationReponseDto>(response);

            return Ok(responseDto);
        }

        [HttpPost, Route("api/v1/mobile/{mobileNumber}/verification/start/"), HandleException]
        public IHttpActionResult VerifyMobileAndTokenV1(string mobileNumber, InitiateVerificationDto initiateVerification)
        {
            if (initiateVerification == null || !RegExValidations.IsValidMobile(mobileNumber))
            {
                return BadRequest("Invalid mobile number");
            }
            if (Request.Headers.Contains("sourceId"))
            {
                Enum.TryParse<Carwale.Entity.Enum.Platform>(Request.Headers.GetValues("sourceId").First(), out platform);
            }
            initiateVerification.Mobile = mobileNumber;
            var requestData = Mapper.Map<RequestData>(initiateVerification);
            if (initiateVerification.MobileVerificationByType == MobileVerificationByType.OtpAndMissedCall)
            {
                requestData.VerificationMedium = Entity.Vernam.VerificationMedium.Otp;
                _apiGatewayInitiateOtpAdapter.AddApiGatewayCallWithCallback(_apiGatewayCaller, requestData);
                requestData.VerificationMedium = Entity.Vernam.VerificationMedium.MissedCall;
                _apiGatewayInitiateMissedCallVerificationAdapter.AddApiGatewayCallWithCallback(_apiGatewayCaller, requestData);
            }
            else if (initiateVerification.MobileVerificationByType == MobileVerificationByType.Otp)
            {
                requestData.VerificationMedium = Entity.Vernam.VerificationMedium.Otp;
                _apiGatewayInitiateOtpAdapter.AddApiGatewayCallWithCallback(_apiGatewayCaller, requestData);
            }
            else
            {
                requestData.VerificationMedium = Entity.Vernam.VerificationMedium.MissedCall;
                _apiGatewayInitiateMissedCallVerificationAdapter.AddApiGatewayCallWithCallback(_apiGatewayCaller, requestData);
            }
            _apiGatewayCaller.Call();
            string tollFreeNumber = _apiGatewayInitiateMissedCallVerificationAdapter.Output ?? string.Empty;
            var responseDto = new MobileVerificationReponseDto
            {
                IsOtpGenerated = _apiGatewayInitiateOtpAdapter.Output,
                TollFreeNumber = platform == Entity.Enum.Platform.CarwaleiOS ? tollFreeNumber : Regex.Replace(tollFreeNumber, @"(\d{4})(\d{3})(\d{3})", "$1 $2 $3")
            };
            return Json(responseDto, _jsonSerializerSettings);
        }

        [HttpPost, Route("api/mobile/{mobileNumber}/verification/start/")]
        public IHttpActionResult VerifyMobileAndToken(string mobileNumber)
        {
            if (!RegExValidations.IsValidMobile(mobileNumber))
            {
                return BadRequest("Invalid mobile number");
            }

            if (Request.Headers.Contains("sourceId"))
            {
                Enum.TryParse<Carwale.Entity.Enum.Platform>(Request.Headers.GetValues("sourceId").First(), out platform);
            }

            string clientTokenId = !string.IsNullOrEmpty(HttpContextUtils.GetCookie("CWC")) ? HttpContextUtils.GetCookie("CWC") : HttpContextUtils.GetHeader<string>("IMEI");
            var response = _customerVerification.IsMobileAndTokenVerified(mobileNumber, platform, clientTokenId);

            Mapper.CreateMap<MobileVerificationReponseEntity, MobileVerificationReponseDto>();

            var responseDto = Mapper.Map<MobileVerificationReponseEntity, MobileVerificationReponseDto>(response);

            return Ok(responseDto);
        }

        [HttpGet, Route("api/mobileVerification/{mobileNumber:regex(^[6-9][0-9]{9}$)}")]
        public IHttpActionResult IsMobileVerified(string mobileNumber)
        {
            var response = new MobileVerificationReponseDto
            {
                IsMobileVerified = _customerVerificationRepo.IsMobileVerified(mobileNumber)
            };
            return Ok(response);
        }

        [HttpGet, Route("api/v1/mobile/{mobileNumber}/verification/status/")]
        public IHttpActionResult IsVerifiedAfterMissedCall(string mobileNumber)
        {
            if (!RegExValidations.IsValidMobile(mobileNumber))
            {
                return BadRequest("Invalid mobile number");
            }

            string clientTokenId = !string.IsNullOrEmpty(HttpContextUtils.GetCookie("CWC")) ? HttpContextUtils.GetCookie("CWC") : HttpContextUtils.GetHeader<string>("IMEI");
            var requestData = new RequestData
            {
                VerificationType = Carwale.Entity.Vernam.VerificationType.Mobile,
                VerificationValue = mobileNumber,
                DeviceId = clientTokenId,
            };
            _apiGatewayIsVerifiedForDeviceAdapter.AddApiGatewayCallWithCallback(_apiGatewayCaller, requestData);
            _apiGatewayCaller.Call();
            var response = new MobileVerificationReponseDto
            {
                IsMobileVerified = _apiGatewayIsVerifiedForDeviceAdapter.Output
            };
            return Ok(response);
        }

        [HttpGet, Route("api/mobile/{mobileNumber}/verification/status/")]
        public IHttpActionResult IsVerifiedAfterMissedCallV1(string mobileNumber)
        {
            if (!RegExValidations.IsValidMobile(mobileNumber))
            {
                return BadRequest("Invalid mobile number");
            }

            string clientTokenId = !string.IsNullOrEmpty(HttpContextUtils.GetCookie("CWC")) ? HttpContextUtils.GetCookie("CWC") : HttpContextUtils.GetHeader<string>("IMEI");
            var response = new MobileVerificationReponseDto
            {
                IsMobileVerified = _customerVerificationRepo.IsVerified(mobileNumber, clientTokenId)
            };
            return Ok(response);
        }

        [HttpPost, Route("api/resendotp/"), HandleException]
        [EnableCors("https://www-carwale-com.cdn.ampproject.org, https://cdn.ampproject.org, https://www-carwale-com.amp.cloudflare.com", "*", "POST")]
        public IHttpActionResult ResendOtp()
        {
            string mobile = string.Empty;
            string ampOrigin = HttpUtility.ParseQueryString(Request.RequestUri.Query)["__amp_source_origin"];
            if (!string.IsNullOrEmpty(ampOrigin))
            {
                mobile = HttpContext.Current.Request.Params["mobile"];
                platform = Carwale.Entity.Enum.Platform.CarwaleMobile;
                HttpContextUtils.AddAmpHeaders(ampOrigin, true);
            }
            else
            {
                var reqContent = Request.Content.ReadAsStringAsync().Result;
                if (reqContent != null)
                {
                    mobile = JsonConvert.DeserializeObject<string>(reqContent);
                }
            }
            if (!Regex.IsMatch(mobile, RegExValidations.MobileRegex))
            {
                return BadRequest();
            }

            if (Request.Headers.Contains("sourceId"))
            {
                Enum.TryParse(Request.Headers.GetValues("sourceId").First(), out platform);
            }

            bool isSent = _customerVerification.ResendOtpSms(string.Empty, mobile);
            if (!isSent)
            {
                var response = _customerVerification.IsMobileVerified(mobile, string.Empty, platform);
                if (response.IsMobileVerified)
                {
                    return BadRequest("Mobile already verified.");
                }
            }
            return Ok("Success");
        }

        [HttpPost, Route("api/v1/resendotp/"), HandleException]
        [EnableCors("https://www-carwale-com.cdn.ampproject.org, https://cdn.ampproject.org, https://www-carwale-com.amp.cloudflare.com", "*", "POST")]
        public IHttpActionResult ResendOtpV1()
        {
            var initiateOtpDto = GetInitiateMobileOtpDto(Request);
            var validationsResult = new InitiateMobileOtpDtoValidator().Validate(initiateOtpDto);
            if (!validationsResult.IsValid)
            {
                return BadRequest(validationsResult.Errors.IsNotNullOrEmpty() ? validationsResult.Errors[0].ErrorMessage : string.Empty);
            }
            var requestData = Mapper.Map<RequestData>(initiateOtpDto);
            _apiGatewayInitiateOtpAdapter.AddApiGatewayCallWithCallback(_apiGatewayCaller, requestData);
            _apiGatewayCaller.Call();
            var isOtpSent = _apiGatewayInitiateOtpAdapter.Output;
            if (!isOtpSent)
            {
                Logger.LogError($"Error resending otp for {initiateOtpDto.Mobile}");
                return InternalServerError();
            }
            return Ok("Success");
        }

        [HttpDelete, ApiAuthorization, HandleException, LogRequest, Route("api/verifiedmobiles/")]
        public IHttpActionResult UnverifyMobile([FromBody]MobileUnverificationInputs inputs)
        {
            if (inputs == null)
            {
                ModelState.AddModelError("Input", "Request body should not be null");
            }
            string addedBy = HttpContextUtils.GetHeader<string>("source");
            if (string.IsNullOrEmpty(addedBy))
            {
                ModelState.AddModelError("Header", "'source' Header missing.");
            }
            else if (addedBy.Length > 30)
            {
                ModelState.AddModelError("Header", "Invalid Header.('source' header length must be less or equal to 30 character.)");
            }
            if (ModelState.IsValid)
            {
                inputs.MobileNos = inputs.MobileNos.Distinct();
                _customerVerificationRepo.DeleteVerifiedMobileNos(inputs.MobileNos);
                return Ok("Successfully changed the status of given mobile numbers to 'Unverified'.");
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Verify whether the otp and mobile number are correct
        /// Uses the Otp microservice as a backend
        /// </summary>
        /// <param name="mobileNumber"></param>
        /// <param name="otpCode"></param>
        /// <returns></returns>
        [Route("api/mobile/{mobileNumber}/verification/verifyotp/"), HttpGet]
        public IHttpActionResult VerifyOtp(string mobileNumber, string otpCode)
        {
            if (string.IsNullOrWhiteSpace(otpCode))
            {
                return BadRequest("Invalid otp code");
            }
            if (!RegExValidations.IsValidMobile(mobileNumber))
            {
                return BadRequest("Invalid mobile number");
            }
            if (!_otpLogic.Verify(otpCode, mobileNumber))
            {
                return NotFound();
            }
            return Ok();
        }

        [EnableCors("https://www-carwale-com.cdn.ampproject.org, https://cdn.ampproject.org, https://www-carwale-com.amp.cloudflare.com", "*", "GET")]
        [Route("api/v1/mobile/{mobileNumber}/verification/verifyotp/"), HttpGet, HandleException, ValidateModel("verifyMobileOtp")]
        public IHttpActionResult VerifyOtp([FromUri]VerifyMobileOtpDto verifyMobileOtp)
        {
            string ampOrigin = HttpUtility.ParseQueryString(Request.RequestUri.Query)["__amp_source_origin"];
            if (!string.IsNullOrWhiteSpace(ampOrigin))
            {
                HttpContextUtils.AddAmpHeaders(ampOrigin, true);
            }
            var requestData = Mapper.Map<RequestData>(verifyMobileOtp);
            _verifyOtpGatewayAdapter.AddApiGatewayCallWithCallback(_apiGatewayCaller, requestData);
            _apiGatewayCaller.Call();
            if (_verifyOtpGatewayAdapter.Output)
            {
                return Json(new { ResponseCode = "1", ResponseMessage = "OK" }, _jsonSerializerSettings);
            }
            return Json(new { ResponseCode = "3", ResponseMessage = "Invalid code" }, _jsonSerializerSettings);
        }

        private static InitiateVerificationDto GetInitiateMobileOtpDto(HttpRequestMessage request)
        {
            var ampOrigin = HttpUtility.ParseQueryString(request.RequestUri.Query)["__amp_source_origin"];
            var initiateOtpDto = new InitiateVerificationDto();
            if (!string.IsNullOrEmpty(ampOrigin))
            {
                var requestParams = HttpContext.Current.Request.Params;
                initiateOtpDto.Mobile = requestParams["mobile"];
                Carwale.Entity.Vernam.SourceModule sourceModule;
                Enum.TryParse<Carwale.Entity.Vernam.SourceModule>(requestParams["sourcemodule"], true, out sourceModule);
                initiateOtpDto.SourceModule = sourceModule;
                HttpContextUtils.AddAmpHeaders(ampOrigin, true);
            }
            else
            {
                var reqContent = request.Content.ReadAsStringAsync().Result;
                if (reqContent != null)
                {
                    initiateOtpDto = JsonConvert.DeserializeObject<InitiateVerificationDto>(reqContent, _jsonSerializerSettings);
                }
            }
            return initiateOtpDto;
        }       
    }
}
