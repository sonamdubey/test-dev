using AutoMapper;
using Carwale.BL.Experiments;
using Carwale.DTOs.Campaigns;
using Carwale.DTOs.CarData;
using Carwale.DTOs.Dealer;
using Carwale.DTOs.Geolocation;
using Carwale.DTOs.LeadForm;
using Carwale.DTOs.NewCars;
using Carwale.DTOs.OfferAndDealerAd;
using Carwale.DTOs.OffersV1;
using Carwale.DTOs.PriceQuote;
using Carwale.Entity;
using Carwale.Entity.AdapterModels;
using Carwale.Entity.CarData;
using Carwale.Entity.CMS;
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
using Carwale.Interfaces.CompareCars;
using Carwale.Interfaces.Dealers;
using Carwale.Interfaces.Deals;
using Carwale.Interfaces.Geolocation;
using Carwale.Interfaces.NewCars;
using Carwale.Interfaces.Offers;
using Carwale.Interfaces.PriceQuote;
using Carwale.Interfaces.Prices;
using Carwale.Notifications.Logs;
using Carwale.Utility;
using System;
using System.Collections.Generic;

namespace Carwale.BL.NewCars
{
    /// <summary>
    /// Created By : Shalini on 30/12/14
    /// </summary>
    public class VersionPageAdapterMobile : IServiceAdapterV2
    {
        private readonly ICarVersionCacheRepository _carVersionsCacheRepo;
        private readonly ICompareCarsBL _compareCarsBL;
        private readonly ICarModelCacheRepository _carModelsCacheRepo;
        private readonly ICampaign _campaign;
        private readonly IDeals _cardeals;
        private readonly ICarPriceQuoteAdapter _iPrices;
        private readonly ICarModels _carModelsBL;
        private readonly ICarVersions _carVersionsBL;
        private readonly IPhotos _photos;
        private readonly IMediaBL _media;
        private readonly IVideosBL _videos;
        private readonly INewCarDealers _newCarDealer;
        private readonly ITemplate _campaignTemplate;
        private readonly ICarDataLogic _carDataLogic;
        private readonly IGeoCitiesCacheRepository _geoCitiesCacheRepository;
        private readonly IDealerAdProvider _dealerAdProviderBl;
        private readonly IOffersAdapter _offersAdapter;
        private readonly IEmiCalculatorAdapter _emiCalculatorAdapter;
        public VersionPageAdapterMobile(ICarVersionCacheRepository carVersionsCacheRepo,
                             ICompareCarsBL compareCarsBL, ICarModelCacheRepository carModelsCacheRepo, IDeals cardeals,
                             ICarPriceQuoteAdapter iPrices, IPhotos photos, IMediaBL media, ICarModels carModelsBL,
                             ICarVersions carVersionsBL, IVideosBL videos, ICampaign campaign, INewCarDealers newCarDealer, ITemplate campaignTemplate,
                             IGeoCitiesCacheRepository geoCitiesCacheRepository, ICarDataLogic carDataLogic, IDealerAdProvider dealerAdProviderBl,
                             IOffersAdapter offersAdapter, IEmiCalculatorAdapter emiCalculatorAdapter)
        {
            _carVersionsCacheRepo = carVersionsCacheRepo;
            _compareCarsBL = compareCarsBL;
            _carModelsCacheRepo = carModelsCacheRepo;
            _cardeals = cardeals;
            _iPrices = iPrices;
            _carModelsBL = carModelsBL;
            _carVersionsBL = carVersionsBL;
            _photos = photos;
            _media = media;
            _videos = videos;
            _campaign = campaign;
            _newCarDealer = newCarDealer;
            _campaignTemplate = campaignTemplate;
            _carDataLogic = carDataLogic;
            _geoCitiesCacheRepository = geoCitiesCacheRepository;
            _dealerAdProviderBl = dealerAdProviderBl;
            _offersAdapter = offersAdapter;
            _emiCalculatorAdapter = emiCalculatorAdapter;
        }

        public T Get<T, U>(U input)
        {
            return (T)Convert.ChangeType(GetVersionPageDTOForMobile(input), typeof(T));
        }
        private VersionPageDTO_Mobile GetVersionPageDTOForMobile<U>(U input)
        {
            try
            {
                VersionPageDTO_Mobile versionDTO;
                CarDataAdapterInputs inputParam = (CarDataAdapterInputs)Convert.ChangeType(input, typeof(U));
                var versionDetails = _carVersionsCacheRepo.GetVersionDetailsById(inputParam.ModelDetails.VersionId);
                List<int> ValVersionIDList = new List<int> { inputParam.ModelDetails.VersionId };
                IDictionary<int, PriceOverview> versionPrice = new Dictionary<int, PriceOverview>();
                if (Convert.ToBoolean(versionDetails.New) || versionDetails.Futuristic)
                {
                    versionPrice = _iPrices.GetVersionsPriceForSameModel(versionDetails.ModelId, ValVersionIDList, inputParam.CustLocation.CityId);
                }
                List<NewCarVersionsDTOV2> carVersions = versionDetails.Futuristic ? Mapper.Map<List<NewCarVersionsDTO>, List<NewCarVersionsDTOV2>>
                   (_carVersionsBL.MapUpcomingVersionDTOWithEntity(versionDetails.ModelId, inputParam.CustLocation.CityId)) : Mapper.Map<List<NewCarVersionsDTO>, List<NewCarVersionsDTOV2>>(
                       _carVersionsBL.MapCarVersionDtoWithCarVersionEntity(versionDetails.ModelId, inputParam.CustLocation.CityId));
                var versionData = _carDataLogic.GetCombinedCarData(ValVersionIDList);
                
                versionDTO = new VersionPageDTO_Mobile
                {
                    VersionDetails = versionDetails,

                    VersionData = Mapper.Map<CarDataPresentation, CCarDataDto>(versionData != null && versionData.Count > 0 ? versionData[0] : new CarDataPresentation()) ?? new CCarDataDto(),

                    ModelDetails = _carModelsCacheRepo.GetModelDetailsById(versionDetails.ModelId),

                    CarPriceOverview = versionPrice.ContainsKey(ValVersionIDList[0]) && versionPrice[ValVersionIDList[0]] != null ? versionPrice[ValVersionIDList[0]] : new PriceOverview(),

                    NewCarVersions = carVersions,

                    IsOtherVariantPriceAvailable = carVersions.FindAll(x => x.Id != versionDetails.VersionId && x.CarPriceOverview != null).Count > 0
                };
                if(versionDetails.Futuristic)
                {
                    versionDTO.UpcomingCarDetails = _carModelsCacheRepo.GetUpcomingCarDetails(versionDetails.ModelId);
                }
                versionDTO.VersionData.Colors = Mapper.Map<List<List<Entity.CompareCars.Color>>, List<List<Color>>>(_carVersionsBL.GetVersionsColors(ValVersionIDList));
                versionDTO.CityDetails = Mapper.Map<CitiesDTO>(_geoCitiesCacheRepository.GetCityDetailsById(inputParam.CustLocation.CityId));

                inputParam.ModelDetails.ModelId = versionDTO.ModelDetails.ModelId;
                versionDTO.IsRenaultLeadCampaign = versionDetails.ModelId.Equals(CWConfiguration.RenaultLeadFormModelId);

                if (versionDTO.VersionDetails.New.Equals(1))
                {
                    var similarCarVmRequest = new SimilarCarVmRequest
                    {
                        ModelId = versionDetails.ModelId,
                        MakeName = versionDetails.MakeName,
                        ModelName = versionDetails.ModelName,
                        MaskingName = versionDetails.MaskingName,
                        CityId = inputParam.CustLocation.CityId,
                        WidgetSource = WidgetSource.VersionPageAlternativeWidgetCompareCarLinkMobile,
                        PageName = "VersionPage",
                        IsMobile = true,
                        CwcCookie = inputParam.CwcCookie
                    };
                    versionDTO.SimilarCars = _carModelsBL.GetSimilarCarVm(similarCarVmRequest);

                    versionDTO.OfferAndDealerAd = new OfferAndDealerAdDTO();
                    versionDTO.OfferAndDealerAd.CarDetails = Mapper.Map<CarDetailsDTO>(versionDTO.ModelDetails);

                    if (versionDTO.OfferAndDealerAd.CarDetails != null)
                    {
                        versionDTO.OfferAndDealerAd.CarDetails.CarVersion = Mapper.Map<PQCarVersionDTO>(versionDetails);
                    }
                    versionDTO.OfferAndDealerAd.Platform = "Msite";

                    if (versionDTO.ModelDetails != null && inputParam?.ModelDetails?.VersionId > 0 && versionDTO.CarPriceOverview != null && versionDTO.CarPriceOverview.Price > 0)
                    {
                        SetOffers(versionDTO, inputParam);
                    }

                    SetLeadSource(versionDTO);
                    versionDTO.OfferAndDealerAd.Location = Mapper.Map<CityAreaDTO>(inputParam.CustLocation);
                }

                SetCampaignsVM(versionDTO, inputParam);

                var queryString = new ModelPhotosBycountURI
                {
                    ApplicationId = (ushort)CMSAppId.Carwale,
                    CategoryIdList = "8,10",
                    ModelId = versionDTO.VersionDetails.ModelId,
                    PlatformId = Platform.CarwaleDesktop.ToString("D")
                };
                var photos = _photos.GetModelPhotosByCount(queryString);
                versionDTO.ModelDetails.PhotoCount = photos != null ? photos.Count : 0;
                versionDTO.ModelPhotosListCarousel = _media.GetModelCarouselImages(photos, versionDTO.VersionDetails.HostURL, versionDTO.VersionDetails.OriginalImgPath);
                List<Video> modelVideos = _videos.GetVideosByModelId(versionDetails.ModelId, CMSAppId.Carwale, 1, -1);
                if (modelVideos != null)
                {
                    versionDTO.ModelDetails.VideoCount = modelVideos.Count;
                    versionDTO.ModelVideos = Mapper.Map<List<Video>, List<VideoDTO>>(modelVideos);
                }
                else
                {
                    versionDTO.ModelDetails.VideoCount = 0;
                }

                versionDTO.AdvantageAdData = _cardeals.IsShowDeals(inputParam.CustLocation.CityId, true) ?
                    _cardeals.GetAdvantageAdContent(versionDetails.ModelId, (inputParam.CustLocation.CityId > 0
                    ? inputParam.CustLocation.CityId : 0), versionDTO.ModelDetails.SubSegmentId, inputParam.CustLocation.CityId > 0
                    ? inputParam.ModelDetails.VersionId : 0) : null;
                versionDTO.ModelDetails.ModelColors = _carModelsCacheRepo.GetModelColorsByModel(versionDTO.ModelDetails.ModelId);
                versionDTO.ModelDetails.IsModelColorPhotosAvailable = _media.IsModelColorPhotosPresent(versionDTO.ModelDetails.ModelColors);
                versionDTO.MobileOverviewDetails = ModelOverview(versionDTO, inputParam.CustLocation);
                versionDTO.CallSlugInfo = BindCallSlug(versionDTO);

                inputParam.CampaignInput.MakeId = versionDTO.ModelDetails.MakeId;
                inputParam.CampaignInput.ModelId = versionDTO.ModelDetails.ModelId;
                versionDTO.MobileOverviewDetails.CampaignTemplates = _campaignTemplate.GetTemplatesByPage(inputParam.CampaignInput, versionDTO.MobileOverviewDetails);
                versionDTO.MobileOverviewDetails.IsToolTipCampaign = versionDTO.MobileOverviewDetails.CampaignTemplates != null 
                                && versionDTO.MobileOverviewDetails.CampaignTemplates.ContainsKey((int)PageProperties.ToolTip);
                versionDTO.VersionData.CampaignTemplates = versionDTO.MobileOverviewDetails.CampaignTemplates;
                versionDTO.ShowTyresLink = _carVersionsBL.CheckTyresExists(inputParam.ModelDetails.VersionId);
                versionDTO.LeadFormModelData = SetLeadFormData(versionDTO.DealerAd);
                return versionDTO;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return null;
            }
        }

        public CarOverviewDTOV2 ModelOverview(VersionPageDTO_Mobile versionDTO_Mobile, Location location)
        {
            CarOverviewDTOV2 overview = new CarOverviewDTOV2();
            try
            {
                var versionDetailsObj = versionDTO_Mobile.VersionDetails;
                overview.New = versionDetailsObj.New > 0;
                overview.Futuristic = versionDetailsObj.Futuristic;
                overview.Discontinue = !(versionDetailsObj.Futuristic || versionDetailsObj.New > 0);
                overview.HostUrl = versionDetailsObj.HostURL;
                overview.OriginalImage = versionDetailsObj.OriginalImgPath;
                overview.ModelName = versionDetailsObj.ModelName;
                overview.CarName = versionDetailsObj.MakeName + " " + versionDetailsObj.ModelName + " " + versionDetailsObj.VersionName;
                overview.MakeName = versionDetailsObj.MakeName;
                overview.MakeId = versionDetailsObj.MakeId;
                overview.ModelId = versionDetailsObj.ModelId;
                overview.MinPrice = versionDetailsObj.MinPrice;
                overview.MaxPrice = versionDetailsObj.MaxPrice;
                overview.MinAvgPrice = versionDetailsObj.MinAvgPrice;
                overview.MaskingName = versionDetailsObj.MaskingName;
                overview.PhotoCount = versionDTO_Mobile.ModelPhotosCount;
                overview.IsVersionPage = true;
                overview.EMI = versionDTO_Mobile.CarPriceOverview != null ? Calculation.Calculation.CalculateEmi(versionDTO_Mobile.CarPriceOverview.Price) : string.Empty;
                overview.VersionId = versionDetailsObj.VersionId;
                overview.VersionName = versionDetailsObj.VersionName;
                overview.ExpectedLaunch = versionDetailsObj.UpcomingExpectedLaunch ?? (versionDTO_Mobile.UpcomingCarDetails?.ExpectedLaunch ?? string.Empty);
                overview.EstimatedPrice = versionDTO_Mobile.CarPriceOverview != null && versionDTO_Mobile.CarPriceOverview.Price > 0 ? Format.FormatFullPrice(versionDTO_Mobile.CarPriceOverview.Price.ToString()) : (versionDTO_Mobile.UpcomingCarDetails?.Price != null ? Format.GetUpcomingFormatPrice(versionDTO_Mobile.UpcomingCarDetails.Price.MinPrice, versionDTO_Mobile.UpcomingCarDetails.Price.MinPrice) : string.Empty);
                overview.MaxDiscount = versionDTO_Mobile.AdvantageAdData != null ? versionDTO_Mobile.AdvantageAdData.Savings : 0;
                overview.StockMaskingName = versionDTO_Mobile.AdvantageAdData != null ? versionDTO_Mobile.AdvantageAdData.Model.MaskingName : string.Empty;
                overview.DealsCityId = versionDTO_Mobile.AdvantageAdData != null ? versionDTO_Mobile.AdvantageAdData.City.CityId : 0;
                overview.DealsModelName = versionDTO_Mobile.AdvantageAdData != null ? versionDTO_Mobile.AdvantageAdData.Model.ModelName : string.Empty;
                if (versionDTO_Mobile.DealerAd != null && versionDTO_Mobile.DealerAd.Campaign != null)
                {
                    overview.AdAvailable = versionDTO_Mobile.DealerAd.Campaign.Id > 0;
                    overview.PredictionData = Mapper.Map<PredictionData>(versionDTO_Mobile.DealerAd.Campaign.PredictionData);
                }
                overview.ShowCampaignLink = versionDTO_Mobile.ShowCampaignLink;
                SetLeadCta(overview, versionDTO_Mobile, location.CityId);
                overview.AdvantageAdData = versionDTO_Mobile.AdvantageAdData;
                overview.CarPriceOverview = Mapper.Map<PriceOverview, PriceOverviewDTOV2>(versionDTO_Mobile.CarPriceOverview);
                overview.UserLocation = Mapper.Map<CityAreaDTO>(location);
                overview.PQPageId = 124;

                if (overview.New && !overview.Futuristic && overview.UserLocation.CityId > 0)
                {
                    var dealerAd = versionDTO_Mobile.OfferAndDealerAd != null ? versionDTO_Mobile.OfferAndDealerAd.DealerAd : null;
                    var price = versionDTO_Mobile.CarPriceOverview != null ? versionDTO_Mobile.CarPriceOverview.Price : 0;
                    var emiCalculatorModelData = _emiCalculatorAdapter.GetEmiData(overview, dealerAd,
                                                        new LeadSourceDTO { LeadClickSourceId = 395, InquirySourceId = 156 }, price, location.CityId);
                    
                    if(emiCalculatorModelData != null && emiCalculatorModelData.IsEligibleForThirdPartyEmi && emiCalculatorModelData.ThirdPartyEmiDetails == null)
                        {
                            emiCalculatorModelData = null;
                        }

                    overview.EmiCalculatorModelData  = emiCalculatorModelData;
                    if (overview.EmiCalculatorModelData != null)
                    {
                        overview.EmiCalculatorModelData.Page = CwPages.VersionMsite;
                        overview.EmiCalculatorModelData.PageName = CwPages.VersionMsite.ToString();
                        overview.EmiCalculatorModelData.Platform = "Msite";
                    }
                }

                SetEmiCalculatorLinks(overview);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return overview;
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

        private static void SetLeadCta(CarOverviewDTOV2 overview, VersionPageDTO_Mobile versionDTO_Mobile, int cityId)
        {
            if (overview == null)
            {
                return;
            }
            if (versionDTO_Mobile != null && versionDTO_Mobile.DealerAd != null)
            {
                if (ProductExperiments.ShowGetEmiAssistanceLink() || cityId < 1)
                {
                    overview.LeadCTA = "Get EMI Assistance";
                }
            }
        }

        public CallSlugDTO BindCallSlug(VersionPageDTO_Mobile versionDTO_Mobile)
        {
            CallSlugDTO callSlug = new CallSlugDTO();
            try
            {
                var versionDetailsObj = versionDTO_Mobile.VersionDetails;
                callSlug.CarName = versionDetailsObj.MakeName + "_" + versionDetailsObj.ModelName + "_" + versionDetailsObj.VersionName;
                callSlug.ModelId = versionDetailsObj.ModelId;
                callSlug.VersionId = versionDetailsObj.VersionId;
                callSlug.IsAdAvailable = versionDTO_Mobile.MobileOverviewDetails.AdAvailable;
                callSlug.DealerName = callSlug.IsAdAvailable ? versionDTO_Mobile.DealerAd.Campaign.ContactName : string.Empty;
                callSlug.DealerMobile = callSlug.IsAdAvailable ? versionDTO_Mobile.DealerAd.Campaign.ContactNumber : string.Empty;
                callSlug.AdvantageAdData = versionDTO_Mobile.AdvantageAdData;
                callSlug.DealerAd = versionDTO_Mobile.DealerAd;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return callSlug;
        }

        private void SetLeadSource(VersionPageDTO_Mobile modelPageDTO)
        {
            try
            {
                var leadSources = new List<LeadSource>();
                leadSources.Add(Carwale.BL.Campaigns.LeadSource.GetLeadSource("OfferCampaignSlugWithOffer", (short)LeadSources.Version, (short)Platform.CarwaleMobile));
                leadSources.Add(Carwale.BL.Campaigns.LeadSource.GetLeadSource("OfferCampaignSlugWithOfferReco", (short)LeadSources.Version, (short)Platform.CarwaleMobile));
                leadSources.Add(Carwale.BL.Campaigns.LeadSource.GetLeadSource("OfferCampaignSlugWithoutOffer", (short)LeadSources.Version, (short)Platform.CarwaleMobile));
                leadSources.Add(Carwale.BL.Campaigns.LeadSource.GetLeadSource("OfferCampaignSlugWithoutOfferReco", (short)LeadSources.Version, (short)Platform.CarwaleMobile));

                modelPageDTO.OfferAndDealerAd.LeadSource.AddRange(Mapper.Map<List<LeadSourceDTO>>(leadSources));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        private void SetOffers(VersionPageDTO_Mobile versionPageDTO, CarDataAdapterInputs inputs)
        {
            try
            {
                var offerInput = new OfferInput
                {
                    ApplicationId = (int)Application.CarWale,
                    MakeId = versionPageDTO.ModelDetails.MakeId,
                    ModelId = versionPageDTO.ModelDetails.ModelId,
                    VersionId = inputs.ModelDetails.VersionId
                };

                if (versionPageDTO.CityDetails == null || versionPageDTO.CityDetails.CityId < 1)
                {
                    offerInput.CityId = -1;
                    offerInput.StateId = -1;
                }
                else
                {
                    offerInput.CityId = versionPageDTO.CityDetails.CityId;
                    offerInput.StateId = versionPageDTO.CityDetails.StateId;
                }

                versionPageDTO.OfferAndDealerAd.Offer = _offersAdapter.GetOffers(offerInput);
                versionPageDTO.OfferAndDealerAd.Page = Pages.VersionPage;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        private void SetCampaignsVM(VersionPageDTO_Mobile versionDTO, CarDataAdapterInputs inputParam)
        {
            try
            {
                if (!versionDTO.VersionDetails.New.Equals(1)) 
                { 
                    versionDTO.DealerAd = new DealerAdDTO();
                    return; 
                }

                //Fetching campaign
                var carDetails = new CarIdEntity { MakeId = versionDTO.ModelDetails.MakeId, ModelId = inputParam.ModelDetails.ModelId };
                var persistedCampaignId = _campaign.GetPersistedCampaign(inputParam.ModelDetails.ModelId, inputParam.CustLocation);
                var dealerAd = _dealerAdProviderBl.GetDealerAd(carDetails, inputParam.CustLocation, Convert.ToInt32(Platform.CarwaleMobile), (int)CampaignAdType.Pq, persistedCampaignId, (int)Pages.ModelPageId);
                
                if (dealerAd != null && dealerAd.Campaign != null)
                {
                    versionDTO.DealerAd = Mapper.Map<DealerAdDTO>(dealerAd);
                    versionDTO.OfferAndDealerAd.DealerAd = versionDTO.DealerAd;
                                        
                        //Getting dealer details
                    var dealerDetails = _newCarDealer.NCDDetails(versionDTO.DealerAd.Campaign.DealerId, versionDTO.DealerAd.Campaign.Id, versionDTO.ModelDetails.MakeId, inputParam.CustLocation.CityId);

                        if (dealerDetails.CampaignId > 0)
                        {
                            versionDTO.OfferAndDealerAd.DealerAd.Campaign.DealerShowroom = AutoMapper.Mapper.Map<Entity.Dealers.DealerDetails, DealersDTO>(dealerDetails);
                        }                    
                }
                else
                {
                    //Handling case when only city is set and campaign is available for atleast one area
                    versionDTO.ShowCampaignLink = _campaign.IsCityCampaignExist(inputParam.ModelDetails.ModelId, inputParam.CustLocation,
                        (int)Platform.CarwaleMobile, (int)Application.CarWale);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        private LeadFormModelData SetLeadFormData(DealerAdDTO dealerAd)
        {
            LeadFormModelData leadFormModelData = new LeadFormModelData();
            if (dealerAd != null)
            {
                List<DealerAdDTO> dealerCampaign = new List<DealerAdDTO>();
                dealerCampaign.Add(dealerAd);
                leadFormModelData.DealerAd = dealerCampaign;
            }
            return leadFormModelData;
        }
    }
}
