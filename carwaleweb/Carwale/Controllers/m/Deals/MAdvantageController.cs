using Carwale.BL.CompareCars;
using Carwale.Interfaces.CompareCars;
using Carwale.Interfaces.Deals;
using Carwale.Interfaces.SponsoredCar;
using Carwale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Carwale.DTOs.Deals;
using System.Configuration;
using MobileWeb.Common;
using Carwale.UI.PresentationLogic;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.Geolocation;
using Carwale.DTOs;
using Carwale.BL.Deals;
using Carwale.Utility;
using Carwale.Entity.Enum;
using Carwale.Entity.CarData;
using Carwale.Entity;

namespace Carwale.UI.Controllers.m.Deals
{
    public class MAdvantageController : Controller
    {
        private readonly ICompareCarsCacheRepository _compareCacheRepo;
        private readonly ISponsoredCarBL _sponsoredRepo;
        private readonly IDeals _carDeals;
        private readonly ICarVersionCacheRepository _carVersionsCacheRepository;
        private readonly IGeoCitiesCacheRepository _cityCache;
        string CarwaleAdvantageMaskingNumber = ConfigurationManager.AppSettings["CarwaleAdvantageMaskingNumber"].ToString();
        private readonly ICarModels _carModelBl;
        private readonly ICarVersions _carVersionBl;

        public MAdvantageController(ICompareCarsCacheRepository compareCacheRepo, ISponsoredCarBL sponsoredRepo, IDeals carDeals, ICarVersionCacheRepository carVersionsCacheRepository, IGeoCitiesCacheRepository cityCache, ICarModels carModelBl, ICarVersions carVersionBl)
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
        // GET : Product Details
        [Route("m/deals/stock/{make}/{model}")]
        public ActionResult StockDetails(string make, string model, int? cityId, int versionId = 0)
         {
             
            ProductDetailsDTO_Msite productDetails = new ProductDetailsDTO_Msite();
            int usercityId = 0;
            try
            {
                if (cityId == null)
                {
                    return RedirectPermanent(Carwale.UI.PresentationLogic.Advanatge.GetRedirectionUrl(usercityId, Platform.CarwaleMobile));
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
                    return RedirectPermanent(cmr.RedirectUrl);
                }

                productDetails.CitiesList = _carDeals.GetAdvantageCities();

                ViewBag.MaskingName = model;
                ViewBag.ModelId = cmr.ModelId;
                ViewBag.CityId = usercityId;

                productDetails.DealsRecommedations = _carDeals.GetRecommendationsBySubSegment(ViewBag.ModelId, usercityId);
                productDetails.AllVersionList = _carDeals.GetAllVersionDeals(ViewBag.ModelId, usercityId);
                List<CarVersions> summaryList = _carVersionBl.GetCarVersions(ViewBag.ModelId, Status.All);
                productDetails.AllVersionList.ForEach(x => x.SpecificationsOverview = summaryList.Where(z => z.Id == x.Version.ID).Select(y => y.SpecsSummary).FirstOrDefault());
                productDetails.DealsCount = _carDeals.GetCarCountByCity(usercityId);
                int offerModelId = Carwale.BL.Deals.Advanatge.GetOfferModelId(usercityId);
                if (offerModelId > 0)
                    productDetails.OfferOfTheWeek = _carDeals.GetOfferOfWeekDetails(offerModelId, usercityId);
                productDetails.Testimonials = _carDeals.GetDealsTestimonials();
                if (versionId != 0)
                    ViewBag.VersionId = versionId;
                else
                    ViewBag.VersionId = "";
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "HomeController.Index()\n Exception : " + ex.Message);
                objErr.LogException();
                return RedirectPermanent(Carwale.UI.PresentationLogic.Advanatge.GetRedirectionUrl(usercityId, Platform.CarwaleMobile));
            }
            return View("~/views/m/deals/productdetails.cshtml", productDetails);
        }


        // GET : Booking Details
        [Route("m/{make}-cars/{model}/advantage/booking/{stockId:int}")]
        public ActionResult Booking(string make, string model, int stockId, int cityId)
        {
            int cookieStockId = 0;
            try
            {

                BookingAndroid_DTO stockDetails = _carDeals.GetStockDetails(stockId, cityId);

                if (stockDetails == null || stockDetails.StockCount == 0 )
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
                return RedirectPermanent(Carwale.UI.PresentationLogic.Advanatge.GetRedirectionUrl(cityId, Platform.CarwaleMobile));
            }
            if (Request.Cookies["isRedirectedFromPD"] == null || stockId != cookieStockId)
            {
                return Redirect("/m/" + make + "-cars/" + model + "/advantage/?cityid=" + cityId);
            }
            return View("~/Views/m/Deals/Booking.cshtml");
        }

        private int FetchModelIdFromMaskingName(string modelMaskingName)
        {
            int modelId = -1;

            if (!string.IsNullOrEmpty(modelMaskingName))
            {
                var cmr = _carModelBl.FetchModelIdFromMaskingName(modelMaskingName,string.Empty);

                if (cmr.IsRedirect)
                {
                    RedirectPermanent(cmr.RedirectUrl);                            
                }
                else
                    modelId = cmr.ModelId;
            }
            return modelId;
        }



        [Route("m/advantage/")]
        public ActionResult Landing(int cityId = 0, string makes = "", string bodyTypes = "", string fuels = "", string transmissions = "", short so = 1, short sc = 2, string budget = "")
        {
            AdvantageSearchModelDTO SearchPageModel = new AdvantageSearchModelDTO();
            SearchPageModel.Filters = new AdvantageSearchFilterDTO();
            SearchPageModel.SelectedCity = new City();
            SearchPageModel.Cities = _carDeals.GetAdvantageCities();
            SearchPageModel.Filters.Manufacturers = makes.Replace(" ", "+");
            SearchPageModel.Filters.Fuels = fuels.Replace(" ", "+");
            SearchPageModel.Filters.Transmissions = transmissions.Replace(" ", "+");
            SearchPageModel.Filters.BodyTypes = bodyTypes.Replace(" ", "+");
            SearchPageModel.Filters.BudgetRange = budget;
            SearchPageModel.Filters.SO = so;
            SearchPageModel.Filters.SC = sc;
            cityId = cityId > 0 ? cityId : CookiesCustomers.MasterCityId;

            SearchPageModel.IsAdvantageCity =  Carwale.UI.PresentationLogic.Advanatge.IsAdvantageCity(cityId, 0);
            cityId = SearchPageModel.IsAdvantageCity ? cityId : 1;
            SearchPageModel.SelectedCity.CityName = _cityCache.GetCityNameById((cityId).ToString());
            SearchPageModel.SelectedCity.CityId = cityId;

            return View("~/views/m/deals/Listings.cshtml", SearchPageModel);
        }

        [Route("m/{make}-cars/{model}/advantage/gallery/{stockId:int}")]
        public ActionResult Gallery(string make, string model,int stockId,int cityId)
        {
            try 
            {
                Gallery_DTO galleryDTO = new Gallery_DTO();
                galleryDTO.StockId = stockId;
                galleryDTO.StockDetails = _carDeals.GetStockDetails(stockId, cityId);
                if (galleryDTO.StockDetails != null)
                {
                    return View("~/views/m/deals/Gallery.cshtml", galleryDTO);
                }
                else
                {
                   return Redirect(Carwale.UI.PresentationLogic.Advanatge.GetRedirectionUrl(cityId, Platform.CarwaleMobile));
                }
                
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "HomeController.gallery : " + ex.Message);
                objErr.LogException();
                return Redirect(Carwale.UI.PresentationLogic.Advanatge.GetRedirectionUrl(cityId, Platform.CarwaleMobile));
            }
        }

        [Route("m/advantage/faq/")]
        public ActionResult DealsFAQ()
        {
            return View("~/Views/m/Deals/DealsFAQ.cshtml");
        }
    }
}