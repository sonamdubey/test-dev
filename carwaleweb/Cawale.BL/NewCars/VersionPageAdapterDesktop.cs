using AutoMapper;
using Carwale.BL.CMS;
using Carwale.DTOs.Campaigns;
using Carwale.DTOs.CarData;
using Carwale.DTOs.CMS.Photos;
using Carwale.DTOs.CMS.ThreeSixtyView;
using Carwale.DTOs.Geolocation;
using Carwale.DTOs.NewCars;
using Carwale.DTOs.PriceQuote;
using Carwale.Entity;
using Carwale.Entity.AdapterModels;
using Carwale.Entity.Campaigns;
using Carwale.Entity.CarData;
using Carwale.Entity.Classified;
using Carwale.Entity.CMS;
using Carwale.Entity.CMS.Articles;
using Carwale.Entity.CMS.Photos;
using Carwale.Entity.CMS.URIs;
using Carwale.Entity.CompareCars;
using Carwale.Entity.Dealers;
using Carwale.Entity.Enum;
using Carwale.Entity.PriceQuote;
using Carwale.Interfaces.Campaigns;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.Classified;
using Carwale.Interfaces.CMS;
using Carwale.Interfaces.CMS.Articles;
using Carwale.Interfaces.CMS.Photos;
using Carwale.Interfaces.Dealers;
using Carwale.Interfaces.Deals;
using Carwale.Interfaces.Geolocation;
using Carwale.Interfaces.NewCars;
using Carwale.Interfaces.Prices;
using Carwale.Interfaces.Template;
using Carwale.Notifications.Logs;
using Carwale.Utility;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Carwale.BL.NewCars
{
    /// <summary>
    /// Created By : Shalini Nair on 18/12/14
    /// </summary>
    public class VersionPageAdapterDesktop : IServiceAdapterV2
    {
        private readonly ICarVersionCacheRepository _carVersionsCacheRepo;
        private readonly IPhotos _carPhotosBL;
        private readonly IStockCountCacheRepository _stockCountCacheRepo;
        private readonly ICMSContent _cmsContentCacheRepo;
        private readonly ICarModelCacheRepository _carModelsCacheRepo;
        private readonly INewCarDealers _newCarDealersBL;
        private readonly ICampaign _campaign;
        private readonly ITemplatesCacheRepository _tempCache;
        private readonly ICarModels _carModelsBL;
        private readonly IMediaBL _mediaBL;
        private readonly IVideosBL _videos;
        private readonly ICarVersions _carVersionsBl;
        private readonly ITemplate _campaignTemplate;
        private readonly IDeals _carDeals;
        private readonly ICarDataLogic _carDataLogic;
        private readonly IGeoCitiesCacheRepository _geoCitiesCacheRepository;
        private readonly IEmiCalculatorAdapter _emiCalculatorAdapter;
        private readonly IDealerAdProvider _dealerAdProvider;

        public VersionPageAdapterDesktop(ICampaign campaign, ITemplatesCacheRepository tempCache, IMediaBL mediaBL, IVideosBL videos, ITemplate campaignTemplate,
        IGeoCitiesCacheRepository geoCitiesCacheRepository, ICarDataLogic carDataLogic, ICarVersionCacheRepository carVersionsCacheRepo, IPhotos carPhotosBL,
        ICarModels carModelsBL, IStockCountCacheRepository stockCountCacheRepo, ICMSContent cmsContentCacheRepo, ICarModelCacheRepository carModelsCacheRepo,
        INewCarDealers newCarDealersBL, ICarVersions carVersionsBl, IDeals carDeals, IEmiCalculatorAdapter emiCalculatorAdapter, IDealerAdProvider dealerAdProvider)
        {
            try
            {
                _carVersionsCacheRepo = carVersionsCacheRepo;
                _carPhotosBL = carPhotosBL;
                _carModelsBL = carModelsBL;
                _stockCountCacheRepo = stockCountCacheRepo;
                _cmsContentCacheRepo = cmsContentCacheRepo;
                _carModelsCacheRepo = carModelsCacheRepo;
                _newCarDealersBL = newCarDealersBL;
                _carVersionsBl = carVersionsBl;
                _carDeals = carDeals;
                _campaign = campaign;
                _tempCache = tempCache;
                _mediaBL = mediaBL;
                _videos = videos;
                _campaignTemplate = campaignTemplate;
                _carDataLogic = carDataLogic;
                _geoCitiesCacheRepository = geoCitiesCacheRepository;
                _emiCalculatorAdapter = emiCalculatorAdapter;
                _dealerAdProvider = dealerAdProvider;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "VersionPageAdapterDesktop Dependency Injection Failed");
            }
        }
        public T Get<T, U>(U input)
        {
            return (T)Convert.ChangeType(GetVersionPageDTOForDesktop(input), typeof(T));
        }

        private VersionPageDTO_Desktop GetVersionPageDTOForDesktop<U>(U input)
        {
            var versionDTO = new VersionPageDTO_Desktop();
            VersionPageDTO_Desktop_Cache versionDTOCache;
            versionDTO.OverviewDetails = new CarOverviewDTO();
            try
            {
                CarDataAdapterInputs inputParam = (CarDataAdapterInputs)Convert.ChangeType(input, typeof(U));
                if (inputParam != null)
                {
                    versionDTOCache = GetDto(inputParam);
                    if (versionDTOCache != null)
                    {
                        versionDTO.NewCarVersions = !versionDTOCache.VersionDetails.Futuristic ? _carVersionsBl?.MapCarVersionDtoWithCarVersionEntity(inputParam.ModelDetails.ModelId, inputParam.CustLocation.CityId) : _carVersionsBl?.MapUpcomingVersionDTOWithEntity(inputParam.ModelDetails.ModelId, inputParam.CustLocation.CityId);

                        if (Convert.ToBoolean(versionDTOCache.VersionDetails.New) || versionDTOCache.VersionDetails.Futuristic)
                        {
                            var version = versionDTO.NewCarVersions.Find(x => x.Id == inputParam.ModelDetails.VersionId);
                            versionDTO.CarPriceOverview = version != null ? version.CarPriceOverview : new PriceOverview();
                        }
                        if (versionDTOCache.VersionDetails.Futuristic)
                        {
                            versionDTO.UpcomingCarDetails = _carModelsCacheRepo.GetUpcomingCarDetails(versionDTOCache.VersionDetails.ModelId);

                            versionDTO.OverviewDetails.EstimatedPrice = versionDTO.CarPriceOverview != null &&
                                versionDTO.CarPriceOverview.Price > 0 ? versionDTO.CarPriceOverview.Price.ToString() : (versionDTO.UpcomingCarDetails.Price != null ? Format.GetUpcomingFormatPrice(versionDTO.UpcomingCarDetails.Price.MinPrice, versionDTO.UpcomingCarDetails.Price.MinPrice) : string.Empty);
                            versionDTO.OverviewDetails.ExpectedLaunch = versionDTOCache.VersionDetails.UpcomingExpectedLaunch ?? (versionDTO.UpcomingCarDetails?.ExpectedLaunch ?? string.Empty);
                        }
                        versionDTO.OverviewDetails.Futuristic = versionDTOCache.VersionDetails.Futuristic;
                        versionDTO.VersionDetails = versionDTOCache.VersionDetails;
                        versionDTO.OverviewDetails.ModelPhotosListCarousel = versionDTOCache.ModelPhotosListCarousel;
                        versionDTO.UsedCarsCount = versionDTOCache.UsedCarsCount;
                        versionDTO.OfferExists = versionDTOCache.OfferExists;
                        versionDTO.ModelNews = versionDTOCache.ModelNews;
                        versionDTO.ModelExpertReviewsCount = versionDTOCache.ModelExpertReviewsCount;
                        List<Video> modelVideos = _videos.GetVideosByModelId(versionDTOCache.VersionDetails.ModelId, CMSAppId.Carwale, 1, -1);
                        versionDTO.ModelVideos = Mapper.Map<List<Video>, List<VideoDTO>>(modelVideos);
                        if (versionDTO.ModelVideos != null)
                        {
                            versionDTO.ModelVideosCount = versionDTO.ModelVideos.Count;
                            versionDTO.OverviewDetails.ModelVideos = modelVideos;
                        }
                        else
                        {
                            versionDTO.ModelVideosCount = 0;
                        }
                        versionDTO.ModelDetails = versionDTOCache.ModelDetails;
                        if (versionDTOCache != null && versionDTOCache.VersionData != null && versionDTOCache.VersionData.Count > 0)
                        {
                            versionDTO.VersionData = versionDTOCache.VersionData[0];
                        }
                        versionDTO.ShowAssistancePopup = versionDTOCache.ShowAssistancePopup;
                        versionDTO.ModelDetails.VideoCount = modelVideos != null ? modelVideos.Count : 0;
                    }
                    if (versionDTO.ModelDetails != null)
                    {

                        versionDTO.ModelDetails.VideoCount = versionDTO.ModelVideosCount;
                        versionDTO.OverviewDetails.ImageUrl360Slug = CMSCommon.Get360DefaultCategory(Mapper.Map<ThreeSixtyAvailabilityDTO>(versionDTO.ModelDetails)) == ThreeSixtyViewCategory.Interior ? CMSCommon.Get360ModelCarouselLinkageImageUrl(versionDTO.ModelDetails, true) : CMSCommon.Get360ModelCarouselLinkageImageUrl(versionDTO.ModelDetails);
                        versionDTO.OverviewDetails.Is360ExteriorAvailable = versionDTO.ModelDetails.Is360ExteriorAvailable;
                        versionDTO.OverviewDetails.Is360InteriorAvailable = versionDTO.ModelDetails.Is360InteriorAvailable;
                        versionDTO.OverviewDetails.Is360OpenAvailable = versionDTO.ModelDetails.Is360OpenAvailable;
                        versionDTO.OverviewDetails.PhotoCount = versionDTO.ModelDetails.PhotoCount;
                        var modelColors = _carModelsCacheRepo.GetModelColorsByModel(versionDTO.ModelDetails.ModelId);
                        versionDTO.OverviewDetails.ShowColours = CMSCommon.IsModelColorPhotosPresent(modelColors);
                        versionDTO.OverviewDetails.ColoursCount = modelColors != null ? modelColors.Count : 0;
                    }

                    if (versionDTO.VersionDetails.New.Equals(1))
                    {
                        var similarCarVmRequest = new SimilarCarVmRequest
                        {
                            ModelId = versionDTO.VersionDetails.ModelId,
                            MakeName = versionDTO.VersionDetails.MakeName,
                            ModelName = versionDTO.VersionDetails.ModelName,
                            MaskingName = versionDTO.VersionDetails.MaskingName,
                            CityId = inputParam.CustLocation.CityId,
                            WidgetSource = WidgetSource.VersionPageAlternativeWidgetCompareCarLinkDesktop,
                            PageName = "VersionPage",
                            CwcCookie = inputParam.CwcCookie
                        };
                        versionDTO.SimilarCars = _carModelsBL?.GetSimilarCarVm(similarCarVmRequest);
                    }

                    versionDTO.NewCarDealersCount = _newCarDealersBL.GetDealersList(-1, Convert.ToInt16(inputParam.CustLocation.CityId), versionDTO.VersionDetails.MakeId, false).NewCarDealers.Count;

                    DealerAdDTO dealerAd = SetCampaignsVm(versionDTO, inputParam);

                    versionDTO.IsRenaultLeadCampaign = versionDTO.ModelDetails.ModelId.Equals(CWConfiguration.RenaultLeadFormModelId);

                    if (_carDeals.IsShowDeals(inputParam.CustLocation.CityId, true))
                    {
                        versionDTO.AdvantageAdData = _carDeals?.GetAdvantageAdContent(versionDTO.ModelDetails.ModelId,
                            (inputParam.CustLocation.CityId > 0 ? inputParam.CustLocation.CityId : 0),
                            versionDTO.ModelDetails.SubSegmentId, inputParam.CustLocation.CityId > 0 ? versionDTO.VersionDetails.VersionId : 0);
                    }
                    string label = "make:" + versionDTO.ModelDetails.MakeName + "|model:" +
                        versionDTO.ModelDetails.ModelName + "|city:" + ((inputParam.CustLocation.ZoneName == string.Empty ||
                        inputParam.CustLocation.ZoneName == "Select Zone") ? (inputParam.CustLocation.CityName) : inputParam.CustLocation.ZoneName);

                    versionDTO.SubNavigation = _carModelsBL?.GetModelQuickMenu(versionDTO.ModelDetails, null, false, versionDTO.ModelExpertReviewsCount > 0, "VersionPage", label);
                    versionDTO.SubNavigation.PQPageId = 43;
                    versionDTO.SubNavigation.PageId = 1;
                    versionDTO.SubNavigation.Page = Pages.VersionPage;
                    versionDTO.SubNavigation.IsVersionDetailPage = true;

                    versionDTO.OverviewDetails = ModelOverview(versionDTO, inputParam,
                        versionDTO.UsedCarsCount != null && (versionDTO.UsedCarsCount.LiveListingCount > 0 && versionDTO.UsedCarsCount.MinLiveListingPrice > 0));
                    versionDTO.PageMetaTags = CreateMetaData(versionDTO);
                    versionDTO.BreadcrumbEntitylist = BindBreadCrumb(versionDTO.ModelDetails, versionDTO.VersionDetails.VersionName);
                    if (versionDTO.VersionDetails.New > 0 || versionDTO.VersionDetails.Futuristic)
                    {
                        versionDTO.JsonLdObject = CreateJsonLdJObject(versionDTO);
                    }
                    if (versionDTO.ModelDetails.New && !versionDTO.ModelDetails.Futuristic && inputParam.CustLocation.CityId > 0)
                    {
                        var price = versionDTO.CarPriceOverview != null ? versionDTO.CarPriceOverview.Price : 0;
                        var overviewDetails = Mapper.Map<CarOverviewDTO, CarOverviewDTOV2>(versionDTO.OverviewDetails);
                        EmiCalculatorModelData emiCalculatorModelData = null;
                        emiCalculatorModelData = (overviewDetails.CarPriceOverview?.PriceStatus ?? -1) == (int)PriceBucket.HaveUserCity ? _emiCalculatorAdapter.GetEmiData(overviewDetails , dealerAd,
                                            new LeadSourceDTO { LeadClickSourceId = 400, InquirySourceId = 113 }, price, inputParam.CustLocation.CityId) : null;
                        if(emiCalculatorModelData != null && emiCalculatorModelData.IsEligibleForThirdPartyEmi && emiCalculatorModelData.ThirdPartyEmiDetails == null)
                        {
                            emiCalculatorModelData = null;
                        }
                        versionDTO.OverviewDetails.EmiCalculatorModelData = emiCalculatorModelData;
                        if (versionDTO.OverviewDetails.EmiCalculatorModelData != null)
                        {
                            versionDTO.OverviewDetails.EmiCalculatorModelData.Platform = "Desktop";
                            versionDTO.OverviewDetails.EmiCalculatorModelData.PageName = CwPages.VersionDesktop.ToString();
                        }
                    }
                    versionDTO.CityDetails = Mapper.Map<CitiesDTO>(_geoCitiesCacheRepository?.GetCityDetailsById(inputParam.CustLocation.CityId));
                    versionDTO.ShowTyresLink = _carVersionsBl.CheckTyresExists(inputParam.ModelDetails.VersionId);
                    versionDTO.ShowEmiCalculator = versionDTO.CityDetails != null && versionDTO.CityDetails.CityId > 0
                                                && versionDTO.VersionDetails.New > 0 && !versionDTO.VersionDetails.Futuristic;
                    return versionDTO;
                }
                else
                {
                    return null;
                }
            }

            catch (Exception ex)
            {
                Logger.LogException(ex);
                return null;
            }
        }

        private VersionPageDTO_Desktop_Cache GetDto(CarDataAdapterInputs inputParameters)
        {
            try
            {
                CarVersionDetails versionDetails = null;
                List<int> valVersionIdList = null;
                if (inputParameters.ModelDetails != null)
                {
                    valVersionIdList = new List<int> { inputParameters.ModelDetails.VersionId };
                    versionDetails = _carVersionsCacheRepo?.GetVersionDetailsById(inputParameters.ModelDetails.VersionId);
                }
                var versionData = _carDataLogic?.GetCombinedCarData(valVersionIdList);
                if (versionDetails != null)
                {
                    List<ModelImage> allPhotos = GetModelPhotos(versionDetails.ModelId);
                    var _ModelPhotosListCarousel = _mediaBL?.GetModelCarouselImages(allPhotos, versionDetails.HostURL, versionDetails.OriginalImgPath);
                    var _ModelNews = GetNewsByModel(versionDetails.ModelId);
                    var _ModelExpertReviewsCount = GetModelExpertReviews(versionDetails.ModelId).Count;
                    var _ModelDetails = _carModelsCacheRepo?.GetModelDetailsById(versionDetails.ModelId);
                    _ModelDetails.PhotoCount = allPhotos.Count;
                    UsedCarCount _UsedCarsCount = null;
                    if (_ModelDetails != null && inputParameters.CustLocation != null)
                    {
                        _UsedCarsCount = _stockCountCacheRepo?.GetUsedCarsCount(_ModelDetails.RootId, inputParameters.CustLocation.CityId);
                        _UsedCarsCount.ModelId = _ModelDetails.ModelId;
                    }
                    var _ShowAssistancePopup = _newCarDealersBL?.CallSlugNumberByMakeId(versionDetails.MakeId);
                    var versionObject = new VersionPageDTO_Desktop_Cache
                    {
                        VersionDetails = versionDetails,
                        ModelPhotosListCarousel = Mapper.Map<List<ModelImage>, List<ModelImageDTO>>(_ModelPhotosListCarousel),
                        UsedCarsCount = _UsedCarsCount,
                        ModelNews = _ModelNews,
                        ModelExpertReviewsCount = _ModelExpertReviewsCount,
                        ModelDetails = _ModelDetails,
                        VersionData = Mapper.Map<List<CarDataPresentation>, List<CCarData>>(versionData) ?? new List<CCarData>(),
                        ShowAssistancePopup = _ShowAssistancePopup
                    };
                    if (versionObject != null && versionObject.VersionData != null && versionObject.VersionData.Count > 0)
                    {
                        versionObject.VersionData[0].Colors = _carVersionsBl?.GetVersionsColors(valVersionIdList);
                    }
                    return versionObject;
                }
                return null;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw;
            }
        }

        private List<ModelImage> GetModelPhotos(int modelId)
        {
            var photosURI = new ModelPhotosBycountURI
            {
                ApplicationId = (ushort)CMSAppId.Carwale,
                CategoryIdList = $"{Convert.ToInt16(CMSContentType.RoadTest)},{Convert.ToInt16(CMSContentType.Images)}",
                ModelId = modelId,
                PlatformId = Platform.CarwaleDesktop.ToString("D"),
            };

            return _carPhotosBL?.GetModelPhotosByCount(photosURI);
        }

        private List<ArticleSummary> GetNewsByModel(int modelId)
        {
            var newsURI = new ArticleRecentURI
            {
                ApplicationId = (ushort)CMSAppId.Carwale,
                ContentTypes = Convert.ToInt16(CMSContentType.News).ToString(),
                TotalRecords = Convert.ToUInt16(ConfigurationManager.AppSettings["ModelNewsCount"]),
                ModelId = modelId,
            };

            return _cmsContentCacheRepo?.GetMostRecentArticles(newsURI);
        }
        private List<ArticleSummary> GetModelExpertReviews(int modelId)
        {
            var articleURI = new ArticleRecentURI
            {
                ApplicationId = (ushort)CMSAppId.Carwale,
                ContentTypes = Convert.ToInt16(CMSContentType.RoadTest).ToString(),
                TotalRecords = Convert.ToUInt16(ConfigurationManager.AppSettings["ExpertReviewsDesktop_Count"]),
                ModelId = modelId,
            };
            List<ArticleSummary> expertReviews = _cmsContentCacheRepo?.GetMostRecentArticles(articleURI);

            return expertReviews;
        }
        private JObject CreateJsonLdJObject(VersionPageDTO_Desktop versionPageDTODesktop)
        {
            try
            {
                JObject engineSpecs = null;
                JObject fuelConsumption = null;
                JObject driveTrain = null;
                JObject width = null;
                JObject height = null;
                JObject transmission = null;
                string airBags = null;
                string coloursOfVersion = string.Empty;
                string seatingCapacity = null;

                JObject manufacturer = new JObject(
                    new JProperty("@type", "organization"),
                    new JProperty("name", versionPageDTODesktop.VersionDetails.MakeName)
                    );

                if (versionPageDTODesktop.VersionData.OverView != null && versionPageDTODesktop.VersionData.OverView.Count > 0)
                {
                    foreach (var item in versionPageDTODesktop.VersionData.OverView)
                    {
                        item.Name = System.Text.RegularExpressions.Regex.Replace(item.Name, "~", string.Empty);
                        if (item.ItemMasterId == "2" && item.Values != null && item.Values.Count > 0)//Width
                        {
                            width = new JObject(
                                new JProperty("@type", "QuantitativeValue"),
                                new JProperty("value", item.Values[0]),
                                new JProperty("unitText", item.UnitType)
                            );
                        }
                        else if (item.ItemMasterId == "3" && item.Values != null && item.Values.Count > 0)//Height
                        {
                            height = new JObject(
                               new JProperty("@type", "QuantitativeValue"),
                               new JProperty("value", item.Values[0]),
                               new JProperty("unitText", item.UnitType)
                           );
                        }
                        else if (item.ItemMasterId == "9" && item.Values != null && item.Values.Count > 0)//Seating Capacity
                        {
                            seatingCapacity = item.Values[0];
                        }
                        else if (item.ItemMasterId == "12" && item.Values != null && item.Values.Count > 0)//Mileage(ARAI)
                        {
                            fuelConsumption = new JObject(
                                new JProperty("@type", "QuantitativeValue"),
                                new JProperty("value", item.Values[0]),
                                new JProperty("unitText", item.UnitType)
                                );
                        }
                        else if (item.ItemMasterId == "14" && item.Values != null && item.Values.Count > 0)//Displacement
                        {
                            engineSpecs = new JObject(
                                new JProperty("@type", "EngineSpecification "),
                                new JProperty("name", item.Values[0] + " cc")
                                );

                        }
                        else if (item.ItemMasterId == "29" && item.Values != null && item.Values.Count > 0)//Transmission Type
                        {
                            transmission = new JObject(
                                new JProperty("@type", "QualitativeValue"),
                                new JProperty("name", item.Values[0])
                                );
                        }
                        else if (item.ItemMasterId == "31" && item.Values != null && item.Values.Count > 0)//Drivetrain
                        {
                            driveTrain = new JObject(
                                new JProperty("@type", "DriveWheelConfigurationValue"),
                                new JProperty("name", item.Values[0])
                                );
                        }
                        else if (item.ItemMasterId == "155" && item.Values != null && item.Values.Count > 0)//Airbags
                        {
                            airBags = item.Values[0];
                        }
                    }
                }
                if (versionPageDTODesktop.VersionData != null && versionPageDTODesktop.VersionData.Colors != null && versionPageDTODesktop.VersionData.Colors.Count > 0)
                {
                    coloursOfVersion = string.Join(",", versionPageDTODesktop.VersionData.Colors[0].Select(c => c.Name));
                }

                JObject jsonObj = new JObject
                (
                new JProperty("@context", "https://schema.org/"),
                new JProperty("@type", "Car"),
                new JProperty("name", versionPageDTODesktop.VersionDetails.MakeName + " " + versionPageDTODesktop.VersionDetails.ModelName + " " + versionPageDTODesktop.VersionDetails.VersionName),

                new JProperty("model", versionPageDTODesktop.VersionDetails.ModelName),
                new JProperty("image", ImageSizes.CreateImageUrl(versionPageDTODesktop.VersionDetails.HostURL,
                ImageSizes._310X174, versionPageDTODesktop.VersionDetails.OriginalImgPath)),
                new JProperty("brand", versionPageDTODesktop.VersionDetails.MakeName),
                new JProperty("bodyType", (((CarBodyStyle)versionPageDTODesktop.VersionDetails.BodyStyleId).ToString())),
                new JProperty("vehicleEngine", engineSpecs),
                new JProperty("fuelType", versionPageDTODesktop.VersionDetails.FuelType),
                new JProperty("vehicleSeatingCapacity", seatingCapacity),
                new JProperty("manufacturer", manufacturer),
                new JProperty("height", height),
                new JProperty("width", width),
                new JProperty("numberOfAirbags", airBags),
                new JProperty("fuelConsumption", fuelConsumption),
                new JProperty("color", coloursOfVersion),
                new JProperty("vehicleTransmission", transmission),
                new JProperty("driveWheelConfiguration", driveTrain),
                new JProperty("description", versionPageDTODesktop.PageMetaTags.Description),
                new JProperty("url", versionPageDTODesktop.PageMetaTags.Canonical)
                );
                if (versionPageDTODesktop.ModelDetails.ReviewCount > 0 && versionPageDTODesktop.ModelDetails.ModelRating > 0)
                {
                    JObject aggregateRating = new JObject(
                        new JProperty("@type", "AggregateRating"),
                        new JProperty("reviewCount", versionPageDTODesktop.ModelDetails.ReviewCount),
                        new JProperty("ratingValue", versionPageDTODesktop.ModelDetails.ModelRating),
                        new JProperty("worstRating", 1),
                        new JProperty("bestRating", 5)
                        );
                    jsonObj.Add(new JProperty("aggregateRating", aggregateRating));
                }
                return jsonObj;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return null;
            }
        }

        public CarOverviewDTO ModelOverview(VersionPageDTO_Desktop versionDTO, CarDataAdapterInputs inputParam, bool isUsedCarAvial)
        {
            CarOverviewDTO overview = versionDTO.OverviewDetails;
            try
            {
                var versionDetailsObj = versionDTO.VersionDetails;
                overview.IsVersionPage = true;
                overview.New = Convert.ToBoolean(versionDTO.VersionDetails.New);
                overview.Discontinue = !overview.New && !versionDetailsObj.Futuristic;
                overview.HostUrl = versionDTO.VersionDetails.HostURL;
                overview.OriginalImage = versionDTO.VersionDetails.OriginalImgPath;
                overview.ModelName = versionDetailsObj.ModelName;
                overview.MakeName = versionDetailsObj.MakeName;
                overview.VersionName = versionDTO.VersionDetails.VersionName;
                overview.MinPrice = versionDetailsObj.MinPrice;
                overview.MaxPrice = versionDetailsObj.MaxPrice;
                overview.ShowCampaignLink = versionDTO.ShowCampaignLink;
                overview.IsUsedCarAvial = isUsedCarAvial;
                overview.MaskingName = versionDTO.ModelDetails.MaskingName;
                overview.RootName = versionDTO.ModelDetails.RootName;
                if (versionDTO.UsedCarsCount != null)
                {
                    overview.LiveListingCount = versionDTO.UsedCarsCount.LiveListingCount;
                    overview.MinLiveListingPrice = versionDTO.UsedCarsCount.MinLiveListingPrice;
                }
                overview.MakeId = versionDetailsObj.MakeId;
                overview.ModelId = versionDetailsObj.ModelId;
                overview.VersionId = versionDetailsObj.VersionId;
                overview.VersionName = versionDetailsObj.VersionName;
                overview.AdvantageAdData = versionDTO.AdvantageAdData;
                overview.EMI = Calculation.Calculation.CalculateEmi(versionDTO.CarPriceOverview != null ? versionDTO.CarPriceOverview.Price : 0);
                overview.CarPriceOverview = versionDTO.CarPriceOverview;

                inputParam.CampaignInput = new CampaignInputv2
                {
                    ModelId = versionDetailsObj.ModelId,
                    PlatformId = (int)Platform.CarwaleDesktop,
                    ApplicationId = (int)Application.CarWale,
                    MakeId = versionDetailsObj.MakeId,
                    PageId = (int)CwPages.VersionDesktop,
                    CityId = inputParam.CustLocation.CityId
                };
                overview.CampaignTemplates = _campaignTemplate?.GetTemplatesByPage(inputParam.CampaignInput, overview);

                int pageId = 31;
                overview.PageId = pageId;
                overview.Is360ExteriorAvailable = versionDTO.ModelDetails.Is360ExteriorAvailable;
                overview.Is360OpenAvailable = versionDTO.ModelDetails.Is360OpenAvailable;
                overview.Is360InteriorAvailable = versionDTO.ModelDetails.Is360InteriorAvailable;
                overview.CarName = string.Format("{0} {1} {2}", versionDTO.ModelDetails.MakeName, versionDTO.ModelDetails.ModelName, versionDTO.VersionDetails.VersionName);
                overview.PhotoCount = versionDTO.ModelDetails.PhotoCount;
                overview.VideosCount = versionDTO.ModelVideosCount;
                if (versionDTO.SponsoredDealerAd != null)
                {
                    overview.AdAvailable = versionDTO.SponsoredDealerAd.DealerId > 0;
                    overview.DealerId = versionDTO.SponsoredDealerAd.DealerId;
                    overview.LeadCTA = versionDTO.SponsoredDealerAd.CTALinkText;
                    overview.CampaignLeadCTA = versionDTO.SponsoredDealerAd.LinkText;
                    overview.PredictionData = Mapper.Map<PredictionData>(versionDTO.SponsoredDealerAd.PredictionData);
                }
                overview.VideoUrl = versionDTO.ModelVideos.IsNotNullOrEmpty() ? versionDTO.ModelVideos[0].VideoTitleUrl : string.Empty;
                overview.NewCarVersions = Mapper.Map<List<NewCarVersionsDTO>, List<NewCarVersionsDTOV2>>(versionDTO.NewCarVersions);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return overview;
        }
        public PageMetaTags CreateMetaData(VersionPageDTO_Desktop versionDTO)
        {
            try
            {
                string priceTag = string.Empty;
                if (versionDTO.VersionDetails.New == 1 && versionDTO.CarPriceOverview != null && versionDTO.CarPriceOverview.Price > 0)
                {
                    string price = Format.FormatFullPrice(versionDTO.CarPriceOverview.Price.ToString());
                    priceTag = versionDTO.CarPriceOverview.PriceStatus == 1 ? string.Format(" Price in {0} - ₹ {1}", versionDTO.CarPriceOverview.City.CityName, price)
                        : string.Format(" Price in India - ₹ {0}", price);
                }
                else if (versionDTO.OverviewDetails != null && versionDTO.OverviewDetails.MinPrice > 0)
                {
                    priceTag = string.Format(" Price in India - ₹ {0}", Format.FormatFullPrice(versionDTO.OverviewDetails.MinPrice.ToString()));
                }
                PageMetaTags metaData = new PageMetaTags
                {
                    Title = string.Format("{0} {1} {2} Price (GST Rates), Features & Specs, {1} {2} Review", System.Text.RegularExpressions.Regex.Replace(versionDTO.ModelDetails.MakeName,
                    "maruti suzuki", "Maruti", System.Text.RegularExpressions.RegexOptions.IgnoreCase),
                    versionDTO.ModelDetails.ModelName, versionDTO.VersionDetails.VersionName),
                    Keywords = string.Format("{0} {1} {2} Price,Features & Specs, {0} {1} {2} Review - CarWale",
                    versionDTO.ModelDetails.MakeName, versionDTO.ModelDetails.ModelName, versionDTO.VersionDetails.VersionName),
                    Description = string.Format("{0} {1} {2} {3}. Check out {1} {2} specifications, features, colours, photos and reviews at CarWale.",
                                  versionDTO.VersionDetails.MakeName, versionDTO.VersionDetails.ModelName,
                                  versionDTO.VersionDetails.VersionName, priceTag),
                    Canonical = ManageCarUrl.CreateVersionUrl(versionDTO.ModelDetails.MakeName, versionDTO.ModelDetails.MaskingName, versionDTO.VersionDetails.VersionMasking, false, true),
                    Alternate = ManageCarUrl.CreateVersionUrl(versionDTO.ModelDetails.MakeName, versionDTO.ModelDetails.MaskingName, versionDTO.VersionDetails.VersionMasking, true, true)
                };

                return metaData;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new PageMetaTags();
            }
        }

        private List<Entity.BreadcrumbEntity> BindBreadCrumb(CarModelDetails modelDetails, string versionName)
        {
            string makeName = Format.FormatSpecial(modelDetails.MakeName);
            List<Entity.BreadcrumbEntity> _BreadcrumbEntitylist = new List<Entity.BreadcrumbEntity>();
            try
            {
                _BreadcrumbEntitylist.Add(new Entity.BreadcrumbEntity
                {
                    Title = string.Format("{0} Cars",
                    modelDetails.MakeName),
                    Link = string.Format("/{0}-cars/", makeName),
                    Text = modelDetails.MakeName
                });
                _BreadcrumbEntitylist.Add(new Entity.BreadcrumbEntity
                {
                    Title = modelDetails.ModelName,
                    Link =
                    string.Format("/{0}-cars/{1}/", makeName, modelDetails.MaskingName),
                    Text = modelDetails.ModelName
                });
                _BreadcrumbEntitylist.Add(new Entity.BreadcrumbEntity { Text = versionName });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return _BreadcrumbEntitylist;
        }

        private DealerAdDTO SetCampaignsVm(VersionPageDTO_Desktop versionDTO, CarDataAdapterInputs inputParam)
        {
            DealerAdDTO dealerAdDto = new DealerAdDTO();
            try
            {
                var carDetails = new CarIdEntity { ModelId = inputParam.ModelDetails.ModelId };
                var persistedCampaignId = _campaign.GetPersistedCampaign(inputParam.ModelDetails.ModelId, inputParam.CustLocation);
                var dealerAd = _dealerAdProvider.GetDealerAd(carDetails, inputParam.CustLocation, Convert.ToInt32(Platform.CarwaleDesktop), (int)CampaignAdType.Pq, persistedCampaignId, (int)CwPages.VersionDesktop);
                dealerAdDto = Mapper.Map<DealerAdDTO>(dealerAd);
                SponsoredDealer sponsoredDealerAd = new SponsoredDealer();
                if(dealerAd != null && dealerAd.Campaign != null)
                {
                    sponsoredDealerAd = Mapper.Map<SponsoredDealer>(dealerAd.Campaign);
                }
                if (sponsoredDealerAd != null && sponsoredDealerAd.DealerId > 0)
                {
                    int templateId = _campaignTemplate.GetCampaignGroupTemplateId(sponsoredDealerAd.AssignedTemplateId, sponsoredDealerAd.AssignedGroupId, (int)Platform.CarwaleDesktop);
                    var template = _tempCache.GetById(templateId);
                    _campaignTemplate.ResolveTemplate(template, ref sponsoredDealerAd);
                }
                else
                {
                    versionDTO.ShowCampaignLink = _campaign.IsCityCampaignExist(versionDTO.VersionDetails.ModelId,
                        inputParam.CustLocation, (int)Platform.CarwaleDesktop, (int)Application.CarWale);
                }
                versionDTO.SponsoredDealerAd = sponsoredDealerAd;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return dealerAdDto;
        }
    }
}
