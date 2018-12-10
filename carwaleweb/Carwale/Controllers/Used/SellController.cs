using Carwale.Entity.CarData;
using Carwale.Interfaces;
using Carwale.UI.ViewModels.Used.SellCar;
using Carwale.Entity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Carwale.Interfaces.Geolocation;
using System.Configuration;
using Carwale.Entity.Classified.SellCarUsed;
using Carwale.Interfaces.Classified.SellCar;
using System.Net;
using Carwale.Utility;
using Carwale.UI.Common;
using Carwale.Entity;
using Carwale.BL.Customers;
using Carwale.Interfaces.CustomerVerification;
using FluentValidation.Results;
using Carwale.Entity.Stock;
using Carwale.Interfaces.Stock;
using Carwale.Notifications.Logs;
using Carwale.Entity.Enum;
using Carwale.UI.Filters;
using Carwale.UI.ClientBL;
using Carwale.UI.Filters.ActionFilters;
using Carwale.Interfaces.Classified.MyListings;

namespace Carwale.UI.Controllers.m.Used
{
    public class SellController : Controller
    {
        private const string _usedMakeType = "used";
        private const string _tempInquiryIdCookieName = "TempInquiry";
        private const string _inquiryIdCookieName = "SellInquiry";
        private const string _abTestCookieName = "_abtest";
        private readonly ICarMakesCacheRepository _carMakesCacheRepo;
        private readonly IGeoCitiesCacheRepository _geoCitiesCacheRepo;
        private readonly IStockConditionCacheRepository _stockConditionCacheRepository;
        private readonly ISellCarBL _sellCarBL;
        private readonly IMyListings _myListings;

        public SellController(ICarMakesCacheRepository carMakesCacheRepo, IGeoCitiesCacheRepository geoCitiesCacheRepo, ISellCarBL sellCarBL, IStockConditionCacheRepository stockConditionCacheRepository, IMyListings myListings)
        {
            _carMakesCacheRepo = carMakesCacheRepo;
            _geoCitiesCacheRepo = geoCitiesCacheRepo;
            _sellCarBL = sellCarBL;
            _stockConditionCacheRepository = stockConditionCacheRepository;
            _myListings = myListings;
        }

        [HttpGet, Route("used/sell/"), ResponsiveViewHeaders]
        public ActionResult SellCar()
        {
            try
            {
                SellCarViewModel sellCarViewModel = new SellCarViewModel()
                {
                    PopularCities = _geoCitiesCacheRepo.GetCities(Modules.SellCar, true),
                    Source = BrowserUtils.IsAndroidWebView() ? Platform.CarwaleAndroid : BrowserUtils.IsIosWebView() ? Platform.CarwaleiOS : Platform.CarwaleMobile,
                    MetaData = _sellCarBL.GetPageMetaTags()
                };

                return (DeviceDetectionManager.IsMobile(this.HttpContext)) 
                    ? View("~/Views/m/Used/SellCar.cshtml", sellCarViewModel)
                    : View("~/Views/Used/SellCar.cshtml", sellCarViewModel);
                
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return null;
            }
        }

        [HttpGet, Route("used/sell/cardetails/"), ResponsiveViewHeaders]
        public PartialViewResult CarDetails()
        {
            try
            {
                int tempInquiryId = 0;
                bool isMobile = DeviceDetectionManager.IsMobile(this.HttpContext);
                if (!Int32.TryParse(CookieManager.GetEncryptedCookie(_tempInquiryIdCookieName), out tempInquiryId))
                {
                    return ErrorHtmlResponse(HttpStatusCode.BadRequest, new ModalPopUpViewModel
                    {
                        Heading = "Data Invalid!",
                        Description = "The data entered is invalid. Please try again."
                    });
                }
                if (tempInquiryId <= 0)
                {
                    return ErrorHtmlResponse(HttpStatusCode.BadRequest, new ModalPopUpViewModel
                    {
                        Heading = "Data Invalid!",
                        Description = "The data entered is invalid. Please try again."
                    });
                }
                CarDetails carDetails = new CarDetails
                {
                    MakeYears = _sellCarBL.GetMakeYears(),
                    Makes = isMobile? _carMakesCacheRepo.GetCarMakesByType(_usedMakeType) : _carMakesCacheRepo.GetCarMakesByType(_usedMakeType, Modules.SellCar, false),
                    PopularMakes = _carMakesCacheRepo.GetCarMakesByType(_usedMakeType, Modules.SellCar, true),
                    BuyingIndexApiUrl = ConfigurationManager.AppSettings["carTradeBuyingIndexApi"],
                    InsuranceYears = _sellCarBL.GetInsuranceYears()
                };
                return (isMobile) 
                ? PartialView("~/Views/m/Used/SellCarPartial/_CarDetails.cshtml", carDetails)
                : PartialView("~/Views/Used/SellCarPartial/_CarDetails.cshtml", carDetails);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorHtmlResponse(HttpStatusCode.InternalServerError, new ModalPopUpViewModel
                {
                    Heading = "Error!",
                    Description = "Something went wrong. Please try again."
                });
            }
        }

        [HttpGet, Route("used/sell/carimages/"), ResponsiveViewHeaders]
        public PartialViewResult GetSellCarImages()
        {
            try
            {
                int inquiryId = 0;
                if (!Int32.TryParse(CookieManager.GetEncryptedCookie(_inquiryIdCookieName), out inquiryId))
                {
                    return ErrorHtmlResponse(HttpStatusCode.BadRequest, new ModalPopUpViewModel
                    {
                        Heading = "Data Invalid!",
                        Description = "The data entered is invalid. Please try again."
                    });
                }
                if (inquiryId <= 0)
                {
                    return ErrorHtmlResponse(HttpStatusCode.BadRequest, new ModalPopUpViewModel
                    {
                        Heading = "Data Invalid!",
                        Description = "The data entered is invalid. Please try again."
                    });
                }

                int abCookie = 0;
                Int32.TryParse(CookieManager.GetCookie(_abTestCookieName), out abCookie);
                CarImagesViewModel carImagesViewModel = new CarImagesViewModel
                {
                    IsStandardImages = (abCookie % 2 != 0) //do not show standard images to 50% users
                };

                return (DeviceDetectionManager.IsMobile(this.HttpContext))
                    ? PartialView("~/Views/m/Used/SellCarPartial/_CarImages.cshtml", carImagesViewModel)
                    : PartialView("~/Views/Used/SellCarPartial/_CarImages.cshtml", carImagesViewModel);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorHtmlResponse(HttpStatusCode.InternalServerError, new ModalPopUpViewModel
                {
                    Heading = "Error!",
                    Description = "Something went wrong. Please try again."
                });
            }
        }
        [HttpGet, Route("used/sell/carcondition/"), ResponsiveViewHeaders]
        public PartialViewResult GetCarCondition(int cityId)
        {
            try
            {
                int inquiryId = 0;
                if (!Int32.TryParse(CookieManager.GetEncryptedCookie(_inquiryIdCookieName), out inquiryId))
                {
                    return ErrorHtmlResponse(HttpStatusCode.BadRequest, new ModalPopUpViewModel
                    {
                        Heading = "Data Invalid!",
                        Description = "The data entered is invalid. Please try again."
                    });
                }
                if (inquiryId <= 0)
                {
                    return ErrorHtmlResponse(HttpStatusCode.BadRequest, new ModalPopUpViewModel
                    {
                        Heading = "Error!",
                        Description = "Something went wrong. Please reload the page.",
                        IsYesButtonActive = true,
                        YesButtonText = "Reload",
                        YesButtonLink = "/used/sell/"
                    });
                }

                if (_sellCarBL.IsC2bCity(cityId))
                {
                    List<StockConditionItems> listStockConditionParts = _stockConditionCacheRepository.GetCarConditionParts();
                    StockConditionViewModel stockConditionViewModel = new StockConditionViewModel
                    {
                        ListParts = listStockConditionParts.Where(stockConditionPart => stockConditionPart.CategoryId == (int)StockConditionCategory.Parts).ToList(),
                        ListMinorScratches = listStockConditionParts.Where(stockConditionPart => stockConditionPart.CategoryId == (int)StockConditionCategory.MinorScratches).ToList(),
                        ListDents = listStockConditionParts.Where(stockConditionPart => stockConditionPart.CategoryId == (int)StockConditionCategory.Dents).ToList(),
                        ListEngineIssues = listStockConditionParts.Where(stockConditionPart => stockConditionPart.CategoryId == (int)StockConditionCategory.Engine).ToList(),
                        ListElectricalIssues = listStockConditionParts.Where(stockConditionPart => stockConditionPart.CategoryId == (int)StockConditionCategory.ElectricalSystem).ToList()
                    };
                    return (DeviceDetectionManager.IsMobile(HttpContext)) 
                        ? PartialView("~/Views/m/Used/SellCarPartial/_CarCondition.cshtml", stockConditionViewModel)
                        : PartialView("~/Views/Used/SellCarPartial/_CarCondition.cshtml", stockConditionViewModel);
                }
                return ErrorHtmlResponse(HttpStatusCode.NotFound, new ModalPopUpViewModel
                {
                    Heading = "Not found"
                });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorHtmlResponse(HttpStatusCode.InternalServerError, new ModalPopUpViewModel
                {
                    Heading = "Error!",
                    Description = "Something went wrong. Please try again."
                });
            }
        }

        [HttpGet, Route("used/sell/fueleconomy/")]
        public PartialViewResult ShowFuelEcomony()
        {
            int inquiryId = 0;
            if (!Int32.TryParse(CookieManager.GetEncryptedCookie(_inquiryIdCookieName), out inquiryId))
            {
                return ErrorHtmlResponse(HttpStatusCode.BadRequest, new ModalPopUpViewModel
                {
                    Heading = "Data Invalid!",
                    Description = "The data entered is invalid. Please try again."
                });
            }
            if (inquiryId <= 0)
            {
                return ErrorHtmlResponse(HttpStatusCode.BadRequest, new ModalPopUpViewModel
                {
                    Heading = "Data Invalid!",
                    Description = "The data entered is invalid. Please try again."
                });
            }
            return PartialView("~/Views/Used/SellCarPartial/_FuelEconomy.cshtml");
        }

        [HttpGet, Route("used/sell/voucher/"), ResponsiveViewHeaders]
        public PartialViewResult ShowVoucher(int cityId)
        {
            try
            {
                int inquiryId = 0;
                if (!Int32.TryParse(CookieManager.GetEncryptedCookie(_inquiryIdCookieName), out inquiryId))
                {
                    return ErrorHtmlResponse(HttpStatusCode.BadRequest, new ModalPopUpViewModel
                    {
                        Heading = "Data Invalid!",
                        Description = "The data entered is invalid. Please try again."
                    });
                }

                if (inquiryId <= 0)
                {
                    return ErrorHtmlResponse(HttpStatusCode.BadRequest, new ModalPopUpViewModel
                    {
                        Heading = "Error!",
                        Description = "Something went wrong. Please reload the page.",
                        IsYesButtonActive = true,
                        YesButtonText = "Reload",
                        YesButtonLink = "/used/sell/"
                    });
                }
                VoucherViewModel voucherViewModel = new VoucherViewModel()
                {
                    IsC2bCity = false,
                    InquiryId = inquiryId
                };

                if (DeviceDetectionManager.IsMobile(this.HttpContext))
                {
                    return PartialView("~/Views/m/Used/SellCarPartial/_Voucher.cshtml", voucherViewModel);
                }
                else
                {
                    return PartialView("~/Views/Used/SellCarPartial/_Voucher.cshtml", voucherViewModel);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorHtmlResponse(HttpStatusCode.InternalServerError, new ModalPopUpViewModel
                {
                    Heading = "Error!",
                    Description = "Something went wrong. Please try again."
                });
            }
        }

        [HttpGet, Route("used/sell/congrats/"), ResponsiveViewHeaders]
        public PartialViewResult Congrats()
        {
            try
            {
                int inquiryId = 0;
                if (!Int32.TryParse(CookieManager.GetEncryptedCookie(_inquiryIdCookieName), out inquiryId))
                {
                    return ErrorHtmlResponse(HttpStatusCode.BadRequest, new ModalPopUpViewModel
                    {
                        Heading = "Data Invalid!",
                        Description = "The data entered is invalid. Please try again."
                    });
                }

                if (inquiryId <= 0)
                {
                    return ErrorHtmlResponse(HttpStatusCode.BadRequest, new ModalPopUpViewModel
                    {
                        Heading = "Error!",
                        Description = "Something went wrong. Please reload the page.",
                        IsYesButtonActive = true,
                        YesButtonText = "Reload",
                        YesButtonLink = "/used/sell/"
                    });
                }
                CongratsViewModel congratsViewModel = new CongratsViewModel
                {
                    InquiryId = inquiryId
                };
                if (DeviceDetectionManager.IsMobile(this.HttpContext))
                {
                    return PartialView("~/Views/m/Used/SellCarPartial/_Congrats.cshtml", congratsViewModel);
                }
                else
                {
                    return PartialView("~/Views/Used/SellCarPartial/_Congrats.cshtml", congratsViewModel);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorHtmlResponse(HttpStatusCode.InternalServerError, new ModalPopUpViewModel
                {
                    Heading = "Error!",
                    Description = "Something went wrong. Please try again."
                });
            }
        }

        private PartialViewResult ErrorHtmlResponse(HttpStatusCode httpStatusCode, ModalPopUpViewModel modalPopUp)
        {
            Response.TrySkipIisCustomErrors = true; //Suppress IIS custom message for returning JSON response on error status code
            Response.StatusCode = (int)httpStatusCode;
            return PartialView("~/Views/m/Used/SellCarPartial/_ModalPopUp.cshtml", modalPopUp);
        }

        [HttpGet, Route("used/sell/terms/")]
        public ActionResult TermsAndCondition()
        {
            try
            {
                return View("~/Views/Used/TermsAndCondition.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Redirect("/used/sell/");
            }
        }
    }
}