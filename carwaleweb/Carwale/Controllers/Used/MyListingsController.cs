using AEPLCore.Cache.Interfaces;
using AEPLCore.Security;
using AEPLCore.Utils.ClientTracker;
using AutoMapper;
using Carwale.DAL.ApiGateway;
using Carwale.DAL.ApiGateway.ApiGatewayHelper;
using Carwale.DTOs.Classified.MyListings;
using Carwale.DTOs.CustomerVerification;
using Carwale.Entity.Classified.ListingPayment;
using Carwale.Entity.Classified.MyListings;
using Carwale.Entity.Classified.SellCar;
using Carwale.Entity.Classified.SellCarUsed;
using Carwale.Entity.Enum;
using Carwale.Entity.Notifications;
using Carwale.Entity.Vernam;
using Carwale.Interfaces.Classified.ListingPayment;
using Carwale.Interfaces.Classified.MyListings;
using Carwale.Interfaces.Classified.SellCar;
using Carwale.Interfaces.Classified.UsedCarPhotos;
using Carwale.Interfaces.Notifications;
using Carwale.Interfaces.Otp;
using Carwale.Notifications.Logs;
using Carwale.UI.ClientBL;
using Carwale.UI.Common;
using Carwale.UI.ViewModels.Used.MyListings;
using Carwale.UI.ViewModels.Used.SellCar;
using Carwale.Utility;
using FluentValidation;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Routing;
using VerNam.ProtoClass;

namespace Carwale.UI.Controllers
{
    public class MyListingsController : Controller
    {
        private readonly IMyListings _myListings;
        private readonly IMyListingsRepository _myListingsRepo;
        private readonly ICarDetailsRepository _carDetailsRepo;
        private readonly ICarPhotosRepository _carPhotosRepo;
        private readonly IListingRepository _listingRepo;
        private readonly ISMSRepository _smsRepo;
        private readonly IReceiptRepository _receiptRepository;
        private readonly ISellCarBL _sellCarBL;
        private readonly IOtpLogic _otpLogic;
        private readonly string _authTokenCookie = ConfigurationManager.AppSettings["encryptedAuthToken"];
        private readonly IApiGatewayAdapter<RequestData, bool, VerNam.ProtoClass.GrpcBool> _apiGatewayInitiateOtpAdapter;
        private readonly IApiGatewayAdapter<RequestData, bool, GrpcBool> _verifyOtpGatewayAdapter;
        private readonly IApiGatewayCaller _apiGatewayCaller;

        public MyListingsController
            (
                IMyListings myListings,
                IMyListingsRepository myListingsRepo,
                ICarDetailsRepository carDetailsRepository,
                ICarPhotosRepository carPhotosRepo,
                IListingRepository listingRepo,
                ICacheManager cacheProvider,
                ISMSRepository smsRepo,
                IReceiptRepository receiptRepository,
                ISellCarBL sellCarBL,
                IOtpLogic otpLogic,
                IApiGatewayCaller apiGatewayCaller,
                [Dependency("InitiateOtp")]IApiGatewayAdapter<RequestData, bool, VerNam.ProtoClass.GrpcBool> apiGatewayInitiateOtpAdapter,
                [Dependency("VerifyOtp")]IApiGatewayAdapter<RequestData, bool, GrpcBool> verifyOtpGatewayAdapter
            )
        {
            _myListings = myListings;
            _myListingsRepo = myListingsRepo;
            _carDetailsRepo = carDetailsRepository;
            _carPhotosRepo = carPhotosRepo;
            _listingRepo = listingRepo;
            _smsRepo = smsRepo;
            _receiptRepository = receiptRepository;
            _sellCarBL = sellCarBL;
            _otpLogic = otpLogic;
            _apiGatewayCaller = apiGatewayCaller;
            _apiGatewayInitiateOtpAdapter = apiGatewayInitiateOtpAdapter;
            _verifyOtpGatewayAdapter = verifyOtpGatewayAdapter;
        }

        [Route("used/mylistings/search")]
        public ActionResult LandingPage()
        {
            LandingPageViewModel viewModel = new LandingPageViewModel
            {
                MetaData = _myListings.GetPageMetaTags(),
                Source = DetectPlatform()
            };
            return View("~/Views/MyListings/LandingPage.cshtml", viewModel);
        }

        /// <summary>
        /// Get Listing for specified mobile or inquiryId
        /// </summary>
        /// <param name="type">mobile/inquiryId</param>
        /// <param name="value">value of type param</param>
        /// <param name="otpCode">otp text (optional)</param>
        /// <param name="authToken">Authorization Token for security (optional)</param>
        /// <returns>otp view or Listing view</returns>
        [Route("used/mylistings/")]
        public ActionResult GetListings(int type, string value, string otpCode = "", string authToken = "")
        {
            try
            {
                if (!_myListings.ValidateSearchType(type))
                {
                    return ReturnBadRequest();
                }
                string mobileNumber = _myListings.GetMobile(type, value);
                if (string.IsNullOrEmpty(mobileNumber) || !RegExValidations.IsValidMobile(mobileNumber))
                {
                    return HttpNotFound();
                }
                ViewBag.Redirect = false;
                if (!string.IsNullOrWhiteSpace(Request.QueryString["isredirect"]))
                {
                    ViewBag.Redirect = Request.QueryString["isredirect"].Equals("true");
                }
                if (string.IsNullOrEmpty(otpCode))
                {
                    if (!string.IsNullOrEmpty(authToken) && _myListings.IsValidToken(authToken, mobileNumber, IpTracker.CurrentUserIp, CurrentUser.CWC))
                    {
                        if (Request.Cookies[_authTokenCookie] == null)
                        {
                            return RedirectToAction("LandingPage");
                        }
                        MyListingsViewModel viewModel = GetViewModel(mobileNumber);
                        return View("~/Views/MyListings/Listings.cshtml", viewModel);
                    }
                    if (ViewBag.Redirect) // invalid token, then redirect to search page
                    {
                        return RedirectToAction("LandingPage");
                    }

                    string smsId = _smsRepo.SaveSMSSentData(new SMS
                    {
                        Message = "sending otp",
                        Mobile = mobileNumber,
                        ReturnedMsg = string.Empty,
                        Status = true,
                        SMSType = (int)SMSType.MobileVerification,
                        PageUrl = "/used/mylistings/"
                    });
                    int id;
                    if (int.TryParse(smsId, out id) && id > 0 &&
                        _otpLogic.Generate(mobileNumber, 1, Platform.CarwaleMobile, "mylistings", IpTracker.CurrentUserIp, id))
                    {
                        return View("~/Views/MyListings/OtpScreen.cshtml");
                    }
                    else
                    {
                        return ServerError();
                    }
                }
                else
                {
                    if (_otpLogic.Verify(otpCode, mobileNumber))
                    {
                        CookieManager.Delete(_authTokenCookie);
                        _myListings.AddCookie(_authTokenCookie,
                                              TokenManager.GenerateToken(mobileNumber, IpTracker.CurrentUserIp, CurrentUser.CWC, DateTime.UtcNow.Ticks),
                                              "/used/");
                        MyListingsViewModel viewModel = GetViewModel(mobileNumber);
                        return View("~/Views/MyListings/Listings.cshtml", viewModel);
                    }
                    return ErrorHtmlResponse(HttpStatusCode.Forbidden, new ModalPopUpViewModel { });
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorHtmlResponse(HttpStatusCode.InternalServerError, new ModalPopUpViewModel
                {
                    Heading = "Something  went wrong",
                    Description = "Something went wrong.Please refresh the page and try again"
                });
            }
        }

        /// <summary>
        /// Get Listing for specified mobile or inquiryId
        /// </summary>
        /// <param name="type">mobile/inquiryId</param>
        /// <param name="value">value of type param</param>
        /// <param name="otpCode">otp text (optional)</param>
        /// <param name="authToken">Authorization Token for security (optional)</param>
        /// <returns>otp view or Listing view</returns>
        [Route("used/v1/mylistings/")]
        public ActionResult GetListingsV1(int type, string value, string otpCode = "", string authToken = "")
        {
            try
            {
                if (!_myListings.ValidateSearchType(type))
                {
                    return ReturnBadRequest();
                }
                string mobileNumber = _myListings.GetMobile(type, value);
                if (string.IsNullOrEmpty(mobileNumber) || !RegExValidations.IsValidMobile(mobileNumber))
                {
                    return HttpNotFound();
                }
                ViewBag.Redirect = false;
                if (!string.IsNullOrWhiteSpace(Request.QueryString["isredirect"]))
                {
                    ViewBag.Redirect = Request.QueryString["isredirect"].Equals("true");
                }
                if (string.IsNullOrEmpty(otpCode))
                {
                    if (!string.IsNullOrEmpty(authToken) && _myListings.IsValidToken(authToken, mobileNumber, IpTracker.CurrentUserIp, CurrentUser.CWC))
                    {
                        if (Request.Cookies[_authTokenCookie] == null)
                        {
                            return RedirectToAction("LandingPage");
                        }
                        MyListingsViewModel viewModel = GetViewModel(mobileNumber);
                        return View("~/Views/MyListings/Listings.cshtml", viewModel);
                    }
                    if (ViewBag.Redirect) // invalid token, then redirect to search page
                    {
                        return RedirectToAction("LandingPage");
                    }
                    InitiateVerificationDto initiateVerification = new InitiateVerificationDto
                    {
                        Mobile = mobileNumber,
                        MobileVerificationByType = MobileVerificationByType.Otp,
                        OtpLength = 4,
                        SourceModule = Entity.Vernam.SourceModule.EditCar,
                    };
                    RequestData requestData = Mapper.Map<RequestData>(initiateVerification);
                    _apiGatewayInitiateOtpAdapter.AddApiGatewayCallWithCallback(_apiGatewayCaller, requestData);
                    _apiGatewayCaller.Call();
                    if (_apiGatewayInitiateOtpAdapter.Output)
                    {
                        return View("~/Views/MyListings/OtpScreen.cshtml");
                    }
                    else
                    {
                        return ServerError();
                    }
                }
                else
                {
                    VerifyMobileOtpDto verifyMobileOtp = new VerifyMobileOtpDto
                    {
                        MobileNumber = mobileNumber,
                        OtpCode = otpCode,
                        SourceModule = Entity.Vernam.SourceModule.EditCar
                    };
                    RequestData requestData = Mapper.Map<RequestData>(verifyMobileOtp);
                    _verifyOtpGatewayAdapter.AddApiGatewayCallWithCallback(_apiGatewayCaller, requestData);
                    _apiGatewayCaller.Call();
                    if (_verifyOtpGatewayAdapter.Output)
                    {
                        CookieManager.Delete(_authTokenCookie);
                        _myListings.AddCookie(_authTokenCookie,
                                              TokenManager.GenerateToken(mobileNumber, IpTracker.CurrentUserIp, CurrentUser.CWC, DateTime.UtcNow.Ticks),
                                              "/used/");
                        MyListingsViewModel viewModel = GetViewModel(mobileNumber);
                        return View("~/Views/MyListings/Listings.cshtml", viewModel);
                    }
                    return ErrorHtmlResponse(HttpStatusCode.Forbidden, new ModalPopUpViewModel { });
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorHtmlResponse(HttpStatusCode.InternalServerError, new ModalPopUpViewModel
                {
                    Heading = "Something  went wrong",
                    Description = "Something went wrong.Please refresh the page and try again"
                });
            }
        }

        [Route("used/mylistings/{inquiryId}")]
        public ActionResult GetCarDetails(int inquiryId, string authToken)
        {
            if (inquiryId <= 0)
            {
                return ReturnBadRequest();
            }
            if (Request.Cookies[_authTokenCookie] == null)
            {
                return RedirectToAction("LandingPage");
            }
            string mobileNumber = _myListings.GetMobileFromProfileId(inquiryId);
            if (!CheckInput(mobileNumber, authToken))
            {
                return RedirectToAction("LandingPage");
            }
            if (!_myListingsRepo.IsCarCustomerEditable(inquiryId))
            {
                return RedirectToAction("GetListings", new { type = 1, value = mobileNumber, authToken, editable = "false", isredirect = "true" });
            }
            MyListingsDTO listingsDTO = _myListings.GetDataForEditListing(inquiryId);
            if (listingsDTO == null)
            {
                return ServerError();
            }
            listingsDTO.Source = BrowserUtils.IsAndroidWebView() ? Platform.CarwaleAndroid : BrowserUtils.IsIosWebView() ? Platform.CarwaleiOS : Platform.CarwaleMobile;
            _myListings.AddCookie("SellInquiry", CarwaleSecurity.Encrypt(inquiryId.ToString()), "/");
            return View("~/Views/m/Used/EditFlow/EditFlowListForm.cshtml", listingsDTO);
        }

        /// <summary>
        /// Update the whole listing details 
        /// </summary>
        /// <param name="inquiryId">Id of listing</param>
        /// <param name="authToken">Authorization Token for security</param>
        /// <param name="sellCarInfo">Data to be updated</param>
        /// <returns>Images view</returns>
        [Route("used/mylistings/{inquiryId}/")]
        [HttpPut]
        public ActionResult UpdateCarDetails(int inquiryId, string authToken, SellCarInfo sellCarInfo)
        {
            bool isCarDetailsUpdated = false;
            try
            {
                if (inquiryId <= 0 || sellCarInfo == null || !(new ListingDetailsValidator().Validate(sellCarInfo, ruleSet: "common").IsValid))
                {
                    return ReturnBadRequest();
                }
                string mobileNumber = _myListings.GetMobileFromProfileId(inquiryId);
                if (!CheckInput(mobileNumber, authToken))
                {
                    return RedirectToAction("LandingPage");
                }
                isCarDetailsUpdated = _carDetailsRepo.UpdateCarDetails(sellCarInfo, inquiryId);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ServerError();
            }

            if (!isCarDetailsUpdated)
            {
                return ServerError();
            }
            else
            {
                _myListings.RefreshCacheWithCriticalRead(inquiryId);
            }
            return Json(new { status = true });
        }

        /// <summary>
        /// Update the whole listing details 
        /// </summary>
        /// <param name="inquiryId">Id of listing</param>
        /// <param name="authToken">Authorization Token for security</param>
        /// <param name="sellCarInfo">Data to be updated</param>
        /// <returns>Images view</returns>
        [Route("v1/used/mylistings/{inquiryId}/")]
        [HttpPut]
        public ActionResult UpdateCarDetailsV1(int inquiryId, string authToken, SellCarInfo sellCarInfo)
        {
            bool isCarDetailsUpdated = false;
            try
            {
                if (inquiryId <= 0 || sellCarInfo == null || !(new ListingDetailsValidator().Validate(sellCarInfo, ruleSet: "common, carregistrationtype").IsValid))
                {
                    return ReturnBadRequest();
                }
                string mobileNumber = _myListings.GetMobileFromProfileId(inquiryId);
                if (!CheckInput(mobileNumber, authToken))
                {
                    return RedirectToAction("LandingPage");
                }
                isCarDetailsUpdated = _carDetailsRepo.UpdateCarDetailsV1(sellCarInfo, inquiryId);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ServerError();
            }

            if (!isCarDetailsUpdated)
            {
                return ServerError();
            }
            else
            {
                _myListings.RefreshCacheWithCriticalRead(inquiryId);
            }
            return Json(new { status = true });
        }

        /// <summary>
        /// Partial update for listing details
        /// </summary>
        /// <param name="inquiryId">Id of listing</param>
        /// <param name="authToken">Authorization Token for security</param>
        /// <param name="sellCarInfo">Data to be updated</param>
        /// <returns>Listing view</returns>
        [Route("used/mylistings/{inquiryId}/")]
        [HttpPatch]
        public ActionResult PatchCarDetails(int? inquiryId, string authToken, SellCarInfo sellCarInfo)
        {
            bool success = false;
            string mobileNumber = string.Empty;
            try
            {
                if (inquiryId == null || inquiryId <= 0 || sellCarInfo == null)
                {
                    return ReturnBadRequest();
                }
                mobileNumber = _myListings.GetMobileFromProfileId(inquiryId.Value);
                if (!CheckInput(mobileNumber, authToken))
                {
                    return RedirectToAction("LandingPage");
                }
                success = _listingRepo.PatchListings(inquiryId.Value, sellCarInfo);
                if (!success)
                {
                    return ServerError();
                }
                else
                {
                    _myListings.RefreshCacheWithCriticalRead(inquiryId.Value);
                }
                if (sellCarInfo.StatusId != null)
                {
                    _myListings.SendMail(inquiryId.Value, sellCarInfo);
                }

                MyListingsViewModel viewModel = GetViewModel(mobileNumber);
                return View("~/Views/MyListings/Listings.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ServerError();
            }
        }

        [Route("used/mylistings/{inquiryId}/images/")]
        [HttpGet]
        public ActionResult GetImagesView(int inquiryId, string authToken)
        {
            ImagePageViewModel imagePageViewModel = null;
            if (inquiryId <= 0)
            {
                return ReturnBadRequest();
            }
            if (Request.Cookies["encryptedAuthToken"] == null)
            {
                return RedirectToAction("LandingPage");
            }
            try
            {
                string mobileNumber = _myListings.GetMobileFromProfileId(inquiryId);
                if (!CheckInput(mobileNumber, authToken))
                {
                    return RedirectToAction("LandingPage");
                }
                if (!_myListingsRepo.IsCarCustomerEditable(inquiryId))
                {
                    return RedirectToAction("GetListings", new { type = 1, value = mobileNumber, authToken, editable = "false", isredirect = "true" });
                }
                imagePageViewModel = new ImagePageViewModel
                {
                    PhotoList = _carPhotosRepo.GetCarPhotos(inquiryId, false),
                    Source = BrowserUtils.IsAndroidWebView() ? Platform.CarwaleAndroid : BrowserUtils.IsIosWebView() ? Platform.CarwaleiOS : Platform.CarwaleMobile
                };
                ViewBag.InquiryId = inquiryId;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ServerError();
            }
            return View("~/Views/MyListings/Images.cshtml", imagePageViewModel);
        }

        [HttpGet, Route("used/mylistings/{inquiryid}/inquiries/")]
        public ActionResult GetLeads(int inquiryId, string authToken, Platform platform = Platform.CarwaleMobile)
        {
            InquiriesViewModel inquiriesViewModel = new InquiriesViewModel();
            if (inquiryId <= 0)
            {
                return ReturnBadRequest();
            }

            try
            {
                string mobileNumber = _myListings.GetMobileFromProfileId(inquiryId);
                if (!CheckInput(mobileNumber, authToken))
                {
                    return ErrorHtmlResponse(HttpStatusCode.Forbidden, new ModalPopUpViewModel
                    {
                        Heading = "Data Invalid!",
                        Description = "Access forbidden"
                    });
                }
                inquiriesViewModel.Inquiries = _myListings.GetClassifiedRequests(inquiryId, -1);
                inquiriesViewModel.Platform = platform;
                Logger.LogInfo("Request url param: " + Request.QueryString[0]);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ServerError();
            }
            return PartialView("~/Views/MyListings/Inquiries.cshtml", inquiriesViewModel);
        }

        [Route("used/mylistings/login")]
        public RedirectResult Login(string Id)
        {
            try
            {
                string[] decryptedValue = CarwaleSecurity.Decrypt(Id, true).Split('#');
                string profileId = decryptedValue[0];
                string page = decryptedValue[1];

                if (string.IsNullOrEmpty(profileId) && string.IsNullOrEmpty(page))
                {
                    throw new ArgumentException("Either profileId is invalid or return url is empty");
                }
                if (DeviceDetectionManager.IsMobile(HttpContext))
                {
                    string mobileNumber = _myListings.GetMobile((int)SearchType.ProfileId, profileId);
                    if (string.IsNullOrEmpty(mobileNumber) || !RegExValidations.IsValidMobile(mobileNumber))
                    {
                        throw new ArgumentException("Mobile is invalid");
                    }
                    string encryptedAuthToken = TokenManager.GenerateToken(mobileNumber, IpTracker.CurrentUserIp, CurrentUser.CWC, DateTime.UtcNow.Ticks);
                    _myListings.AddCookie(_authTokenCookie, encryptedAuthToken, "/used/");
                    _myListings.AddCookie("SellInquiry", CarwaleSecurity.Encrypt(profileId), "/");
                    page += page.Contains('?') ? '&' : '?';
                    return Redirect(page + "authToken=" + encryptedAuthToken);
                }
                else
                {
                    string deskUrl = _myListings.GetDestinationUrl(Convert.ToInt32(profileId), page);
                    string[] urlParts = page.Split('?');
                    if (urlParts.Length > 1)
                    {
                        deskUrl += deskUrl.Contains('?') ? '&' : '?';
                        deskUrl += urlParts[urlParts.Length - 1];
                    }
                    return Redirect(deskUrl);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return DeviceDetectionManager.IsMobile(this.HttpContext) ? Redirect("/m/pagenotfound.aspx") : Redirect("/pagenotfound.aspx");
            }
        }
        private MyListingsViewModel GetViewModel(string mobileNumber)
        {
            List<CustomerSellInquiry> listings = _myListingsRepo.GetListingsByMobile(mobileNumber);
            MyListingsViewModel viewModel = null;
            if (listings != null && listings.Any())
            {
                viewModel = new MyListingsViewModel
                {
                    Listings = new List<MyListingsDTO>()
                };
                foreach (var listing in listings)
                {
                    viewModel.Listings.Add(new MyListingsDTO
                    {
                        Id = listing.Id,
                        CarName = listing.CarName,
                        MakeMonthYearFormatted = listing.MakeYear.ToString("MMM yyyy"),
                        Price = Format.FormatFullPrice(listing.Price.ToString()),
                        ImageURL = ImageSizes.CreateImageUrl(listing.HostURL, ImageSizes._0X0, listing.OriginalImgPath),
                        PhotoCount = listing.PhotoCount,
                        ClassifiedExpiryDate = listing.ClassifiedExpiryDate.ToString("dd-MMM-yyyy"),
                        Status = listing.Status,
                        TotalInq = listing.TotalInq + _myListings.GetC2BLeadsCount(listing.Id) + _myListings.GetCarTradeLeadsCount(listing.Id),
                        IsImagePendingToApprove = _myListings.IsImagesPendingToApprove(listing.Id),
                        PackageType = listing.PackageType,
                    });
                }
                viewModel.Source = DetectPlatform();
            }
            return viewModel;
        }

        private PartialViewResult ReturnBadRequest()
        {
            return ErrorHtmlResponse(HttpStatusCode.BadRequest, new ModalPopUpViewModel
            {
                Heading = "Data Invalid!",
                Description = "The data entered is invalid. Please refresh the page and try again."
            });
        }

        private PartialViewResult ServerError()
        {
            return ErrorHtmlResponse(HttpStatusCode.InternalServerError, new ModalPopUpViewModel
            {
                Heading = "Error!",
                Description = "Something went wrong. Please refresh the page and try again."
            });
        }
        private PartialViewResult ErrorHtmlResponse(HttpStatusCode httpStatusCode, ModalPopUpViewModel modalPopUp)
        {
            Response.TrySkipIisCustomErrors = true; //Suppress IIS custom message for returning JSON response on error status code
            Response.StatusCode = (int)httpStatusCode;
            return PartialView("~/Views/m/Used/SellCarPartial/_ModalPopUp.cshtml", modalPopUp);
        }
        private string GetS3FileKey(int inquiryId)
        {
            string key = string.Empty;
            List<Receipt> receipt = _receiptRepository.GetForInquiry(inquiryId);
            if (receipt != null && receipt.Any())
            {
                key = receipt.OrderByDescending(o => o.PgTransactionId).FirstOrDefault().Path;
            }
            return key;
        }

        private Platform DetectPlatform()
        {
            Platform platform;
            int sourceQueryParam = 43;//defaulting to mobile
            if (Request.QueryString["platform"] != null)
            {

                int.TryParse(Request.QueryString["platform"], out sourceQueryParam);
            }
            if (BrowserUtils.IsAndroidWebView() || sourceQueryParam == (int)Platform.CarwaleAndroid)
            {
                platform = Platform.CarwaleAndroid;
            }
            else if (BrowserUtils.IsIosWebView() || sourceQueryParam == (int)Platform.CarwaleiOS)
            {
                platform = Platform.CarwaleiOS;
            }
            else
            {
                platform = Platform.CarwaleMobile;
            }
            return platform;
        }

        private bool CheckInput(string mobileNumber, string authToken)
        {
            if (string.IsNullOrWhiteSpace(mobileNumber) || !_myListings.IsValidToken(authToken, mobileNumber, IpTracker.CurrentUserIp, CurrentUser.CWC))
            {
                Logger.LogInfo("authToken is empty/invalid");
                return false;
            }
            return true;
        }
    }
}