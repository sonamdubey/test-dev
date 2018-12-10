using Carwale.DTOs.CarData;
using Carwale.DTOs.NewCars;
using Carwale.DTOs.PriceQuote;
using Carwale.Entity.Geolocation;
using Carwale.Interfaces.Geolocation;
using Carwale.Interfaces.NewCars;
using Carwale.Interfaces.PriceQuote;
using Carwale.UI.Common;
using Carwale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using System.Web.SessionState;
using Carwale.Entity.Enum;
using Carwale.Interfaces.CarData;
using Carwale.Entity.CarData;
using AutoMapper;
using Carwale.Entity.AdapterModels;
using Carwale.Entity;
using Carwale.Interfaces.Campaigns;
using Carwale.Notifications.Logs;
using System.Linq;
using Carwale.UI.ClientBL;
using Carwale.DTOs.ViewModels;
using Carwale.Entity.PriceQuote;
using Carwale.Entity.Campaigns;
using Carwale.BL.Experiments;
using Carwale.Interfaces.Prices;
using Carwale.DTOs.Campaigns;
using Carwale.DTOs.Geolocation;
using Carwale.Entity.Common;

namespace Carwale.UI.Controllers.NewCars
{
    [SessionState(SessionStateBehavior.Disabled)]
    public class PriceInCityController : Controller
    {
        private readonly IGeoCitiesCacheRepository _cityRepo;
        private readonly static string _domainName = ConfigurationManager.AppSettings["adSlotDomain"] ?? string.Empty;
        private readonly ICarModels _carModelsBl;
        private readonly IPrices _prices;
        private readonly ITemplate _campaignTemplate;
        private readonly IServiceAdapterV2 _priceAdaptor;
        private readonly ICarPriceQuoteAdapter _carPrices;
        private readonly IEmiCalculatorAdapter _emiCalculatorAdapter;
        public PriceInCityController(Func<string, IServiceAdapterV2> factory, IGeoCitiesCacheRepository cityRepo,
                                     ICarModels carModelsBl, IPrices prices, ITemplate campaignTemplate, ICarPriceQuoteAdapter carPrices, IEmiCalculatorAdapter emiCalculatorAdapter)
        {
            try
            {
                _priceAdaptor = factory("PriceInCity");
                _cityRepo = cityRepo;
                _carModelsBl = carModelsBl;
                _prices = prices;
                _campaignTemplate = campaignTemplate;
                _carPrices = carPrices;
                _emiCalculatorAdapter = emiCalculatorAdapter;
            }
            catch (Exception ex)
            {

                Logger.LogException(ex, "Dependency Injection block at PriceInCityController");
            }
        }

        public ActionResult Index()
        {
            Response.AddHeader("Vary", "User-Agent");
            ///These are accrossed all views and are scenario specific, hence hardcoded configuration
            ViewBag.IsCityPage = true;
            ViewBag.IsVersionPage = false;
            ///
            CarDataAdapterInputs modelInput = null;
            PriceInCityPageDTO modelDTO = null;
            try
            {
                string modelMaskingName = Request["model"] != null ? Request.QueryString["model"] : string.Empty;
                string makeMaskingName = Request["make"] != null ? Request.QueryString["make"] : string.Empty;
                string cityMaskingName = Request["city"] != null ? Request.QueryString["city"] : string.Empty;
                string queryModelid = Request["modelid"] != null ? Request.QueryString["modelid"] : string.Empty;

                ModelMaskingValidationEntity modelinfo = _carModelsBl.FetchModelIdFromMaskingName(modelMaskingName, queryModelid, makeMaskingName, DeviceDetectionManager.IsMobile(this.HttpContext));

                modelInput = GetCityDetails(cityMaskingName);
                modelInput.IsMobile = DeviceDetectionManager.IsMobile(this.HttpContext);

                if (!modelinfo.IsValid)
                {
                    return HttpNotFound();
                }
                else if (modelInput.CustLocation == null || modelInput.CustLocation.CityId <= 0 || string.IsNullOrEmpty(modelInput.CustLocation.CityMaskingName))
                {
                    return RedirectPermanent(modelinfo.IsRedirect ? modelinfo.RedirectUrl : ManageCarUrl.CreateModelUrl(makeMaskingName, modelMaskingName));
                }
                else if (modelinfo.IsRedirect)
                {
                    if (modelinfo.Status == CarStatus.None)
                    {
                        return RedirectPermanent(modelinfo.RedirectUrl);
                    }
                    else
                    {
                        return RedirectPermanent($"{modelinfo.RedirectUrl}price-in-{Format.FormatURL(modelInput.CustLocation.CityMaskingName)}/");
                    }
                }
                else if (cityMaskingName != modelInput.CustLocation.CityMaskingName)
                {
                    return RedirectPermanent(ManageCarUrl.CreatePriceInCityUrl(makeMaskingName, modelMaskingName, modelInput.CustLocation.CityMaskingName));
                }

                GetModelInput(modelInput, modelinfo.ModelId);

                //setting cookie to track if the request came from a banner
                SetBannerCookie(modelinfo.ModelId);
                modelDTO = _priceAdaptor.Get<PriceInCityPageDTO, CarDataAdapterInputs>(modelInput);
                if (modelDTO.NewCarDealersCount < 1)
                {
                    return RedirectPermanent(ManageCarUrl.CreateModelUrl(modelDTO.ModelDetails.MakeName, modelDTO.ModelDetails.MaskingName));
                }
                ViewBag.CityName = modelInput.CustLocation.CityName;
                ViewBag.CityMaskingName = modelInput.CustLocation.CityMaskingName;
                ViewBag.CityZone = (CookiesCustomers.MasterZoneId <= 0) ? (CookiesCustomers.MasterCityId <= 0 ? "No City" : CookiesCustomers.MasterCity) : CookiesCustomers.MasterZone;
                ViewBag.CityId = modelInput.CustLocation.CityId;
                ViewBag.Make = modelDTO.ModelDetails.MakeName;
                ViewBag.IsVersionPage = false;
                ViewBag.MaskingNumber = modelDTO.DealerAd != null ? modelDTO.DealerAd.Campaign.ContactNumber : string.Empty;
                ViewBag.CarModelName = modelDTO.ModelDetails.ModelName;
                ViewBag.DomainName = _domainName;
                ViewBag.IsMobile = modelInput.IsMobile;


                ViewBag.ModelName = modelDTO.ModelDetails.MakeName + " " + modelDTO.ModelDetails.ModelName;
                ViewBag.MaskingName = modelDTO.ModelDetails.MaskingName;
                ViewBag.MakeName = modelDTO.ModelDetails.MakeName;
                ViewBag.PhotoCount = modelDTO.ModelDetails.PhotoCount;
                ViewBag.IsPriceShown = (modelDTO.CarPriceOverview != null && modelDTO.CarPriceOverview.PriceStatus == (int)PriceBucket.HaveUserCity);
                SetCampaignsEventTags(modelDTO, modelInput);
                SetPriceEventTags(modelDTO, modelInput);
                if (modelInput.IsMobile)
                {
                    modelDTO.MobileOverviewDetails.TrackingData = GetTrackingData(modelDTO.ModelDetails.MakeName, modelDTO.ModelDetails.ModelName, modelInput.CustLocation.CityName, true);
                    if (modelInput.IsAmp)
                    {
                        return View("~/Views/m/amp/priceincity.cshtml", modelDTO);
                    }
                    else
                    {
                        return View(ProductExperiments.IsPicPqMerger(modelDTO.MobileOverviewDetails.MakeId) && modelDTO.MobileOverviewDetails.New ? "~/Views/m/cardata/priceincityexperiment.cshtml" : "~/Views/m/cardata/priceincity.cshtml", modelDTO);
                    }
                }
                else
                {
                    if (ProductExperiments.IsPicPqMerger(modelDTO.DesktopOverviewDetails.MakeId))
                    {
                        return View("~/Views/newcar/priceincityexperiment.cshtml", modelDTO);
                    }
                    else
                    {
                        return View("~/Views/newcar/priceincity.cshtml", modelDTO);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return HttpNotFound();
        }

        private void SetCampaignsEventTags(PriceInCityPageDTO modelPageDTO_Mobile, CarDataAdapterInputs modelInput)
        {
            try
            {
                string cityNametext = CookiesCustomers.MasterCityId <= 0 ? "No City" : CookiesCustomers.MasterCity;
                ViewBag.CityZone = CookiesCustomers.MasterZoneId <= 0 ? cityNametext : CookiesCustomers.MasterZone;
                string getOffersLinkCityLabel = modelInput.IsCityPage ? modelInput.CustLocation.CityName : (CookiesCustomers.MasterCity == "Select City" ? "No City" : CookiesCustomers.MasterCity);
                string getOffersLinkZoneLabel = modelInput.IsCityPage ? "" : (CookiesCustomers.MasterCity != "Select City" && CookiesCustomers.MasterCity != ViewBag.CityZone ? ViewBag.CityZone : "");
                if (modelPageDTO_Mobile.DealerAd != null)
                {
                    ViewBag.GetOffersLinkLabel = string.Empty;
                    
                        if (!string.IsNullOrWhiteSpace(getOffersLinkZoneLabel)) //if zone exists
                        {
                            ViewBag.GetOffersLinkLabel = string.Format("{0},{1},{2},{3},{4}", modelPageDTO_Mobile.ModelDetails.ModelName, modelPageDTO_Mobile.DealerAd.Campaign.ContactName
                            , modelPageDTO_Mobile.DealerAd.Campaign.DealerId, getOffersLinkCityLabel, getOffersLinkZoneLabel);
                        }
                        else
                        {
                            ViewBag.GetOffersLinkLabel = string.Format("{0},{1},{2},{3}", modelPageDTO_Mobile.ModelDetails.ModelName, modelPageDTO_Mobile.DealerAd.Campaign.ContactName
                            , modelPageDTO_Mobile.DealerAd.Campaign.DealerId, getOffersLinkCityLabel);
                        }
                    
                    ViewBag.CityNameForDLSlug = CookiesCustomers.MasterCity;

                    ViewBag.DealerMaskingNumber = modelPageDTO_Mobile.CallSlugInfo.DealerMobile;
                    if (modelPageDTO_Mobile.CallSlugInfo.IsAdAvailable)
                    {
                        ViewBag.CallSlugTrackingAction = "Model-Page-Slug";
                        ViewBag.CallSlugTrackingLabel = String.Format("{0},{1},{2},{3},{4}",
                                                            modelPageDTO_Mobile.CallSlugInfo.CarName.Replace('_', ','),
                                                            ViewBag.CityZone,
                                                            modelPageDTO_Mobile.CallSlugInfo.DealerName,
                                                            modelPageDTO_Mobile.CallSlugInfo.DealerAd.Campaign.DealerId,
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

        private void SetPriceEventTags(PriceInCityPageDTO modelPageDTO_Mobile, CarDataAdapterInputs modelInput)
        {
            try
            {
                ViewBag.PricingEventAction = "";
                ViewBag.PricingEventLabel = "";
                ViewBag.IsPriceShown = false;
                if ((CookiesCustomers.IsEligibleForORP || modelInput.IsMobile) && modelPageDTO_Mobile.CarPriceOverview != null)
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
                    ViewBag.PricingEventLabel = string.Format("{0}_{1}_{2}", (priceCityId < 1 ? "NA" : priceCityName),
                        modelPageDTO_Mobile.ModelDetails.MakeName, modelPageDTO_Mobile.ModelDetails.ModelName);
                    ViewBag.IsPriceShown = (modelPageDTO_Mobile.CarPriceOverview != null && modelPageDTO_Mobile.CarPriceOverview.PriceStatus == (int)PriceBucket.HaveUserCity);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        private void GetModelInput(CarDataAdapterInputs modelInput, int modelId)
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
                modelInput.ModelDetails = new CarEntity() { ModelId = modelId, VersionId = versionId };
                modelInput.CwcCookie = CurrentUser.CWC;
                modelInput.MobileCookies = CustomerCookie.Mobile;
                modelInput.CompareVersionsCookie = CustomerCookie.CompareVersionsCookie;
                modelInput.UserModelHistory = CustomerCookie.UserModelHistory.Replace("~", ",");
                modelInput.IsAmp = Request["amp"] != null;
                modelInput.IsMobile = DeviceDetectionManager.IsMobile(this.HttpContext) || modelInput.IsAmp;
                modelInput.AbTest = CookiesCustomers.AbTest;
                if (modelInput.IsMobile)
                {
                    modelInput.CampaignInput = new CampaignInputv2
                    {
                        ModelId = modelId,
                        PlatformId = (short)Platform.CarwaleMobile,
                        ApplicationId = (int)Application.CarWale,
                        CityId = modelInput.CustLocation.CityId,
                        PageId = (int)CwPages.PicMsite
                    };
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        /// <summary>
        /// Returns the CityId 
        /// </summary>
        /// <returns></returns>
        private CarDataAdapterInputs GetCityDetails(string cityMaskingName)
        {
            CarDataAdapterInputs carDataAdapterInputs = new CarDataAdapterInputs();
            Entity.Geolocation.City cityDetails = null;
            try
            {
                if (!string.IsNullOrWhiteSpace(cityMaskingName))
                {
                    cityDetails = _cityRepo.GetCityDetailsByMaskingName(cityMaskingName);

                    if (cityDetails != null)
                    {
                        if (CookiesCustomers.MasterCityId <= 0 && !string.IsNullOrWhiteSpace(cityDetails.CityName))
                        {
                            CookiesCustomers.MasterCity = cityDetails.CityName;
                            CookiesCustomers.MasterCityId = cityDetails.CityId;

                        }
                        carDataAdapterInputs.IsCityPage = true;
                        carDataAdapterInputs.CustLocation = new Location
                        {
                            CityId = cityDetails.CityId,
                            CityName = cityDetails.CityName,
                            CityMaskingName = cityDetails.CityMaskingName,
                            StateMaskingName = cityDetails.StateMaskingName,
                            ZoneId = (CookiesCustomers.MasterCityId == cityDetails.CityId) ? CookiesCustomers.MasterZoneId : -1,
                            AreaId = (CookiesCustomers.MasterCityId == cityDetails.CityId) ? CookiesCustomers.MasterAreaId : -1
                        };
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return carDataAdapterInputs;

        }

        private void SetBannerCookie(int ModelId)
        {
            try
            {
                string targetModel = Request.QueryString["tm"];
                string targetVersion = Request.QueryString["tv"];

                if (!string.IsNullOrWhiteSpace(targetModel) || !string.IsNullOrWhiteSpace(targetVersion))
                {
                    string featuredVersion = (!String.IsNullOrWhiteSpace(Request.QueryString["fv"])) ? Request.QueryString["fv"] : string.Empty;
                    string bannerCookieName = "_sb" + ModelId;
                    uint bannerCookieExpiry = 7; // 7 days 
                    string bannerCookieVal = string.Format("tm={0}|tv={1}|fv={2}", targetModel ?? string.Empty,
                        targetVersion ?? string.Empty, featuredVersion); // pipe delimited string of target model, target version and feature version
                    CookiesCustomers.SetCookie(bannerCookieName, bannerCookieExpiry, bannerCookieVal);
                }
            }
            catch (NullReferenceException ex)
            {
                Logger.LogException(ex);
            }
        }

        [System.Web.Mvc.Route("pricebreakup/")]
        public ActionResult GetPriceBreakUp(CarEntity carData, bool isCampaignAvailble, int cityId, string cityName,  string campaignLeadCTA, bool showCampaignLink,bool IsPicSnippetExperiment = false, string campaignDealerId = "")
        {
            try
            {
                PriceBreakUpModel priceBreakUpModel = new PriceBreakUpModel();
                CarPriceQuote carPriceQuote = _prices.GetModelCompulsoryPrices(carData.ModelId, cityId, true, true);
                VersionPriceQuote versionPriceQuote = carPriceQuote != null ?
                                            carPriceQuote.VersionPricesList
                                            .Where(x => x.VersionId.Equals(carData.VersionId))
                                            .OrderBy(y => y.IsMetallic)
                                            .FirstOrDefault(z => z.PricesList.Count > 0 && z.PricesList[0].PQItemId > 0) : null;

                var pricesList = versionPriceQuote != null && versionPriceQuote.PricesList != null ? versionPriceQuote.PricesList : null;

                priceBreakUpModel.IsCampaignAvailable = isCampaignAvailble;
                priceBreakUpModel.MakeName = carData.MakeName;
                priceBreakUpModel.ModelName = carData.ModelName;
                priceBreakUpModel.ModelId = carData.ModelId;
                priceBreakUpModel.VersionId = carData.VersionId;
                priceBreakUpModel.VersionName = carData.VersionName;
                priceBreakUpModel.CityName = cityName;
                priceBreakUpModel.CityId = cityId;
                priceBreakUpModel.ShowCampaignLink = showCampaignLink;
                priceBreakUpModel.IsPicSnippetExperiment = IsPicSnippetExperiment;
                priceBreakUpModel.CampaignDealerId = campaignDealerId;
                priceBreakUpModel.CampaignLeadCTA = campaignLeadCTA;
                CampaignInputv2 campaignInput = new CampaignInputv2
                {
                    ModelId = carData.ModelId,
                    MakeId = carData.MakeId,
                    PlatformId = (short)Platform.CarwaleDesktop,
                    ApplicationId = (int)Application.CarWale,
                    CityId = cityId,
                    PageId = (int)CwPages.PicDesktop
                };
                priceBreakUpModel.CampaignTemplates = _campaignTemplate.GetTemplatesByPage(campaignInput, priceBreakUpModel);
                

                if (pricesList != null && pricesList.Count > 0)
                {
                    priceBreakUpModel.PricesList = pricesList;
                    priceBreakUpModel.OnRoadPrice = (int)priceBreakUpModel.PricesList.Sum(value => value.PQItemValue);
                    priceBreakUpModel.Emi = Carwale.BL.Calculation.Calculation.CalculateEmi(priceBreakUpModel.OnRoadPrice);
                }
                else
                {
                    var versionPrice = _carPrices.GetVersionsPriceForSameModel(carData.ModelId, new List<int>() { carData.VersionId }, cityId, true);
                    PriceOverview priceOverview;
                    versionPrice.TryGetValue(carData.VersionId, out priceOverview);
                    priceBreakUpModel.PriceOverview = Mapper.Map<PriceOverview, PriceOverviewDTOV2>(priceOverview);
                    priceBreakUpModel.Emi = priceBreakUpModel.PriceOverview != null &&
                        priceBreakUpModel.PriceOverview.PriceForSorting > 0 ? Carwale.BL.Calculation.Calculation.CalculateEmi(priceBreakUpModel.PriceOverview.PriceForSorting) : string.Empty;
                }

                priceBreakUpModel.EmiCalculatorModelLink = new EmiCalculatorModelLink();
                if(priceBreakUpModel.OnRoadPrice > 0)
                {
                    var carOverviewDto = Mapper.Map<PriceBreakUpModel, CarOverviewDTOV2>(priceBreakUpModel);
                    carOverviewDto.ModelId = carData.ModelId;
                    carOverviewDto.VersionName = versionPriceQuote != null? versionPriceQuote.VersionName : "";
                    carOverviewDto.MakeId = carData.MakeId;
                    carOverviewDto.EmiCalculatorModelData = _emiCalculatorAdapter.GetEmiData(carOverviewDto,  null, null, priceBreakUpModel.OnRoadPrice, priceBreakUpModel.CityId);

                    priceBreakUpModel.EmiCalculatorModelLink.EmiCalculatorModelData = carOverviewDto.EmiCalculatorModelData;
                    if(priceBreakUpModel.EmiCalculatorModelLink.EmiCalculatorModelData != null)
                    {
                        priceBreakUpModel.EmiCalculatorModelLink.EmiCalculatorModelData.PageName = CwPages.PicDesktop.ToString();
                    }

                    priceBreakUpModel.EmiCalculatorModelLink.DealerData = SetEmiCalculatorDealerAdData(campaignDealerId, cityId,cityName, carData.ModelId, priceBreakUpModel.CampaignTemplates);
                    
                    priceBreakUpModel.CarOverviewDto = Mapper.Map<CarOverviewDTOV2, CarOverviewDTO>(carOverviewDto);
                    if(priceBreakUpModel.CarOverviewDto != null)
                    {
                        priceBreakUpModel.EmiCalculatorModelLink.DealerData.AdAvailable = isCampaignAvailble;
                        priceBreakUpModel.CarOverviewDto.AdAvailable = isCampaignAvailble;
                    }
                }
                

                return PartialView(ProductExperiments.IsPicPqMerger(carData.MakeId) ? "~/Views/Shared/NewCars/_PriceBreakUpExperiment.cshtml" : "~/Views/Shared/NewCars/_PriceBreakUp.cshtml", priceBreakUpModel);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return new EmptyResult();
        }

        private TrackingDataDTO GetTrackingData(string makeName, string modelName, string cityName, bool isMobile)
        {
            TrackingDataDTO trackingData = new TrackingDataDTO();
            try
            {
                if (isMobile)
                {
                    trackingData.ClickCategory = "BBLinkClick_mobile";
                    trackingData.ImpCategory = "BBLinkImpressions_mobile";
                    trackingData.LandingUrl = "https://www.bankbazaar.com/car-loan.html?WT.mc_id=bb01|CL|mobile_PIC&utm_source=bb01&utm_medium=display&utm_campaign=bb01&variant=slide&variantOptions=mobileRequired";
                }
                else
                {
                    trackingData.ClickCategory = "BBLinkClick_desktop";
                    trackingData.ImpCategory = "BBLinkImpressions_desktop";
                    trackingData.LandingUrl = "https://www.bankbazaar.com/car-loan.html?WT.mc_id=bb01|CL|desk_PIC&utm_source=bb01&utm_medium=display&utm_campaign=bb01&variant=slide&variantOptions=mobileRequired";
                }
                trackingData.Text = "Get Car Loan Offers";
                trackingData.ClickAction = trackingData.ImpAction = "finance_pic_page";
                trackingData.ClickLabel = trackingData.ImpLabel = string.Format("{0} {1} {2}", makeName, modelName, cityName);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return trackingData;
        }
        private EmiCalculatorDealerAdDto SetEmiCalculatorDealerAdData(string campaignDealerId, int cityId, string cityName, int ModelId, Dictionary<int, IdName> CampaignTemplates)
        {
            EmiCalculatorDealerAdDto emiCalculatorDealerAdDto = new EmiCalculatorDealerAdDto();
            emiCalculatorDealerAdDto.CampaignDealerId = campaignDealerId;
            emiCalculatorDealerAdDto.ModelId = ModelId;
            CityAreaDTO userLocation = new CityAreaDTO();
            userLocation.CityId = cityId;
            userLocation.CityName = cityName;
            emiCalculatorDealerAdDto.UserLocation = userLocation;
            emiCalculatorDealerAdDto.CampaignTemplates = CampaignTemplates;
            return emiCalculatorDealerAdDto;
        }

    }
}