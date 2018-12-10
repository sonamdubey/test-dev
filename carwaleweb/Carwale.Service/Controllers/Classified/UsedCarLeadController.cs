using AutoMapper;
using Carwale.DAL.ApiGateway;
using Carwale.DAL.ApiGateway.ApiGatewayHelper;
using Carwale.DTOs.Classified.CarDetails;
using Carwale.DTOs.Classified.Leads;
using Carwale.Entity.Classified;
using Carwale.Entity.Classified.Leads;
using Carwale.Entity.Classified.UsedLeads;
using Carwale.Entity.Enum;
using Carwale.Entity.UsedCarsDealer;
using Carwale.Entity.Vernam;
using Carwale.Interfaces.Classified.Leads;
using Carwale.Interfaces.CustomerVerification;
using Carwale.Interfaces.Elastic;
using Carwale.Interfaces.Stock;
using Carwale.Service.Filters;
using Carwale.Utility;
using Microsoft.Practices.Unity;
using FluentValidation;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Carwale.Service.Controllers.Classified
{
    public class UsedCarLeadController : ApiController
    {
        private readonly IStockBL _stockBL;
        private readonly ILeadBL _leadBL;
        private readonly ILeadCacheRepository _usedCarLeadsCache;
        private readonly IStockCertificationCacheRepository _certCacheRepo;
        private readonly IElasticSearchManager _searchManager;
        private readonly ICustomerVerification _customerVerification;
        private readonly IEnumerable<int> certificationOrigins = ConfigurationManager.AppSettings["CertificationLeadOrigins"].Split(',').Select(int.Parse);
        private readonly IStockRecommendationsBL _stockRecommendationsBL;
        private readonly IApiGatewayAdapter<RequestData, bool, VerNam.ProtoClass.GrpcBool> _apiGatewayIsVerifiedForDeviceAdapter;
        private readonly IApiGatewayAdapter<RequestData, bool, VerNam.ProtoClass.GrpcBool> _apiGatewayInitiateOtpAdapter;
        private readonly IApiGatewayCaller _apiGatewayCaller;
        private readonly ICustomerVerificationRepository _custVerificationRepo;
        private static readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore
        };

        private const string _stockSoldOutMsg = "Stock has been sold out.";
        private const string _limitReachedMsg = "Oops! You have reached the maximum limit for viewing inquiry details. Please try after a few days.";
        private const string _unverifiedMobileMsg = " Mobile number not verified ";
        private const string _mismatchedMobileMsg = "Mobile number mismatched.";
        private readonly string _appId = ConfigurationManager.AppSettings["ApplozicChatApplicationId"];

        public UsedCarLeadController(IStockBL stockBL, ILeadBL leadBL, ILeadCacheRepository usedCarLeadsCache, IStockCertificationCacheRepository certCacheRepo, IElasticSearchManager searchManager, ICustomerVerification customerVerification, IStockRecommendationsBL stockRecommendationsBL,
            [Dependency("IsVerifiedForDevice")]IApiGatewayAdapter<RequestData, bool, VerNam.ProtoClass.GrpcBool> apiGatewayIsVerifiedForDeviceAdapter,
            [Dependency("InitiateOtp")]IApiGatewayAdapter<RequestData, bool, VerNam.ProtoClass.GrpcBool> apiGatewayInitiateOtpAdapter,
            IApiGatewayCaller apiGatewayCaller,
            ICustomerVerificationRepository custVerificationRepo)
        {
            _stockBL = stockBL;
            _leadBL = leadBL;
            _usedCarLeadsCache = usedCarLeadsCache;
            _certCacheRepo = certCacheRepo;
            _searchManager = searchManager;
            _customerVerification = customerVerification;
            _stockRecommendationsBL = stockRecommendationsBL;
            _apiGatewayIsVerifiedForDeviceAdapter = apiGatewayIsVerifiedForDeviceAdapter;
            _apiGatewayInitiateOtpAdapter = apiGatewayInitiateOtpAdapter;
            _apiGatewayCaller = apiGatewayCaller;
            _custVerificationRepo = custVerificationRepo;
        }

        [HttpGet]
        [Route("api/usedcarleads"), HandleException]
        public IHttpActionResult GetUsedCarsLeadsCount([FromUri]DealerInfo dId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            DealerLeadsCount currentMonthCount = _usedCarLeadsCache.GetLeadsCountForCurrentMonth(dId.dealerId);
            DealerLeadsCount lastMonthCount = _usedCarLeadsCache.GetLeadsCountForLastMonth(dId.dealerId);
            DealerLeadsCount secondLastMonthCount = _usedCarLeadsCache.GetLeadsCountForSecondLastMonth(dId.dealerId);

            UsedCarLeadCount usedCarLeadCount = new UsedCarLeadCount()
            {
                CurrentMonthLeadCount = currentMonthCount.VerifiedLeadCount + currentMonthCount.UnverifiedLeadCount,
                LastMonthLeadCount = lastMonthCount.VerifiedLeadCount + lastMonthCount.UnverifiedLeadCount,
                SecondLastMonthLeadCount = secondLastMonthCount.VerifiedLeadCount + secondLastMonthCount.UnverifiedLeadCount
            };

            return Ok(usedCarLeadCount);
        }

        [HttpGet, Route("api/stockbuyers/"), HandleException, EnableCors("https://www-carwale-com.cdn.ampproject.org, https://cdn.ampproject.org, https://www-carwale-com.amp.cloudflare.com", "*", "GET")]
        public IHttpActionResult GetStockBuyer()
        {
            string ampOrigin = HttpUtility.ParseQueryString(Request.RequestUri.Query)["__amp_source_origin"];
            if (!string.IsNullOrWhiteSpace(ampOrigin))
            {
                HttpContextUtils.AddAmpHeaders(ampOrigin, true);
            }

            Buyer buyer = new Buyer();
            var leadCookie = HttpContextUtils.GetCookie("TempCurrentUser");
            if (!string.IsNullOrWhiteSpace(leadCookie))
            {
                string[] parts = leadCookie.Split(':');
                buyer.Name = parts[0];
                buyer.Mobile = parts[1];
            }
            return Json(buyer, _serializerSettings);
        }

        [HttpPost]
        [Route("api/v1/stockleads"), HandleException, EnableCors("https://www-carwale-com.cdn.ampproject.org, https://cdn.ampproject.org, https://www-carwale-com.amp.cloudflare.com", "*", "POST")]
        public IHttpActionResult PostV1()
        {
            Lead lead = null;
            string ampOrigin = HttpUtility.ParseQueryString(Request.RequestUri.Query)["__amp_source_origin"];
            if (!string.IsNullOrWhiteSpace(ampOrigin))
            {
                lead = GetAmpLeadRequestBody();
                HttpContextUtils.AddAmpHeaders(ampOrigin, true);
            }
            else
            {
                var reqContent = Request.Content.ReadAsStringAsync().Result;
                if (reqContent != null)
                {
                    lead = JsonConvert.DeserializeObject<Lead>(reqContent, _serializerSettings);
                }
            }

            if (lead == null || !(new LeadValidator().Validate(lead).IsValid))
            {
                return BadRequest("Information you provided was invalid. Please provide valid information.");
            }

            var stock = _stockBL.GetStock(lead.ProfileId);
            if (stock == null || stock.BasicCarInfo == null || stock.IsSold)
            {
                return BadRequest(_stockSoldOutMsg);
            }

            LeadStockSummary stockSummary = GetStockSummary(stock);
            string cwc = HttpContextUtils.GetCookie("CWC");
            string imei = HttpContextUtils.GetHeader<string>("IMEI");
            Entity.Enum.Platform platform = string.IsNullOrWhiteSpace(ampOrigin) ? HttpContextUtils.GetHeader<Entity.Enum.Platform>("sourceID") : Entity.Enum.Platform.CarwaleMobile;
            string clientIp = UserTracker.GetUserIp()?.Split(',')[0].Trim();
            //Parameter necessary for checking is Verified
            var requestData = new RequestData
            {
                VerificationType = VerificationType.Mobile,
                VerificationValue = lead.Buyer.Mobile,
                DeviceId = cwc ?? imei,
            };
           _apiGatewayIsVerifiedForDeviceAdapter.AddApiGatewayCallWithCallback(_apiGatewayCaller, requestData);
            _apiGatewayCaller.Call();
            bool isVerified = _apiGatewayIsVerifiedForDeviceAdapter.Output;

            var leadReport = _leadBL.ProcessLead(new LeadDetail()
            {
                Stock = stockSummary,
                Buyer = lead.Buyer,
                LeadSource = platform,
                LeadTrackingParams = lead.LeadTrackingParams,
                IPAddress = clientIp,
                IMEICode = imei,
                AppVersion = HttpContextUtils.GetHeader<int>("appVersion"),
                LTSrc = HttpContextUtils.GetCookie("CWLTS"),
                Cwc = cwc,
                CWUtmzCookie = HttpContextUtils.GetCookie("_cwutmz"),
                UtmaCookie = HttpContextUtils.GetCookie("__utma"),
                UtmzCookie = HttpContextUtils.GetCookie("__utmz"),
                AbTestCookie = HttpContextUtils.GetCookie("_abtest"),
                IsLeadFromChatSms = lead.IsChatSms,
                IsVerified = isVerified
            });
            if (leadReport.Status == LeadStatus.Unverified)
            {
                if (!string.IsNullOrWhiteSpace(ampOrigin))
                {
                    //Required param for Otp Initiation apart from other added earlier
                    requestData.PlatformId = platform;
                    requestData.SourceModule = SourceModule.UsedCarLeads;
                    requestData.ClientIp = clientIp;
                    requestData.Validity = 30;
                    requestData.OtpLength = 5;
                    _apiGatewayInitiateOtpAdapter.AddApiGatewayCallWithCallback(_apiGatewayCaller, requestData);
                    _apiGatewayCaller.Call();
                }
                ModelState.AddModelError("MobileUnverified", _unverifiedMobileMsg);
                return this.ModelStateContent(HttpStatusCode.Forbidden);
            }
            else if (leadReport.Status == LeadStatus.MobileLimitExceeded || leadReport.Status == LeadStatus.IpLimitExceeded
                || leadReport.Status == LeadStatus.MobileBlocked || leadReport.Status == LeadStatus.IpBlocked)
            {
                return this.Message(_limitReachedMsg, HttpStatusCode.Forbidden);
            }
            else if (leadReport.Status == LeadStatus.InvalidChatSmsLead)
            {
                ModelState.AddModelError("MobileMismatched", _mismatchedMobileMsg);
                return this.ModelStateContent(HttpStatusCode.BadRequest);
            }
            else if (leadReport.LeadId > 0)
            {
                if (!string.IsNullOrWhiteSpace(ampOrigin))
                {
                    var cookie = new HttpCookie("TempCurrentUser", $"{lead.Buyer.Name}:{lead.Buyer.Mobile}::0");
                    cookie.Expires = DateTime.Now.AddDays(90);
                    HttpContext.Current.Response.Cookies.Add(cookie);
                }
                return GetLeadSuccessResponse(lead.LeadTrackingParams, stockSummary, leadReport.BuyerInfo, leadReport.Seller);
            }
            else
            {
                return InternalServerError();
            }
        }

        [HttpPost]
        [Route("api/stockleads"), HandleException, EnableCors("https://www-carwale-com.cdn.ampproject.org, https://cdn.ampproject.org, https://www-carwale-com.amp.cloudflare.com", "*", "POST")]
        public IHttpActionResult Post()
        {
            Lead lead = null;
            string ampOrigin = HttpUtility.ParseQueryString(Request.RequestUri.Query)["__amp_source_origin"];
            if (!string.IsNullOrWhiteSpace(ampOrigin))
            {
                lead = GetAmpLeadRequestBody();
                HttpContextUtils.AddAmpHeaders(ampOrigin, true);
            }
            else
            {
                var reqContent = Request.Content.ReadAsStringAsync().Result;
                if (reqContent != null)
                {
                    lead = JsonConvert.DeserializeObject<Lead>(reqContent, _serializerSettings);
                }
            }

            if (lead == null || !(new LeadValidator().Validate(lead).IsValid))
            {
                return BadRequest("Information you provided was invalid. Please provide valid information.");
            }

            var stock = _stockBL.GetStock(lead.ProfileId);
            if (stock == null || stock.BasicCarInfo == null || stock.IsSold)
            {
                return BadRequest(_stockSoldOutMsg);
            }

            LeadStockSummary stockSummary = GetStockSummary(stock);
            string cwc = HttpContextUtils.GetCookie("CWC");
            string imei = HttpContextUtils.GetHeader<string>("IMEI");
            bool isVerified = _custVerificationRepo.IsVerified(lead.Buyer.Mobile, cwc ?? imei);

            var leadReport = _leadBL.ProcessLead(new LeadDetail()
            {
                Stock = stockSummary,
                Buyer = lead.Buyer,
                LeadSource = string.IsNullOrWhiteSpace(ampOrigin) ? HttpContextUtils.GetHeader<Entity.Enum.Platform>("sourceID") : Entity.Enum.Platform.CarwaleMobile,
                LeadTrackingParams = lead.LeadTrackingParams,
                IPAddress = UserTracker.GetUserIp()?.Split(',')[0].Trim(),
                IMEICode = imei,
                AppVersion = HttpContextUtils.GetHeader<int>("appVersion"),
                LTSrc = HttpContextUtils.GetCookie("CWLTS"),
                Cwc = cwc,
                CWUtmzCookie = HttpContextUtils.GetCookie("_cwutmz"),
                UtmaCookie = HttpContextUtils.GetCookie("__utma"),
                UtmzCookie = HttpContextUtils.GetCookie("__utmz"),
                AbTestCookie = HttpContextUtils.GetCookie("_abtest"),
                IsLeadFromChatSms = lead.IsChatSms,
                IsVerified = isVerified
            });
            if (leadReport.Status == LeadStatus.Unverified)
            {
                if (!string.IsNullOrWhiteSpace(ampOrigin))
                {                    
                    string clientTokenId = cwc;
                    _customerVerification.IsMobileAndTokenVerified(lead.Buyer.Mobile, Entity.Enum.Platform.CarwaleMobile, clientTokenId);
                }
                ModelState.AddModelError("MobileUnverified", _unverifiedMobileMsg);
                return this.ModelStateContent(HttpStatusCode.Forbidden);
            }
            else if (leadReport.Status == LeadStatus.MobileLimitExceeded || leadReport.Status == LeadStatus.IpLimitExceeded
                || leadReport.Status == LeadStatus.MobileBlocked || leadReport.Status == LeadStatus.IpBlocked)
            {
                return this.Message(_limitReachedMsg, HttpStatusCode.Forbidden);
            }
            else if (leadReport.Status == LeadStatus.InvalidChatSmsLead)
            {
                ModelState.AddModelError("MobileMismatched", _mismatchedMobileMsg);
                return this.ModelStateContent(HttpStatusCode.BadRequest);
            }
            else if (leadReport.LeadId > 0)
            {
                if (!string.IsNullOrWhiteSpace(ampOrigin))
                {
                    var cookie = new HttpCookie("TempCurrentUser", $"{lead.Buyer.Name}:{lead.Buyer.Mobile}::0");
                    cookie.Expires = DateTime.Now.AddDays(90);
                    HttpContext.Current.Response.Cookies.Add(cookie);
                }
                return GetLeadSuccessResponse(lead.LeadTrackingParams, stockSummary, leadReport.BuyerInfo, leadReport.Seller);
            }
            else
            {
                return InternalServerError();
            }
        }

        private static LeadStockSummary GetStockSummary(Entity.Classified.CarDetails.CarDetailsEntity stock)
        {
            LeadStockSummary stockSummary = Mapper.Map<LeadStockSummary>(stock.BasicCarInfo);
            stockSummary.MainImageUrl = string.IsNullOrEmpty(stock.ImageList?.MainImageUrl)
                                       ? "https://img.carwale.com/used/no-cars.jpg?q=85"
                                       : stock.ImageList.MainImageUrl;
            stockSummary.RatingText = stock.DealerInfo?.RatingText;
            return stockSummary;
        }

        private Lead GetAmpLeadRequestBody()
        {
            var reqParams =  HttpContext.Current.Request.Params;
            Buyer buyer = new Buyer
            {
                Name = reqParams["name"],
                Mobile = reqParams["mobile"]
            };

            int id;
            var trackingParams = new LeadTrackingParams
            {
                Rank = reqParams["rank"],
                OriginId = int.TryParse(reqParams["originId"], out id) ? id : 0,
                DeliveryCity = int.TryParse(reqParams["deliveryCity"], out id) ? id : 0,
                QueryString = reqParams["queryString"]
            };

            Lead lead = new Lead
            {
                ProfileId = reqParams["profileId"],
                Buyer = buyer,
                LeadTrackingParams = trackingParams
            };
            return lead;
        }

        private IHttpActionResult GetLeadSuccessResponse(LeadTrackingParams leadTrackingParams, LeadStockSummary stock, BuyerInfo buyerInfo, Seller seller = null)
        {
            int originId = leadTrackingParams == null ? 0 : leadTrackingParams.OriginId;
            LeadResponse response = new LeadResponse
            {
                Buyer = Mapper.Map<BuyerDTO>(buyerInfo),
                AppId = _appId,
                WhatsAppMessage = leadTrackingParams.LeadType == LeadType.WhatsAppLead 
                                ? $"Hi, I would like to enquire about the { stock.Color } { stock.Make } { stock.CarName }, { stock.Kilometers } Kms which you have listed for Rs. { stock.Price } on CarWale"
                                : null
            };
            if (certificationOrigins.Contains(originId))
            {
                var certification = _certCacheRepo.GetCarCertification(stock.InquiryId, stock.IsDealer);
                if (certification != null && !string.IsNullOrEmpty(certification.ReportUrl))
                {
                    response.CertificationReportUrl = certification.ReportUrl;
                }
                return Json(response, _serializerSettings);
            }
            else if (originId >= 0)
            {
                seller = seller ?? _leadBL.GetSeller(stock.InquiryId, stock.IsDealer);
                if (seller != null)
                {
                    response.Seller = Mapper.Map<SellerDTO>(seller);
                    response.Seller.Mobile = leadTrackingParams?.LeadType == LeadType.WhatsAppLead && !string.IsNullOrEmpty(seller.WhatsAppNumber) 
                                            ? seller.WhatsAppNumber 
                                            : seller.DisplayNumber;
                    response.Seller.RatingText = stock.RatingText;
                    response.Stock = stock;
                    return Json(response, _serializerSettings);
                }
                else
                {
                    return this.Message("Seller details not available.");
                }
            }
            else
            {
                return Ok();
            }
        }

        [HttpGet]
        [Route("api/sellerdetails"), HandleException]
        public IHttpActionResult GetSellerDetails(string profileId, string buyerName, string buyerEmail, string buyerMobile, string dc = "", int originId = -1, bool isCertificationDownload = false)
        {
            Entity.Enum.Platform source = HttpContextUtils.GetHeader<Entity.Enum.Platform>("sourceID");
            string imeicode = HttpContextUtils.GetHeader<string>("IMEI");
            int deliveryCity = 0;

            string certificationReport = string.Empty;
            SellerDetailsApp sellerDetails = new SellerDetailsApp()
            {
                SourceId = source.ToString("D"),
                IMEICode = imeicode
            };
            if (!string.IsNullOrEmpty(dc))
            {
                sellerDetails.DeliveryCity = Convert.ToInt32(dc);
                sellerDetails.DeliveryText = _stockBL.GetDeliveryText(Convert.ToInt32(dc));
                Int32.TryParse(dc, out deliveryCity);
            }

            BuyerProcessResponseCode responseCode;
            string responseMessage;
            bool isLeadValid;

            Lead lead = FormatLead(profileId, buyerName, buyerEmail, buyerMobile, originId, deliveryCity, out isLeadValid);
            if (!isLeadValid)
            {
                responseCode = BuyerProcessResponseCode.InvalidUser;
                responseMessage = "(Invalid Data)";
            }
            else
            {
                sellerDetails.MobileVerified = "True";
                sellerDetails.NewCVID = string.Empty;

                var stock = _stockBL.GetStock(profileId);
                if (stock == null || stock.BasicCarInfo == null || stock.IsSold)
                {
                    responseCode = BuyerProcessResponseCode.InvalidUser;
                    responseMessage = "(Invalid Data)";
                }
                else
                {
                    LeadStockSummary stockSummary = GetStockSummary(stock);
                    var appVersion = HttpContextUtils.GetHeader<int>("appVersion");
                    bool isVerified = _custVerificationRepo.IsMobileVerified(lead.Buyer.Mobile);
                    var report = _leadBL.ProcessLead(new LeadDetail()
                    {
                        Stock = stockSummary,
                        Buyer = lead.Buyer,
                        LeadSource = source,
                        LeadTrackingParams = lead.LeadTrackingParams,
                        AppVersion = appVersion,
                        IMEICode = imeicode,
                        AbTestCookie = "-1",
                        IsVerified = isVerified
                    });

                    if (report.Status == LeadStatus.Unverified)
                    {
                        sellerDetails.MobileVerified = "False";
                        sellerDetails.NewCVID = "-1";
                        var verificationResponse = _customerVerification.IsMobileVerified(lead.Buyer.Mobile, string.Empty, source);
                        sellerDetails.ZipDialNum = source == Entity.Enum.Platform.CarwaleiOS ? null : verificationResponse.TollFreeNumber;
                        responseCode = BuyerProcessResponseCode.Unverified;
                        responseMessage = _unverifiedMobileMsg;
                    }
                    else if (report.Status == LeadStatus.MobileLimitExceeded || report.Status == LeadStatus.IpLimitExceeded
                        || report.Status == LeadStatus.MobileBlocked || report.Status == LeadStatus.IpBlocked)
                    {
                        responseCode = BuyerProcessResponseCode.LimitReached;
                        responseMessage = _limitReachedMsg;
                    }
                    else
                    {
                        if (isCertificationDownload)
                        {
                            certificationReport = GetCertificationReport(stockSummary.InquiryId, stockSummary.IsDealer, out responseCode, out responseMessage);
                        }
                        else
                        {
                            SetSellerStatus(sellerDetails, stockSummary, report.Seller, out responseCode, out responseMessage);
                        }
                    }
                }
            }
            return GetLeadResponseApp(profileId, isCertificationDownload, responseCode, responseMessage, sellerDetails, certificationReport);
        }

        private Lead FormatLead(string profileId, string buyerName, string buyerEmail, string buyerMobile, int originId, int deliveryCity, out bool isValid)
        {
            Buyer buyer = new Buyer()
            {
                Name = buyerName,
                Email = buyerEmail,
                Mobile = buyerMobile
            };
            LeadTrackingParams leadTrackingParams = new LeadTrackingParams()
            {
                OriginId = originId,
                DeliveryCity = deliveryCity
            };
            Lead lead = new Lead()
            {
                ProfileId = profileId,
                Buyer = buyer,
                LeadTrackingParams = leadTrackingParams
            };
            isValid = new LeadValidator().Validate(lead).IsValid;
            if (!string.IsNullOrEmpty(buyerEmail) && buyerEmail.Contains("@unknown.com"))
            {
                buyer.Email = string.Empty;
            }
            if (!string.IsNullOrEmpty(buyerName) && buyerName.Trim().ToLower() == "unknown")
            {
                buyer.Name = string.Empty;
            }
            return lead;
        }

        private string GetCertificationReport(int inquiryId, bool isDealer, out BuyerProcessResponseCode responseCode, out string responseMessage)
        {
            string certificationReport;
            var certification = _certCacheRepo.GetCarCertification(inquiryId, isDealer);
            if (certification != null && !string.IsNullOrEmpty(certification.ReportUrl))
            {
                certificationReport = certification.ReportUrl;
                responseCode = BuyerProcessResponseCode.CertificationReportSuccess;
                responseMessage = "OK";
            }
            else
            {
                certificationReport = string.Empty;
                responseCode = BuyerProcessResponseCode.CertificationReportNotAvailable;
                responseMessage = "Report not available.";
            }
            return certificationReport;
        }

        private void SetSellerStatus(SellerDetailsApp sellerDetails, LeadStockSummary stockSummary, Seller seller, out BuyerProcessResponseCode responseCode, out string responseMessage)
        {
            seller = seller ?? _leadBL.GetSeller(stockSummary.InquiryId, stockSummary.IsDealer);
            if (seller != null)
            {
                sellerDetails.SellerName = seller.Name;
                sellerDetails.SellerEmail = seller.Email;
                sellerDetails.SellerContact = seller.DisplayNumber;
                sellerDetails.SellerAddress = seller.Address;
                sellerDetails.SellerContactPerson = seller.ContactPerson;
                sellerDetails.RatingText = stockSummary.RatingText;

                responseCode = BuyerProcessResponseCode.Success;
                responseMessage = "OK";
            }
            else
            {
                throw new Exception($"Seller Details not available for : {stockSummary.InquiryId}");
            }
        }

        private IHttpActionResult GetLeadResponseApp(string profileId, bool isCertificationDownload, BuyerProcessResponseCode responseCode, string responseMessage, SellerDetailsApp sellerDetails, string certificationReport)
        {
            if (isCertificationDownload)
            {
                ReportResponse report = new ReportResponse()
                {
                    ResponseCode = responseCode.ToString("D"),
                    ResponseMessage = responseMessage,
                    CertificationReportUrl = certificationReport,
                    ZipDialNum = sellerDetails.ZipDialNum
                };
                return Ok(report);
            }
            else
            {
                sellerDetails.ResponseCode = responseCode.ToString("D");
                sellerDetails.ResponseMessage = responseMessage;
                if (responseCode == BuyerProcessResponseCode.Success)
                {
                    var recommendations = _stockRecommendationsBL.GetRecommendations(profileId.ToUpper());
                    if (recommendations != null && recommendations.Count > 0)
                    {
                        sellerDetails.AlternativeCars = Mapper.Map<List<StockBaseEntity>, List<UsedCar>>(recommendations);
                    }
                }
                return Ok(sellerDetails);
            }
        }
    }
}
