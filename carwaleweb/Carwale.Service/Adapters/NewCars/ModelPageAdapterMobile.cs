using AutoMapper;
using Carwale.BL.Experiments;
using Carwale.DTOs.Campaigns;
using Carwale.DTOs.CarData;
using Carwale.DTOs.Dealer;
using Carwale.DTOs.Geolocation;
using Carwale.DTOs.NewCars;
using Carwale.DTOs.OfferAndDealerAd;
using Carwale.DTOs.OffersV1;
using Carwale.DTOs.PageProperty;
using Carwale.DTOs.PriceQuote;
using Carwale.DTOs.Template;
using Carwale.Entity;
using Carwale.Entity.AdapterModels;
using Carwale.Entity.Campaigns;
using Carwale.Entity.CarData;
using Carwale.Entity.CMS;
using Carwale.Entity.CMS.Photos;
using Carwale.Entity.CMS.URIs;
using Carwale.Entity.Dealers;
using Carwale.Entity.Enum;
using Carwale.Entity.Geolocation;
using Carwale.Entity.OffersV1;
using Carwale.Entity.PriceQuote;
using Carwale.Interfaces.Campaigns;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.CMS;
using Carwale.Interfaces.CMS.Photos;
using Carwale.Interfaces.Dealers;
using Carwale.Interfaces.Deals;
using Carwale.Interfaces.Geolocation;
using Carwale.Interfaces.NewCars;
using Carwale.Interfaces.Offers;
using Carwale.Interfaces.PriceQuote;
using Carwale.Interfaces.Prices;
using Carwale.Notifications.Logs;
using Carwale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Carwale.DAL.ApiGateway.Extensions;
using Carwale.DAL.ApiGateway;
using EditCMSWindowsService.Messages;
using Carwale.BL.GrpcFiles;
using Carwale.DTOs.LeadForm;
using Carwale.Adapters.Cms;
using Carwale.DTOs.CMS.Articles;
using ProtoBufClass.Campaigns;
using Carwale.Adapters.NewCars;
using Carwale.Adapters.Offers;

namespace Carwale.Service.Adapters.NewCars
{
    /// <summary>
    /// Created By : Shalini on 30/10/14
    /// </summary>
    public class ModelPageAdapterMobile : IServiceAdapterV2
    {
        private readonly ICarModelCacheRepository _carModelsCacheRepo;
        private readonly ICarVersions _carVersionsBL;
        private readonly ICarModels _carModelsBL;
        private readonly IDeals _carDeals;
        private readonly ICarPriceQuoteAdapter _iPrices;
        private readonly INewCarDealers _newCarDealersBL;
        private readonly IMediaBL _media;
        private readonly IPhotos _photos;
        private readonly IVideosBL _videos;
        private readonly ICampaign _campaign;
        private readonly ITemplate _campaignTemplate;
        private readonly IGeoCitiesCacheRepository _geoCitiesCacheRepository;
        private readonly ICarMileage _carMileage;
        private readonly IDealerAdProvider _dealerAdProviderBl;
        private readonly IEmiCalculatorAdapter _emiCalculatorAdapter;
        private readonly IOffersAdapter _offersAdapter;
        private const short _whatsNewExpiry = 90;


        public ModelPageAdapterMobile(IPhotos photos, IVideosBL videos, ITemplate campaignTemplate, IGeoCitiesCacheRepository geoCitiesCacheRepository,
                        ICarMileage carMileage, ICarModels carModelBl, ICarModelCacheRepository carModelCacheRepo, ICarPriceQuoteAdapter priceQuoteAdapter,
                           ICarVersions carVersionsBl, IDeals carDeals, INewCarDealers newCarDealersBl, IMediaBL mediaBl, ICampaign campaign,
                        IDealerAdProvider dealerAdProviderBl, IEmiCalculatorAdapter emiCalculatorAdapter, IOffersAdapter offersAdapter)
        {
            try
            {
                _carModelsBL = carModelBl;
                _carModelsCacheRepo = carModelCacheRepo;
                _iPrices = priceQuoteAdapter;
                _carVersionsBL = carVersionsBl;
                _carDeals = carDeals;
                _newCarDealersBL = newCarDealersBl;
                _media = mediaBl;
                _photos = photos;
                _videos = videos;
                _campaign = campaign;
                _campaignTemplate = campaignTemplate;
                _geoCitiesCacheRepository = geoCitiesCacheRepository;
                _dealerAdProviderBl = dealerAdProviderBl;
                _carMileage = carMileage;
                _emiCalculatorAdapter = emiCalculatorAdapter;
                _offersAdapter = offersAdapter;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        public T Get<T, U>(U input)
        {
            return (T)Convert.ChangeType(GetModelPageDTOForMobile(input), typeof(T));
        }

        private ModelPageDTO_Mobile GetModelPageDTOForMobile<U>(U input)
        {
            try
            {
                CarDataAdapterInputs inputParam = (CarDataAdapterInputs)Convert.ChangeType(input, typeof(U));
                var modelDetails = _carModelsCacheRepo.GetModelDetailsById(inputParam.ModelDetails.ModelId);
                var apiGatewayCaller = new ApiGatewayCaller();
                if (modelDetails.MakeId > 0)
                {
                    var modelPageDTO = new ModelPageDTO_Mobile();
                    modelPageDTO.ModelDetails = modelDetails;
                    modelPageDTO.CityDetails = Mapper.Map<CitiesDTO>(_geoCitiesCacheRepository.GetCityDetailsById(inputParam.CustLocation.CityId));

                    ModelPhotoListAdapter _modelPhotoListAdapter;
                    ContentDetailsByCategoryAdapter _whatsNewAdapter;
                    ContentDetailsByCategoryAdapter _prosConsAdapter;
                    ContentDetailsByCategoryAdapter _verdictAdapter;
                    CarSynopsisAdapter _carSynopsisAdapter;
                    MostRecentArticlesAdapter _modelExpertReviewListAdapter;
                    MostRecentArticlesAdapter _modelNewsListAdapter;
                    VideosByModelIdAdapter _modelVideoListAdapter;
                    VersionOfferAvailableAdapter _versionOfferAvailable;

                    _modelPhotoListAdapter = new ModelPhotoListAdapter();
                    _modelNewsListAdapter = new MostRecentArticlesAdapter();
                    _modelVideoListAdapter = new VideosByModelIdAdapter();
                    _modelExpertReviewListAdapter = new MostRecentArticlesAdapter();
                    _carSynopsisAdapter = new CarSynopsisAdapter();
                    _whatsNewAdapter = new ContentDetailsByCategoryAdapter();
                    _prosConsAdapter = new ContentDetailsByCategoryAdapter();
                    _verdictAdapter = new ContentDetailsByCategoryAdapter();
                    _versionOfferAvailable = new VersionOfferAvailableAdapter();

                    int cityId = inputParam.CustLocation.CityId;
                    int stateId = (inputParam.CustLocation.CityId > 0 && modelPageDTO.CityDetails != null) ? modelPageDTO.CityDetails.StateId : -1;

                    if (inputParam.ShowOfferUpfront)
                    {
                        _versionOfferAvailable = addCallVersionOfferAvailable(ref apiGatewayCaller, modelPageDTO.ModelDetails.ModelId, stateId, cityId);
                    }

                    _modelPhotoListAdapter.AddApiGatewayCallWithCallback(apiGatewayCaller, new ModelPhotosBycountURI
                    {
                        ApplicationId = (ushort)Application.CarWale,
                        CategoryIdList = "8,10",
                        ModelId = modelPageDTO.ModelDetails.ModelId,
                        PlatformId = Platform.CarwaleDesktop.ToString("D")
                    });
                    _prosConsAdapter.AddApiGatewayCallWithCallback(apiGatewayCaller, GetOpinionRequets(modelPageDTO.ModelDetails.ModelId, CmsContentCategory.ProsCons));
                    _verdictAdapter.AddApiGatewayCallWithCallback(apiGatewayCaller, GetOpinionRequets(modelPageDTO.ModelDetails.ModelId, CmsContentCategory.Verdict));
                    if (modelPageDTO.ModelDetails.ModelLaunchDate == null ||
                        (DateTime.Now.Date >= modelPageDTO.ModelDetails.ModelLaunchDate && DateTime.Now.Date <=
                         modelPageDTO.ModelDetails.ModelLaunchDate.Value.AddDays(_whatsNewExpiry)))
                    {
                        _whatsNewAdapter.AddApiGatewayCallWithCallback(apiGatewayCaller,
                            GetOpinionRequets(modelPageDTO.ModelDetails.ModelId, CmsContentCategory.WhatsNew));
                    }
                    _carSynopsisAdapter.AddApiGatewayCallWithCallback(apiGatewayCaller, modelPageDTO.ModelDetails.ModelId);

                    _modelVideoListAdapter.AddApiGatewayCallWithCallback(apiGatewayCaller, new VideosByIdURI
                    {
                        ModelId = modelPageDTO.ModelDetails.ModelId,
                        ApplicationId = (ushort)Application.CarWale,
                        StartIndex = 1,
                        EndIndex = 1000
                    });
                    _modelNewsListAdapter.AddApiGatewayCallWithCallback(apiGatewayCaller, new ArticleRecentURI()
                    {
                        ApplicationId = (ushort)Application.CarWale,
                        ContentTypes = CMSContentType.News.ToString("D"),
                        TotalRecords = 5,
                        ModelId = modelPageDTO.ModelDetails.ModelId
                    });

                    _modelExpertReviewListAdapter.AddApiGatewayCallWithCallback(apiGatewayCaller, new ArticleRecentURI()
                    {
                        ApplicationId = (ushort)CMSAppId.Carwale,
                        ContentTypes = CMSContentType.RoadTest.ToString("D"),
                        TotalRecords = 3,
                        ModelId = modelPageDTO.ModelDetails.ModelId
                    });

                    SimilarCarsAdapter _similarCarsAdapter = UnityBootstrapper.Resolve<SimilarCarsAdapter>("SimilarCarsAdapter");
                    if (modelPageDTO.ModelDetails.New || modelPageDTO.ModelDetails.Futuristic)
                    {
                        var similarCarModels = _carModelsBL.GetSimilarCarsByModel(modelDetails.ModelId, inputParam.CwcCookie);
                        if (similarCarModels.IsNotNullOrEmpty())
                        {
                            var similarCarVmRequest = new SimilarCarVmRequest
                            {
                                ModelId = modelDetails.ModelId,
                                MakeName = modelDetails.MakeName,
                                ModelName = modelDetails.ModelName,
                                MaskingName = modelDetails.MaskingName,
                                CityId = cityId,
                                StateId = stateId,
                                WidgetSource = WidgetSource.ModelPageAlternativeWidgetCompareCarLinkMobile,
                                PageName = "ModelPage",
                                IsMobile = true,
                                IsFuturistic = modelDetails.Futuristic,
                                CwcCookie = inputParam.CwcCookie,
                                SimilarCarModelList = similarCarModels
                            };
                            _similarCarsAdapter.AddApiGatewayCall(apiGatewayCaller, similarCarVmRequest);
                        }
                    }

                    apiGatewayCaller.Call();

                    List<NewCarVersionsDTO> newCarVersions = null;
                    if (modelPageDTO.ModelDetails.New || (!modelPageDTO.ModelDetails.Futuristic)) //new or discontinue car
                    {
                        if (modelPageDTO.ModelDetails.New)
                        {
                            var versionDetailsList = _carVersionsBL.GetCarVersions(inputParam.ModelDetails.ModelId, Entity.Status.New);
                            //get version for model and map to respective DTO
                            newCarVersions = _carVersionsBL.MapCarVersionDtoWithCarVersionEntity(inputParam.ModelDetails.ModelId, inputParam.CustLocation.CityId, true, versionDetailsList);
                            modelPageDTO.NewCarVersions = Mapper.Map<List<NewCarVersionsDTO>, List<NewCarVersionsDTOV2>>(newCarVersions);
                            VariantListDTO variantList = new VariantListDTO();
                            variantList.FuelTypes = newCarVersions.Equals(null) ? new List<string>() : newCarVersions.Select(x => x.CarFuelType).Distinct().ToList();
                            variantList.TransmissionTypes = newCarVersions.Equals(null) ? new List<string>() : newCarVersions.Select(x => x.TransmissionType).Distinct().ToList();
                            modelPageDTO.VariantList = variantList;
                            if (newCarVersions != null && newCarVersions.Count > 0 && newCarVersions[0].CarPriceOverview != null && newCarVersions[0].CarPriceOverview.Price > 0)
                            {
                                modelPageDTO.ModelDetails.MinPrice = newCarVersions.Where(c => c.CarPriceOverview.Price > 0).Min(x => x.CarPriceOverview.Price);
                                modelPageDTO.ModelDetails.MaxPrice = newCarVersions.Max(x => x.CarPriceOverview.Price);
                            }

                            IEnumerable<int> offerVersionList = null;
                            if (inputParam.ShowOfferUpfront)
                            {
                                offerVersionList = _versionOfferAvailable.Output;
                            }
                            SetDefaultVersion(newCarVersions, modelPageDTO, inputParam, offerVersionList);
                            modelPageDTO.IsRenaultLeadCampaign = modelPageDTO.ModelDetails.ModelId.Equals(CWConfiguration.RenaultLeadFormModelId);
                            modelPageDTO.MileageData = _carMileage.GetMileageData(versionDetailsList);

                            modelPageDTO.OfferAndDealerAd = new OfferAndDealerAdDTO();
                            modelPageDTO.OfferAndDealerAd.CarDetails = Mapper.Map<CarDetailsDTO>(modelDetails);
                            modelPageDTO.OfferAndDealerAd.Page = Pages.ModelPage;
                            modelPageDTO.OfferAndDealerAd.Platform = "Msite";

                            if (modelPageDTO.ModelDetails != null && modelPageDTO.CarVersion != null && modelPageDTO.CarPriceOverview != null && modelPageDTO.CarPriceOverview.Price > 0
                                && !inputParam.IsAmp)
                            {
                                SetOffers(modelPageDTO);
                            }

                            SetLeadSource(modelPageDTO);
                            SetCampaignsVm(modelPageDTO, inputParam);
                            modelPageDTO.OfferAndDealerAd.Location = Mapper.Map<CityAreaDTO>(inputParam.CustLocation);
                        }

                        modelPageDTO.UpgradedModelDetails = _carModelsCacheRepo.GetUpgradedModel(modelDetails.ModelId);
                        modelPageDTO.AdvantageAdData = _carDeals.IsShowDeals(inputParam.CustLocation.CityId, true) ? _carDeals.GetAdvantageAdContent(inputParam.ModelDetails.ModelId,
                            (inputParam.CustLocation.CityId > 0 ? inputParam.CustLocation.CityId : 0), modelPageDTO.ModelDetails.SubSegmentId) : null;
                    }
                    else if (modelPageDTO.ModelDetails.Futuristic)
                    {
                        modelPageDTO.UpcomingCarDetails = _carModelsCacheRepo.GetUpcomingCarDetails(inputParam.ModelDetails.ModelId);
                        GetUpcomingCarVm(modelPageDTO, inputParam);
                        SetUpcomingModelCampaign(modelPageDTO);
                    }
                    modelPageDTO.SelectedVersions = SetComparisionCarVm(inputParam, newCarVersions);
                    modelPageDTO.ModelMenu = new ModelMenuDTO { ActiveSection = ModelMenuEnum.Overview };
                    modelPageDTO.MobileOverviewDetails = ModelOverview(modelPageDTO, inputParam.AbTest, inputParam.CustLocation);
                    modelPageDTO.CallSlugInfo = SetCallSlugVm(modelPageDTO);
                    GetPageMetaTags(modelPageDTO);

                    
                    if (modelPageDTO.ModelDetails.New)
                    {
                        var processVersionData = _carVersionsBL.ProcessVersionsData(inputParam.ModelDetails.ModelId, newCarVersions);
                        modelPageDTO.Summary = _carModelsBL.Summary(modelPageDTO.PageMetaTags.Summary, modelPageDTO.ModelDetails.MakeName, modelPageDTO.ModelDetails.ModelName,
                            processVersionData, newCarVersions, inputParam.IsCityPage, inputParam.CustLocation.CityName);
                        SetVariantList(modelPageDTO);
                    }
                    List<ModelImage> modelPhotos = _modelPhotoListAdapter.Output;

                    modelPageDTO.ModelDetails.PhotoCount = modelPhotos.IsNotNullOrEmpty() ? modelPhotos.Count : 0;
                    modelPageDTO.ModelPhotosListCarousel = _media.GetModelCarouselImages(modelPhotos);
                    modelPageDTO.ModelPhotosList = _media.GetModelImages(modelPhotos);

                    List<Video> modelVideos = _modelVideoListAdapter.Output;

                    modelPageDTO.ExpertReviews = _modelExpertReviewListAdapter.Output;
                    modelPageDTO.ExpertReviewOpinion = _carSynopsisAdapter.Output;
                    modelPageDTO.Verdict = _verdictAdapter.Output;
                    modelPageDTO.WhatsNew = _whatsNewAdapter.Output;
                    modelPageDTO.SimilarCars = _similarCarsAdapter.Output;
                    if (modelPageDTO.WhatsNew != null && modelPageDTO.WhatsNew.PageList.IsNotNullOrEmpty())
                    {
                        modelPageDTO.WhatsNew.SmallDescription = StringUtility.GetHtmlSubString(modelPageDTO.WhatsNew.PageList[0].Content, 175);
                    }
                    if (modelPageDTO.Verdict != null && modelPageDTO.Verdict.PageList.IsNotNullOrEmpty())
                    {
                        modelPageDTO.Verdict.SmallDescription = StringUtility.GetHtmlSubString(modelPageDTO.Verdict.PageList[0].Content, 175);
                    }
                    modelPageDTO.ProsCons = _prosConsAdapter.Output;
                    modelPageDTO.TopNews = _modelNewsListAdapter.Output;

                    modelPageDTO.ModelVideos = modelVideos;
                    modelPageDTO.ModelDetails.VideoCount = modelVideos.IsNotNullOrEmpty() ? modelVideos.Count : 0;
                    if (Array.BinarySearch(CWConfiguration.BestCarsBodyTypes, modelDetails.BodyStyleId) >= 0)
                    {
                        Dictionary<CarBodyStyle, Tuple<int[], string>> carRanksByBodyType = _carModelsCacheRepo
                                                        .GetCarRanksByBodyType(ConfigurationManager.AppSettings["BestCarsBodyTypes"] ?? "1,3,6,10", CWConfiguration.TopCarByBodyTypeCount);
                        if (carRanksByBodyType != null)
                        {
                            int[] currentBodyStyleRanks = carRanksByBodyType[(CarBodyStyle)modelDetails.BodyStyleId].Item1;
                            modelPageDTO.NoOfTopCarsInBodyType = currentBodyStyleRanks.Length;
                            int rank = Array.IndexOf(currentBodyStyleRanks, modelDetails.ModelId);
                            if (rank >= 0)
                            {
                                modelPageDTO.Rank = rank + 1;
                            }
                        }
                    }
                    modelPageDTO.ModelColors = _carModelsCacheRepo.GetModelColorsByModel(modelDetails.ModelId);
                    modelPageDTO.ModelDetails.ModelColors = modelPageDTO.ModelColors;
                    modelPageDTO.ModelDetails.IsModelColorPhotosAvailable = _media.IsModelColorPhotosPresent(modelPageDTO.ModelDetails.ModelColors);
                    if (modelPageDTO.ModelDetails != null
                         && modelPageDTO.ModelDetails.New
                         && modelPageDTO.ModelDetails.ReplacedModelId > 0
                         && DateTimeUtility.ShowReplaceModel(modelPageDTO.ModelDetails.ModelLaunchDate, false, _whatsNewExpiry))
                    {
                        modelPageDTO.MobileOverviewDetails.ReplacedModelDetails = _carModelsBL.GetReplacedModelDetails(modelPageDTO.ModelDetails.ReplacedModelId, modelPageDTO.ModelDetails, true);
                    }
                    inputParam.CampaignInput = new CampaignInputv2
                    {
                        ModelId = modelDetails.ModelId,
                        MakeId = modelDetails.MakeId,
                        PlatformId = (int)Platform.CarwaleMobile,
                        PageId = (int)CwPages.ModelMsite,
                        ApplicationId = (int)Application.CarWale,
                        CityId = inputParam.CustLocation.CityId
                    };
                    modelPageDTO.MobileOverviewDetails.CampaignTemplates = _campaignTemplate.GetTemplatesByPage(inputParam.CampaignInput, modelPageDTO.MobileOverviewDetails);
                    modelPageDTO.MobileOverviewDetails.IsToolTipCampaign = modelPageDTO.MobileOverviewDetails.CampaignTemplates != null
                                && modelPageDTO.MobileOverviewDetails.CampaignTemplates.ContainsKey((int)PageProperties.ToolTip);
                    if (modelPageDTO.ModelDetails.New)
                    {
                        modelPageDTO.LeadFormModelData = SetLeadFormData(modelPageDTO.DealerAd);
                    }
                    return modelPageDTO;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return new ModelPageDTO_Mobile();
        }

        private VersionOfferAvailableAdapter addCallVersionOfferAvailable(ref ApiGatewayCaller apiGatewayCaller, int modelId, int stateId, int cityId)
        {
            var versionOfferAvailable = new VersionOfferAvailableAdapter();
            versionOfferAvailable.AddApiGatewayCallWithCallback(apiGatewayCaller, new OfferInput
            {
                ModelId = modelId,
                StateId = stateId,
                CityId = cityId,
                ApplicationId = (int)Application.CarWale
            });
            return versionOfferAvailable;
        }

        private void SetLeadSource(ModelPageDTO_Mobile modelPageDTO)
        {
            try
            {
                var leadSources = new List<LeadSource>();
                leadSources.Add(Carwale.BL.Campaigns.LeadSource.GetLeadSource("OfferCampaignSlugWithOffer", (short)LeadSources.Model, (short)Platform.CarwaleMobile));
                leadSources.Add(Carwale.BL.Campaigns.LeadSource.GetLeadSource("OfferCampaignSlugWithOfferReco", (short)LeadSources.Model, (short)Platform.CarwaleMobile));
                leadSources.Add(Carwale.BL.Campaigns.LeadSource.GetLeadSource("OfferCampaignSlugWithoutOffer", (short)LeadSources.Model, (short)Platform.CarwaleMobile));
                leadSources.Add(Carwale.BL.Campaigns.LeadSource.GetLeadSource("OfferCampaignSlugWithoutOfferReco", (short)LeadSources.Model, (short)Platform.CarwaleMobile));

                modelPageDTO.OfferAndDealerAd.LeadSource.AddRange(Mapper.Map<List<LeadSourceDTO>>(leadSources));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        private void SetOffers(ModelPageDTO_Mobile modelPageDTO)
        {
            try
            {
                var offerInput = new OfferInput
                {
                    ApplicationId = (int)Application.CarWale,
                    MakeId = modelPageDTO.ModelDetails.MakeId,
                    ModelId = modelPageDTO.ModelDetails.ModelId,
                    VersionId = modelPageDTO.CarVersion.ID
                };
                if (modelPageDTO.CityDetails == null || modelPageDTO.CityDetails.CityId < 1)
                {
                    offerInput.CityId = -1;
                    offerInput.StateId = -1;
                }
                else
                {
                    offerInput.CityId = modelPageDTO.CityDetails.CityId;
                    offerInput.StateId = modelPageDTO.CityDetails.StateId;
                }

                modelPageDTO.OfferAndDealerAd.Offer = _offersAdapter.GetOffers(offerInput);
                modelPageDTO.OfferAndDealerAd.Page = Pages.ModelPage;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        private void SetCampaignsVm(ModelPageDTO_Mobile modelPageDTO, CarDataAdapterInputs inputParam)
        {
            try
            {
                //Fetching campaign
                var carDetails = new CarIdEntity { MakeId = modelPageDTO.ModelDetails.MakeId, ModelId = inputParam.ModelDetails.ModelId };
                var persistedCampaignId = _campaign.GetPersistedCampaign(inputParam.ModelDetails.ModelId, inputParam.CustLocation);
                var dealerAd = _dealerAdProviderBl.GetDealerAd(carDetails, inputParam.CustLocation, Convert.ToInt32(Platform.CarwaleMobile), (int)CampaignAdType.Pq, persistedCampaignId, (int)Pages.ModelPageId);
                modelPageDTO.OfferAndDealerAd.DealerAd = Mapper.Map<DealerAdDTO>(dealerAd);

                if (dealerAd != null && dealerAd.Campaign != null)
                {
                    //Mapping to old object to make the exiting code work
                    modelPageDTO.DealerAd = Mapper.Map<DealerAdDTO>(dealerAd);
                    modelPageDTO.OfferAndDealerAd.DealerAd = modelPageDTO.DealerAd;


                    //Getting dealer details
                    var dealerDetails = _newCarDealersBL.NCDDetails(modelPageDTO.DealerAd.Campaign.DealerId, modelPageDTO.DealerAd.Campaign.Id, modelPageDTO.ModelDetails.MakeId, inputParam.CustLocation.CityId);

                    if (dealerDetails.CampaignId > 0)
                    {
                        modelPageDTO.OfferAndDealerAd.DealerAd.Campaign.DealerShowroom = AutoMapper.Mapper.Map<Entity.Dealers.DealerDetails, DealersDTO>(dealerDetails);
                    }
                    
                }
                else
                {
                    //Handling case when only city is set and campaign is available for atleast one area
                    modelPageDTO.ShowCampaignLink = _campaign.IsCityCampaignExist(inputParam.ModelDetails.ModelId, inputParam.CustLocation,
                        (int)Platform.CarwaleMobile, (int)Application.CarWale);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        private void SetUpcomingModelCampaign(ModelPageDTO_Mobile modelPageDTO)
        {
            if(modelPageDTO.ModelDetails.ModelId == CWConfiguration.MahindraAlturasModelid)
            {
                modelPageDTO.DealerAd = new DealerAdDTO();
                modelPageDTO.DealerAd.Campaign = new CampaignDTO
                {
                    Id = CWConfiguration.MahindraAlturasCampaignid,
                    ContactName = "Mahindra India",
                    LeadPanel = 1 //for ES campaigns
                };
            }
        }

        private List<CarVersionDetails> SetComparisionCarVm(CarDataAdapterInputs inputParam, List<NewCarVersionsDTO> newCarVersions)
        {
            var selectedVersions = _carVersionsBL.GetSelectedVersionDetails(inputParam.CompareVersionsCookie);

            if (selectedVersions.IsNotNullOrEmpty())
            {
                foreach (var comparecar in selectedVersions)
                {
                    if (comparecar.ModelId > 0)
                    {
                        if (comparecar.ModelId == inputParam.ModelDetails.ModelId && newCarVersions != null)
                        {
                            var version = newCarVersions.First(x => x.Id == comparecar.VersionId);
                            comparecar.MinPrice = version?.CarPriceOverview?.Price ?? 0;
                        }
                        else
                        {
                            var versionList = new List<int>();
                            versionList.Add(comparecar.VersionId);
                            var verPrices = _iPrices.GetVersionsPriceForSameModel(comparecar.ModelId, versionList, inputParam.CustLocation.CityId);
                            comparecar.MinPrice = (verPrices != null && verPrices[comparecar.VersionId] != null) ? verPrices[comparecar.VersionId].Price : 0;
                        }
                    }
                }
            }
            return selectedVersions;
        }

        private void GetUpcomingCarVm(ModelPageDTO_Mobile modelPageDTO, CarDataAdapterInputs inputParam)
        {
            try
            {
                List<NewCarVersionsDTO> versionList = _carVersionsBL.MapUpcomingVersionDTOWithEntity
                                                              (
                                                                inputParam.ModelDetails.ModelId,
                                                                inputParam.CustLocation.CityId,
                                                                inputParam.IsCityPage
                                                              );
                modelPageDTO.NewCarVersions = Mapper.Map<List<NewCarVersionsDTO>, List<NewCarVersionsDTOV2>>(
                                                versionList
                                              );
                modelPageDTO.ShowCompareColumn = modelPageDTO.NewCarVersions != null && modelPageDTO.NewCarVersions.FindAll(x => x.IsSpecsExist).Count > 0;
                modelPageDTO.ShowPriceColumn = modelPageDTO.NewCarVersions != null && modelPageDTO.NewCarVersions.Any(x => x.CarPriceOverview != null);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "ModelPageAdapterMobile.GetUpcomingCarVm");
            }
        }

        private void GetPageMetaTags(ModelPageDTO_Mobile modelDetails)
        {
            try
            {
                modelDetails.PageMetaTags = _carModelsCacheRepo.GetModelPageMetaTags(modelDetails.ModelDetails.ModelId, Convert.ToInt16(Pages.ModelPageId));
                if (modelDetails.PageMetaTags != null)
                {
                    modelDetails.PageMetaTags.Title = _carModelsBL.Title(modelDetails.PageMetaTags.Title, modelDetails.ModelDetails.MakeName, modelDetails.ModelDetails.ModelName);
                    modelDetails.PageMetaTags.Description = GetDescription(modelDetails);
                    modelDetails.PageMetaTags.Heading = _carModelsBL.Heading(modelDetails.PageMetaTags.Heading, modelDetails.ModelDetails.MakeName, modelDetails.ModelDetails.ModelName);
                    modelDetails.PageMetaTags.Canonical = ManageCarUrl.CreateModelUrl(modelDetails.ModelDetails.MakeName, modelDetails.ModelDetails.MaskingName, true);
                    modelDetails.PageMetaTags.Alternate = modelDetails.PageMetaTags.Canonical;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

        }
        private string GetDescription(ModelPageDTO_Mobile modelPageDTO_Mobile)
        {
            try
            {
                var modelDetails = modelPageDTO_Mobile.ModelDetails;
                string modelPrice;
                if (modelPageDTO_Mobile.ModelDetails.New && modelPageDTO_Mobile.CarPriceOverview != null)
                {
                    modelPrice = Format.GetFormattedPriceV2(modelPageDTO_Mobile.CarPriceOverview.Price.ToString());
                }
                else if (modelPageDTO_Mobile.ModelDetails.Futuristic)
                {
                    modelPrice = modelPageDTO_Mobile.MobileOverviewDetails.EstimatedPrice;
                }
                else
                {
                    modelPrice = Format.GetFormattedPriceV2(modelPageDTO_Mobile.ModelDetails.MinPrice.ToString());
                }
                return _carModelsBL.GetDescription(modelDetails.MakeName, modelDetails.ModelName, modelPrice);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return string.Empty;
            }
        }

        public CarOverviewDTOV2 ModelOverview(ModelPageDTO_Mobile modelPageDTO_Mobile, int abTest, Location location)
        {
            try
            {
                CarOverviewDTOV2 overview = new CarOverviewDTOV2();
                var modelDetailsObj = modelPageDTO_Mobile.ModelDetails;
                overview.New = modelDetailsObj.New;
                overview.Futuristic = modelDetailsObj.Futuristic;
                overview.Discontinue = !(modelDetailsObj.Futuristic || modelDetailsObj.New);
                overview.HostUrl = modelDetailsObj.HostUrl;
                overview.OriginalImage = modelDetailsObj.OriginalImage;
                overview.ModelName = modelDetailsObj.ModelName;
                overview.CarName = modelDetailsObj.MakeName + " " + modelDetailsObj.ModelName;
                overview.MakeName = modelDetailsObj.MakeName;
                overview.ModelId = modelDetailsObj.ModelId;
                overview.MakeId = modelDetailsObj.MakeId;
                overview.MinPrice = modelDetailsObj.MinPrice;
                overview.MaxPrice = modelDetailsObj.MaxPrice;
                overview.MinAvgPrice = modelDetailsObj.MinAvgPrice;
                overview.MaskingName = modelDetailsObj.MaskingName;
                overview.EMI = modelPageDTO_Mobile.CarPriceOverview != null ? Carwale.BL.Calculation.Calculation.CalculateEmi(modelPageDTO_Mobile.CarPriceOverview.Price) : string.Empty;
                overview.PhotoCount = modelPageDTO_Mobile.ModelPhotosList != null ? modelPageDTO_Mobile.ModelPhotosList.Count : 0;
                overview.IsVersionPage = false;
                overview.MaxDiscount = modelPageDTO_Mobile.AdvantageAdData != null ? modelPageDTO_Mobile.AdvantageAdData.Savings : 0;
                overview.StockMaskingName = modelPageDTO_Mobile.AdvantageAdData != null ? modelPageDTO_Mobile.AdvantageAdData.Model.MaskingName : string.Empty;
                overview.DealsCityId = modelPageDTO_Mobile.AdvantageAdData != null ? modelPageDTO_Mobile.AdvantageAdData.City.CityId : 0;
                overview.DealsModelName = modelPageDTO_Mobile.AdvantageAdData != null ? modelPageDTO_Mobile.AdvantageAdData.Model.ModelName : string.Empty;
                overview.AdvantageAdData = modelPageDTO_Mobile.AdvantageAdData;
                overview.CarPriceOverview = modelPageDTO_Mobile.CarPriceOverview != null ? AutoMapper.Mapper.Map<PriceOverview, PriceOverviewDTOV2>(modelPageDTO_Mobile.CarPriceOverview) : null;
                if (modelPageDTO_Mobile.CarVersion != null)
                {
                    overview.VersionId = modelPageDTO_Mobile.CarVersion.ID;
                    overview.VersionName = modelPageDTO_Mobile.CarVersion.Name;
                }
                overview.AdAvailable = false;

                //TBD - where is PredictionData used when DealerAd is null
                overview.PredictionData = new PredictionData();
                if (modelPageDTO_Mobile.DealerAd != null)
                {
                    overview.AdAvailable = modelPageDTO_Mobile.DealerAd.Campaign.Id > 1;
                    overview.PredictionData = modelPageDTO_Mobile.DealerAd.Campaign.PredictionData;
                } 
                SetLeadCta(overview, modelPageDTO_Mobile, location.CityId);

                SetEmiCalculatorLinks(overview);
                overview.UserLocation = Mapper.Map<CityAreaDTO>(location);
                if (modelDetailsObj.Futuristic && modelPageDTO_Mobile.UpcomingCarDetails != null)
                {
                    overview.ExpectedLaunch = modelPageDTO_Mobile.UpcomingCarDetails.ExpectedLaunch;
                    if (modelPageDTO_Mobile.UpcomingCarDetails.Price != null)
                        overview.EstimatedPrice = Format.GetUpcomingFormatPrice(modelPageDTO_Mobile.UpcomingCarDetails.Price.MinPrice, modelPageDTO_Mobile.UpcomingCarDetails.Price.MinPrice);
                }
                overview.PQPageId = 124;
                overview.ShowCampaignLink = modelPageDTO_Mobile.ShowCampaignLink;

                if (overview.New && !overview.Futuristic && location.CityId > 0)
                {
                    var dealerAd = modelPageDTO_Mobile.OfferAndDealerAd != null ? modelPageDTO_Mobile.OfferAndDealerAd.DealerAd : null;
                    var price = modelPageDTO_Mobile.CarPriceOverview != null ? modelPageDTO_Mobile.CarPriceOverview.Price : 0;
                    overview.EmiCalculatorModelData = _emiCalculatorAdapter.GetEmiData(overview, dealerAd,
                                                        new LeadSourceDTO { LeadClickSourceId = 393, InquirySourceId = 154 }, price, location.CityId);
                    if (overview.EmiCalculatorModelData != null)
                    {
                        overview.EmiCalculatorModelData.Page = CwPages.ModelMsite;
                        overview.EmiCalculatorModelData.PageName = CwPages.ModelMsite.ToString();
                        overview.EmiCalculatorModelData.Platform = "Msite";
                    }
                }
                return overview;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return null;
            }
        }

        private static void SetEmiCalculatorLinks(CarOverviewDTOV2 overview)
        {
            if (overview == null)
            {
                return;
            }
            overview.ShowCustomiseYourEmiButton = ProductExperiments.ShowCustomiseYourEmiButton();
            overview.ShowChangeTextLink = ProductExperiments.ShowChangeTextLink();
        }

        private static void SetLeadCta(CarOverviewDTOV2 overview, ModelPageDTO_Mobile modelPageDTO_Mobile, int cityId)
        {
            if (overview == null)
            {
                return;
            }
            if (modelPageDTO_Mobile != null && modelPageDTO_Mobile.DealerAd != null)
            {
                if (ProductExperiments.ShowGetEmiAssistanceLink() || cityId < 1)
                {
                    overview.LeadCTA = "Get EMI Assistance";
                }
            }
        }

        public CallSlugDTO SetCallSlugVm(ModelPageDTO_Mobile modelPageDTO_Mobile)
        {
            CallSlugDTO callSlug = new CallSlugDTO();
            var modelDetailsObj = modelPageDTO_Mobile.ModelDetails;

            callSlug.CarName = modelDetailsObj.MakeName + "_" + modelDetailsObj.ModelName;
            callSlug.ModelId = modelDetailsObj.ModelId;
            callSlug.VersionId = 0;
            callSlug.IsAdAvailable = false;
            callSlug.DealerName = string.Empty;
            callSlug.DealerMobile = string.Empty;
            if (modelPageDTO_Mobile.DealerAd != null)
            {
                callSlug.IsAdAvailable = modelPageDTO_Mobile.DealerAd.Campaign.Id > 1;
                callSlug.DealerName = callSlug.IsAdAvailable ? modelPageDTO_Mobile.DealerAd.Campaign.ContactName : callSlug.DealerName;
                callSlug.DealerMobile = callSlug.IsAdAvailable ? modelPageDTO_Mobile.DealerAd.Campaign.ContactNumber : callSlug.DealerMobile;
            }
            callSlug.AdvantageAdData = modelPageDTO_Mobile.AdvantageAdData;
            callSlug.DealerAd = modelPageDTO_Mobile.DealerAd;
            return callSlug;
        }
        private void SetDefaultVersion(IEnumerable<NewCarVersionsDTO> carVersionsDTOList, ModelPageDTO_Mobile modelPageDTO, 
                                         CarDataAdapterInputs inputParam, IEnumerable<int> offerVersionsList)
        {
            try
            {
                if (carVersionsDTOList.IsNotNullOrEmpty() && inputParam != null)
                {
                    var versionId = inputParam.ModelDetails?.VersionId ?? 0;
                    var defaultVersion = new NewCarVersionsDTO();
                    if (inputParam.ShowOfferUpfront && offerVersionsList.IsNotNullOrEmpty())
                    {
                        var versionOfferDict = offerVersionsList.ToDictionary(x => x, x => true);
                        bool isOffersOnAllVersions = versionOfferDict.ContainsKey(-1);
                        if(isOffersOnAllVersions)
                        {
                            defaultVersion = carVersionsDTOList.ElementAt(0);
                        }
                        else
                        {
                            defaultVersion = carVersionsDTOList.FirstOrDefault(x => versionOfferDict.ContainsKey(x.Id)) 
                                                                ?? carVersionsDTOList.ElementAt(0);
                        }
                    }
                    else
                    {
                        defaultVersion = carVersionsDTOList.FirstOrDefault(x => x.Id == versionId) ?? carVersionsDTOList.ElementAt(0);
                    }
                    modelPageDTO.CarVersion = new CarVersionsDTO
                    {
                        ID = defaultVersion.Id,
                        Name = defaultVersion.Version
                    };
                    modelPageDTO.CarPriceOverview = defaultVersion.CarPriceOverview ?? new PriceOverview();
                }
                else
                {
                    modelPageDTO.CarPriceOverview = new PriceOverview();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
        private void SetVariantList(ModelPageDTO_Mobile mobilepageDTO)
        {
            try
            {
                mobilepageDTO.VariantList.Summary = mobilepageDTO.Summary;
                mobilepageDTO.VariantList.DealerAd = mobilepageDTO.DealerAd;
                mobilepageDTO.VariantList.ModelDetails = mobilepageDTO.ModelDetails;
                mobilepageDTO.VariantList.MobileOverviewDetails = mobilepageDTO.MobileOverviewDetails;
                mobilepageDTO.VariantList.NewCarVersions = mobilepageDTO.NewCarVersions;
                mobilepageDTO.VariantList.ShowCampaignLink = mobilepageDTO.ShowCampaignLink;
                mobilepageDTO.VariantList.PageId = (int)CwPages.ModelMsite;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        private ArticleByCatURI GetOpinionRequets(int modelId, CmsContentCategory cmsContentCategory)
        {
            return new ArticleByCatURI
            {
                ModelId = modelId,
                CategoryIdList = cmsContentCategory.ToString("D"),
                ApplicationId = (ushort)Application.CarWale
            };
        }

        private LeadFormModelData SetLeadFormData(DealerAdDTO dealerAd)
        {
            LeadFormModelData leadFormModelData = new LeadFormModelData();
            if (dealerAd != null)
            {
                List<DealerAdDTO> dealerCampaignList = new List<DealerAdDTO>();
                dealerCampaignList.Add(dealerAd);
                leadFormModelData.DealerAd = dealerCampaignList;
            }
            return leadFormModelData;
        }
    }
}
