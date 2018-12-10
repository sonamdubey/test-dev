using Carwale.BL.PriceQuote;
using Carwale.DTOs.CarData;
using Carwale.DTOs.NewCars;
using Carwale.Entity.CarData;
using Carwale.Entity.CMS;
using Carwale.Entity.CMS.URIs;
using Carwale.Entity.Enum;
using Carwale.Interfaces.CMS;
using Carwale.Interfaces.PriceQuote;
using Microsoft.Practices.Unity;
using MobileWeb.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using System.Web.SessionState;
using Carwale.Interfaces.CarData;
using Carwale.Entity.CMS.Articles;
using Carwale.Interfaces.CMS.Articles;
using Carwale.Interfaces.CMS.UserReviews;
using Carwale.Notifications;
using System.Linq;
using Carwale.Interfaces;
using Carwale.Utility;
using Carwale.Entity.CMS.UserReviews;
using Carwale.BL.CMS;
using Carwale.Entity.PriceQuote;
using Carwale.DTOs.CMS.ThreeSixtyView;
using Carwale.Notifications.Logs;
using Carwale.Entity.ViewModels.CarData;
using Carwale.BL.Experiments;
using Carwale.UI.ViewModels.NewCars;

namespace Carwale.UI.Controllers.m.CarData
{
    [SessionState(SessionStateBehavior.Disabled)]
    public class ModelComponentController : Controller
    {
        protected string RedirectUrl = string.Empty;
        protected bool isRedirect = false;
        protected IUnityContainer _unityContainer;
        protected ICarModelCacheRepository _modelCacheRepo;
        protected IVideosBL carModelVideos;
        protected ICMSContent _cmsContentCacheRepo;
        protected IUserReviewsCache _userreviewCache;
        protected IPrices _prices;
        protected string defaultView = "~/views/m/cardata/modelcomponent.cshtml";

        public ModelComponentController(IUnityContainer _unityContainer, IUserReviewsCache userreviewCache, ICMSContent cmsContentCacheRepo,
              IVideosBL carModelVideos, IPrices prices, ICarModelCacheRepository modelsCacheRepo)
        {
            try
            {
                this._unityContainer = _unityContainer;
                this._modelCacheRepo = modelsCacheRepo;
                this.carModelVideos = carModelVideos;
                this._cmsContentCacheRepo = cmsContentCacheRepo;
                this._userreviewCache = userreviewCache;
                this._prices = prices;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "Dependency Injection Block at ModelComponentController");
            }

        }

        [Carwale.UI.Common.OutputCacheAttr("modelId")]
        public ActionResult ColorsComponent(int modelId, CarModelDetails modelDetails = null)
        {
            try
            {
                if (modelDetails == null && _modelCacheRepo != null)
                {
                    modelDetails = _modelCacheRepo.GetModelDetailsById(modelId);
                }

                if (modelDetails != null)
                {
                    return PartialView("~/views/shared/m/cardata/_Colors.cshtml", modelDetails);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "ModelComponentController.ColorsComponent()");
            }
            return new EmptyResult();
        }

        public ActionResult MileageComponent(int modelId, List<MileageDataEntity> data = null, bool isOverView = false, string modelName = "", bool isMobile = true, bool isAmp = false, string mileageHeading = "", bool isMileagePage = false)
        {
            try
            {
                if (data == null || data.Count == 0)
                {
                    return new EmptyResult();
                }
                ViewBag.IsMobile = isMobile;
                if (!string.IsNullOrEmpty(modelName)) ViewBag.ModelName = modelName;
                else
                {
                    if (_modelCacheRepo != null)
                    {
                        CarModelDetails modelDetails = _modelCacheRepo.GetModelDetailsById(modelId);
                        ViewBag.ModelName = modelDetails.ModelName;
                        ViewBag.MakeName = modelDetails.MakeName;
                    }
                    else
                    {
                        ViewBag.ModelName = string.Empty;
                    }
                }
                ViewBag.MileageHeading = mileageHeading;
                ViewBag.IsMileagePage = isMileagePage;
                if (isOverView)
                {
                    return PartialView("~/Views/Shared/NewCars/_ModelMileage.cshtml", data);
                }
                return isAmp ? PartialView("~/Views/Shared/m/AMP/_MileageWidgetAmp.cshtml", data) :
                              PartialView("~/views/shared/m/cardata/_mileage.cshtml", data);
            }
            catch (Exception ex)
            {

                Logger.LogException(ex, "ModelComponentController.MileageComponent()");
            }
            return new EmptyResult();
        }

        [Carwale.UI.Common.OutputCacheAttr("modelId;tab;isAmp")]
        public ActionResult ModelMenu(int modelId, ModelMenuEnum tab = ModelMenuEnum.Overview, CarModelDetails modelDetails = null, bool isAmp = false, bool isPriceInCityPage = false)
        {
            try
            {

                List<ArticleSummary> expertReviews = null;
                if (_cmsContentCacheRepo != null)
                {
                    expertReviews = _cmsContentCacheRepo.GetMostRecentArticles(new ArticleRecentURI()
                    {
                        ApplicationId = (ushort)CMSAppId.Carwale,
                        ContentTypes = Convert.ToInt16(CMSContentType.RoadTest).ToString(),
                        TotalRecords = Convert.ToUInt16(ConfigurationManager.AppSettings["ExpertReviewsForMobile_Count"]),
                        ModelId = modelId,
                    });
                }
                var modelColor = _modelCacheRepo.GetModelColorsByModel(modelId);
                ModelMenuDTO_V1 menudto = new ModelMenuDTO_V1()
                {
                    ActiveSection = tab,
                    IsExpertReviewAvailable = expertReviews != null && expertReviews.Count > 0,
                    IsColorAvailable = modelColor != null && modelColor.Count > 0,
                    IsColorsImagesAvailable = CMSCommon.IsModelColorPhotosPresent(modelColor),
                    IsVariantAvailable = modelDetails.New, // assumption: if model is new then version will be there
                    isPhotoAvailable = !string.IsNullOrEmpty(modelDetails.OriginalImage),
                    PhotoCount = (short?)modelDetails.PhotoCount,
                    IsMileageAvailable = (modelDetails.New && !modelDetails.Futuristic),
                    Is360Available = CMSCommon.IsThreeSixtyViewAvailable(modelDetails),
                    CarName = string.Format("{0} {1}", modelDetails.MakeName, modelDetails.ModelName),
                    ModelName = modelDetails.ModelName,
                    MaskingName = modelDetails.MaskingName,
                    MakeName = modelDetails.MakeName,
                    TrackingLabel = string.Format("{0}-{1}-{2}", tab.ToPageName(), modelDetails.ModelName, CookiesCustomers.MasterCity ?? string.Empty),
                    VideoCount = (short?)(modelDetails.VideoCount > 0 ? modelDetails.VideoCount : carModelVideos.GetVideosByModelId(modelId, CMSAppId.Carwale, 1, -1).Count),
                    Default360Category = CMSCommon.Get360DefaultCategory(AutoMapper.Mapper.Map<ThreeSixtyAvailabilityDTO>(modelDetails)),
                    IsPriceInCityPage = isPriceInCityPage
                };
                if (menudto.IsExpertReviewAvailable || menudto.IsColorAvailable || menudto.IsVariantAvailable || menudto.isPhotoAvailable || menudto.IsMileageAvailable)
                {
                    if (isAmp)
                        return PartialView("~/Views/Shared/m/AMP/_ModelMainMenuAmp.cshtml", menudto);
                    else
                        return PartialView("~/views/shared/m/cardata/_ModelMainMenu.cshtml", menudto);
                }
            }
            catch (Exception ex)
            {

                Logger.LogException(ex, "ModelComponentController.ModelMenu()");
            }

            return new EmptyResult();
        }

        [Carwale.UI.Common.OutputCacheAttr("modelId;page")]
        public ActionResult ModelVideos(int modelId, ContentPages page, List<Video> modelVideos = null, CarModelDetails modelDetails = null)
        {
            try
            {
                if (modelDetails == null && _modelCacheRepo != null)
                {
                    modelDetails = _modelCacheRepo.GetModelDetailsById(modelId);
                }

                ViewBag.MakeName = modelDetails != null ? modelDetails.MakeName : string.Empty;
                ViewBag.ModelName = modelDetails != null ? modelDetails.ModelName : string.Empty;
                ViewBag.MaskingName = modelDetails != null ? modelDetails.MaskingName : string.Empty;

                if (modelVideos == null || modelVideos.Count <= 0)
                {
                    modelVideos = carModelVideos.GetVideosByModelId(modelId, CMSAppId.Carwale, 1, -1);
                }

                Tuple<List<Video>, ContentPages> tuple = new Tuple<List<Video>, ContentPages>(modelVideos, page);

                return PartialView("~/views/shared/m/cardata/_Videos.cshtml", tuple);
            }
            catch (Exception ex)
            {

                Logger.LogException(ex, "ModelComponentController.ModelVideos()");
            }

            return new EmptyResult();

        }

        [Carwale.UI.Common.OutputCacheAttr("modelId;isAmp")]
        public ActionResult TopLatestNews(int modelId, CarModelDetails modelDetails = null, string page = "modelpage", bool isAmp = false)
        {
            try
            {
                if (modelDetails == null && _modelCacheRepo != null)
                {
                    modelDetails = _modelCacheRepo.GetModelDetailsById(modelId);
                }

                ArticleRecentURI queryString = new ArticleRecentURI()
                {
                    ApplicationId = (ushort)CMSAppId.Carwale,
                    ContentTypes = ((ushort)CMSContentType.News).ToString(),
                    TotalRecords = Convert.ToUInt16(ConfigurationManager.AppSettings["MobileNewsWidget_Count"]),
                    ModelId = modelId
                };

                List<ArticleSummary> news = _cmsContentCacheRepo.GetMostRecentArticles(queryString);

                ViewBag.MaskingName = modelDetails != null ? modelDetails.MaskingName : string.Empty;
                ViewBag.MakeName = modelDetails != null ? modelDetails.MakeName : string.Empty;
                ViewBag.Pagename = page;
                ViewBag.ModelName = modelDetails != null ? modelDetails.ModelName : string.Empty;
                if (isAmp)
                {
                    return PartialView("~/Views/Shared/m/AMP/_NewsCarouselAmp.cshtml", news);
                }
                return PartialView("~/views/shared/m/_TopLatestNews.cshtml", news);
            }
            catch (Exception ex)
            {

                Logger.LogException(ex, "ModelComponentController.TopLatestNews()");
            }

            return new EmptyResult();

        }

        [Carwale.UI.Common.OutputCacheAttr("modelId;isAmp"), Route("html-api/expertreview-widget/")]
        public ActionResult ExpertReviews(int modelId, CarModelDetails modelDetails = null, bool isAmp = false)
        {
            try
            {
                bool modelValid = !(modelDetails == null || modelDetails.MakeId < 1);
                if (!modelValid && modelId > 0 && _modelCacheRepo != null)
                {
                    modelDetails = _modelCacheRepo.GetModelDetailsById(modelId);
                    modelValid = true;
                }

                List<ArticleSummary> expertReviews = new List<ArticleSummary>();

                if (modelValid)
                {
                    var articleURI = new ArticleRecentURI()
                    {
                        ApplicationId = (ushort)CMSAppId.Carwale,
                        ContentTypes = Convert.ToInt16(CMSContentType.RoadTest).ToString(),
                        TotalRecords = Convert.ToUInt16(ConfigurationManager.AppSettings["ExpertReviewsForMobile_Count"]),
                        ModelId = modelId,
                    };
                    if (_cmsContentCacheRepo != null)
                    {
                        expertReviews = _cmsContentCacheRepo.GetMostRecentArticles(articleURI);
                    }
                    ViewBag.MaskingName = modelDetails != null ? modelDetails.MaskingName : string.Empty;
                    ViewBag.MakeName = modelDetails != null ? modelDetails.MakeName : string.Empty;
                    ViewBag.ModelName = modelDetails != null ? modelDetails.ModelName : string.Empty;
                }

                if (isAmp)
                    return PartialView("~/Views/Shared/m/AMP/_ExpertReviewWidgetAmp.cshtml", expertReviews);
                else
                    return PartialView("~/views/shared/m/cardata/_ExpertReviews.cshtml", expertReviews);
            }
            catch (Exception ex)
            {

                Logger.LogException(ex, "ModelComponentController.ExpertReviews()");
            }

            return new EmptyResult();
        }


        [Carwale.UI.Common.OutputCacheAttr("modelId;isAmp")]
        public ActionResult UserReviews(int modelId, CarModelDetails modelDetails = null, bool isAmp = false)
        {
            try
            {
                List<UserReviewEntity> reviews = GetUserReviews(modelId, modelDetails);
                if (isAmp)
                    return PartialView("~/Views/Shared/m/AMP/_UserReviewWidgetAmp.cshtml", reviews);
                else
                    return PartialView("~/views/shared/m/cardata/_UserReviews.cshtml", reviews);
            }
            catch (Exception ex)
            {

                Logger.LogException(ex, "ModelComponentController.UserReviews()");
            }

            return new EmptyResult();

        }

        public ActionResult UserReviewsVersion(int modelId, int versionId, CarModelDetails modelDetails = null)
        {
            try
            {
                List<UserReviewEntity> reviews = GetUserReviews(modelId, modelDetails, versionId);
                return PartialView("~/views/shared/m/cardata/_UserReviews.cshtml", reviews);
            }
            catch (Exception ex)
            {

                Logger.LogException(ex, "ModelComponentController.UserReviewsVersion()");
            }

            return new EmptyResult();

        }

        List<UserReviewEntity> GetUserReviews(int modelId, CarModelDetails modelDetails, int versionId = 0)
        {
            try
            {

                if (_modelCacheRepo != null)
                    modelDetails = _modelCacheRepo.GetModelDetailsById(modelId);
                if (modelDetails == null)
                {
                    modelDetails = new CarModelDetails();
                }
                int count = Convert.ToInt16(ConfigurationManager.AppSettings["ModelUserReviewsCount_Mobile"]);

                ViewBag.ModelName = modelDetails.ModelName ?? string.Empty;
                ViewBag.ModelId = modelId;
                ViewBag.MakeId = modelDetails.MakeId;
                ViewBag.IsVersionPage = versionId > 0;
                ViewBag.VersionId = versionId;
                return _userreviewCache.GetUserReviewsList(modelDetails.MakeId, modelDetails.ModelId, versionId, 0, count, (int)UserReviewsSorting.EntryDateTime);
            }
            catch (Exception ex)
            {

                Logger.LogException(ex, "ModelComponentController.GetUserReviews()");
            }
            
            return new List<UserReviewEntity>();

        }

        [Carwale.UI.Common.OutputCacheAttr("modelId;isAmp")]
        public ActionResult FullReviews(int modelId, string modelName, bool futuristic, bool isAmp = false)
        {
            try
            {
                CarSynopsisEntity carSynopsis = new CarSynopsisEntity();
                ViewBag.isUpcoming = futuristic;
                ViewBag.ModelName = modelName;
                if (_cmsContentCacheRepo != null)
                {
                    carSynopsis = _cmsContentCacheRepo.GetCarSynopsis(modelId, 1);
                }
                if (isAmp)
                {
                    if (carSynopsis != null && !string.IsNullOrEmpty(carSynopsis.Content) && !string.IsNullOrEmpty(carSynopsis.Description))
                    {
                        carSynopsis.Content = carSynopsis.Content.ConvertToAmpContent();
                        carSynopsis.Description = carSynopsis.Description.ConvertToAmpContent();
                    }
                    return PartialView("~/Views/Shared/m/AMP/_CarSynopsisWidgetAmp.cshtml", carSynopsis);
                }
                else
                    return PartialView("~/views/shared/m/cardata/_FullReview.cshtml", carSynopsis);
            }
            catch (Exception ex)
            {

                Logger.LogException(ex, "ModelComponentController.FullReviews()");
            }

            return new EmptyResult();

        }

        public ActionResult FullReviewsV1(int modelId, string modelName, bool isFuturistic, ArticlePageDetails articlePageDetails, bool showH2Tag = true)
        {
            try
            {
                if (articlePageDetails != null && articlePageDetails.PageList.IsNotNullOrEmpty())
                {
                    string headingOfSynposis = string.Format("{0} {1}", modelName, (isFuturistic ? "Preview" : "Review"));

                    CarSynopsisModelPage carSynopsis = new CarSynopsisModelPage {
                        PageList = articlePageDetails.PageList,
                        Description = articlePageDetails.Description,
                        IsFuturistic = isFuturistic,
                        Heading = headingOfSynposis,
                        BasicId = articlePageDetails.BasicId,
                        ModelName = modelName,
                        ShowH2Tag = showH2Tag,
                        ModelId = modelId,
                        IsExpertReview = (articlePageDetails.CategoryId == (int)CmsContentCategory.ExpertReview)
                    };
                    return PartialView("~/Views/Shared/m/cardata/_FullReviewV1.cshtml", carSynopsis);
                }
                return new EmptyResult();
            }
            catch (Exception ex)
            {

                Logger.LogException(ex, "ModelComponentController.FullReviewsV1()");
            }

            return new EmptyResult();

        }

        public ActionResult ModelsCarousel(int makeId, int cityId, string makeName, List<object> data = null, bool isMobile = true, bool isAmp = false)
        {
            try
            {
                List<CarModelSummary> NewCarModelsDetails = new List<CarModelSummary>();

                ICarMakes _carMakesBL = _unityContainer.Resolve<ICarMakes>();
                if (_carMakesBL != null)
                {
                    NewCarModelsDetails = _carMakesBL.GetNewCarModelsWithDetails(cityId, makeId, true);
                }
                if (NewCarModelsDetails.Count > 5)
                {
                    NewCarModelsDetails = NewCarModelsDetails.GetRange(0, 5);
                }
                ModelCarousel Model = new ModelCarousel() { Cars = AutoMapper.Mapper.Map<List<CarModelSummary>, List<CarModelSummaryDTOV2>>(NewCarModelsDetails), MakeName = makeName };
                if (isAmp)
                    return PartialView("~/Views/Shared/m/AMP/_ModelsCarouselAmp.cshtml", Model);
                else if (isMobile)
                    return PartialView("~/views/shared/m/cardata/_ModelsCarousel.cshtml", Model);
                else
                    return PartialView("~/Views/Shared/NewCars/_ModelsCarousel.cshtml", Model);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "ModelComponentController.ModelsCarousel()\n Exception : " + ex.Message);
                objErr.LogException();
            }
            return new EmptyResult();

        }

        public ActionResult ModelVariantsPrice(List<NewCarVersionsDTOV2> _newCarVersions, string makeName, string modelName,
            int widgetSource, string imgUrl = "", bool isMobile = true, string cityName = "")
        {
            try
            {
                if (_newCarVersions.IsNotNullOrEmpty())
                {
                    ViewBag.IsMobile = isMobile;
                    ViewBag.ModelSmallImgUrl = imgUrl;
                    ViewBag.MakeName = makeName;
                    ViewBag.ModelName = modelName;
                    ViewBag.CityName = cityName;
                    ViewBag.WidgetSource = widgetSource;
                    return PartialView("~/Views/Shared/NewCars/_ModelVariantsPrice.cshtml", _newCarVersions);
                }
            }
            catch (Exception ex)
            {

                Logger.LogException(ex, "ModelComponentController.ModelVariantsPrice()");
            }

            return new EmptyResult();
        }

        public ActionResult UpcomingModels(int makeId, string makeName, string pageName = "makepage", int modelId = 0, bool isAmp = false, int cityId = 0)
        {
            try
            {
                List<UpcomingCarModel> upcomingModels = new List<UpcomingCarModel>();
                ViewBag.MakeName = makeName;
                ViewBag.Pagename = pageName;
                if (cityId > 0) ViewBag.CityId = cityId;
                if (_modelCacheRepo != null)
                {
                    upcomingModels = _modelCacheRepo.GetUpcomingCarModelsByMake(makeId).Where(x => !x.ModelId.Equals(modelId)).Take(Convert.ToInt16(ConfigurationManager.AppSettings["UpcomingCarsCount_Mobile"])).ToList();
                }
                if (isAmp)
                {
                    return PartialView("~/Views/Shared/m/AMP/_UpcomingCarCarouselAmp.cshtml", upcomingModels);
                }
                return PartialView("~/views/shared/m/_UpcomingCarousel.cshtml", upcomingModels);
            }
            catch (Exception ex)
            {

                Logger.LogException(ex, "ModelComponentController.UpcomingModels()");
            }

            return new EmptyResult();

        }


        [Carwale.UI.Common.OutputCacheAttr("makeId;isAmp")]
        public ActionResult Description(int makeId, string makeName, bool isAmp = false)
        {
            try
            {
                CarMakeDescription makeDesc = new CarMakeDescription();
                ICarMakesCacheRepository _carMakesCacheRepo = _unityContainer.Resolve<ICarMakesCacheRepository>();
                ViewBag.MakeName = makeName;
                if (_carMakesCacheRepo != null)
                {
                    makeDesc = _carMakesCacheRepo.GetCarMakeDescription(makeId);
                }
                if (isAmp)
                {
                    return PartialView("~/Views/Shared/m/AMP/_MakeDescriptionAMP.cshtml", makeDesc);
                }
                return PartialView("~/views/shared/m/CarData/_MakeDescription.cshtml", makeDesc);
            }
            catch (Exception ex)
            {

                Logger.LogException(ex, "ModelComponentController.Description()");
            }

            return new EmptyResult();

        }

        [Carwale.UI.Common.OutputCacheAttr("makeId;isAmp")]
        public ActionResult TopLatestNewsForMake(int makeId, string makeName, string pageName = "makepage", bool isAmp = false)
        {
            try
            {
                ICMSContent _cmsContentCacheRepo = _unityContainer.Resolve<ICMSContent>();
                ViewBag.MakeName = makeName;
                ViewBag.Pagename = pageName;
                var newsURI = new ArticleRecentURI()
                {
                    ApplicationId = (ushort)CMSAppId.Carwale,
                    ContentTypes = CMSContentType.News.ToString("D"),
                    TotalRecords = Convert.ToUInt16(ConfigurationManager.AppSettings["MakeNewsCount_Mobile"]),
                    MakeId = makeId,
                };
                List<ArticleSummary> news = new List<ArticleSummary>();
                if (_cmsContentCacheRepo != null)
                {
                    news = _cmsContentCacheRepo.GetMostRecentArticles(newsURI);
                }
                foreach (var x in news)
                {
                    x.FormattedDisplayDate = x.DisplayDate.ConvertDateToDays();
                }

                if (isAmp)
                {
                    return PartialView("~/Views/Shared/m/AMP/_NewsCarouselAmp.cshtml", news);
                }
                return PartialView("~/views/shared/m/_TopLatestNews.cshtml", news);
            }
            catch (Exception ex)
            {

                Logger.LogException(ex, "ModelComponentController.TopLatestNewsForMake()");
            }

            return new EmptyResult();

        }

        public ActionResult MetaData(PageMetaTags obj)
        {
            try
            {
                if (obj != null)
                {
                    return PartialView("~/views/shared/m/_MetaData.cshtml", obj);
                }
            }
            catch (Exception ex)
            {

                Logger.LogException(ex, "ModelComponentController.MetaData()");
            }

            return new EmptyResult();

        }

        public ActionResult PriceOverview(CarOverviewDTOV2 overview, bool isPicPage, string cityName = "", string cityMaskingName = "", string cityZone = "", string pricingEventAction = "", string pricingEventLabel = "", bool isAmp = false, string campaignDealerId = "0")
        {
            try
            {
                ViewBag.IsCityPage = isPicPage;
                ViewBag.CityName = cityName;
                ViewBag.CityZone = cityZone;
                ViewBag.PricingEventAction = pricingEventAction;
                ViewBag.PricingEventLabel = pricingEventLabel;
                ViewBag.NonAmpUrl = ManageCarUrl.CreatePriceInCityUrl(overview.MakeName, overview.MaskingName, cityMaskingName);
                overview.CampaignDealerId = campaignDealerId;
                if (isPicPage)
                {
                    CarPriceQuote carPriceQuote = new CarPriceQuote();
                    if (_prices != null)
                    {
                        carPriceQuote = _prices.GetModelCompulsoryPrices(overview.ModelId, overview.UserLocation.CityId, overview.New, true);
                    }
                    overview.VersionPriceQuote = carPriceQuote != null ? carPriceQuote
                                                                   .VersionPricesList.OrderBy(x => x.IsMetallic)
                                                                   .ToList()
                                                                   .Find(x => x.VersionId.Equals(overview.VersionId) && x.PricesList.Count > 0 && x.PricesList[0].PQItemId > 0) ?? new VersionPriceQuote() : new VersionPriceQuote();
                    if (isAmp)
                    {
                        return PartialView("~/views/shared/m/AMP/_PICPricebreakupAMP.cshtml", overview);
                    }
                    else
                    {
                        return PartialView(ProductExperiments.IsPicPqMerger(overview.MakeId) && (overview.New) ? "~/views/shared/m/cardata/_PICPricebreakUpExperiment.cshtml" : "~/views/shared/m/cardata/_PICPriceBreakUp.cshtml", overview);
                    }
                }
                else
                {
                    return PartialView("~/views/shared/m/cardata/_PriceOverview.cshtml", overview);
                }
            }
            catch (Exception ex)
            {

                Logger.LogException(ex, "ModelComponentController.PriceOverview()");
            }

            return new EmptyResult();
        }

        public ActionResult ModelDataSummary(int modelId, string modelName, ModelDataSummary modelData)
        {
            try
            {
                if (modelData != null)
                {
                    modelData.ModelName = modelName;
                    modelData.ModelId = modelId;
                    if (modelData.ModelFeature.IsNotNullOrEmpty())
                    {
                        return PartialView("~/Views/Shared/NewCars/_ModelFeatures.cshtml", modelData);
                    }
                    else if (modelData.SpecsSummary.IsNotNullOrEmpty())
                    {
                        return PartialView("~/Views/Shared/NewCars/_ModelSpecsSummary.cshtml", modelData);
                    }
                }
            }
            catch (Exception ex)
            {

                Logger.LogException(ex, "ModelComponentController.MetaData()");
            }

            return new EmptyResult();

        }

        [Carwale.UI.Common.OutputCacheAttr("id;page")]
        public ActionResult VideosByCarId(int id, ContentPages page, List<Video> videos)
        {
            try
            {

                if (videos.IsNotNullOrEmpty())
                {
                    VideosCarousel videosCarousel = new VideosCarousel();
                    videosCarousel.Videos = videos;
                    videosCarousel.Page = page;
                    if (page == ContentPages.MakePage)
                    {
                        videosCarousel.ViewAllVideoUrl = string.Format("{0}videos/", ManageCarUrl.CreateMakeUrl(videos[0].MakeName, true));
                        videosCarousel.CarName = videos[0].MakeName;
                        videosCarousel.Title = string.Format("{0} Video{1}", videosCarousel.CarName, (videos.Count > 1 ? "s" : ""));
                    }
                    else if (page == ContentPages.ModelPage)
                    {
                        videosCarousel.ViewAllVideoUrl = string.Format("/m/{0}-cars/{1}/videos/", CommonOpn.FormatSpecial(videos[0].MakeName), videos[0].MaskingName);
                        videosCarousel.CarName = string.Format("{0} {1}", videos[0].MakeName, videos[0].ModelName);
                        videosCarousel.Title = string.Format("{0} Video Review", videosCarousel.CarName);
                        videosCarousel.ModelName = videos[0].ModelName;
                    }
                    return PartialView("~/Views/Shared/m/AMP/_VideoCarouselAmp.cshtml", videosCarousel);

                }
            }
            catch (Exception ex)
            {

                Logger.LogException(ex, "ModelComponentController.VideosById()");
            }

            return new EmptyResult();
        }
    }
}