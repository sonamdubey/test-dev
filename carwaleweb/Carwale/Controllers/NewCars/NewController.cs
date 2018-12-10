using Carwale.Entity;
using Carwale.Entity.CMS;
using Carwale.Entity.CMS.URIs;
using Carwale.Entity.Enum;
using Carwale.Entity.ViewModels;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.CompareCars;
using Carwale.Interfaces.Deals;
using Carwale.Interfaces.Deals.Cache;
using Carwale.Interfaces.IPToLocation;
using Carwale.Interfaces.SponsoredCar;
using Carwale.Notifications;
using Carwale.UI.ClientBL;
using Carwale.UI.Common;
using Carwale.UI.Filters;
using Carwale.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;

namespace Carwale.Controllers
{
    [SessionState(SessionStateBehavior.Disabled)]
    public class NewController : Controller
    {
        private readonly ICarModels _models;
        private readonly ISponsoredCarCache _sponsoredCars;
        private readonly ICompareCarsBL _compRepo;
        private readonly IIPToLocation _iPToLocation;
        private readonly static string _domainName = ConfigurationManager.AppSettings["adSlotDomain"] ?? string.Empty;
        public NewController(ICarModels models, ISponsoredCarCache sponsoredCars, ICompareCarsBL compRepo, IIPToLocation iPToLocation)
        {
            _models = models;
            _sponsoredCars = sponsoredCars;
            _compRepo = compRepo;
            _iPToLocation = iPToLocation;
        }

        [Route("new/")]
        [Route("new/sbi-yono")]
        public ActionResult Index()
        {
            Response.AddHeader("Vary", "User-Agent");
            try
            {
                if (Request.Path == "/new/sbi-yono/")
                    ViewBag.IsSBIAd = true;
                else
                    ViewBag.IsSBIAd = false;

                bool isMobile = DeviceDetectionManager.IsMobile(this.HttpContext);
                if (isMobile)
                {
                    return ReturnMobileView();
                }
                else
                {
                    return ReturnDesktopView();
                }          
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "NewController.Index()\n Exception : " + ex.Message);
                objErr.LogException();
                return HttpNotFound();
            }
        }

        private ActionResult ReturnDesktopView()
        {
            NewCarModel Model = new NewCarModel();
            int cityId = CookiesCustomers.MasterCityId;
            //To fetch sponsored campaign for autosuggest placeholder PQWidget
            var sponsorPlaceHolder = _sponsoredCars.GetSponsoredCampaigns((int)Carwale.Entity.Enum.CampaignCategory.CarSearchPlaceHolder, (int)Carwale.Entity.Enum.Platform.CarwaleDesktop, (int)Carwale.Entity.Enum.PlaceHolderCategory.PQWidget);
            if (sponsorPlaceHolder != null && sponsorPlaceHolder.Count > 0)
            {
                ViewData["PQWidget"] = sponsorPlaceHolder.First().Ad_Html;
            }
            Pagination page = new Pagination() { PageNo = 1, PageSize = 6 };
            Model.HotComparisons = _compRepo.GetHotComaprisons(page, cityId, true);

            int widgetSource = (int)WidgetSource.NewCarLandingPageCompareCarWidgetDesktop;
            if (Model.HotComparisons != null && Model.HotComparisons.Count > 0)
                Model.HotComparisons.ForEach(x => x.WidgetPage = widgetSource);

            ViewBag.CityId = cityId;
            ViewBag.City = _iPToLocation.GetCity();
            ViewBag.PageId = 2;
            ViewBag.DomainName = _domainName;
            return View("~/Views/NewCar/New.cshtml", Model);
        }

        private ActionResult ReturnMobileView()
        {
            HomeModel Model = new HomeModel();
            TopSellingModel topSellingModel = new TopSellingModel();

            Pagination page = new Pagination() { PageNo = 1, PageSize = 4 };
            Pagination pageForComparison = new Pagination() { PageNo = 1, PageSize = 4 };
            ArticleFeatureURI queryString = new ArticleFeatureURI() { ApplicationId = (ushort)CMSAppId.Carwale, ContentTypes = "1", TotalRecords = 2 };
            int cityId = CookiesCustomers.MasterCityId;

            topSellingModel.TopSelling = _models.GetTopSellingCarModels(page, cityId);

            //To fetch sponsored campaigns for popular cars
            topSellingModel.SponsoredPopularCars = _sponsoredCars.GetSponsoredCampaigns(Convert.ToInt16(Carwale.Entity.Enum.CampaignCategory.PopularCars), Convert.ToInt16(Carwale.Entity.Enum.Platform.CarwaleMobile));

            Model.TopSellingModel = topSellingModel;

            Model.NewLaunches = _models.GetLaunchedCarModelsV1(page, cityId);

            Model.Upcoming = _models.GetUpcomingCarModels(page);

            Model.HotComparisons = _compRepo.GetHotComaprisons(pageForComparison);

            int widgetSource = (int)WidgetSource.NewCarLandingPageCompareCarWidgetMobile;
            if (Model.HotComparisons != null && Model.HotComparisons.Count > 0)
                Model.HotComparisons.ForEach(x => x.WidgetPage = widgetSource);

            ViewBag.City = _iPToLocation.GetCity();
            ViewBag.PageId = 2;
            return View("~/Views/m/New/index.cshtml", Model);
        } 
    }
}