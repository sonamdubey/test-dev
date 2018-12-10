using AEPLCore.Security;
using AEPLCore.Utils.ClientTracker;
using Carwale.DAL.ApiGateway;
using Carwale.DAL.ApiGateway.ApiGatewayHelper;
using Carwale.Entity.Blocking;
using Carwale.Entity.Blocking.Enums;
using Carwale.Entity.Classified.SellCarUsed;
using Carwale.Entity.Enum;
using Carwale.Entity.Stock;
using Carwale.Entity.Vernam;
using Carwale.Interfaces.Blocking;
using Carwale.Interfaces.Classified.MyListings;
using Carwale.Interfaces.Classified.SellCar;
using Carwale.Interfaces.Otp;
using Carwale.Interfaces.Stock;
using Carwale.Service.Filters;
using Carwale.Service.Filters.ExceptionFilters;
using Carwale.Service.Filters.ExceptionFilters.Classified;
using Carwale.Utility;
using Carwale.Utility.Classified;
using FluentValidation;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Configuration;
using System.Net;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;

namespace Carwale.Service.Controllers.Classified
{
    [ApiLogExceptionFilter, SellCarApiExceptionFilter]
    public class SellController : ApiController
    {
        private readonly ISellCarBL _sellCarBL;
        private readonly IStockConditionRepository _stockConditionRepository;
        private readonly ISellCarRepository _sellCarRepository;
        private readonly IConsumerToBusinessBL _consumerToBusinessBL;
        private readonly IBlockedCommunicationsRepository _blockedCommunicationRepo;
        private readonly IMyListings _myListings;
        private readonly IOtpLogic _otpLogic;
        private readonly IApiGatewayAdapter<RequestData, bool, VerNam.ProtoClass.GrpcBool> _apiGatewayIsVerifiedAdapter;
        private readonly IApiGatewayAdapter<RequestData, bool, VerNam.ProtoClass.GrpcBool> _apiGatewayInitiateOtpAdapter;
        private readonly IApiGatewayCaller _apiGatewayCaller;
        private static readonly string _authTokenCookie = ConfigurationManager.AppSettings["encryptedAuthToken"];

        private static readonly JsonMediaTypeFormatter _jsonFormatter = new JsonMediaTypeFormatter
        {
            SerializerSettings =
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
            }
        };
        public SellController(
                              ISellCarBL sellCarBL,
                              IStockConditionRepository stockConditionRepository,
                              ISellCarRepository sellCarRepository,
                              IConsumerToBusinessBL consumerToBusinessBL,
                              IBlockedCommunicationsRepository blockedCommunicationRepo,
                              IMyListings myListings,
                              IOtpLogic otpLogic,
                              [Dependency("IsVerified")] IApiGatewayAdapter<RequestData, bool, VerNam.ProtoClass.GrpcBool> apiGatewayIsVerifiedAdapter,
                              [Dependency("InitiateOtp")]IApiGatewayAdapter<RequestData, bool, VerNam.ProtoClass.GrpcBool> apiGatewayInitiateOtpAdapter,
                              IApiGatewayCaller apiGatewayCaller
                             )
        {
            _sellCarBL = sellCarBL;
            _stockConditionRepository = stockConditionRepository;
            _sellCarRepository = sellCarRepository;
            _consumerToBusinessBL = consumerToBusinessBL;
            _blockedCommunicationRepo = blockedCommunicationRepo;
            _myListings = myListings;
            _otpLogic = otpLogic;
            _apiGatewayIsVerifiedAdapter = apiGatewayIsVerifiedAdapter;
            _apiGatewayInitiateOtpAdapter = apiGatewayInitiateOtpAdapter;
            _apiGatewayCaller = apiGatewayCaller;
        }

        [HttpPost, Route("api/used/sell/contactdetails/")]
        public IHttpActionResult SaveContactDetails([FromBody]SellCarCustomer sellCustomer, string tempId = "")
        {
            int tempInquiryId = 0;
            if (!string.IsNullOrWhiteSpace(tempId) && !Int32.TryParse(CarwaleSecurity.Decrypt(tempId, true), out tempInquiryId))
            {
                ModelState.AddModelError("Data Invalid", "Invalid TempInquiryId");
            }
            if (sellCustomer == null || !ModelState.IsValid)
            {
                return Content(HttpStatusCode.BadRequest, new ModalPopUp
                {
                    Heading = "Data Invalid",
                    Description = "The data entered is invalid. Please try again."
                }, _jsonFormatter);
            }
            if (tempInquiryId > 0)
            {
                sellCustomer.TempInquiryId = tempInquiryId;
            }
            if (_blockedCommunicationRepo.IsCommunicationBlocked(new BlockedCommunication { Value = sellCustomer.Mobile, Type = CommunicationType.Mobile, Module = CommunicationModule.SellCar }))
            {
                return Content(HttpStatusCode.Forbidden, new ModalPopUp
                {
                    Heading = "Number Blocked",
                    Description = "Your number entered is blocked. Please contact CarWale."
                }, _jsonFormatter);
            }

            if (!_sellCarBL.CheckFreeListingAvailability(sellCustomer.Mobile))
            {
                return Content(HttpStatusCode.Forbidden, new ModalPopUp
                {
                    Heading = "Listing Limit Reached",
                    Description = Constants.IndividualListingLimitMessage
                }, _jsonFormatter);
            }

            tempInquiryId = _sellCarBL.ProcessContactDetails(sellCustomer);
            if (tempInquiryId < 1)
            {
                return Content(HttpStatusCode.InternalServerError, new ModalPopUp
                {
                    Heading = "Error!",
                    Description = "Something went wrong. Please try again."
                }, _jsonFormatter);
            }
            string encryptedTempInquiryId = CarwaleSecurity.Encrypt(tempInquiryId.ToString());
            return Ok(new { tempInquiryId = encryptedTempInquiryId });
        }

        [HttpPost, Route("api/used/sell/carimages/")]
        public IHttpActionResult SendC2BImages(string encryptedId)
        {
            int inquiryId = Convert.ToInt32(CarwaleSecurity.Decrypt(encryptedId, true));
            _consumerToBusinessBL.PushToIndividualStockQueue(-1, C2BActionType.AddImages, inquiryId);
            return Ok();
        }

        [HttpPost, Route("api/used/sell/carcondition/")]
        public IHttpActionResult SaveCarCondition([FromBody]StockCondition stockCondition, string inquiryId, int cityId)
        {
            if (_sellCarBL.IsC2bCity(cityId))
            {
                if (stockCondition == null || string.IsNullOrEmpty(inquiryId) || !(new StockConditionValidator().Validate(stockCondition).IsValid))
                {
                    return Content(HttpStatusCode.BadRequest, new ModalPopUp
                    {
                        Heading = "Data Invalid",
                        Description = "The data entered is invalid. Please try again."
                    }, _jsonFormatter);
                }
                int id = Convert.ToInt32(CarwaleSecurity.Decrypt(inquiryId, true));
                if (id <= 0)
                {
                    return Content(HttpStatusCode.InternalServerError, new ModalPopUp
                    {
                        Heading = "Error!",
                        Description = "Something went wrong. Please try again."
                    }, _jsonFormatter);
                }
                bool isSaved = _stockConditionRepository.AddStockCondition(id, stockCondition);
                if (isSaved)
                {
                    _consumerToBusinessBL.PushToIndividualStockQueue(-1, C2BActionType.AddCondition, id);
                    return Ok(true);
                }
                return Content(HttpStatusCode.InternalServerError, new ModalPopUp
                {
                    Heading = "Error!",
                    Description = "Something went wrong. Please try again."
                }, _jsonFormatter);
            }
            return Content(HttpStatusCode.NotFound, new ModalPopUp
            {
                Heading = "Not found"
            }, _jsonFormatter);
        }

        [HttpPost, Route("api/used/sell/otherdetails/")]
        public IHttpActionResult SaveOtherDetails(SellCarInfo sellCarInfo, string inquiryId)
        {

            if (sellCarInfo == null || string.IsNullOrWhiteSpace(inquiryId))
            {
                return Content(HttpStatusCode.BadRequest, new ModalPopUp
                {
                    Heading = "Data Invalid",
                    Description = "The data entered is invalid. Please try again."
                }, _jsonFormatter);
            }
            int id = Convert.ToInt32(CarwaleSecurity.Decrypt(inquiryId, true));
            if (id <= 0)
            {
                return Content(HttpStatusCode.InternalServerError, new ModalPopUp
                {
                    Heading = "Error!",
                    Description = "Something went wrong. Please try again."
                }, _jsonFormatter);
            }
            if (_sellCarRepository.SaveOtherDetails(sellCarInfo, id))
            {
                return Ok();
            }
            else
            {
                return Content(HttpStatusCode.InternalServerError, new ModalPopUp
                {
                    Heading = "Error!",
                    Description = "Something went wrong. Please try again."
                }, _jsonFormatter);
            }
        }

        [Obsolete("use v2")]
        [HttpPost, Route("api/used/sell/live/")]
        public IHttpActionResult TakeLive([FromBody]string tempId)
        {
            int tempInquiryId;
            if (string.IsNullOrWhiteSpace(tempId) || !Int32.TryParse(CarwaleSecurity.Decrypt(tempId, true), out tempInquiryId) || tempInquiryId <= 0)
            {
                return Content(HttpStatusCode.BadRequest, new ModalPopUp()
                {
                    Heading = "Data Invalid",
                    Description = "Data entered is incorrect"
                }, _jsonFormatter);
            }

            TempCustomerSellInquiry tempInquiry = _sellCarRepository.GetTempCustomerSellInquiry(tempInquiryId);
            var sellCarInfoValidation = new SellCarInfoValidator().Validate(tempInquiry.sellCarInfo, ruleSet: "address,carmake,carmodel,carversion,cardetails,carregistration,carinsurance");
            if (!sellCarInfoValidation.IsValid)
            {
                FluentValidationError.LogValidationErrorsAsync(sellCarInfoValidation.Errors, "sellCarValidationErrors ");
                return Content(HttpStatusCode.BadRequest, new ModalPopUp()
                {
                    Heading = "Data Invalid",
                    Description = "Data entered is incorrect"
                }, _jsonFormatter);
            }
            if (!_otpLogic.IsNumberVerified(tempInquiry.sellCarCustomer.Mobile))
            {
                return Content(HttpStatusCode.Forbidden, new ModalPopUp
                {
                    Heading = "Forbidden",
                    Description = "Mobile number is not verified"
                }, _jsonFormatter);
            }

            if (_sellCarBL.CreateCustomer(tempInquiry.sellCarCustomer)) //auto login if new customer
            {
                HttpContext.Current.Response.Cookies.Add(CustomerCookie.StartSession(
                    tempInquiry.sellCarCustomer.Name,
                    tempInquiry.sellCarCustomer.Id.ToString(),
                    tempInquiry.sellCarCustomer.Email
                    ));
            }
            else
            {
                if (CustomerCookie.GetUserId() != "-1" && CustomerCookie.GetUserId() != tempInquiry.sellCarCustomer.Id.ToString())
                {
                    CustomerCookie.EndSession();
                }
            }

            int inquiryId = _sellCarBL.CreateSellCarInquiry(tempInquiry);
            if (inquiryId <= 0)
            {
                return Content(HttpStatusCode.InternalServerError, new ModalPopUp()
                {
                    Heading = "Error!",
                    Description = "Something went wrong"
                }, _jsonFormatter);
            }
            _myListings.AddCookie(_authTokenCookie, 
                                  TokenManager.GenerateToken(tempInquiry.sellCarCustomer.Mobile, 
                                                             IpTracker.CurrentUserIp,
                                                             HttpContext.Current.Request.Cookies["CWC"]?.Value ?? "-1",
                                                             DateTime.UtcNow.Ticks), 
                                  "/used/");
            return Ok(new { inquiryId = CarwaleSecurity.Encrypt(inquiryId.ToString()) });
        }

        [Obsolete("use v2")]
        [HttpPost, Route("api/v1/used/sell/live/")]
        public IHttpActionResult TakeLiveV1([FromBody]string tempId)
        {
            int tempInquiryId;
            if (string.IsNullOrWhiteSpace(tempId) || !Int32.TryParse(CarwaleSecurity.Decrypt(tempId, true), out tempInquiryId) || tempInquiryId <= 0)
            {
                return Content(HttpStatusCode.BadRequest, new ModalPopUp()
                {
                    Heading = "Data Invalid",
                    Description = "Data entered is incorrect"
                }, _jsonFormatter);
            }

            TempCustomerSellInquiry tempInquiry = _sellCarRepository.GetTempCustomerSellInquiry(tempInquiryId);
            var sellCarInfoValidation = new SellCarInfoValidator().Validate(tempInquiry.sellCarInfo, ruleSet: "address,carmake,carmodel,carversion,cardetails,carregistration,carregistrationtype,carinsurance");
            if (!sellCarInfoValidation.IsValid)
            {
                FluentValidationError.LogValidationErrorsAsync(sellCarInfoValidation.Errors, "sellCarValidationErrors ");
                return Content(HttpStatusCode.BadRequest, new ModalPopUp()
                {
                    Heading = "Data Invalid",
                    Description = "Data entered is incorrect"
                }, _jsonFormatter);
            }
            if (!_otpLogic.IsNumberVerified(tempInquiry.sellCarCustomer.Mobile))
            {
                return Content(HttpStatusCode.Forbidden, new ModalPopUp
                {
                    Heading = "Forbidden",
                    Description = "Mobile number is not verified"
                }, _jsonFormatter);
            }

            if (_sellCarBL.CreateCustomer(tempInquiry.sellCarCustomer)) //auto login if new customer
            {
                HttpContext.Current.Response.Cookies.Add(CustomerCookie.StartSession(
                    tempInquiry.sellCarCustomer.Name,
                    tempInquiry.sellCarCustomer.Id.ToString(),
                    tempInquiry.sellCarCustomer.Email
                    ));
            }
            else
            {
                if (CustomerCookie.GetUserId() != "-1" && CustomerCookie.GetUserId() != tempInquiry.sellCarCustomer.Id.ToString())
                {
                    CustomerCookie.EndSession();
                }
            }

            int inquiryId = _sellCarBL.CreateSellCarInquiryV1(tempInquiry);
            if (inquiryId <= 0)
            {
                return Content(HttpStatusCode.InternalServerError, new ModalPopUp()
                {
                    Heading = "Error!",
                    Description = "Something went wrong"
                }, _jsonFormatter);
            }
            return Ok(new { inquiryId = CarwaleSecurity.Encrypt(inquiryId.ToString()) });
        }

        /// <summary>
        /// Api to take indvidual cars live
        /// Added sourceid for detecting platform header
        /// use vernam service for checking whenther user is verified
        /// </summary>
        /// <param name="tempId"></param>
        /// <returns></returns>
        [HttpPost, Route("api/v2/used/sell/live/")]
        public IHttpActionResult TakeLiveV2([FromBody]string tempId)
        {

            int tempInquiryId;
            Platform platform;
            if(!Enum.TryParse(HttpContextUtils.GetHeader<string>("sourceId"), out platform))
            {
                platform = Platform.CarwaleDesktop;
            }
            if (string.IsNullOrWhiteSpace(tempId) || !int.TryParse(CarwaleSecurity.Decrypt(tempId, true), out tempInquiryId) || tempInquiryId <= 0)
            {
                return Content(HttpStatusCode.BadRequest, new ModalPopUp()
                {
                    Heading = "Data Invalid",
                    Description = "Data entered is incorrect"
                }, _jsonFormatter);
            }
            string validationRuleSet = "address,carmake,carmodel,carversion,cardetails,carregistration,carinsurance";
            //car registration type is mandatory for desktop but not for msite
            if (platform == Platform.CarwaleDesktop)
            {
                validationRuleSet = $"{validationRuleSet},carregistrationtype";
            }

            TempCustomerSellInquiry tempInquiry = _sellCarRepository.GetTempCustomerSellInquiry(tempInquiryId);
            var sellCarInfoValidation = new SellCarInfoValidator().Validate(tempInquiry.sellCarInfo, ruleSet: validationRuleSet);
            if (!sellCarInfoValidation.IsValid)
            {
                FluentValidationError.LogValidationErrorsAsync(sellCarInfoValidation.Errors, "sellCarValidationErrors ");
                return Content(HttpStatusCode.BadRequest, new ModalPopUp()
                {
                    Heading = "Data Invalid",
                    Description = "Data entered is incorrect"
                }, _jsonFormatter);
            }
            var requestData = new RequestData
            {
                VerificationType = VerificationType.Mobile,
                VerificationValue = tempInquiry.sellCarCustomer.Mobile,
                
            };
            _apiGatewayIsVerifiedAdapter.AddApiGatewayCallWithCallback(_apiGatewayCaller, requestData);
            _apiGatewayCaller.Call();
            bool isVerified = _apiGatewayIsVerifiedAdapter.Output;

            if (!isVerified)
            {
                return Content(HttpStatusCode.Forbidden, new ModalPopUp
                {
                    Heading = "Forbidden",
                    Description = "Mobile number is not verified"
                }, _jsonFormatter);
            }

            if (_sellCarBL.CreateCustomer(tempInquiry.sellCarCustomer)) //auto login if new customer
            {
                HttpContext.Current.Response.Cookies.Add(CustomerCookie.StartSession(
                    tempInquiry.sellCarCustomer.Name,
                    tempInquiry.sellCarCustomer.Id.ToString(),
                    tempInquiry.sellCarCustomer.Email
                    ));
            }
            else
            {
                if (CustomerCookie.GetUserId() != "-1" && CustomerCookie.GetUserId() != tempInquiry.sellCarCustomer.Id.ToString())
                {
                    CustomerCookie.EndSession();
                }
            }
            tempInquiry.Platform = platform;
            int inquiryId = platform == Platform.CarwaleDesktop ? _sellCarBL.CreateSellCarInquiryV1(tempInquiry) : _sellCarBL.CreateSellCarInquiry(tempInquiry);
            if (inquiryId <= 0)
            {
                return Content(HttpStatusCode.InternalServerError, new ModalPopUp()
                {
                    Heading = "Error!",
                    Description = "Something went wrong"
                }, _jsonFormatter);
            }
            //Creat cookie for msite since we need to redirect to mylisting page on completion
            if(platform == Platform.CarwaleMobile)
            {
                _myListings.AddCookie(_authTokenCookie,
                                  TokenManager.GenerateToken(tempInquiry.sellCarCustomer.Mobile,
                                                             IpTracker.CurrentUserIp,
                                                             HttpContext.Current.Request.Cookies["CWC"]?.Value ?? "-1",
                                                             DateTime.UtcNow.Ticks),
                                  "/used/");
            }
            return Ok(new { inquiryId = CarwaleSecurity.Encrypt(inquiryId.ToString()) });
        }

        [Obsolete("use v1")]
        [HttpPost, Route("api/used/sell/verify/")]
        [ValidateSourceFilter]
        public IHttpActionResult Verify([FromBody]string mobileNumber)
        {
            int sourceId = Request.Headers.GetValueFromHttpHeader<int>("sourceId");

            if (!RegExValidations.IsValidMobile(mobileNumber))
            {
                return Content(HttpStatusCode.BadRequest, new ModalPopUp
                {
                    Heading = "Data Invalid",
                    Description = "The data entered is invalid. Please try again."
                }, _jsonFormatter);
            }

            if (_blockedCommunicationRepo.IsCommunicationBlocked(new BlockedCommunication { Value = mobileNumber, Type = CommunicationType.Mobile, Module = CommunicationModule.SellCar }))
            {
                return Content(HttpStatusCode.Forbidden, new ModalPopUp
                {
                    Heading = "Number Blocked",
                    Description = "Your number entered is blocked. Please contact CarWale."
                }, _jsonFormatter);
            }

            if (!_sellCarBL.CheckFreeListingAvailability(mobileNumber))
            {
                return Content(HttpStatusCode.Forbidden, new ModalPopUp
                {
                    Heading = "Listing Limit Reached",
                    Description = Constants.IndividualListingLimitMessage
                }, _jsonFormatter);
            }
            // SaveSms data is temporary, and will be removed once we start sms microservice for sending sms
            int smsSentId = _otpLogic.SaveSmsData(mobileNumber, "sending otp", (int)SMSType.MobileVerification, true, string.Empty, Request.RequestUri);
            var response = _otpLogic.Generate(mobileNumber, (int)Application.CarWale, (Platform)sourceId, "sellcar", IpTracker.CurrentUserIp,smsSentId);
            return Content(HttpStatusCode.OK, response, _jsonFormatter);
        }

        /// <summary>
        /// Verify mobile and generate otp
        /// Call vernam for otp generation
        /// </summary>
        /// <param name="mobileNumber"></param>
        /// <returns></returns>
        [HttpPost, Route("api/v1/used/sell/verify/")]
        [ValidateSourceFilter]
        public IHttpActionResult VerifyV1([FromBody]string mobileNumber)
        {
            
            if (!RegExValidations.IsValidMobile(mobileNumber))
            {
                return Content(HttpStatusCode.BadRequest, new ModalPopUp
                {
                    Heading = "Data Invalid",
                    Description = "The data entered is invalid. Please try again."
                }, _jsonFormatter);
            }

            if (_blockedCommunicationRepo.IsCommunicationBlocked(new BlockedCommunication { Value = mobileNumber, Type = CommunicationType.Mobile, Module = CommunicationModule.SellCar }))
            {
                return Content(HttpStatusCode.Forbidden, new ModalPopUp
                {
                    Heading = "Number Blocked",
                    Description = "Your number entered is blocked. Please contact CarWale."
                }, _jsonFormatter);
            }

            if (!_sellCarBL.CheckFreeListingAvailability(mobileNumber))
            {
                return Content(HttpStatusCode.Forbidden, new ModalPopUp
                {
                    Heading = "Listing Limit Reached",
                    Description = Constants.IndividualListingLimitMessage
                }, _jsonFormatter);
            }
            string cwc = HttpContextUtils.GetCookie("CWC");
            string imei = HttpContextUtils.GetHeader<string>("IMEI");
            string clientIp = UserTracker.GetUserIp()?.Split(',')[0].Trim();
            Platform platform;
            if (!Enum.TryParse(HttpContextUtils.GetHeader<string>("sourceId"), out platform))
            {
                platform = Platform.CarwaleDesktop;
            }
            var requestData = new RequestData
            {
                VerificationType = VerificationType.Mobile,
                VerificationValue = mobileNumber,
                DeviceId = cwc ?? imei,
                PlatformId = platform,
                SourceModule = SourceModule.SellCar,
                ClientIp = clientIp,
                Validity = 30,
                OtpLength = 4
            };
            _apiGatewayInitiateOtpAdapter.AddApiGatewayCallWithCallback(_apiGatewayCaller, requestData);
            _apiGatewayCaller.Call();
            var response = _apiGatewayInitiateOtpAdapter.Output;
            return Content(HttpStatusCode.OK, response, _jsonFormatter);
        }

        [HttpGet, Route("api/used/sell/c2bcity/")]
        public IHttpActionResult IsC2BCity(int cityId)
        {
            return Ok(_sellCarBL.IsC2bCity(cityId));
        }
    }
}
