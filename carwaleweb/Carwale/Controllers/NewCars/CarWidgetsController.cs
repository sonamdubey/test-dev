using Carwale.Entity;
using Carwale.Entity.CarData;
using Carwale.Entity.CMS;
using Carwale.Entity.CMS.Articles;
using Carwale.Entity.CMS.URIs;
using Carwale.Entity.ViewModels;
using Carwale.Interfaces;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.CMS;
using Carwale.Interfaces.CMS.Articles;
using Carwale.Interfaces.SponsoredCar;
using Carwale.Interfaces.CMS.UserReviews;
using Carwale.DTOs.WidgetDTOs;
using Carwale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Carwale.Entity.CMS.UserReviews;
using Carwale.Entity.Enum;
using Carwale.UI.Common;
using Carwale.Interfaces.Classified;
using Carwale.Entity.Classified;
using Carwale.Entity.Dealers;
using Carwale.Interfaces.Dealers;
using Carwale.DTOs.NewCars;
using System.Diagnostics;
using log4net;
using Carwale.Notifications;
using EditCMSWindowsService.Messages;
using Grpc.CMS;
using Carwale.BL.GrpcFiles;
using ApiGatewayLibrary;
using Carwale.DTOs.CarData;
using Google.Protobuf;
using Carwale.Notifications.Logs;
using AEPLCore.Utils.Serializer;
using Carwale.UI.ViewModels.NewCars;
using Carwale.Entity.ViewModels.CarData;
using Carwale.DAL.ApiGateway;
using Carwale.Adapters.Specs;

namespace Carwale.UI.Controllers.NewCars
{
    public class CarWidgetsController : Controller
    {
        private readonly IUnityContainer _container;
        private static readonly ushort ExpertReviewCount = Convert.ToUInt16(ConfigurationManager.AppSettings["ExpertReviewsDesktop_Count"] ?? "5");
        private static readonly string ExpertReviewCategory = Convert.ToInt16(CMSContentType.RoadTest).ToString();
        private static readonly ushort MakeNewsCount_Mobile = Convert.ToUInt16(ConfigurationManager.AppSettings["MakeNewsCount_Mobile"] ?? "5");
        private static readonly string NewsCategory = (Convert.ToInt16(CMSContentType.News)).ToString();
        private static readonly int UpcomingCarsCountMobile = Convert.ToInt16(ConfigurationManager.AppSettings["UpcomingCarsCount_Mobile"] ?? "5");
        private static readonly ILog _logger = LogManager.GetLogger(typeof(CarWidgetsController));
        private static readonly bool useAPIGateway = ConfigurationManager.AppSettings["useApiGateway"] == "true";
        private readonly static ushort _similarUpcomingCarCount = Convert.ToUInt16(ConfigurationManager.AppSettings["SimilarUpcomingCarCount"] ?? "5");
        private static Dictionary<int, List<String>> compatableMakes = new Dictionary<int, List<string>>
            {
                {7,new List<string>{"Tata","Hyundai","Maruti Suzuki","Mahindra"}},
                {8,new List<string>{"Tata","Maruti Suzuki","Honda","Mahindra"}},
                {9,new List<string>{"Tata","Hyundai","Honda","Maruti Suzuki"}},
                {10,new List<string>{"Tata","Hyundai","Honda","Mahindra"}},
                {16,new List<string>{"Maruti Suzuki","Hyundai","Honda","Mahindra"}},

                {4,new List<string>{"Volkswagen","Ford","Skoda","Renault"}},
                {5,new List<string>{"Volkswagen","Skoda","Fiat","Renault"}},
                {15,new List<string>{"Volkswagen","Ford","Fiat","Renault"}},
                {20,new List<string>{"Skoda","Ford","Fiat","Renault"}},
                {45,new List<string>{"Volkswagen","Ford","Fiat","Skoda"}},

                {1,new List<string>{"Audi","Mercedes-Benz","Jeep"}},
                {11,new List<string>{"Audi","BMW","Jeep"}},
                {18,new List<string>{"BMW","Mercedes-Benz","Jeep"}},
                {43,new List<string>{"Audi","Mercedes-Benz","BMW"}},

                {37,new List<string>{"Jaguar","Mini"}},
                {44,new List<string>{"Volvo","Mini"}},
                {51,new List<string>{"Jaguar","Volvo"}}
            };
        public CarWidgetsController(IUnityContainer container)
        {
            _container = container;
        }
        // GET: CarWidgets
        [Carwale.UI.Common.OutputCacheAttr("pageId", Duration = 86400)]
        public ActionResult NewCarSearchWidget(int pageId)
        {
            ViewBag.PageId = pageId;
            return PartialView("~/Views/Shared/NewCars/_NewCarSearchWidget.cshtml");
        }

        [Carwale.UI.Common.OutputCacheAttr("pageNo")]
        public ActionResult UpcomingCarsHomeScreen(ushort pageNo, ushort pageSize = 9)
        {
            var page = new Pagination() { PageNo = pageNo, PageSize = pageSize };
            var _models = _container.Resolve<ICarModels>();
            List<UpcomingCarModel> upcomingModels = _models.GetUpcomingCarModels(page);
            if (upcomingModels != null && upcomingModels.Count > 0)
                return PartialView("~/Views/Shared/NewCars/_UpcomingCarsCards.cshtml", upcomingModels);
            return new EmptyResult();
        }

        public ActionResult PopularCars(int cityId, ushort pageNo, ushort pageSize = 9)
        {
            var page = new Pagination() { PageNo = pageNo, PageSize = pageSize };
            var _models = _container.Resolve<ICarModels>();
            var _sponsoredCars = _container.Resolve<ISponsoredCarCache>();
            TopSellingModel topSellingModels = new TopSellingModel()
            {
                TopSelling = _models.GetTopSellingCarModels(page, cityId, true),
                SponsoredPopularCars = _sponsoredCars.GetSponsoredCampaigns((int)Carwale.Entity.Enum.CampaignCategory.PopularCars, (int)Carwale.Entity.Enum.Platform.CarwaleDesktop)
            };
            if (topSellingModels.TopSelling != null && topSellingModels.TopSelling.Count > 0)
                return PartialView("~/Views/Shared/NewCars/_TopSellingCarsCards.cshtml", topSellingModels);
            return new EmptyResult();
        }

        public ActionResult JustLaunchedCars(int cityId, ushort pageNo, ushort pageSize = 9)
        {
            var page = new Pagination() { PageNo = pageNo, PageSize = pageSize };
            var _models = _container.Resolve<ICarModels>();
            List<Entity.CarData.LaunchedCarModel> newLaunches = _models.GetLaunchedCarModelsV1(page, cityId, true);
            if (newLaunches != null && newLaunches.Count > 0)
                return PartialView("~/Views/Shared/NewCars/_TopNewLaunchesCards.cshtml", newLaunches);
            return new EmptyResult();
        }

        [Carwale.UI.Common.OutputCacheAttr("pageId")]
        public ActionResult EditorialWidget(ushort pageId)
        {
            try
            {
                EditorialWidgetSummary editorialWidgetSummary = new EditorialWidgetSummary();
                var _cmsContentCacheRepo = _container.Resolve<ICMSContent>();
                var _carModelVideos = _container.Resolve<IVideosBL>();
                ArticleRecentURI qs;

                qs = new ArticleRecentURI() { ApplicationId = (ushort)CMSAppId.Carwale, ContentTypes = "1,22", TotalRecords = 2 };
                ArticleSummary sponsoredArticle = null;
                if (useAPIGateway)
                {
                    KeyValuePair<string, IMessage>[] calls = {
                        new KeyValuePair<string, IMessage>("GetMostRecentArticles", new GrpcArticleRecentURI()
                        {
                            ApplicationId = qs.ApplicationId,
                            ContentTypes = qs.ContentTypes,
                            TotalRecords = (uint)qs.TotalRecords
                        }),
                    new KeyValuePair<string, IMessage>("GetSponsoredArticle", new GetSponsoredArticleURI()
                    {
                        CategoryList = qs.ContentTypes,
                        Author = CWConfiguration.SponsoredAuthorId
                    }),
                    new KeyValuePair<string, IMessage>("GetMostRecentArticles", new GrpcArticleRecentURI()
                    {
                        ApplicationId = qs.ApplicationId,
                        ContentTypes = "8",
                        TotalRecords = (uint)qs.TotalRecords
                    }),
                    new KeyValuePair<string, IMessage>("GetNewModelsVideosBySubCategory", new GrpcVideosBySubCategoryURI()
                    {
                        ApplicationId = Convert.ToUInt32(CMSAppId.Carwale),
                        SubCategoryId = Convert.ToUInt32(EnumVideoCategory.FeaturedAndLatest),
                        StartIndex = 1,
                        EndIndex = 2
                    }) };
                    var result = GrpcMethods.GetDataFromGateway(calls);
                    editorialWidgetSummary.TopNews = GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(Serializer.ConvertBytesToMsg<GrpcArticleSummaryList>(result.OutputMessages[0].Payload));
                    sponsoredArticle = GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(Serializer.ConvertBytesToMsg<GrpcArticleSummary>(result.OutputMessages[1].Payload));
                    editorialWidgetSummary.TopReviews = GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(Serializer.ConvertBytesToMsg<GrpcArticleSummaryList>(result.OutputMessages[2].Payload));
                    editorialWidgetSummary.TopVideos = GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(Serializer.ConvertBytesToMsg<GrpcVideosList>(result.OutputMessages[3].Payload));
                    editorialWidgetSummary.TopVideos = _carModelVideos.GetVideosBySubCategory(editorialWidgetSummary.TopVideos, 1, 2);
                }
                else
                {
                    editorialWidgetSummary.TopNews = _cmsContentCacheRepo.GetMostRecentArticles(qs);
                    sponsoredArticle = _cmsContentCacheRepo.GetSponsoredArticle(qs.ContentTypes, CWConfiguration.SponsoredAuthorId);
                    qs = new ArticleRecentURI() { ApplicationId = (ushort)CMSAppId.Carwale, ContentTypes = "8", TotalRecords = 2 };
                    editorialWidgetSummary.TopReviews = _cmsContentCacheRepo.GetMostRecentArticles(qs);
                    editorialWidgetSummary.TopVideos = _carModelVideos.GetNewModelsVideosBySubCategory(EnumVideoCategory.FeaturedAndLatest, CMSAppId.Carwale, 1, 2);
                }
                if (sponsoredArticle != null && sponsoredArticle.BasicId > 0 && editorialWidgetSummary.TopNews != null)
                {
                    editorialWidgetSummary.TopNews.Insert(0, sponsoredArticle);
                }
                return PartialView("~/Views/Shared/FRQ/_EditorialWidget.cshtml", editorialWidgetSummary);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "EditorialWidget Exception");
                objErr.LogException();
            }
            return new EmptyResult();
        }

        [Carwale.UI.Common.OutputCacheAttr("makeId")]
        public ActionResult MakeDescription(int makeId, string makeName)
        {
            ViewBag.MakeName = makeName;
            var _carMakesCacheRepo = _container.Resolve<ICarMakesCacheRepository>();
            CarMakeDescription makeDesc = _carMakesCacheRepo.GetCarMakeDescription(makeId);
            if (makeDesc != null)
                return PartialView("~/views/shared/m/CarData/_MakeDescription.cshtml", makeDesc);
            return new EmptyResult();
        }
        [Carwale.UI.Common.OutputCacheAttr("makeId;modelId")]
        public ActionResult ExpertReview(int makeId, string makeName, int modelId = 0, string modelName = "", string maskingName = "", List<ArticleSummary> expertReviews = null)
        {
            if (expertReviews == null || expertReviews.Count <= 0)
            {
                var articleURI = new ArticleRecentURI()
                {
                    ApplicationId = (ushort)CMSAppId.Carwale,
                    ContentTypes = ExpertReviewCategory,
                    TotalRecords = ExpertReviewCount,
                    ModelId = modelId,
                    MakeId = makeId
                };
                var _cmsContentCacheRepo = _container.Resolve<ICMSContent>();
                expertReviews = _cmsContentCacheRepo.GetMostRecentArticles(articleURI) ?? new List<ArticleSummary>();
            }
            foreach (var x in expertReviews)
            {
                x.FormattedDisplayDate = x.DisplayDate.ConvertDateToDays();
            }
            if (expertReviews != null && expertReviews.Count > 0)
            {
                ArticlesWidget widgetExpertReviews = new ArticlesWidget()
                {
                    Articles = expertReviews,
                    LandingUrl = string.Format("/{0}-cars/{1}expert-reviews/", Carwale.UI.Common.UrlRewrite.FormatSpecial(makeName ?? ""), (modelId > 0 ? maskingName + "/" : "")),
                    Heading = string.Format("{0}{1} Expert Reviews", makeName, (modelId > 0 ? " " + modelName : ""))
                };
                ViewBag.ModelName = modelName;
                ViewBag.MakeName = makeName;
                return PartialView("~/Views/Shared/NewCars/_ExpertReviews.cshtml", widgetExpertReviews);
            }
            return new EmptyResult();
        }
        [Carwale.UI.Common.OutputCacheAttr("makeId;modelId")]
        public ActionResult TopLatestNews(int makeId, string makeName, int modelId = 0, string modelName = "", string maskingName = "")
        {
            ViewBag.MakeName = makeName;
            ViewBag.ModelName = modelName;
            var newsURI = new ArticleRecentURI()
            {
                ApplicationId = (ushort)CMSAppId.Carwale,
                ContentTypes = NewsCategory,
                TotalRecords = MakeNewsCount_Mobile,
                MakeId = makeId,
                ModelId = modelId
            };
            var _cmsContentCacheRepo = _container.Resolve<ICMSContent>();
            List<ArticleSummary> news = _cmsContentCacheRepo.GetMostRecentArticles(newsURI);
            foreach (var x in news)
            {
                x.FormattedDisplayDate = x.DisplayDate.ConvertDateToDays();
            }
            if (news != null && news.Count > 0)
            {
                ArticlesWidget widgetnews = new ArticlesWidget()
                {
                    Articles = news,
                    LandingUrl = string.Format("/{0}-cars/{1}news/", Carwale.UI.Common.UrlRewrite.FormatSpecial(makeName ?? ""), (modelId > 0 ? maskingName + "/" : "")),
                    Heading = string.Format("{0}{1} News", makeName, (modelId > 0 ? " " + modelName : ""))
                };
                return PartialView("~/Views/Shared/NewCars/_CarNews.cshtml", widgetnews);
            }
            return new EmptyResult();
        }
        [Carwale.UI.Common.OutputCacheAttr("makeId;modelId")]
        public ActionResult UserReview(int makeId, string makeName, int modelId = 0, string modelName = "", string maskingName = "")
        {
            try
            {
                var _userreviewCache = _container.Resolve<IUserReviewsCache>();
                List<UserReviewEntity> userReviewsList = _userreviewCache.GetUserReviewsList(makeId, (modelId > 0 ? modelId : 0), 0, 0, 5, (int)UserReviewsSorting.EntryDateTime);
                if (userReviewsList != null && userReviewsList.Count > 0)
                {
                    ArticlesWidget widgetUserReviews = new ArticlesWidget()
                    {
                        UserReviewList = userReviewsList,
                        LandingUrl = modelId > 0 ? string.Format("/{0}-cars/{1}/userreviews/", UrlRewrite.FormatSpecial(makeName ?? ""), maskingName) : "",
                        Heading = string.Format("{0}{1} User Reviews", makeName, (modelId > 0 ? " " + modelName : ""))
                    };
                    return PartialView("~/Views/Shared/NewCars/_UsersReviews.cshtml", widgetUserReviews);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "UserReview render action");
            }
            return new EmptyResult();
        }
        public ActionResult UpcomingCars(int makeId, string makeName, int modelId = -1)
        {
            ViewBag.MakeName = makeName;
            var _carModelsCacheRepo = _container.Resolve<ICarModelCacheRepository>();
            List<UpcomingCarModel> upcomingModels = _carModelsCacheRepo.GetUpcomingCarModelsByMake(makeId)?.Take(UpcomingCarsCountMobile)?.ToList();
            if (upcomingModels != null && upcomingModels.Count > 0)
                upcomingModels = upcomingModels.Where(x => x.ModelId != modelId).ToList();
            if (upcomingModels != null && upcomingModels.Count > 0)
                return PartialView("~/Views/Shared/NewCars/_UpcomingCars.cshtml", new UpcomingCarWidget { MakeId = makeId, UpcomingCarsList = upcomingModels });
            return new EmptyResult();
        }
        [Carwale.UI.Common.OutputCacheAttr("makeId;modelId")]
        public ActionResult UsedLuxuryCars(int makeId, int modelId = 0)
        {
            var _classifiedListing = _container.Resolve<IClassifiedListing>();
            List<StockSummary> usedLuxuryCars = _classifiedListing.GetLuxuryCarRecommendations(modelId > 0 ? modelId : makeId, 3849, modelId > 0 ? 2 : 1);
            if (usedLuxuryCars != null && usedLuxuryCars.Count > 0)
                return PartialView("~/Views/Shared/NewCars/_UsedLuxuryCarView.cshtml", usedLuxuryCars);
            return new EmptyResult();
        }

        [Carwale.UI.Common.OutputCacheAttr("makeId")]
        public ActionResult LocateDealer(int makeId, string makeName)
        {
            ViewBag.MakeName = makeName;
            var _newCarDealersBL = _container.Resolve<INewCarDealers>();
            List<DealerCityEntity> LocateDealerCities = _newCarDealersBL.GetStatesAndCitiesByMake(makeId).SelectMany<DealerStateEntity, DealerCityEntity>(state => state.cities).OrderBy(city => city.CityName).ToList();
            if (LocateDealerCities != null && LocateDealerCities.Count > 0)
                return PartialView("~/Views/Shared/NewCars/MakePage/_DealerCityDropdown.cshtml", LocateDealerCities);
            return new EmptyResult();
        }

        [Carwale.UI.Common.OutputCacheAttr("makeId")]
        public ActionResult CarBuyerTool(int makeId, string makeName)
        {
            ViewBag.MakeName = makeName;
            return PartialView("~/Views/Shared/NewCars/_CarBuyersTool.cshtml");
        }

        [Carwale.UI.Common.OutputCacheAttr("modelId")]
        public ActionResult ModelSynopsis(int modelId, bool isFuturistic = false, string modelName = "", string makeName = "")
        {
            ICMSContent _cmsContentCacheRepo = _container.Resolve<ICMSContent>();
            CarSynopsisEntity reviews = _cmsContentCacheRepo.GetCarSynopsis(modelId, (int)Application.CarWale);
            var modelSynopsis = reviews != null ? (new ModelReview() { FullReview = reviews.Content, SmallReview = reviews.Description }) : new ModelReview();
            ViewBag.IsFuturistic = isFuturistic;
            ViewBag.ModelName = makeName + " " + modelName;
            if (!string.IsNullOrEmpty(modelSynopsis.SmallReview) && !string.IsNullOrEmpty(modelSynopsis.FullReview))
                return PartialView("~/Views/Shared/NewCars/_ModelSynopsis.cshtml", modelSynopsis);
            return new EmptyResult();
        }

        public ActionResult ModelSynopsisV1(int modelId, bool isFuturistic, string modelName, string makeName, ArticlePageDetails expertReview)
        {
            if (expertReview != null && expertReview.PageList.IsNotNullOrEmpty())
            {
                string headingOfSynposis = makeName + " " + modelName + " " + (isFuturistic ? "Preview" : "Review");
                CarSynopsisModelPage carSynopsis = new CarSynopsisModelPage {
                    PageList = expertReview.PageList,
                    Description = expertReview.Description,
                    IsFuturistic = isFuturistic,
                    Heading = headingOfSynposis,
                    BasicId = expertReview.BasicId,
                    ModelName = modelName,
                    ModelId = modelId,
                    IsExpertReview = (expertReview.CategoryId==8)
                };
                return PartialView("~/Views/Shared/NewCars/_ModelSynopsisv1.cshtml", carSynopsis);
            }
            return new EmptyResult();
        }

        [Carwale.UI.Common.OutputCacheAttr("modelId")]
        public ActionResult ModelColours(int modelId, CarModelDetails modelDetails = null, List<Carwale.Entity.CarData.ModelColors> modelcolor = null)
        {
            var _carModelsCacheRepo = _container.Resolve<ICarModelCacheRepository>();
            if (modelDetails == null) modelDetails = _carModelsCacheRepo.GetModelDetailsById(modelId) ?? new CarModelDetails();
            ViewBag.ModelName = modelDetails.MakeName + " " + modelDetails.ModelName;
            if (modelcolor == null) modelcolor = _carModelsCacheRepo.GetModelColorsByModel(modelId);
            if (modelcolor != null && modelcolor.Count > 0) return PartialView("~/Views/Shared/NewCars/_ModelColors.cshtml", new Tuple<CarModelDetails, List<ModelColors>>(modelDetails, modelcolor));
            return new EmptyResult();
        }

        [Carwale.UI.Common.OutputCacheAttr("modelId;page")]
        public ActionResult ModelVideos(int modelId, ContentPages page, List<Video> modelVideos = null, CarModelDetails modelDetails = null)
        {
            if (modelDetails == null)
            {
                modelDetails = _container.Resolve<ICarModelCacheRepository>().GetModelDetailsById(modelId) ?? new CarModelDetails();
            }
            if (modelVideos == null)
            {
                modelVideos = _container.Resolve<IVideosBL>().GetVideosByModelId(modelId, CMSAppId.Carwale, 1, -1);
            }
            ViewBag.MakeName = modelDetails.MakeName;
            ViewBag.ModelName = modelDetails.ModelName;
            ViewBag.MaskingName = modelDetails.MaskingName;
            ViewBag.GalleryObject = Newtonsoft.Json.JsonConvert.SerializeObject(new { modelId = modelId, makeId = modelDetails.MakeId, makeName = modelDetails.MakeName, modelName = modelDetails.ModelName, isNew = modelDetails.New, isFuturistic = modelDetails.Futuristic, modelMaskingName = modelDetails.MaskingName });

            Tuple<List<Carwale.Entity.CMS.Video>, ContentPages> modelVideosTuple = new Tuple<List<Carwale.Entity.CMS.Video>, ContentPages>(modelVideos, page);

            if (modelVideos != null && modelVideos.Count > 0)
                return PartialView("~/Views/Shared/NewCars/_ModelVideos.cshtml", modelVideosTuple);
            return new EmptyResult();
        }
        [Carwale.UI.Common.OutputCacheAttr("modelId")]
        public ActionResult UsedCarRecentListing(int modelId, string modelName)
        {
            IClassifiedListing _classifiedListing = _container.Resolve<IClassifiedListing>();
            ViewBag.Modelname = modelName;
            List<StockSummary> usedCarSuggestions = _classifiedListing.GetSimilarUsedModels(modelId);
            if (usedCarSuggestions != null && usedCarSuggestions.Count > 0)
                return PartialView("~/Views/Shared/NewCars/_UsedCarsRecentListing.cshtml", usedCarSuggestions);
            return new EmptyResult();
        }
        public ActionResult GetActiveModelCarousel(int makeId, string makeName, int cityId = -1, string cityName = "", string cityMaskingName = "")
        {
            List<CarModelSummary> modelListWithDetails = _container.Resolve<ICarMakes>().GetNewCarModelsWithDetails(cityId, makeId, true);
            if (modelListWithDetails != null && modelListWithDetails.Count > 0)
            {
                ViewBag.CityName = cityName;
                ViewBag.MakeName = makeName;
                ViewBag.CityMaskingName = cityMaskingName;
                return PartialView("~/Views/Shared/NewCars/_ActiveModelCarousel.cshtml", modelListWithDetails);
            }
            return new EmptyResult();
        }

        [Carwale.UI.Common.OutputCacheAttr("currentBodyStyle;isMobile")]
        public ActionResult TopCarsByBodyTypeCarousel(CarBodyStyle currentBodyStyle = 0, bool isMobile = false)
        {
            ICarModelCacheRepository carModelsCacheRepo = _container.Resolve<ICarModelCacheRepository>();
            Dictionary<CarBodyStyle, Tuple<int[], string>> carRanksByBodyType = carModelsCacheRepo
                                                     .GetCarRanksByBodyType(ConfigurationManager.AppSettings["BestCarsBodyTypes"] ?? "1,3,6,10", CWConfiguration.TopCarByBodyTypeCount);

            if (carRanksByBodyType != null && carRanksByBodyType.Count > 0)
            {
                if ((int)currentBodyStyle > 0)
                {
                    carRanksByBodyType.Remove(currentBodyStyle);
                }
                return PartialView(isMobile ? "~/Views/Shared/m/_MTopCarsByBodyTypeCarousel.cshtml" : "~/Views/Shared/FRQ/_TopCarsByBodyTypeCarousel.cshtml", carRanksByBodyType);
            }
            return new EmptyResult();
        }

        public ActionResult PriceInOtherCities(int cityId, string cityName, CarModelDetails modelDetails = null, ICollection<int> excludedCities = null, int modelId = 0, bool isMobile = false, bool isAmp = false)
        {
            ICarModels carModelsBl = _container.Resolve<ICarModels>();
            ICarModelCacheRepository carModelsCacheRepo = _container.Resolve<ICarModelCacheRepository>();
            PriceInOtherCitiesDTO priceInOtherCitiesDto = new PriceInOtherCitiesDTO();
            priceInOtherCitiesDto.CityId = cityId;
            priceInOtherCitiesDto.CityName = cityName;
            if (modelDetails != null && modelDetails.MakeId > 0)
            {
                priceInOtherCitiesDto.ModelDetails = modelDetails;
            }
            else
            {
                priceInOtherCitiesDto.ModelDetails = carModelsCacheRepo.GetModelDetailsById(modelId);
            }

            if (priceInOtherCitiesDto.ModelDetails != null && priceInOtherCitiesDto.ModelDetails.New)
            {
                priceInOtherCitiesDto.PriceInOtherCities = carModelsBl.GetPricesInOtherCities(priceInOtherCitiesDto.ModelDetails.ModelId, cityId);
            }

            if (priceInOtherCitiesDto.PriceInOtherCities.IsNotNullOrEmpty())
            {
                if (excludedCities.IsNotNullOrEmpty())
                {
                    priceInOtherCitiesDto.PriceInOtherCities.RemoveAll(x => excludedCities.Contains(x.CityId));
                }

                if (isAmp)
                {
                    return PartialView("~/Views/Shared/m/AMP/_PriceInOtherCityWidgetAmp.cshtml", priceInOtherCitiesDto);
                }
                else if (isMobile)
                {
                    return PartialView("~/Views/Shared/m/CarData/_MModelPriceInOtherCities.cshtml", priceInOtherCitiesDto);
                }
                else
                {
                    return PartialView("~/Views/Shared/NewCars/_ModelPriceInOtherCities.cshtml", priceInOtherCitiesDto);
                }
            }
            return new EmptyResult();
        }

        public ActionResult SimilarUpcomingCars(int makeId, int modelId, DateTime launchDate = default(DateTime), double expectedPrice = 0, bool isMobile = false, bool isAmp = false)
        {
            ICarModels _carModelsBl = _container.Resolve<ICarModels>();
            List<UpcomingCarModel> similarUpcomingCars = _carModelsBl.GetSimilarUpcomingCars(modelId, launchDate, expectedPrice, _similarUpcomingCarCount);
            if (similarUpcomingCars != null && similarUpcomingCars.Count > 0)
            {
                ViewBag.UpcomingSimilarCars = true;
                if (isAmp)
                {
                    return PartialView("~/Views/Shared/m/AMP/_UpcomingCarCarouselAmp.cshtml", similarUpcomingCars);
                }
                else if (isMobile)
                {
                    return PartialView("~/views/shared/m/_UpcomingCarousel.cshtml", similarUpcomingCars);
                }
                else
                {
                    return PartialView("~/Views/Shared/NewCars/_UpcomingCars.cshtml", new UpcomingCarWidget { MakeId = makeId, UpcomingCarsList = similarUpcomingCars });
                }
            }
            return new EmptyResult();
        }

        public ActionResult Footer()
        {
            try
            {
                return PartialView("~/Views/Shared/_Footer.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return new EmptyResult();
        }
        public ActionResult AllMakesWidget(int excludeMakeId = 0, bool isMobile = false, bool isAmp = false)
        {
            ViewBag.IsMobile = isMobile;
            List<string> makes = null;
            if (excludeMakeId != 0 && compatableMakes.ContainsKey(excludeMakeId))
            {
                makes = compatableMakes[excludeMakeId];
            }
            else
            {
                var _makes = _container.Resolve<ICarMakesCacheRepository>();
                List<CarMakeEntityBase> allMakeList = _makes.GetCarMakesByType("new");
                makes = allMakeList.Where(make => make.MakeId != excludeMakeId).Select(x => x.MakeName).ToList();
            }
            if (makes != null && makes.Count > 0)
            {
                return isAmp ? PartialView("~/Views/Shared/m/AMP/_AllMakesWidgetAmp.cshtml", makes) : PartialView("~/Views/Shared/NewCars/_AllMakesWidget.cshtml", makes);
            }
            else
            {
                return new EmptyResult();
            }
        }
        [Carwale.UI.Common.OutputCacheAttr("modelId")]
        public ActionResult SpecificationSection(int modelId)
        {
            return PartialView("~/Views/Shared/NewCars/_SpecificationSection.cshtml", modelId);
        }
        public ActionResult KeyFeatures(int modelId, List<MileageDataEntity> mileageData, List<string> transmissionTypes, string makeName, string modelName, string price)
        {
            try
            {
                CarKeyFeaturesDto keyFeatures = new CarKeyFeaturesDto();
                keyFeatures.MakeName = makeName;
                keyFeatures.ModelName = modelName;
                keyFeatures.Price = string.Format("{0} onwards", Format.GetFormattedPriceV2(price.ToString()));
                keyFeatures.Transmission = Format.GetSentenceFromList(transmissionTypes);
                if (mileageData.IsNotNullOrEmpty())
                {
                    var lowestMileage = mileageData[0].Arai;
                    var heighstMileage = lowestMileage;
                    var displacementLength = string.IsNullOrWhiteSpace(mileageData[0].Displacement) ? 0 : mileageData[0].Displacement.Length;
                    var lowestDisplacement = displacementLength > 2 ? Convert.ToInt32(mileageData[0].Displacement.Substring(0, displacementLength - 2)) : 0;
                    var heighestDisplacement = lowestDisplacement;
                    foreach (var mileage in mileageData)
                    {
                        if (lowestMileage > mileage.Arai)
                        {
                            lowestMileage = mileage.Arai;
                        }
                        if (heighstMileage < mileage.Arai)
                        {
                            heighstMileage = mileage.Arai;
                        }
                        var currentDisplacementLength = string.IsNullOrWhiteSpace(mileage.Displacement) ? 0 : mileage.Displacement.Length;
                        var currentDisplacement = currentDisplacementLength > 2 ? Convert.ToInt32(mileage.Displacement.Substring(0, currentDisplacementLength - 2)) : 0;
                        if (currentDisplacement < lowestDisplacement)
                        {
                            lowestDisplacement = currentDisplacement;
                        }
                        if (currentDisplacement > heighestDisplacement)
                        {
                            heighestDisplacement = currentDisplacement;
                        }
                    }
                    keyFeatures.Mileage = Format.GetRangeText(lowestMileage, heighstMileage, mileageData[0].MileageUnit);
                    keyFeatures.Engine = Format.GetRangeText(lowestDisplacement, heighestDisplacement, "cc");
                }
                if (keyFeatures != null)
                {
                    return PartialView("~/Views/Shared/NewCars/_KeyFeatures.cshtml", keyFeatures);
                }
                else
                {
                    return new EmptyResult();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new EmptyResult();
            }
        }

        [Carwale.UI.Common.OutputCacheAttr("modelId")]
        public ActionResult WhatsNew(int modelId, string modelName, ArticlePageDetails whatsNew, int length, bool canSetLargeDescriptionNull = false)
        {
            try
            {
                if (whatsNew != null && whatsNew.PageList.IsNotNullOrEmpty())
                {
                    OpinionData whatsNewData = new OpinionData
                    {
                        ModelName = modelName,
                        LargeDescription = SetLargeDescription(whatsNew.PageList[0].Content, length, canSetLargeDescriptionNull),
                        SmallDescription = StringUtility.GetHtmlSubString(whatsNew.PageList[0].Content, length),
                        ModelId = modelId
                    };
                    return PartialView("~/Views/Shared/m/CarData/_WhatsNew.cshtml", whatsNewData);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return new EmptyResult();
        }

        [Carwale.UI.Common.OutputCacheAttr("modelId")]
        public ActionResult ProsCons(int modelId, ArticlePageDetails prosCons)
        {
            try
            {
                if (prosCons != null && prosCons.PageList.IsNotNullOrEmpty())
                {
                    return PartialView("~/Views/Shared/m/CarData/_ProsAndCons.cshtml", prosCons);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return new EmptyResult();
        }

        [Carwale.UI.Common.OutputCacheAttr("modelId")]
        public ActionResult Verdict(int modelId, string modelName, ArticlePageDetails verdict, int length, bool canSetLargeDescriptionNull = false)
        {
            try
            {
                if (verdict != null && verdict.PageList.IsNotNullOrEmpty())
                {
                    OpinionData verdictData = new OpinionData
                    {
                        ModelName = modelName,
                        ModelId = modelId,
                        LargeDescription = SetLargeDescription(verdict.PageList[0].Content, length, canSetLargeDescriptionNull),
                        SmallDescription = StringUtility.GetHtmlSubString(verdict.PageList[0].Content, length)
                    };
                    return PartialView("~/Views/Shared/m/CarData/_Verdict.cshtml", verdictData);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return new EmptyResult();
        }

       private string SetLargeDescription(string largeDescription, int length, bool canSetLargeDescriptionNull)
       {
           if (largeDescription.IsNotNullOrEmpty() && canSetLargeDescriptionNull)
           {
               string stringWithoutpTages = largeDescription.Replace("<p>", string.Empty).Replace("</p>", string.Empty);
               if (stringWithoutpTages.Length <= length)
               {
                   return string.Empty;
               }
           }
           return largeDescription;
       }
       [HttpGet]
       [System.Web.Mvc.Route("specsinfo/")]
       public ActionResult GetSpecsInfo(int bodyStyleId, int itemId, int customDataTypeId, string itemName, string itemValue)
       {
           try
           {
               var apiGatewayCaller = new ApiGatewayCaller();
               var specsImageDetailsRequest = new SpecsImageDetailRequest{
                   BodyStyleId = bodyStyleId,
                   ItemId = itemId,
                   CustomDataTypeId = customDataTypeId
               };
               var specsDetails = new SpecsImageDetailsAdaptor();
               specsDetails.AddApiGatewayCallWithCallback(apiGatewayCaller, specsImageDetailsRequest);
               apiGatewayCaller.Call();
               var specsInfo = specsDetails.Output;
               if(specsInfo != null && !String.IsNullOrEmpty(specsInfo.ImageUrl))
               {
                   specsInfo.ItemName = itemName;
                   specsInfo.ItemValue = itemValue;
                   specsInfo.ItemId = itemId;
                   return PartialView("~/Views/m/New/_SpecsInfo.cshtml", specsInfo);
               }
               return new EmptyResult();
           }
           catch (Exception ex)
           {
               Logger.LogException(ex);
               return new EmptyResult();
           }
       }
    }
}