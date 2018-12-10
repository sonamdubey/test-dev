using Carwale.BL.Experiments;
using Carwale.DTOs.Geolocation;
using Carwale.DTOs.NewCars;
using Carwale.DTOs.PriceQuote;
using Carwale.DTOs.ViewModels.New;
using Carwale.Entity;
using Carwale.Entity.AdapterModels;
using Carwale.Entity.CarData;
using Carwale.Entity.Enum;
using Carwale.Entity.Geolocation;
using Carwale.Entity.PriceQuote;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.Geolocation;
using Carwale.Interfaces.NewCars;
using Carwale.Interfaces.PriceQuote;
using Carwale.Interfaces.Prices;
using Carwale.Notifications.Logs;
using Carwale.UI.ClientBL;
using Carwale.UI.Common;
using Carwale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.SessionState;

namespace Carwale.UI.Controllers.NewCars
{
    [SessionState(SessionStateBehavior.Disabled)]
    public class CarModelAdaptiveController : Controller
    {
        private readonly IServiceAdapterV2 _modelPageAdapterDesktop, _modelPageAdapterMobile;
        private readonly ICarModels _carModelsBl;
        private readonly IGeoCitiesCacheRepository _cityRepo;
        private readonly IPrices _prices;
        private readonly ICarVersions _carVersionsBl;
        private readonly IEmiCalculatorBl _emiCalculatorBl;
        private readonly static string _domainName = ConfigurationManager.AppSettings["adSlotDomain"] ?? string.Empty;
        private string _masterCityName = string.Empty;
        private string _masterZoneName = string.Empty;
        private int _masterCityId;
        private int _masterZoneId;
        private int _masterAreaId;
        private readonly static DateTime _parallaxCampaignEndDate = new DateTime(2018, 3, 18, 23, 59, 0);
        public CarModelAdaptiveController(Func<string, IServiceAdapterV2> adaptorFactory, ICarModels carModelsBl, IGeoCitiesCacheRepository cityRepo, IPrices prices, ICarVersions carVersionsBl, IEmiCalculatorBl emiCalculatorBl)
        {
            try
            {
                _modelPageAdapterDesktop = adaptorFactory("ModelPageDesktop");
                _modelPageAdapterMobile = adaptorFactory("ModelPageMobile");
                _carModelsBl = carModelsBl;
                _cityRepo = cityRepo;
                _prices = prices;
                _carVersionsBl = carVersionsBl;
                _emiCalculatorBl = emiCalculatorBl;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "Dependency Injection Block at CarModelAdaptiveController");
            }
        }

        public ActionResult Index()
        {
            Response.AddHeader("Vary", "User-Agent");
            try
            {
                ModelPageDTO_Desktop modelPageDesktop = null;
                ModelPageDTO_Mobile modelPageMobile = null;
                CarDataAdapterInputs modelInput = null;
                SetLocationVariables();
                string modelMaskingName = Request["model"] != null ? Request.QueryString["model"] : string.Empty;
                string makeMaskingName = Request["make"] != null ? Request.QueryString["make"] : string.Empty;
                string queryModelId = Request["modelid"] != null ? Request.QueryString["modelid"] : string.Empty;
                bool isMobile = DeviceDetectionManager.IsMobile(this.HttpContext);
                ModelMaskingValidationEntity modelInfo = _carModelsBl.FetchModelIdFromMaskingName(modelMaskingName, queryModelId, makeMaskingName, isMobile);
                if (modelInfo.IsRedirect)
                {
                    return RedirectPermanent(modelInfo.RedirectUrl);
                }
                else if (!modelInfo.IsValid)
                {
                    return HttpNotFound();
                }

                SetBannerCookie(modelInfo.ModelId);
                modelInput = GetModelInput(modelInfo.ModelId);
                modelInput.IsMobile = isMobile;
                if (modelInput.IsMobile)
                {
                    modelPageMobile = _modelPageAdapterMobile.Get<ModelPageDTO_Mobile, CarDataAdapterInputs>(modelInput);

                    if (modelPageMobile != null && modelPageMobile.ModelDetails != null)
                    {
                        SetCommonViewBags(modelPageMobile.ModelDetails, modelPageMobile.CityDetails);
                        SetViewBagsMobile(modelPageMobile);
                    }
                    if (Request["amp"] != null)
                    {
                        return View("~/views/m/amp/model.cshtml", modelPageMobile);
                    }
                    return View("~/views/m/cardata/model.cshtml", modelPageMobile);
                }
                else
                {
                    modelPageDesktop = _modelPageAdapterDesktop.Get<ModelPageDTO_Desktop, CarDataAdapterInputs>(modelInput);
                    if (modelPageDesktop != null && modelPageDesktop.ModelDetails != null)
                    {
                        SetCommonViewBags(modelPageDesktop.ModelDetails, modelPageDesktop.CityDetails);
                        SetViewBagsDesktop(modelPageDesktop);
                        SetFloatingCtaVm(modelPageDesktop, modelInput.CustLocation?.CityId ?? 0, modelInput.CustLocation?.CityName);
                    }

                    return View("~/Views/NewCar/Model.cshtml", modelPageDesktop);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return HttpNotFound();
            }

        }
        private void SetBannerCookie(int ModelId)
        {
            try
            {
                string targetmodel = Request.QueryString["tm"];
                string targetversion = Request.QueryString["tv"];

                if (!string.IsNullOrWhiteSpace(targetmodel) || !string.IsNullOrWhiteSpace(targetversion))
                {
                    string featuredversion = (!String.IsNullOrWhiteSpace(Request.QueryString["fv"])) ? Request.QueryString["fv"] : string.Empty;
                    string bannerCookieName = "_sb" + ModelId;
                    uint bannerCookieExpiry = 7; // 7 days 
                    string bannerCookieVal = string.Format("tm={0}|tv={1}|fv={2}", targetmodel ?? string.Empty, targetversion ?? string.Empty, featuredversion);
                    // pipe delimited string of target model, target version and feature version
                    CookiesCustomers.SetCookie(bannerCookieName, bannerCookieExpiry, bannerCookieVal);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
        private CarDataAdapterInputs GetModelInput(int modelId)
        {
            try
            {
                int versionId;
                if (!string.IsNullOrEmpty(Request.QueryString["vid"]))
                {
                    int.TryParse(Request.QueryString["vid"], out versionId);
                }
                else
                {
                    versionId = CookiesCustomers.GetVersionStateForModel(modelId);
                }
                string showOfferUpfront = Request.QueryString["showOfferUpfront"];
                CarDataAdapterInputs modelInput = new CarDataAdapterInputs
                {
                    ModelDetails = new CarEntity { ModelId = modelId, VersionId = versionId },
                    CwcCookie = CurrentUser.CWC,
                    MobileCookies = CustomerCookie.Mobile,
                    UserModelHistory = CustomerCookie.UserModelHistory.Replace("~", ","),
                    CompareVersionsCookie = CustomerCookie.CompareVersionsCookie,
                    AbTest = CustomerCookie.AbTest,
                    CustLocation = new Location { CityId = _masterCityId, CityName = _masterCityName, ZoneId = _masterZoneId, AreaId = _masterAreaId },
                    ShowOfferUpfront = !string.IsNullOrEmpty(showOfferUpfront) && showOfferUpfront == "true"
                };

                return modelInput;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return null;
            }
        }
        private void SetLocationVariables()
        {
            try
            {
                if (!string.IsNullOrEmpty(Request.QueryString["ampcityId"]))
                {
                    int.TryParse(Request.QueryString["ampcityId"], out _masterCityId);
                    CookiesCustomers.MasterCityId = _masterCityId;
                    _masterCityName = _cityRepo.GetCityNameById(Request.QueryString["ampcityId"]);
                    CookiesCustomers.MasterCity = _masterCityName;
                    if (CookiesCustomers.MasterCityId != _masterCityId)
                    {
                        _masterZoneName = string.Empty;
                        CookiesCustomers.MasterZone = _masterZoneName;
                        _masterZoneId = -1;
                        CookiesCustomers.MasterZoneId = _masterZoneId;
                        CookiesCustomers.MasterAreaId = -1;
                        CookiesCustomers.MasterArea = string.Empty;
                    }
                }
                else
                {
                    _masterCityId = CookiesCustomers.MasterCityId;
                    _masterCityName = CookiesCustomers.MasterCity;
                    _masterZoneId = CookiesCustomers.MasterZoneId;
                    _masterZoneName = CookiesCustomers.MasterZone;
                    _masterAreaId = CookiesCustomers.MasterAreaId;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
        private void SetCommonViewBags(CarModelDetails modelDetails, CitiesDTO cityDetails)
        {
            try
            {
                //these are independent of modeldetails object
                ViewBag.CityId = _masterCityId;
                ViewBag.CityName = _masterCityName;
                ViewBag.CityMaskingName = (cityDetails != null) ? cityDetails.CityMaskingName ?? string.Empty : string.Empty;
                ViewBag.ZoneId = _masterZoneId;
                string cityNametext = _masterCityId <= 0 ? "No City" : _masterCityName;
                ViewBag.CityZone = _masterZoneId <= 0 ? cityNametext : _masterZoneName;
                ViewBag.IsVersionPage = false;
                ViewBag.IsCityPage = false;
                ViewBag.DomainName = _domainName;
                ViewBag.PhotoCount = modelDetails.PhotoCount;
                //these are dependent on modeldetails param
                ViewBag.MakeName = modelDetails.MakeName;
                ViewBag.ModelName = modelDetails.ModelName;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
        private void SetFloatingCtaVm(ModelPageDTO_Desktop modelPageDTO, int cityId, string cityName)
        {
            try
            {
                var floatingCta = new ModelFloatingCtaViewModel();
                floatingCta.IsShowFloatingCta = modelPageDTO.ModelDetails.New;
                if (floatingCta.IsShowFloatingCta)
                {
                    floatingCta.IsAdAvailable = (modelPageDTO.SponsoredDealerAd != null && modelPageDTO.SponsoredDealerAd.DealerId > 0) || modelPageDTO.ShowCampaignLink;
                    floatingCta.CarName = string.Format("{0} {1}", modelPageDTO.ModelDetails?.MakeName,
                        modelPageDTO.ModelDetails?.ModelName);
                    floatingCta.ModelName = modelPageDTO.ModelDetails?.ModelName;
                    floatingCta.MakeName = modelPageDTO.ModelDetails?.MakeName;
                    floatingCta.VersionName = modelPageDTO.CarVersion?.Name;
                    floatingCta.CityName = cityName;
                    floatingCta.ModelImagePath = modelPageDTO.ModelDetails?.OriginalImage;
                    floatingCta.CityTrackingLabel = ViewBag.CityZone ?? string.Empty;
                    floatingCta.PageId = 31;
                    floatingCta.PredicationLabel = modelPageDTO.OverviewDetails?.PredictionData?.Label;
                    floatingCta.PredicationScore = modelPageDTO.OverviewDetails?.PredictionData?.Score ?? 0;
                    floatingCta.ShowCampaignLink = modelPageDTO.ShowCampaignLink;
                    floatingCta.CityId = cityId;
                    floatingCta.PriceText = string.Format("₹ {0}",
                        FormatPrice.GetFormattedPriceV2(modelPageDTO.CarPriceOverview.Price.ToString()));
                    floatingCta.PriceLabel = !string.IsNullOrWhiteSpace(modelPageDTO.CarPriceOverview?.PriceLabel)
                        ? (modelPageDTO.CarPriceOverview?.PriceLabel +
                        (modelPageDTO.CarPriceOverview.PriceStatus != (int)PriceBucket.HaveUserCity ? "" : ", " + modelPageDTO.CarPriceOverview.City.CityName)) : string.Empty;
                }
                modelPageDTO.FloatingCtaViewModel = floatingCta;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
        private void SetViewBagsDesktop(ModelPageDTO_Desktop dto)
        {
            try
            {
                ViewBag.MaskingNumber = dto.SponsoredDealerAd != null ? dto.SponsoredDealerAd.DealerMobile : string.Empty;
                ViewBag.MaskingName = dto.ModelDetails.MaskingName;
                ViewBag.IsPriceShown = (dto.CarPriceOverview != null && dto.CarPriceOverview.PriceStatus == (int)PriceBucket.HaveUserCity);
                ViewBag.CarName = dto.ModelDetails.MakeName + " " + dto.ModelDetails.ModelName;
                ViewBag.TrackingPageName = "ModelPage";
                ViewBag.Pagename = "modelpage";
                ViewBag.Pagename = "modelpage";
                string getOffersLinkCityLabel = _masterCityId <= 0 ? "No City" : _masterCityName;
                string getOffersLinkZoneLabel = (_masterCityId > 0
                         && _masterCityName != ViewBag.CityZone ? ViewBag.CityZone : "");
                ViewBag.GetOffersLinkLabel = string.Empty;
                if (dto.SponsoredDealerAd != null)
                {
                    ViewBag.GetOffersLinkLabel = string.Format("{0},{1},{2},{3}{4}", dto.ModelDetails.ModelName, dto.SponsoredDealerAd.DealerName
                    , dto.SponsoredDealerAd.ActualDealerId, getOffersLinkCityLabel, !string.IsNullOrEmpty(getOffersLinkZoneLabel) ? "," + getOffersLinkZoneLabel : string.Empty);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
        private void SetViewBagsMobile(ModelPageDTO_Mobile dto)
        {
            try
            {
                ViewBag.ModelSmallImage = dto.ModelDetails.ModelImageSmall;
                ViewBag.MaskingName = dto.ModelDetails.MaskingName;
                SetCampaignsEventTags(dto);
                SetPriceEventTags(dto);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
        private void SetCampaignsEventTags(ModelPageDTO_Mobile modelPageDTO_Mobile)
        {
            try
            {
                if (modelPageDTO_Mobile.DealerAd != null && modelPageDTO_Mobile.DealerAd.Campaign != null)
                {
                    string getOffersCityLabel = _masterCityId <= 0 ? "No City" : _masterCityName;
                    ViewBag.GetOffersLinkLabel = string.Format("{0},{1},{2},{3}", modelPageDTO_Mobile.ModelDetails.ModelName, modelPageDTO_Mobile.DealerAd.Campaign.ContactName, modelPageDTO_Mobile.DealerAd.Campaign.DealerId,
                        getOffersCityLabel);
                    ViewBag.CityNameForDLSlug = _masterCityName;

                    ViewBag.DealerMaskingNumber = modelPageDTO_Mobile.CallSlugInfo != null ? modelPageDTO_Mobile.CallSlugInfo.DealerMobile : string.Empty;
                    if (modelPageDTO_Mobile.CallSlugInfo != null && modelPageDTO_Mobile.CallSlugInfo.IsAdAvailable)
                    {
                        ViewBag.CallSlugTrackingAction = "Model-Page-Slug";
                        ViewBag.CallSlugTrackingLabel = String.Format("{0},{1},{2},{3},{4}",
                                                            modelPageDTO_Mobile.CallSlugInfo.CarName.Replace('_', ','),
                                                            ViewBag.CityZone,
                                                            modelPageDTO_Mobile.CallSlugInfo.DealerName,
                                                            modelPageDTO_Mobile.DealerAd.Campaign.DealerId,
                                                            modelPageDTO_Mobile.CallSlugInfo.DealerMobile);
                    }
                    ViewBag.TrackingPageName = "ModelPage";

                    ViewBag.GetOffersEventAction = ViewBag.TrackingPageName + "_FloatingGetOffers";
                    ViewBag.ShowPriceInMyCityEventAction = ViewBag.TrackingPageName + "_FloatingShowPriceInMyCity";
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
        private void SetPriceEventTags(ModelPageDTO_Mobile modelPageDTO_Mobile)
        {
            try
            {
                ViewBag.PricingEventAction = "";
                ViewBag.PricingEventLabel = "";
                ViewBag.IsPriceShown = false;
                if (CookiesCustomers.IsEligibleForORP && modelPageDTO_Mobile.CarPriceOverview != null)
                {
                    int priceStatus = modelPageDTO_Mobile.CarPriceOverview.PriceStatus;
                    int priceCityId = -1;
                    string priceCityName = "";

                    if (modelPageDTO_Mobile.CarPriceOverview.City != null)
                    {
                        priceCityId = modelPageDTO_Mobile.CarPriceOverview.City.CityId;
                        priceCityName = modelPageDTO_Mobile.CarPriceOverview.City.CityName;
                    }

                    if (priceStatus == (int)PriceBucket.NoUserCity)
                    {
                        ViewBag.PricingEventAction = ViewBag.TrackingPageName + "_CityNotSet";
                    }
                    else if (priceStatus == (int)PriceBucket.HaveUserCity)
                    {
                        ViewBag.PricingEventAction = ViewBag.TrackingPageName + "_ORPShown";
                    }
                    else
                    {
                        ViewBag.PricingEventAction = ViewBag.TrackingPageName + "_ORPNotAvailable";
                    }
                    ViewBag.PricingEventLabel = string.Format("{0}_{1}_{2}", (priceCityId < 1 ? "NA" : priceCityName), modelPageDTO_Mobile.ModelDetails.MakeName, modelPageDTO_Mobile.ModelDetails.ModelName);

                    ViewBag.IsPriceShown = (modelPageDTO_Mobile.CarPriceOverview != null && modelPageDTO_Mobile.CarPriceOverview.PriceStatus == (int)PriceBucket.HaveUserCity);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
        [System.Web.Mvc.Route("m/versionpopup/{modelId}")]
        public ActionResult GetVersionListPopup(int modelId, int makeId, int cityId, bool isCityPage, bool isVersionPage)
        {
            try
            {
                var versions = _carVersionsBl.MapCarVersionDtoWithCarVersionEntity(modelId, cityId);
                if (versions != null && versions.Count > 0)
                {
                    ViewBag.IsVersionPage = isVersionPage;
                    ViewBag.TrackingPageName = isVersionPage ? "VersionPage" : isCityPage ? "PriceInCityPage" : "ModelPage";
                    ViewBag.VersionPopupCityId = (versions.FirstOrDefault().CarPriceOverview.City != null) ? versions.FirstOrDefault().CarPriceOverview.City.CityId : -1;
                    ViewBag.VersionPopupCityName = (versions.FirstOrDefault().CarPriceOverview.City != null) ? versions.FirstOrDefault().CarPriceOverview.City.CityName : string.Empty;
                    List<NewCarVersionsDTOV2> versionsList = AutoMapper.Mapper.Map<List<NewCarVersionsDTO>, List<NewCarVersionsDTOV2>>(versions);

                    var versionPriceList = _prices.GetModelCompulsoryPrices(modelId, cityId, versions[0].New, true)
                                                            .VersionPricesList.OrderBy(x => x.IsMetallic)
                                                            .ToList();
                    versionsList.ForEach(x =>
                    {
                        StringBuilder versionPriceBreakUp = new StringBuilder();
                        VersionPriceQuote priceQuote = versionPriceList
                        .Find(version => version.VersionId.Equals(x.Id) && version.PricesList.Count > 0 && version.PricesList[0].PQItemId > 0);
                        
                        if (isCityPage)
                        {
                            if (priceQuote != null)
                            {
                                priceQuote
                                    .PricesList
                                    .ForEach(pqItem =>
                                                versionPriceBreakUp.AppendFormat("{0}:{1}|", pqItem.PQItemName, Format.FormatNumericCommaSep(pqItem.PQItemValue.ToString())));
                            }

                            versionPriceBreakUp.Append("orp:" + Format.FormatNumericCommaSep(x.CarPriceOverview.PriceForSorting.ToString()));
                            x.VersionPriceQuote = versionPriceBreakUp.ToString();
                        }

                        if (priceQuote != null && priceQuote.PricesList != null && priceQuote.PricesList.Count > 0)
                        {
                            int exShowroom = CustomParser.parseIntObject(priceQuote.PricesList.FirstOrDefault(p => p.PQItemId == (int)PricesCategoryItem.Exshowroom).PQItemValue);
                            x.EmiCalculatorModelData = GetEmiData(x.Id, exShowroom, CustomParser.parseIntObject(priceQuote.OnRoadPrice));
                            
                        }
                    });

                    return PartialView("~/Views/Shared/m/cardata/_versionspopup.cshtml", versionsList);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return new EmptyResult();
        }


        private EmiCalculatorModelData GetEmiData(int versionId, int exShowroom, int onRoadPrice)
        {
            try
            {
                if (exShowroom > 0)
                {
                    var EmiCalculatorModelData = new EmiCalculatorModelData();
                    EmiCalculatorModelData.DownPaymentMinValue = onRoadPrice - exShowroom;
                    EmiCalculatorModelData.DownPaymentMaxValue = onRoadPrice;
                    EmiCalculatorModelData.DownPaymentDefaultValue = _emiCalculatorBl.CalculateDownPaymentDefaultValue(EmiCalculatorModelData.DownPaymentMinValue, exShowroom);
                    EmiCalculatorModelData.UniqueKey = versionId.ToString();

                    return EmiCalculatorModelData;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return null;
        }

    }
}