using Carwale.Interfaces.Deals;
using Carwale.Notifications;
using Carwale.Interfaces.SponsoredCar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Carwale.DTOs.Deals;
using Carwale.BL.CompareCars;
using Carwale.Interfaces.CompareCars;
using System.Configuration;
using Carwale.UI.Common;
using System.Web.UI;
using Carwale.DTOs;
using Carwale.UI.PresentationLogic;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.Geolocation;
using Carwale.UI.Filters;
using Carwale.Utility;
using Carwale.Entity.Enum;
using Carwale.Entity;
using Carwale.Entity.CarData;

namespace Carwale.UI.Controllers.Deals
{
    public class AdvantageController : Controller
    {
        private readonly ICompareCarsCacheRepository _compareCacheRepo;
        private readonly ISponsoredCarBL _sponsoredRepo;
        private readonly IDeals _carDeals;
        private readonly ICarVersionCacheRepository _carVersionsCacheRepository;
        private readonly IGeoCitiesCacheRepository _cityCache;
        private readonly ICarModels _carModelBl;
        private readonly ICarVersions _carVersionBl;

        string CarwaleAdvantageMaskingNumber = ConfigurationManager.AppSettings["CarwaleAdvantageMaskingNumber"].ToString();

        public AdvantageController(ICompareCarsCacheRepository compareCacheRepo, ISponsoredCarBL sponsoredRepo, IDeals carDeals, ICarVersionCacheRepository carVersionsCacheRepository,
            IGeoCitiesCacheRepository cityCache,ICarModels carModelBl,ICarVersions carVersionBl)
        {
            _compareCacheRepo = compareCacheRepo;
            _sponsoredRepo = sponsoredRepo;
            _carDeals = carDeals;
            _carVersionsCacheRepository = carVersionsCacheRepository;
            _cityCache = cityCache;
            _carModelBl = carModelBl;
            _carVersionBl = carVersionBl;
        }

        // GET : Product Details
        [DeviceDetectionFilter]
        [Route("deals/stock/{make}/{model}")]
        public ActionResult StockDetails(string make, string model, int? cityId, int versionId = 0)
        {
            ProductDetailsDTO_Desktop productDetails = new ProductDetailsDTO_Desktop();
            int usercityId = 0;
            try
            {
                if (cityId == null)
                {
                    Response.Redirect(Advanatge.GetRedirectionUrl(usercityId, Platform.CarwaleDesktop));
                }
                else
                {
                    usercityId = Convert.ToInt32(cityId);
                }

                var cmr = _carModelBl.FetchModelIdFromMaskingName(model, string.Empty);
                if (!cmr.IsValid)
                {
                    return HttpNotFound();
                }
                else if (cmr.IsRedirect)
                {
                    Response.Redirect(cmr.RedirectUrl);
                }

                productDetails.CitiesList = _carDeals.GetAdvantageCities();
                ViewBag.ModelId = cmr.ModelId;
                ViewBag.MaskingName = model;
               
                productDetails.DealsRecommedations = _carDeals.GetRecommendationsBySubSegment(ViewBag.ModelId, usercityId);
                productDetails.VersionsDeals = _carDeals.GetAllVersionDeals(ViewBag.ModelId, usercityId);
                List<CarVersions>  summaryList = _carVersionBl.GetCarVersions(ViewBag.ModelId, Status.All);
                productDetails.VersionsDeals.ForEach(x => x.SpecificationsOverview = summaryList.Where(z => z.Id == x.Version.ID).Select(y => y.SpecsSummary).FirstOrDefault());
                productDetails.DealsCount = _carDeals.GetCarCountByCity(usercityId);
                int offerModelId = Carwale.BL.Deals.Advanatge.GetOfferModelId(usercityId);
                if (offerModelId > 0)
                    productDetails.OfferOfTheWeek = _carDeals.GetOfferOfWeekDetails(offerModelId, usercityId);
                productDetails.Testimonials = _carDeals.GetDealsTestimonials();

                ViewBag.CityId = usercityId;
                if (versionId != 0)
                    ViewBag.VersionId = versionId;
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "HomeController.Index()\n Exception : " + ex.Message);
                objErr.LogException();
                return Redirect(Advanatge.GetRedirectionUrl(usercityId, Platform.CarwaleDesktop));
            }
            return View("~/views/deals/productdetails.cshtml", productDetails);
        }

        [Route("{make}-cars/{model}/advantage/booking/{stockId:int}")]
        public ActionResult Booking(string make, string model, int stockId, int cityId)
        {
            var stockDetails = new BookingAndroid_DTO();
            int cookieStockId = 0;
            try
            {
                stockDetails = _carDeals.GetStockDetails((Int32)stockId, (Int32)cityId);
                if (stockDetails == null || stockDetails.StockCount == 0)
                    return HttpNotFound();

                ViewBag.StockDetails = stockDetails;
                ViewBag.maskingNumber = CarwaleAdvantageMaskingNumber;
                ViewBag.MakeName = make;
                ViewBag.ModelName = model;
                ViewBag.StockId = stockId;
                if (Request.Cookies["_dealStockId"] != null)
                    Int32.TryParse(Request.Cookies["_dealStockId"].Value, out cookieStockId);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "HomeController.Index()\n Exception : " + ex.Message);
                objErr.LogException();
                Response.Redirect(Advanatge.GetRedirectionUrl(cityId, Platform.CarwaleDesktop));
            }
            if (Request.Cookies["isRedirectedFromPD"] == null || stockId != cookieStockId)
            {
                return Redirect("/" + make + "-cars/" + model + "/advantage/?cityid=" + cityId);
            }
            return View("~/views/deals/booking.cshtml");
        }

        [Route("advantage/")]
        public ActionResult Listing(int cityId = 0, string makes = "", string fuels = "", string transmissions = "", string bodyTypes = "", short so = 1, short sc = 2 ,string budget= "")
        {
            AdvantageSearchModelDTO SearchPageModel = new AdvantageSearchModelDTO() ;
            SearchPageModel.Filters = new AdvantageSearchFilterDTO();
            SearchPageModel.SelectedCity = new City();
            DeviceDetection dd = new DeviceDetection(Request.ServerVariables["HTTP_X_REWRITE_URL"].ToString());
            dd.DetectDevice();
            SearchPageModel.Cities = _carDeals.GetAdvantageCities();
            SearchPageModel.Filters.Manufacturers = makes.Replace(" ", "+");
            SearchPageModel.Filters.Fuels = fuels.Replace(" ", "+");
            SearchPageModel.Filters.Transmissions = transmissions.Replace(" ", "+");
            SearchPageModel.Filters.BodyTypes = bodyTypes.Replace(" ", "+");
            SearchPageModel.Filters.BudgetRange = budget;
            SearchPageModel.Filters.SO = so;
            SearchPageModel.Filters.SC = sc;

            cityId = cityId > 0 ? cityId : CookiesCustomers.MasterCityId;

            SearchPageModel.IsAdvantageCity = Advanatge.IsAdvantageCity(cityId, 0);
            cityId = SearchPageModel.IsAdvantageCity ? cityId : 1;
            SearchPageModel.SelectedCity.CityName = _cityCache.GetCityNameById((cityId).ToString());
            SearchPageModel.SelectedCity.CityId = cityId;
            return View("~/views/deals/Listings.cshtml", SearchPageModel);
        }

        // GET : Deals FAQ
        [Route("advantage/faq/")]
        public ActionResult DealsFAQ()
        {
            return View("~/Views/Deals/DealsFAQ.cshtml");
        }
    }
}
