using Carwale.BL.CarData;
using Carwale.BL.Classified.PopularUsedCars;
using Carwale.BL.CompareCars;
using Carwale.BL.SponsoredCar;
using Carwale.BL.Videos;
using Carwale.Cache.CarData;
using Carwale.Cache.CMS;
using Carwale.Cache.CompareCars;
using AEPLCore.Cache;
using Carwale.Cache.SponsoredData;
using Carwale.DAL.CarData;
using Carwale.DAL.CompareCars;
using Carwale.DAL.IPToLocation;
using Carwale.DAL.SponsoredCar;
using Carwale.DTOs.Classified.PopularUC;
using Carwale.Entity;
using Carwale.Entity.CMS;
using Carwale.Entity.CMS.URIs;
using Carwale.Entity.Enum;
using Carwale.Entity.Geolocation;
using Carwale.Entity.ViewModels;
using Carwale.Interfaces;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.Classified.PopularUsedCarsDetails;
using Carwale.Interfaces.CMS;
using Carwale.Interfaces.CMS.Articles;
using Carwale.Interfaces.CompareCars;
using Carwale.Interfaces.Deals;
using Carwale.Interfaces.Deals.Cache;
using Carwale.Interfaces.IPToLocation;
using Carwale.Interfaces.SponsoredCar;
using Carwale.Notifications;
using Carwale.UI.Common;
using Carwale.UI.Filters;
using Carwale.UI.PresentationLogic;
using Carwale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace Carwale.MobileWeb.Controllers
{
    [SessionState(SessionStateBehavior.Disabled)]
    public class HomeController : Controller
    {

        private readonly ICarModels _carModels;
        private readonly ICompareCarsBL _compareCar;
        private readonly ICMSContent _cmsContentCacheRepo;
        private readonly IVideosBL _video;
        private readonly IPopularUCDetails _popularUCDetails;
        private readonly IIPToLocation _ipLocation;
        private readonly ISponsoredCarCache _sponsoredCarsCache;
        private readonly IDealsCache _dealsCache;
        private readonly IDeals _dealsBL;

        public HomeController(ICarModels carModels, ICompareCarsBL compareCar, ICMSContent cmsContentCacheRepo,
            IVideosBL video, IPopularUCDetails popularUCDetails, IIPToLocation ipLocation, ISponsoredCarCache sponsoredCarsCache, IDealsCache dealsCache, IDeals dealsBL)
        {
            _carModels = carModels;
            _compareCar = compareCar;
            _cmsContentCacheRepo = cmsContentCacheRepo;
            _video = video;
            _popularUCDetails = popularUCDetails;
            _ipLocation = ipLocation;
            _sponsoredCarsCache = sponsoredCarsCache;
            _dealsCache = dealsCache;
            _dealsBL = dealsBL;
        }

        // GET: m   
        [Route("m/home"),CaptchaValidationFilter]
        public ActionResult Index()
        {
            HomeModel Model = new HomeModel();
            TopSellingModel topSellingModel = new TopSellingModel();
            try
            {
                Pagination page = new Pagination() { PageNo = 1, PageSize = 4 };
                Pagination pageForComparison = new Pagination() { PageNo = 1, PageSize = 4 };
                ArticleRecentURI queryString = new ArticleRecentURI() { ApplicationId = (ushort)CMSAppId.Carwale, ContentTypes = System.Configuration.ConfigurationManager.AppSettings["NewsCategories"], TotalRecords = Convert.ToUInt16(ConfigurationManager.AppSettings["MobileNewsWidget_Count"]) };
                int cityId = CookiesCustomers.MasterCityId;
                topSellingModel.TopSelling = _carModels.GetTopSellingCarModels(page, cityId);
                //To fetch sponsored campaigns for Popular cars 

                topSellingModel.SponsoredPopularCars = _sponsoredCarsCache.GetSponsoredCampaigns(Convert.ToInt16(Carwale.Entity.Enum.CampaignCategory.PopularCars), Convert.ToInt16(Carwale.Entity.Enum.Platform.CarwaleMobile));

                var carwaleMobileType = (int)Carwale.Entity.Enum.Platform.CarwaleMobile;
                var campaignCategoryId = (int)Carwale.Entity.Enum.CampaignCategory.CarSearchPlaceHolder;
                //To fetch sponsored campaigns for home campaign banner
                var sponsorHomeBanner = _sponsoredCarsCache.GetSponsoredCampaigns((int)Carwale.Entity.Enum.CampaignCategory.Banner, carwaleMobileType, (int)CategorySection.HomePageBanner);
                if (sponsorHomeBanner != null && sponsorHomeBanner.Count > 0)
                {
                    Model.SponsoredHomeBanner = sponsorHomeBanner.First();
                }

                var sponsoredPqBanner = _sponsoredCarsCache.GetSponsoredCampaigns((int)Carwale.Entity.Enum.CampaignCategory.Banner, carwaleMobileType, (int)CategorySection.PQWidget);
                if (sponsoredPqBanner != null && sponsoredPqBanner.Count > 0)
                {
                    Model.SponsoredPQBanner = sponsoredPqBanner.First();
                }

                //To fetch sponsored campaign for autosuggest placeholder HomePageBanner
                var sponsorPlaceHolder = _sponsoredCarsCache.GetSponsoredCampaigns(campaignCategoryId, carwaleMobileType, (int)CategorySection.HomePageBanner);
                if (sponsorPlaceHolder != null && sponsorPlaceHolder.Count > 0)
                {
                    ViewData["HomePageBanner"] = sponsorPlaceHolder.First().Ad_Html;
                }

                //To fetch sponsored campaign for autosuggest placeholder PQWidget
                sponsorPlaceHolder = _sponsoredCarsCache.GetSponsoredCampaigns(campaignCategoryId, carwaleMobileType, (int)CategorySection.PQWidget);
                if (sponsorPlaceHolder != null && sponsorPlaceHolder.Count > 0)
                {
                    ViewData["PQWidget"] = sponsorPlaceHolder.First().Ad_Html;
                }

                Model.TopSellingModel = topSellingModel;

                Model.NewLaunches = _carModels.GetLaunchedCarModelsV1(page, cityId);

                Model.Upcoming = _carModels.GetUpcomingCarModels(page);

                Model.HotComparisons = _compareCar.GetHotComaprisons(pageForComparison);
                int widgetSource = (int)WidgetSource.HomePageCompareCarWidgetMobile;
                if (Model.HotComparisons != null && Model.HotComparisons.Count > 0)
                    Model.HotComparisons.ForEach(x => x.WidgetPage = widgetSource);
                Model.TopNews = _cmsContentCacheRepo.GetMostRecentArticles(queryString);

                var sponsoredArticle = _cmsContentCacheRepo.GetSponsoredArticle(queryString.ContentTypes, CWConfiguration.SponsoredAuthorId);

                if (sponsoredArticle != null && sponsoredArticle.BasicId > 0 && Model.TopNews != null)
                {
                    Model.TopNews.Insert(0, sponsoredArticle);
                }

                queryString.ContentTypes = "8"; queryString.TotalRecords = 5;
                Model.TopReviews = _cmsContentCacheRepo.GetMostRecentArticles(queryString);

                Model.TopVideos = _video.GetNewModelsVideosBySubCategory(EnumVideoCategory.FeaturedAndLatest, CMSAppId.Carwale, startIndex: 1, endindex: 4);

                var city = _popularUCDetails.FetchCityById(CookiesCustomers.MasterCityId.ToString());
                ViewBag.UsedCity = city;
                Model.PopularUsedCars = _popularUCDetails.FillPopularUsedCarDetails(city);

                ViewBag.City = _ipLocation.GetCity();
                ViewBag.PageId = 1;

                ViewBag.IsNewCarSearchType = Request.Cookies["_carSearchType"] != null && Request.Cookies["_carSearchType"].Value.ToString() != string.Empty ? Request.Cookies["_carSearchType"].Value.ToString() == "1" ? true : false : true;

            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "HomeController.Index()\n Exception : " + ex.Message);
                objErr.LogException();
            }
            return View("~/Views/m/MHome/Index.cshtml", Model);
        }

    }
}