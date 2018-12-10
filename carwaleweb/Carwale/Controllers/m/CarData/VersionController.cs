using Carwale.DTOs.CarData;
using Carwale.DTOs.NewCars;
using Carwale.Entity.Geolocation;
using Carwale.Interfaces.NewCars;
using Carwale.Interfaces.PriceQuote;
using Carwale.Notifications;
using Carwale.UI.Common;
using Microsoft.Practices.Unity;
using System;
using System.Configuration;
using System.Web.Mvc;
using Carwale.Interfaces.Dealers;
using AutoMapper;
using Carwale.Entity.Enum;
using Carwale.BL.PriceQuote;
using Carwale.Utility;
using Carwale.Interfaces.Geolocation;
using Carwale.Interfaces.Campaigns;
using Carwale.Interfaces.CMS;
using Carwale.Entity;
using System.Collections.Generic;
using Carwale.Interfaces.CarData;
using Carwale.Entity.CarData;
using Carwale.Entity.PriceQuote;
using System.Linq;
using System.Text.RegularExpressions;
using Carwale.DTOs.Campaigns;
using Carwale.BL.Experiments;
using Carwale.DTOs.PriceQuote;
using Carwale.Notifications.Logs;
using Carwale.Entity.AdapterModels;
using Carwale.Entity.Campaigns;

namespace Carwale.UI.Controllers.m.CarData
{
    public class VersionController : Controller
    {
        private readonly IUnityContainer _unityContainer;
        public VersionController(IUnityContainer container)
        {
            _unityContainer = container;
        }

        public ActionResult GetVersionDetails()
        {
            VersionPageDTO_Mobile versionDTO = null;
            try
            {
                string makeMaskingName = Request["make"] != null ? Request.QueryString["make"] : string.Empty;
                string modelMaskingName = Request["model"] != null ? Request.QueryString["model"] : string.Empty;
                string versionMaskingName = Request["version"] != null ? Request.QueryString["version"] : string.Empty;
                string urlVersionId = Request["car"] != null ? Request.QueryString["car"] : string.Empty;
                ICarVersions _carVersionBl = _unityContainer.Resolve<ICarVersions>();
                VersionMaskingNameValidation versionInfo = _carVersionBl.FetchVersionInfoFromMaskingName(modelMaskingName, versionMaskingName, string.Empty, urlVersionId, true, makeMaskingName);
                if (versionInfo.IsRedirect)
                {
                    return RedirectPermanent(versionInfo.RedirectUrl);
                }
                else if (!versionInfo.IsValid)
                {
                    return HttpNotFound();
                }
                Location custLocation = new Location
                {
                    CityId = CookiesCustomers.MasterCityId,
                    ZoneId = CookiesCustomers.MasterZoneId,
                    AreaId = CookiesCustomers.MasterAreaId,
                    ZoneName = CookiesCustomers.MasterZone,
                    CityName = CookiesCustomers.MasterCityId > 0 ? CookiesCustomers.MasterCity : "No City"
                };
                CarDataAdapterInputs versionInput = new CarDataAdapterInputs
                {
                    ModelDetails = new CarEntity { VersionId = versionInfo.VersionId },
                    CustLocation = custLocation,
                    CampaignInput = new CampaignInputv2
                    {
                        PlatformId = (int)Platform.CarwaleMobile,
                        ApplicationId = (int)Application.CarWale,
                        CityId = custLocation.CityId,
                        PageId = (int)CwPages.VersionMsite
                    },
                    CwcCookie = CurrentUser.CWC,
                    AbTest = CookiesCustomers.AbTest
                };

                IServiceAdapterV2 _versionPageAdapter = _unityContainer.Resolve<IServiceAdapterV2>("VersionPageMobile");
                versionDTO = _versionPageAdapter.Get<VersionPageDTO_Mobile, CarDataAdapterInputs>(versionInput);
                if (versionDTO != null)
                {
                    SetViewBags(versionDTO, custLocation);
                }
                else
                {
                    return HttpNotFound();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return View("~/views/m/cardata/Version.cshtml", versionDTO);
        }

        void SetViewBags(VersionPageDTO_Mobile versionDTO, Location custLocation)
        {
            try
            {
                ViewBag.CityName = CookiesCustomers.MasterCity;
                ViewBag.CityMaskingName = (versionDTO.CityDetails != null) ? versionDTO.CityDetails.CityMaskingName ?? string.Empty : string.Empty;
                ViewBag.CityId = CookiesCustomers.MasterCityId;
                ViewBag.ZoneId = custLocation.ZoneId > 0 ? custLocation.ZoneId : 0;
                ViewBag.IsCityPage = false;
                ViewBag.CityZone = (custLocation.ZoneName == string.Empty || custLocation.ZoneName == "Select Zone") ? custLocation.CityName : custLocation.ZoneName;
                ViewBag.MakeName = versionDTO.VersionDetails.MakeName;
                ViewBag.ModelName = versionDTO.ModelDetails.ModelName;
                ViewBag.VersionName = versionDTO.VersionDetails.VersionName;
                ViewBag.IsVersionPage = versionDTO.MobileOverviewDetails.IsVersionPage;
                ViewBag.DealerMaskingNumber = string.Empty;
                if (versionDTO.CallSlugInfo != null && versionDTO.CallSlugInfo.IsAdAvailable)
                {
                    var dealerId = 0;
                    if(versionDTO.DealerAd != null && versionDTO.DealerAd.Campaign != null)
                    {
                        dealerId = versionDTO.DealerAd.Campaign.DealerId;
                    }
                    ViewBag.DealerMaskingNumber = versionDTO.CallSlugInfo.DealerMobile;
                    ViewBag.CallSlugTrackingAction = "Version-Page-Slug";
                    ViewBag.CallSlugTrackingLabel = String.Format("{0},{1},{2},{3},{4},{5}",
                                                    versionDTO.VersionDetails.MakeName,
                                                    versionDTO.VersionDetails.ModelName,
                                                    ViewBag.CityName == "Select City" ? "No City" : ViewBag.CityName,
                                                    versionDTO.CallSlugInfo.DealerName,
                                                    dealerId,
                                                    versionDTO.CallSlugInfo.DealerMobile);
                }
                ViewBag.GalleryURL = string.IsNullOrEmpty(versionDTO.ModelDetails.OriginalImage) ? string.Empty :
                    Regex.Replace(Regex.Replace((versionDTO.MobileOverviewDetails.OriginalImage ?? string.Empty),
                    CWConfiguration.IMAGE_PATH_PATTERN, string.Empty), CWConfiguration.IMAGE_PATTERN, "html");

                ViewBag.TrackingPageName = "VersionPage";

                ViewBag.GetOffersEventAction = ViewBag.TrackingPageName + "_FloatingGetOffers";
                ViewBag.DealerId = versionDTO.DealerAd == null || versionDTO.DealerAd.Campaign == null ? 0 : versionDTO.DealerAd.Campaign.DealerId; //TODO DealerId should be having CampaignId (as per SponsoredDealer references) - find appropriate references in view
                ViewBag.ShowPriceInMyCityEventAction = ViewBag.TrackingPageName + "_FloatingShowPriceInMyCity";

                ViewBag.PricingEventAction = string.Empty;
                ViewBag.PricingEventLabel = string.Empty;
                if (CookiesCustomers.IsEligibleForORP && versionDTO.CarPriceOverview != null)
                {
                    int priceStatus = versionDTO.CarPriceOverview.PriceStatus;

                    int priceCityId = -1;
                    string priceCityName = string.Empty;

                    if (versionDTO.CarPriceOverview.City != null)
                    {
                        priceCityId = versionDTO.CarPriceOverview.City.CityId;
                        priceCityName = versionDTO.CarPriceOverview.City.CityName;
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

                    ViewBag.PricingEventLabel = (priceCityId < 1 ? "NA" : priceCityName)
                        + "_" + versionDTO.VersionDetails.MakeName + "_" + versionDTO.ModelDetails.ModelName + "_" + versionDTO.VersionDetails.VersionName;
                }

                ViewBag.IsPriceShown = (versionDTO.CarPriceOverview != null && versionDTO.CarPriceOverview.PriceStatus == (int)PriceBucket.HaveUserCity);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
    }
}