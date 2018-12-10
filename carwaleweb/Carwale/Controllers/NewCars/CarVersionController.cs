using Carwale.BL.PriceQuote;
using Carwale.DTOs.NewCars;
using Carwale.Entity.Geolocation;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.NewCars;
using Carwale.Interfaces.PriceQuote;
using Carwale.UI.Common;
using Carwale.UI.Filters;
using Microsoft.Practices.Unity;
using System;
using System.Configuration;
using System.Web.Mvc;
using System.Web.SessionState;
using Carwale.Interfaces.Campaigns;
using Carwale.Entity.CarData;
using Carwale.Entity.AdapterModels;
using Carwale.Entity;
using Carwale.Notifications.Logs;
using Carwale.DTOs.ViewModels.New;

namespace Carwale.UI.Controllers.NewCars
{
    [SessionState(SessionStateBehavior.Disabled)]
    public class CarVersionController : Controller
    {
        private readonly IServiceAdapterV2 _versionPageAdaptorDesktop;
        private readonly static string _domainName = ConfigurationManager.AppSettings["adSlotDomain"] ?? string.Empty;
        private readonly ICarVersions _carVersionBl;

        public CarVersionController(Func<string, IServiceAdapterV2> versionPageAdaptorDesktop, ICarVersions carVersionBl)
        {
            try
            {
                _versionPageAdaptorDesktop = versionPageAdaptorDesktop("VersionPageDesktop");
                _carVersionBl = carVersionBl;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "CarVersionController Dependency Injection Failed");
            }

        }
        [DeviceDetectionFilter]
        public ActionResult Index()
        {
            VersionPageDTO_Desktop versionDTO = null;
            try
            {
                string makeMaskingName = Request["make"] != null ? Request.QueryString["make"] : string.Empty;
                string modelMaskingName = Request["model"] != null ? Request.QueryString["model"] : string.Empty;
                string versionMaskingName = Request["version"] != null ? Request.QueryString["version"] : string.Empty;
                string urlVersionId = Request["car"] != null ? Request.QueryString["car"] : string.Empty;
                VersionMaskingNameValidation versionInfo;
                if (_carVersionBl != null)
                {
                    versionInfo = _carVersionBl.FetchVersionInfoFromMaskingName(modelMaskingName, versionMaskingName, string.Empty, urlVersionId, false, makeMaskingName);
                }
                else
                {
                    return HttpNotFound();
                }
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
                    CityName = CookiesCustomers.MasterCityId <= 0 ? "No City" : CookiesCustomers.MasterCity
                };
                CarDataAdapterInputs versionInput = new CarDataAdapterInputs
                {
                    ModelDetails = new CarEntity { VersionId = versionInfo.VersionId, ModelId = versionInfo.ModelId },
                    CustLocation = custLocation,
                    CwcCookie = CurrentUser.CWC,
                    AbTest = CookiesCustomers.AbTest
                };
                if (_versionPageAdaptorDesktop != null)
                {
                    versionDTO = _versionPageAdaptorDesktop.Get<VersionPageDTO_Desktop, CarDataAdapterInputs>(versionInput);
                }
                if (versionDTO != null)
                {
                    SetViewBags(versionDTO, custLocation);
                    SetFloatingCtaVm(versionDTO, custLocation?.CityId ?? -1, custLocation?.CityName ?? string.Empty);
                    return View("~/Views/NewCar/Version.cshtml", versionDTO);
                }
                else
                {
                    return HttpNotFound();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return HttpNotFound();
            }

        }
        private void SetFloatingCtaVm(VersionPageDTO_Desktop versionPageDTO, int cityId, string cityName)
        {
            try
            {
                var floatingCta = new ModelFloatingCtaViewModel();
                floatingCta.IsShowFloatingCta = versionPageDTO.VersionDetails.New > 0;
                if (floatingCta.IsShowFloatingCta)
                {
                    floatingCta.IsAdAvailable = (versionPageDTO.SponsoredDealerAd != null && versionPageDTO.SponsoredDealerAd.DealerId > 0) || versionPageDTO.ShowCampaignLink;
                    floatingCta.CarName = string.Format("{0} {1}", versionPageDTO.ModelDetails?.MakeName,
                        versionPageDTO.ModelDetails?.ModelName);
                    floatingCta.ModelName = versionPageDTO.ModelDetails?.ModelName;
                    floatingCta.MakeName = versionPageDTO.ModelDetails?.MakeName;
                    floatingCta.VersionName = versionPageDTO.VersionDetails?.VersionName;
                    floatingCta.CityName = cityName;
                    floatingCta.ModelImagePath = versionPageDTO.ModelDetails?.OriginalImage;
                    floatingCta.CityTrackingLabel = ViewBag.CityZone ?? string.Empty;
                    floatingCta.PageId = 31;
                    floatingCta.PredicationLabel = versionPageDTO.OverviewDetails?.PredictionData?.Label;
                    floatingCta.PredicationScore = versionPageDTO.OverviewDetails?.PredictionData?.Score ?? 0;
                    floatingCta.ShowCampaignLink = versionPageDTO.ShowCampaignLink;
                    floatingCta.CityId = cityId;
                    floatingCta.PriceText = string.Format("₹ {0}", FormatPrice.GetFormattedPriceV2(versionPageDTO.CarPriceOverview.Price.ToString()));
                    floatingCta.PriceLabel = !string.IsNullOrWhiteSpace(versionPageDTO.CarPriceOverview?.PriceLabel)
                        ? (versionPageDTO.CarPriceOverview?.PriceLabel +
                        (versionPageDTO.CarPriceOverview.PriceStatus != (int)PriceBucket.HaveUserCity ? string.Empty : ", " + versionPageDTO.CarPriceOverview.City.CityName)) : string.Empty;
                }
                versionPageDTO.FloatingCtaViewModel = floatingCta;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
        void SetViewBags(VersionPageDTO_Desktop versionDTO, Location custLocation)
        {
            try
            {
                ViewBag.CityId = CookiesCustomers.MasterCityId;
                ViewBag.IsCityPage = false;
                ViewBag.CityName = CookiesCustomers.MasterCity;
                ViewBag.CityMaskingName = (versionDTO.CityDetails != null) ? versionDTO.CityDetails.CityMaskingName ?? string.Empty : string.Empty;
                ViewBag.CityZone = (custLocation.ZoneName == string.Empty || custLocation.ZoneName == "Select Zone") ? ((CookiesCustomers.MasterCity == string.Empty || CookiesCustomers.MasterCity == "Select City") ? "No City" : CookiesCustomers.MasterCity) : custLocation.ZoneName;
                ViewBag.ModelName = versionDTO.ModelDetails.MakeName + " " + versionDTO.ModelDetails.ModelName;
                ViewBag.Make = versionDTO.ModelDetails.MakeName;
                ViewBag.DealerCount = versionDTO.NewCarDealersCount;
                ViewBag.isExpandable = "1";
                ViewBag.VerticalDisplay = false;
                ViewBag.IsVersionPage = true;
                ViewBag.MaskingNumber = versionDTO.SponsoredDealerAd.DealerMobile;
                ViewBag.CarModelName = versionDTO.ModelDetails.ModelName;
                ViewBag.DomainName = _domainName;
                ViewBag.CarVersionName = versionDTO.VersionDetails.VersionName;
                ViewBag.IsPriceShown = (versionDTO.CarPriceOverview != null && versionDTO.CarPriceOverview.PriceStatus == (int)PriceBucket.HaveUserCity);
                ViewBag.TrackingPageName = "VersionPage";
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
    }
}