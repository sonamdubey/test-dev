using AutoMapper;
using Carwale.BL.CMS;
using Carwale.DTOs.Campaigns;
using Carwale.DTOs.CarData;
using Carwale.DTOs.CMS.Photos;
using Carwale.DTOs.NewCars;
using Carwale.Entity.AdapterModels;
using Carwale.Entity.CarData;
using Carwale.Entity.Classified;
using Carwale.Entity.CMS;
using Carwale.Entity.CMS.Photos;
using Carwale.Entity.CMS.URIs;
using Carwale.Entity.Enum;
using Carwale.Entity.PriceQuote;
using Carwale.Interfaces.Campaigns;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.Classified;
using Carwale.Interfaces.CMS;
using Carwale.Interfaces.CMS.Articles;
using Carwale.Interfaces.CMS.Photos;
using Carwale.Interfaces.Deals;
using Carwale.Interfaces.NewCars;
using Carwale.Interfaces.PriceQuote;
using Carwale.Interfaces.Template;
using Carwale.Notifications.Logs;
using Carwale.Utility;
using Microsoft.Practices.Unity;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using log4net;
using System.Diagnostics;
using Google.Protobuf;
using EditCMSWindowsService.Messages;
using Grpc.CMS;
using ApiGatewayLibrary;
using Carwale.BL.GrpcFiles;
using Carwale.DTOs.CMS.ThreeSixtyView;
using Carwale.Entity.Campaigns;
using AEPLCore.Utils.Serializer;
using Carwale.Interfaces.Geolocation;
using Carwale.DTOs.Geolocation;
using Carwale.Interfaces.Prices;
using Carwale.DTOs.PriceQuote;
using Carwale.Entity;
using Carwale.Entity.Dealers;
using Carwale.Utility;
namespace Carwale.BL.NewCars
{
    /// <summary>
    /// Created By : Shalini Nair
    /// </summary>
    public class ModelPageAdapterDesktop : IServiceAdapterV2
    {
        private readonly ICarModelCacheRepository _carModelsCacheRepo;
        private readonly ICMSContent _cmsContentCacheRepo;
        private readonly ICarVersions _carVersionsBL;
        private readonly IMediaBL _mediaBL;
        private readonly ICarModels _carModelsBL;
        private readonly IStockCountCacheRepository _stockCountCacheRepo;
        private readonly IClassifiedListing _classifiedListing;
        private readonly IDeals _cardeals;
        private readonly static ushort _expertReviewsDesktopCount = Convert.ToUInt16(ConfigurationManager.AppSettings["ExpertReviewsDesktop_Count"]);
        private readonly ICarPriceQuoteAdapter _iPrices;
        private readonly ICampaign _campaign;
        private readonly ITemplatesCacheRepository _tempCache;
        private readonly IPhotos _photos;
        private readonly IVideosBL _videos;
        private readonly ITemplate _campaignTemplate;
        private readonly IGeoCitiesCacheRepository _geoCitiesCacheRepository;
        private readonly ICarMileage _carMileage;
        private readonly IDealerAdProvider _dealerAdProviderBl;
        private readonly IEmiCalculatorAdapter _emiCalculatorAdapter;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(ModelPageAdapterDesktop));
        private static readonly bool useAPIGateway = ConfigurationManager.AppSettings["useApiGateway"] == "true";
        private static readonly List<int> _newSpecification = new List<int> { 852, 853, 930, 949, 995, 1078, 1082 };
        private const short replaceModelDateInterval= 30;
        public ModelPageAdapterDesktop(
            ICarModelCacheRepository carModelsCacheRepo,
            ICMSContent cmsContentCacheRepo,
            ICarVersions carVersionsBL,
            IMediaBL mediaBL,
            ICarModels carModelsBL,
            IStockCountCacheRepository stockCountCacheRepo,
            IClassifiedListing classifiedListing,
            IDeals cardeals,
            ICarPriceQuoteAdapter iPrices,
            ICampaign campaign,
            IDealerAdProvider dealerAdProviderBl,
            ITemplatesCacheRepository tempCache,
            IPhotos photos, IVideosBL videos,
             ITemplate campaignTemplate,
             IGeoCitiesCacheRepository geoCitiesCacheRepository,
             ICarMileage carMileage,
            IEmiCalculatorAdapter emiCalculatorAdapter)
        {
            try
            {
                _carModelsCacheRepo = carModelsCacheRepo;
                _cmsContentCacheRepo = cmsContentCacheRepo;
                _carVersionsBL = carVersionsBL;
                _mediaBL = mediaBL;
                _carModelsBL = carModelsBL;
                _stockCountCacheRepo = stockCountCacheRepo;
                _classifiedListing = classifiedListing;
                _cardeals = cardeals;
                _iPrices = iPrices;
                _campaign = campaign;
                _tempCache = tempCache;
                _photos = photos;
                _videos = videos;
                _campaignTemplate = campaignTemplate;
                _geoCitiesCacheRepository = geoCitiesCacheRepository;
                _dealerAdProviderBl = dealerAdProviderBl;
                _carMileage = carMileage;
                _emiCalculatorAdapter = emiCalculatorAdapter;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        public T Get<T, U>(U input)
        {
            return (T)Convert.ChangeType(GetModelPageDtoForDesktop<U>(input), typeof(T));
        }

        /// <summary>
        /// Returns the complete details required for ModelPage
        /// </summary>
        /// <returns></returns>
        private ModelPageDTO_Desktop GetModelPageDtoForDesktop<U>(U input)
        {
            var modelPageDTO = new ModelPageDTO_Desktop();
            try
            {
                CarDataAdapterInputs inputParam = (CarDataAdapterInputs)Convert.ChangeType(input, typeof(U));

                modelPageDTO.ModelDetails = _carModelsCacheRepo.GetModelDetailsById(inputParam.ModelDetails.ModelId) ?? new CarModelDetails();
                if (modelPageDTO.ModelDetails != null && modelPageDTO.ModelDetails.MakeId > 0)
                {
                    if (modelPageDTO.ModelDetails.New || !modelPageDTO.ModelDetails.Futuristic)
                    {
                        GetLaunchedCarVm(modelPageDTO, inputParam); //both for discontinue and new car
                    }
                    else
                    {
                        modelPageDTO.UpcomingCarDetails = _carModelsCacheRepo.GetUpcomingCarDetails(inputParam.ModelDetails.ModelId);
                        GetUpcomingCarVm(modelPageDTO, inputParam);
                        SetUpcomingModelCampaign(modelPageDTO);
                    }
                    List<ModelImage> modelPhotos;
                    SetCmsData(modelPageDTO, out modelPhotos);

                    if (modelPageDTO.ModelDetails.New || modelPageDTO.ModelDetails.Futuristic)
                    {
                        var similarCarVmRequest = new SimilarCarVmRequest
                        {
                            ModelId = modelPageDTO.ModelDetails.ModelId,
                            MakeName = modelPageDTO.ModelDetails.MakeName,
                            ModelName = modelPageDTO.ModelDetails.ModelName,
                            MaskingName = modelPageDTO.ModelDetails.MaskingName,
                            CityId = inputParam.CustLocation.CityId,
                            WidgetSource = WidgetSource.ModelPageAlternativeWidgetCompareCarLinkDesktop,
                            PageName = "ModelPage",
                            IsMobile = false,
                            IsFuturistic = modelPageDTO.ModelDetails.Futuristic,
                            CwcCookie = inputParam.CwcCookie
                        };
                        modelPageDTO.SimilarCars = _carModelsBL.GetSimilarCarVm(similarCarVmRequest);
                    }
                    SetUsedCarsVm(modelPageDTO, inputParam);
                    modelPageDTO.BreadcrumbEntitylist = BindBreadCrumb(modelPageDTO.ModelDetails, inputParam.IsCityPage, inputParam.CustLocation.CityName) ?? new List<Entity.BreadcrumbEntity>();
                    modelPageDTO.ModelColors = _carModelsCacheRepo.GetModelColorsByModel(inputParam.ModelDetails.ModelId);
                    modelPageDTO.ModelDetails.VideoCount = modelPageDTO.ModelVideos != null ? modelPageDTO.ModelVideos.Count : 0;
                    modelPageDTO.SubNavigation = modelPageDTO.ModelDetails.New ? BindModelQuickMenu(modelPageDTO, inputParam) : null;
                    modelPageDTO.OverviewDetails = ModelOverview(modelPageDTO, inputParam, modelPhotos) ?? new CarOverviewDTO();
                    GetPageMetaTags(modelPageDTO);
                    modelPageDTO.CityDetails = Mapper.Map<CitiesDTO>(_geoCitiesCacheRepository.GetCityDetailsById(inputParam.CustLocation.CityId));
                    if (modelPageDTO.ModelDetails.New)
                    {
                        var processVersionData = _carVersionsBL.ProcessVersionsData(inputParam.ModelDetails.ModelId, modelPageDTO.NewCarVersions);
                        modelPageDTO.Summary = _carModelsBL.Summary(modelPageDTO.PageMetaTags.Summary,
                                                    modelPageDTO.ModelDetails.MakeName, modelPageDTO.ModelDetails.ModelName, processVersionData,
                                                    modelPageDTO.NewCarVersions, inputParam.IsCityPage, inputParam.CustLocation.CityName);
                    }
                    if (modelPageDTO.ModelDetails.New && !modelPageDTO.ModelDetails.Futuristic)
                    {
                        modelPageDTO.JsonLdObject = CreateJsonLdJObject(modelPageDTO);

                        if (modelPageDTO.OverviewDetails != null && modelPageDTO.ModelDetails != null && modelPageDTO.ModelDetails.ReplacedModelId > 0 && DateTimeUtility.ShowReplaceModel(modelPageDTO.ModelDetails.ModelLaunchDate, false, replaceModelDateInterval))
                        {
                            modelPageDTO.OverviewDetails.ReplacedModelDetails = _carModelsBL.GetReplacedModelDetails(modelPageDTO.ModelDetails.ReplacedModelId, modelPageDTO.ModelDetails, false);
                        }
                        if(inputParam.CustLocation.CityId > 0)
                        {
                            var overviewDetails = Mapper.Map<CarOverviewDTO, CarOverviewDTOV2>(modelPageDTO.OverviewDetails);
                            var price = modelPageDTO.CarPriceOverview != null ? modelPageDTO.CarPriceOverview.Price : 0;
                            EmiCalculatorModelData emiData = null;
                            emiData = overviewDetails.CarPriceOverview.PriceStatus == (int)PriceBucket.HaveUserCity ? _emiCalculatorAdapter.GetEmiData(overviewDetails, modelPageDTO.DealerAd,
                                new LeadSourceDTO { LeadClickSourceId = 400, InquirySourceId = 114 }, price, inputParam.CustLocation.CityId) : null;
                            if (emiData != null)
                            {
                                emiData.PageName = CwPages.ModelDesktop.ToString();
                                emiData.Platform = "Desktop";
                            }
                            modelPageDTO.OverviewDetails.EmiCalculatorModelData = emiData;

                            foreach (var version in modelPageDTO.OverviewDetails.NewCarVersions)
                            {
                                overviewDetails.VersionId = version.Id;
                                overviewDetails.VersionName = version.Version;
                                version.EmiCalculatorModelData = _emiCalculatorAdapter.GetEmiData(overviewDetails, modelPageDTO.DealerAd, new LeadSourceDTO { LeadClickSourceId = 400, InquirySourceId = 114 }, 
                                                                    version.CarPriceOverview.PriceForSorting, inputParam.CustLocation.CityId);
                            }
                        }
                    }

                }
                modelPageDTO.IsNewSpecsShow = _newSpecification.Contains(inputParam.ModelDetails.ModelId);
                modelPageDTO.ShowEmiCalculator = modelPageDTO.CityDetails != null && modelPageDTO.CityDetails.CityId > 0
                                                && modelPageDTO.ModelDetails.New && !modelPageDTO.ModelDetails.Futuristic
                                                && modelPageDTO.OverviewDetails.NewCarVersions.Any(x => x.EmiCalculatorModelData != null);                                       
            }
            catch (NullReferenceException ex)
            {
                Logger.LogException(ex);
            }

            return modelPageDTO;
        }
        private void GetLaunchedCarVm(ModelPageDTO_Desktop modelPageDTO, CarDataAdapterInputs inputParam)
        {
            try
            {
                if (modelPageDTO.ModelDetails.New)
                {
                    var versionDetailsList = _carVersionsBL.GetCarVersions(inputParam.ModelDetails.ModelId, Entity.Status.New);
                    modelPageDTO.NewCarVersions = _carVersionsBL.MapCarVersionDtoWithCarVersionEntity(inputParam.ModelDetails.ModelId, inputParam.CustLocation.CityId, true, versionDetailsList);

                    modelPageDTO.FuelTypes = modelPageDTO.NewCarVersions.Equals(null) ? new List<string>() : modelPageDTO.NewCarVersions.Select(x => x.CarFuelType).TakeWhile(x => !string.IsNullOrEmpty(x)).Distinct().ToList();
                    modelPageDTO.TransmissionTypes = modelPageDTO.NewCarVersions.Equals(null) ? new List<string>() : modelPageDTO.NewCarVersions.Select(x => x.TransmissionType).TakeWhile(x => !string.IsNullOrEmpty(x)).Distinct().ToList();
                    if (modelPageDTO.NewCarVersions != null)
                    {
                        if (modelPageDTO.NewCarVersions.Count > 0 && modelPageDTO.NewCarVersions[0].CarPriceOverview.Price > 0)
                        {
                            modelPageDTO.ModelDetails.MinPrice = modelPageDTO.NewCarVersions.Where(c => c.CarPriceOverview.Price > 0).Min(x => x.CarPriceOverview.Price);
                            modelPageDTO.ModelDetails.MaxPrice = modelPageDTO.NewCarVersions.Max(x => x.CarPriceOverview.Price);
                        }

                        if (inputParam.ModelDetails.VersionId > 0)
                        {
                            NewCarVersionsDTO version = modelPageDTO.NewCarVersions.Find(x => x.Id == inputParam.ModelDetails.VersionId);
                            modelPageDTO.CarPriceOverview = version != null ? version.CarPriceOverview : new PriceOverview();
                            modelPageDTO.CarVersion = new CarVersionsDTO
                            {
                                ID = inputParam.ModelDetails.VersionId,
                                Name = version != null ? version.Version : string.Empty
                            };
                        }
                        else
                        {
                            modelPageDTO.CarPriceOverview = modelPageDTO.NewCarVersions.Count > 0 ? modelPageDTO.NewCarVersions[0].CarPriceOverview : new PriceOverview();
                            modelPageDTO.CarVersion = new CarVersionsDTO
                            {
                                ID = modelPageDTO.NewCarVersions.Count > 0 ? modelPageDTO.NewCarVersions[0].Id : 0,
                                Name = modelPageDTO.NewCarVersions.Count > 0 ? modelPageDTO.NewCarVersions[0].Version : String.Empty
                            };
                        }
                    }

                    if (modelPageDTO.NewCarVersions.IsNotNullOrEmpty())
                    {
                        modelPageDTO.ModelDataSummary = _carModelsBL.GetProcessedCarModelDataSummary(modelPageDTO.NewCarVersions.Select(x => x.Id).ToList(), inputParam.ModelDetails.ModelId);
                    }

                    SetCampaignsVm(modelPageDTO, inputParam);

                    modelPageDTO.IsRenaultLeadCampaign = modelPageDTO.ModelDetails.ModelId.Equals(CWConfiguration.RenaultLeadFormModelId);
                    modelPageDTO.MileageData = _carMileage.GetMileageData(versionDetailsList);
                    modelPageDTO.Drivetrain = versionDetailsList.Select(x => x.Drivetrain).ToList();
                    modelPageDTO.SeatingCapacity = versionDetailsList.Select(x => x.SeatingCapacity).ToList();
                    modelPageDTO.VersionCountWithSpecs = modelPageDTO.NewCarVersions.FindAll(x => x.IsSpecsExist).Count;
                }
                modelPageDTO.UpgradedModelDetails = _carModelsCacheRepo.GetUpgradedModel(inputParam.ModelDetails.ModelId);
                modelPageDTO.AdvantageAdData = _cardeals.IsShowDeals(inputParam.CustLocation.CityId, true) ?
                    _cardeals.GetAdvantageAdContent(inputParam.ModelDetails.ModelId,
                    (inputParam.CustLocation.CityId > 0 ? inputParam.CustLocation.CityId : 0), modelPageDTO.ModelDetails.SubSegmentId) : null;
            }
            catch (NullReferenceException ex)
            {
                Logger.LogException(ex);
            }
        }

        private void GetUpcomingCarVm(ModelPageDTO_Desktop modelPageDTO, CarDataAdapterInputs inputParam)
        {
            try
            {
                modelPageDTO.NewCarVersions = _carVersionsBL.MapUpcomingVersionDTOWithEntity(inputParam.ModelDetails.ModelId, inputParam.CustLocation.CityId, inputParam.IsCityPage);
                modelPageDTO.VersionCountWithSpecs = modelPageDTO.NewCarVersions.FindAll(x => x.IsSpecsExist).Count;
                modelPageDTO.ShowPriceColumn = modelPageDTO.NewCarVersions.Any(x => x.CarPriceOverview != null);

            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "ModelPageAdapterDesktop.GetUpcomingCarVm");
            }
        }

        private void SetCampaignsVm(ModelPageDTO_Desktop modelPageDTO, CarDataAdapterInputs inputParam)
        {
            try
            {
                //Fetching campaign
                var carDetails = new CarIdEntity { ModelId = inputParam.ModelDetails.ModelId };
                var persistedCampaignId = _campaign.GetPersistedCampaign(inputParam.ModelDetails.ModelId, inputParam.CustLocation);
                var dealerAd = _dealerAdProviderBl.GetDealerAd(carDetails, inputParam.CustLocation, Convert.ToInt32(Platform.CarwaleDesktop), (int)CampaignAdType.Pq, persistedCampaignId, (int)Pages.ModelPageId);
                modelPageDTO.DealerAd = Mapper.Map<DealerAdDTO>(dealerAd);
                //Mapping to old object to make the exiting code work
                modelPageDTO.SponsoredDealerAd = new SponsoredDealer();
                if (dealerAd != null && dealerAd.Campaign != null)
                {
                    //Mapping to old object to make the exiting code work
                    modelPageDTO.SponsoredDealerAd = Mapper.Map<SponsoredDealer>(dealerAd.Campaign);
                }
                
                if (modelPageDTO.SponsoredDealerAd != null && modelPageDTO.SponsoredDealerAd.DealerId > 0)
                {
                    modelPageDTO.SponsoredDealerAd.MakeName = modelPageDTO.ModelDetails.MakeName;
                    int templateId = _campaignTemplate.GetCampaignGroupTemplateId(modelPageDTO.SponsoredDealerAd.AssignedTemplateId,
                        modelPageDTO.SponsoredDealerAd.AssignedGroupId, (int)Platform.CarwaleDesktop);
                    var template = _tempCache.GetById(templateId);
                    var sponsordealerObj = modelPageDTO.SponsoredDealerAd;
                    _campaignTemplate.ResolveTemplate(template, ref sponsordealerObj);
                    modelPageDTO.SponsoredDealerAd = sponsordealerObj;
                }
                else
                {
                    modelPageDTO.ShowCampaignLink = _campaign.IsCityCampaignExist(inputParam.ModelDetails.ModelId,
                        inputParam.CustLocation, (int)Platform.CarwaleDesktop, (int)Application.CarWale);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        private void SetUpcomingModelCampaign(ModelPageDTO_Desktop modelPageDTO)
        {
            if (modelPageDTO.ModelDetails.Futuristic && modelPageDTO.ModelDetails.ModelId == CWConfiguration.MahindraAlturasModelid)
            {
                modelPageDTO.SponsoredDealerAd = new SponsoredDealer
                {
                    DealerId = CWConfiguration.MahindraAlturasCampaignid,
                    DealerName = "Mahindra India",
                    DealerLeadBusinessType = 1 //for ES campaigns
                };
            }
        }

        private void SetUsedCarsVm(ModelPageDTO_Desktop modelPageDTO, CarDataAdapterInputs inputParam)
        {
            try
            {
                modelPageDTO.UsedCarsCount = _stockCountCacheRepo.GetUsedCarsCount(modelPageDTO.ModelDetails.RootId, inputParam.CustLocation.CityId) ?? new UsedCarCount();
                modelPageDTO.UsedCarsCount.ModelId = modelPageDTO.ModelDetails.ModelId;
                modelPageDTO.UsedLuxuryCars = _classifiedListing.GetLuxuryCarRecommendations(inputParam.ModelDetails.ModelId, 3849, 2);
            }
            catch (NullReferenceException ex)
            {
                Logger.LogException(ex);
            }
        }

        /// <summary>
        /// Returns photos for the model
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        private List<ModelImage> GetModelPhotos(int modelId)
        {
            try
            {
                var photosURI = new ModelPhotosBycountURI
                {
                    ApplicationId = (ushort)CMSAppId.Carwale,
                    CategoryIdList = String.Format("{0},{1}", CMSContentType.RoadTest.ToString("D"), CMSContentType.Images.ToString("D")),
                    ModelId = modelId,
                    PlatformId = Platform.CarwaleDesktop.ToString("D")
                };
                return _photos.GetModelPhotosByCount(photosURI);
            }
            catch (NullReferenceException ex)
            {
                Logger.LogException(ex);
                return null;
            }
        }

        public CarOverviewDTO ModelOverview(ModelPageDTO_Desktop modelDTO, CarDataAdapterInputs inputparam, List<ModelImage> modelPhotos)
        {
            try
            {
                CarOverviewDTO overview = new CarOverviewDTO();
                var modelDetailsObj = modelDTO.ModelDetails;
                overview.IsVersionPage = false;
                overview.Futuristic = modelDetailsObj.Futuristic;
                overview.New = modelDetailsObj.New;
                overview.Discontinue = !(modelDetailsObj.Futuristic || modelDetailsObj.New);
                overview.HostUrl = modelDetailsObj.HostUrl;
                overview.OriginalImage = modelDetailsObj.OriginalImage;
                overview.ModelName = modelDetailsObj.ModelName;
                overview.MakeName = modelDetailsObj.MakeName;
                overview.MinPrice = modelDetailsObj.MinPrice;
                overview.MaxPrice = modelDetailsObj.MaxPrice;
                overview.MaskingName = modelDetailsObj.MaskingName;
                overview.BodyStyleId = modelDTO.ModelDetails.BodyStyleId;
                if (modelDTO.UsedCarsCount != null)
                {
                    overview.IsUsedCarAvial = (modelDTO.UsedCarsCount.LiveListingCount > 0 && modelDTO.UsedCarsCount.MinLiveListingPrice > 0);
                    overview.LiveListingCount = modelDTO.UsedCarsCount.LiveListingCount;
                    overview.MinLiveListingPrice = modelDTO.UsedCarsCount.MinLiveListingPrice;
                }
                overview.MakeId = modelDetailsObj.MakeId;
                overview.ModelId = modelDetailsObj.ModelId;
                if (modelDTO.SponsoredDealerAd != null && modelDTO.SponsoredDealerAd.DealerId > 0)
                {
                    overview.AdAvailable = modelDTO.SponsoredDealerAd.ActualDealerId > 0;
                    overview.DealerId = modelDTO.SponsoredDealerAd.DealerId;
                    overview.LeadCTA = modelDTO.SponsoredDealerAd.CTALinkText;
                    overview.CampaignLeadCTA = modelDTO.SponsoredDealerAd.LinkText;
                    overview.PredictionData = Mapper.Map<PredictionData>(modelDTO.SponsoredDealerAd.PredictionData);
                }
                overview.ShowCampaignLink = modelDTO.ShowCampaignLink;
                if (modelDetailsObj.Futuristic && modelDTO.UpcomingCarDetails != null)
                {
                    overview.ExpectedLaunch = modelDTO.UpcomingCarDetails.ExpectedLaunch;
                    if (modelDTO.UpcomingCarDetails.Price != null)
                    {
                        overview.EstimatedPrice = Format.GetUpcomingFormatPrice(modelDTO.UpcomingCarDetails.Price.MinPrice, modelDTO.UpcomingCarDetails.Price.MinPrice);
                    }
                }
                overview.AdvantageAdData = modelDTO.AdvantageAdData;
                overview.CarPriceOverview = modelDTO.CarPriceOverview;
                overview.RootName = modelDTO.ModelDetails.RootName;
                overview.EMI = Calculation.Calculation.CalculateEmi(inputparam.IsCityPage ? (int)modelDetailsObj.MinPrice : (modelDTO.CarPriceOverview != null ? modelDTO.CarPriceOverview.Price : 0));
                overview.Is360ExteriorAvailable = modelDetailsObj.Is360ExteriorAvailable;
                overview.Is360OpenAvailable = modelDetailsObj.Is360OpenAvailable;
                overview.Is360InteriorAvailable = modelDetailsObj.Is360InteriorAvailable;
                overview.ImageUrl360Slug = CMSCommon.Get360DefaultCategory(Mapper.Map<ThreeSixtyAvailabilityDTO>(modelDetailsObj)) == ThreeSixtyViewCategory.Interior ? CMSCommon.Get360ModelCarouselLinkageImageUrl(modelDetailsObj, true) : CMSCommon.Get360ModelCarouselLinkageImageUrl(modelDetailsObj);
                overview.CarName = string.Format("{0} {1}", modelDetailsObj.MakeName, modelDetailsObj.ModelName);
                inputparam.CampaignInput = new CampaignInputv2
                {
                    ModelId = modelDetailsObj.ModelId,
                    MakeId = modelDetailsObj.MakeId,
                    PlatformId = (int)Platform.CarwaleDesktop,
                    PageId = (int)CwPages.ModelDesktop,
                    ApplicationId = (int)Application.CarWale,
                    CityId = inputparam.CustLocation.CityId
                };
                overview.CampaignTemplates = _campaignTemplate.GetTemplatesByPage(inputparam.CampaignInput, overview);
                int pageId = (int)CwPages.ModelDesktop;
                overview.PageId = pageId;
                overview.PhotoCount = modelDTO.ModelDetails.PhotoCount;
                overview.ShowColours = CMSCommon.IsModelColorPhotosPresent(modelDTO.ModelColors);
                overview.ColoursCount = modelDTO.ModelColors != null ? modelDTO.ModelColors.Count : 0;
                overview.ModelPhotosListCarousel = Mapper.Map<List<ModelImage>, List<ModelImageDTO>>(_mediaBL.GetModelCarouselImages(modelPhotos));
                if (modelDTO.ModelVideos != null)
                {
                    overview.VideosCount = modelDTO.ModelVideos.Count;
                    if (modelDTO.ModelVideos.Count > 0)
                    {
                        overview.VideoUrl = modelDTO.ModelVideos[0].VideoTitleUrl;
                        overview.ModelVideos = modelDTO.ModelVideos;
                    }
                }
                else
                {
                    overview.VideosCount = 0;
                }

                overview.NewCarVersions = AutoMapper.Mapper.Map<List<NewCarVersionsDTO>, List<NewCarVersionsDTOV2>>(modelDTO.NewCarVersions);

                overview.NewCarVersions.ForEach(x => x.VersionEmi = x.CarPriceOverview != null && x.CarPriceOverview.PriceForSorting > 0 ? Carwale.BL.Calculation.Calculation.CalculateEmi(x.CarPriceOverview.PriceForSorting) : string.Empty);

                if (modelDTO.CarVersion != null)
                {
                    overview.VersionId = modelDTO.CarVersion.ID;
                    overview.VersionName = modelDTO.CarVersion.Name;
                }
                else
                {
                    overview.VersionId = -1;
                    overview.VersionName = string.Empty;
                }
                return overview;
            }
            catch (NullReferenceException ex)
            {
                Logger.LogException(ex);
                return null;
            }
        }

        private void GetPageMetaTags(ModelPageDTO_Desktop modelPageDTO)
        {
            try
            {
                modelPageDTO.PageMetaTags = _carModelsCacheRepo.GetModelPageMetaTags(modelPageDTO.ModelDetails.ModelId, Convert.ToInt16(Pages.ModelPageId)) ?? new PageMetaTags();
                modelPageDTO.PageMetaTags.Title = _carModelsBL.Title(modelPageDTO.PageMetaTags.Title, modelPageDTO.ModelDetails.MakeName, modelPageDTO.ModelDetails.ModelName);
                modelPageDTO.PageMetaTags.Description = GetDescription(modelPageDTO);
                modelPageDTO.PageMetaTags.Heading = _carModelsBL.Heading(modelPageDTO.PageMetaTags.Heading, modelPageDTO.ModelDetails.MakeName, modelPageDTO.ModelDetails.ModelName);
                modelPageDTO.PageMetaTags.Canonical = ManageCarUrl.CreateModelUrl(modelPageDTO.ModelDetails.MakeName, modelPageDTO.ModelDetails.MaskingName, true);
                modelPageDTO.PageMetaTags.Alternate = ManageCarUrl.CreateModelUrl(modelPageDTO.ModelDetails.MakeName, modelPageDTO.ModelDetails.MaskingName, true);
            }
            catch (NullReferenceException ex)
            {
                Logger.LogException(ex);
            }
        }

        private string GetDescription(ModelPageDTO_Desktop modelPageDTO_Desktop)
        {
            try
            {
                var modelDetails = modelPageDTO_Desktop.ModelDetails;
                string modelPrice;
                if (modelPageDTO_Desktop.ModelDetails.New)
                {
                    modelPrice = Format.GetFormattedPriceV2(modelPageDTO_Desktop.CarPriceOverview.Price.ToString());
                }
                else if (modelPageDTO_Desktop.ModelDetails.Futuristic)
                {
                    modelPrice = modelPageDTO_Desktop.OverviewDetails.EstimatedPrice;
                }
                else
                {
                    modelPrice = Format.GetFormattedPriceV2(modelPageDTO_Desktop.ModelDetails.MinPrice.ToString());
                }
                return _carModelsBL.GetDescription(modelDetails.MakeName, modelDetails.ModelName, modelPrice);
            }
            catch (NullReferenceException ex)
            {
                Logger.LogException(ex);
                return string.Empty;
            }
        }

        /// Binds the ModelDetails Overview Section
        public SubNavigationDTO BindModelQuickMenu(ModelPageDTO_Desktop modelDTO, CarDataAdapterInputs modelInput)
        {
            try
            {
                string label = "make:" + modelDTO.ModelDetails.MakeName + "|model:" +
                    modelDTO.ModelDetails.ModelName + "|city:" + (modelInput.IsCityPage ? modelInput.CustLocation.CityName : modelInput.CustLocation.ZoneId.ToString());
                var SubNavigation = _carModelsBL.GetModelQuickMenu(modelDTO.ModelDetails, null,
                    !modelInput.IsCityPage, (modelDTO.ExpertReviewsByModel != null && modelDTO.ExpertReviewsByModel.Count > 0), "ModelPage", label);
                SubNavigation.PQPageId = modelInput.IsCityPage ? 45 : 30;
                SubNavigation.Page = Pages.ModelPageId;
                SubNavigation.ShowColorsLink = CMSCommon.IsModelColorPhotosPresent(modelDTO.ModelColors);
                return SubNavigation;
            }
            catch (NullReferenceException ex)
            {
                Logger.LogException(ex);
                return null;
            }
        }

        private List<Entity.BreadcrumbEntity> BindBreadCrumb(CarModelDetails modelDetails, bool isCityPage, string cityName)
        {
            try
            {
                string makeName = Format.FormatSpecial(modelDetails.MakeName);
                List<Entity.BreadcrumbEntity> _BreadcrumbEntitylist = new List<Entity.BreadcrumbEntity>();
                _BreadcrumbEntitylist.Add(new Entity.BreadcrumbEntity
                {
                    Title = string.Format("{0} Cars",
                    modelDetails.MakeName),
                    Link = ManageCarUrl.CreateMakeUrl(modelDetails.MakeName),
                    Text = modelDetails.MakeName
                });
                if (isCityPage)
                {
                    _BreadcrumbEntitylist.Add(new Entity.BreadcrumbEntity
                    {
                        Title = modelDetails.ModelName,
                        Link = string.Format("/{0}-cars/{1}/", makeName, modelDetails.MaskingName),
                        Text = modelDetails.ModelName
                    });
                    _BreadcrumbEntitylist.Add(new Entity.BreadcrumbEntity { Text = string.Format("Price in {0}", cityName) });
                }
                else
                {
                    _BreadcrumbEntitylist.Add(new Entity.BreadcrumbEntity { Text = modelDetails.ModelName });
                }
                return _BreadcrumbEntitylist;
            }
            catch (NullReferenceException ex)
            {
                Logger.LogException(ex);
                return null;
            }
        }
        private JObject CreateJsonLdJObject(ModelPageDTO_Desktop modelPageDTO)
        {
            try
            {
                ModelSchema modelSchema = new ModelSchema
                {
                    ModelDetails = modelPageDTO.ModelDetails,
                    ModelColors = modelPageDTO.ModelColors,
                    PageMetaTags = modelPageDTO.PageMetaTags,
                    SeatingCapacity = modelPageDTO.SeatingCapacity,
                    Drivetrain = modelPageDTO.Drivetrain,
                    MileageData = modelPageDTO.MileageData
                };
                return _carModelsBL.CreateJsonLdJObject(modelSchema, modelPageDTO.SimilarCars.SimilarCarModels);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return null;
            }
     
        }
        private void SetCmsData(ModelPageDTO_Desktop model, out List<ModelImage> modelPhotos)
        {
            modelPhotos = null;
            try
            {
                    ushort applicationId = (ushort)CMSAppId.Carwale;
                    KeyValuePair<string, IMessage>[] calls = {
                new KeyValuePair<string, IMessage>("GetModelPhotosList", new GrpcModelPhotoURI() {  ApplicationId = applicationId,
                    CategoryIdList = String.Format("{0},{1}", CMSContentType.RoadTest.ToString("D"), CMSContentType.Images.ToString("D")),
                    ModelId = model.ModelDetails.ModelId }),

                new KeyValuePair<string, IMessage>("GetVideosByModelId", new GrpcVideosByIdURI()
                    {
                        Id = model.ModelDetails.ModelId,
                        ApplicationId = applicationId,
                        StartIndex = 1,
                        EndIndex = 1000
                    }),
                    new KeyValuePair<string, IMessage>("GetMostRecentArticles", new GrpcArticleRecentURI()
                    {
                        ApplicationId = applicationId,
                        ContentTypes = "8",
                        TotalRecords = _expertReviewsDesktopCount,
                        ModelId =  model.ModelDetails.ModelId
                    }),
                     new KeyValuePair<string, IMessage>("GetCarSynopsisV1", new GrpcCarSynopsisURI
                    {
                          ApplicationId = applicationId,
                          ModelId = model.ModelDetails.ModelId
                    })
                    };

                    var result = GrpcMethods.GetDataFromGateway(calls);
                    if (string.IsNullOrWhiteSpace(result.OutputMessages[0].Exception))
                    {
                        modelPhotos = _photos.GetModelPhotosByCount(new ModelPhotosBycountURI(), GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(Serializer.ConvertBytesToMsg<GrpcModelImageList>(result.OutputMessages[0].Payload))) ?? new List<ModelImage>();
                    }
                    else
                    {
                        modelPhotos = new List<ModelImage>();
                    }
                    model.ModelDetails.PhotoCount = modelPhotos.Count;
                    model.ModelPhotosList = _mediaBL.GetModelImages(modelPhotos);
                    if (string.IsNullOrWhiteSpace(result.OutputMessages[1].Exception))
                    {
                        model.ModelVideos = GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(Serializer.ConvertBytesToMsg<GrpcVideosList>(result.OutputMessages[1].Payload));
                    }
                    if (string.IsNullOrWhiteSpace(result.OutputMessages[2].Exception))
                    {
                        model.ExpertReviewsByModel = _cmsContentCacheRepo.GetExpertReviewByModel(0, 0, GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(Serializer.ConvertBytesToMsg<GrpcArticleSummaryList>(result.OutputMessages[2].Payload)));
                    }
                    if (string.IsNullOrWhiteSpace(result.OutputMessages[3].Exception))
                    {
                        GrpcArticlePageDetails expertReviewResponse = Serializer.ConvertBytesToMsg<GrpcArticlePageDetails>(result.OutputMessages[3].Payload);
                        if (expertReviewResponse != null && expertReviewResponse.PageList.IsNotNullOrEmpty())
                        {
                            model.ExpertReview = GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(expertReviewResponse);
                        }
                    }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
    }
}
