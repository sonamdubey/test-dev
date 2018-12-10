using AutoMapper;
using Carwale.DTOs.Campaigns;
using Carwale.DTOs.CarData;
using Carwale.DTOs.Geolocation;
using Carwale.DTOs.NewCars;
using Carwale.DTOs.PriceQuote;
using Carwale.Entity.AdapterModels;
using Carwale.Entity.CarData;
using Carwale.Entity.CMS;
using Carwale.Entity.CMS.URIs;
using Carwale.Entity.Enum;
using Carwale.Entity.Geolocation;
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
using Carwale.Interfaces.PriceQuote;
using Carwale.Notifications.Logs;
using Carwale.Utility;
using Microsoft.Practices.Unity;
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
        public VersionPageAdapterMobile(ICarVersionCacheRepository carVersionsCacheRepo,
                         ICompareCarsBL compareCarsBL, ICarModelCacheRepository carModelsCacheRepo, IDeals cardeals,
                         ICarPriceQuoteAdapter iPrices, IPhotos photos, IMediaBL media, ICarModels carModelsBL,
                         ICarVersions carVersionsBL, IVideosBL videos, ICampaign campaign, INewCarDealers newCarDealer, ITemplate campaignTemplate, 
                         IGeoCitiesCacheRepository geoCitiesCacheRepository, ICarDataLogic carDataLogic)
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

                    VersionData = Mapper.Map<CarDataPresentation, CCarDataDto>(versionData!= null && versionData.Count > 0 ? versionData[0] :new CarDataPresentation()) ?? new CCarDataDto(),

                    ModelDetails = _carModelsCacheRepo.GetModelDetailsById(versionDetails.ModelId),

                    CarPriceOverview = versionPrice.ContainsKey(ValVersionIDList[0]) && versionPrice[ValVersionIDList[0]] != null ? versionPrice[ValVersionIDList[0]] : new PriceOverview(),

                    NewCarVersions = carVersions,

                    IsOtherVariantPriceAvailable = carVersions.FindAll(x => x.Id != versionDetails.VersionId && x.CarPriceOverview != null).Count > 0
                };
                versionDTO.VersionData.Colors = Mapper.Map<List<List<Entity.CompareCars.Color>>, List<List<Color>>>(_carVersionsBL.GetVersionsColors(ValVersionIDList));
                inputParam.ModelDetails.ModelId = versionDTO.ModelDetails.ModelId;

                if (versionDTO.VersionDetails.New.Equals(1))
                {
                    versionDTO.SimilarCars = _carModelsBL.GetSimilarCarVM(versionDetails.ModelId, versionDetails.MakeName, versionDetails.ModelName, versionDetails.MaskingName, inputParam.CustLocation.CityId, WidgetSource.VersionPageAlternativeWidgetCompareCarLinkMobile, "VersionPage", true, true);
                }
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

                SetCampaignsVM(versionDTO, inputParam);

                versionDTO.AdvantageAdData = _cardeals.IsShowDeals(inputParam.CustLocation.CityId, true) ?
                    _cardeals.GetAdvantageAdContent(versionDetails.ModelId, (inputParam.CustLocation.CityId > 0
                    ? inputParam.CustLocation.CityId : 0), versionDTO.ModelDetails.SubSegmentId, inputParam.CustLocation.CityId > 0
                    ? inputParam.ModelDetails.VersionId : 0) : null;
                versionDTO.ModelDetails.ModelColors = _carModelsCacheRepo.GetModelColorsByModel(versionDTO.ModelDetails.ModelId);
                versionDTO.ModelDetails.IsModelColorPhotosAvailable = _media.IsModelColorPhotosPresent(versionDTO.ModelDetails.ModelColors);
                versionDTO.MobileOverviewDetails = ModelOverview(versionDTO, inputParam.CustLocation);
                versionDTO.CallSlugInfo = BindCallSlug(versionDTO);

                var dealerDetails = _newCarDealer.NCDDetails(versionDTO.SponsoredDealerAd.ActualDealerId,
                    versionDTO.SponsoredDealerAd.DealerId, versionDTO.ModelDetails.MakeId, inputParam.CustLocation.CityId);

                if (dealerDetails.CampaignId > 0)
                {
                    versionDTO.DealerDetails = Mapper.Map<Entity.Dealers.DealerDetails, DTO.Dealers.DealerDetails>(dealerDetails);
                }
                inputParam.CampaignInput.MakeId = versionDTO.ModelDetails.MakeId;
                inputParam.CampaignInput.ModelId = versionDTO.ModelDetails.ModelId;
                versionDTO.MobileOverviewDetails.CampaignTemplates = _campaignTemplate.GetTemplatesByPage(inputParam.CampaignInput, versionDTO.MobileOverviewDetails);
                versionDTO.VersionData.CampaignTemplates = versionDTO.MobileOverviewDetails.CampaignTemplates;
                versionDTO.IsRenaultLeadCampaign = versionDetails.ModelId.Equals(CWConfiguration.RenaultLeadFormModelId);
				versionDTO.CityDetails = Mapper.Map<CitiesDTO>(_geoCitiesCacheRepository.GetCityDetailsById(inputParam.CustLocation.CityId));
                versionDTO.ShowTyresLink = _carVersionsBL.CheckTyresExists(inputParam.ModelDetails.VersionId);
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
                overview.ModelId = versionDetailsObj.ModelId;
                overview.MinPrice = versionDetailsObj.MinPrice;
                overview.MaxPrice = versionDetailsObj.MaxPrice;
                overview.MinAvgPrice = versionDetailsObj.MinAvgPrice;
                overview.MaskingName = versionDetailsObj.MaskingName;
                overview.PhotoCount = versionDTO_Mobile.ModelPhotosCount;
                overview.IsVersionPage = true;
                overview.EMI = Calculation.Calculation.CalculateEmi(versionDTO_Mobile.CarPriceOverview.Price);
                overview.VersionId = versionDetailsObj.VersionId;
                overview.VersionName = versionDetailsObj.VersionName;
                overview.ExpectedLaunch = versionDetailsObj.UpcomingExpectedLaunch ?? string.Empty;
                overview.EstimatedPrice = versionDTO_Mobile.CarPriceOverview.Price > 0 ? Format.FormatFullPrice(versionDTO_Mobile.CarPriceOverview.Price.ToString()) : string.Empty;
                overview.MaxDiscount = versionDTO_Mobile.AdvantageAdData != null ? versionDTO_Mobile.AdvantageAdData.Savings : 0;
                overview.StockMaskingName = versionDTO_Mobile.AdvantageAdData != null ? versionDTO_Mobile.AdvantageAdData.Model.MaskingName : string.Empty;
                overview.DealsCityId = versionDTO_Mobile.AdvantageAdData != null ? versionDTO_Mobile.AdvantageAdData.City.CityId : 0;
                overview.DealsModelName = versionDTO_Mobile.AdvantageAdData != null ? versionDTO_Mobile.AdvantageAdData.Model.ModelName : string.Empty;
                if (versionDTO_Mobile.SponsoredDealerAd != null)
                {
                    overview.AdAvailable = versionDTO_Mobile.SponsoredDealerAd.DealerId > 0;
                    overview.PredictionData = Mapper.Map<PredictionData>(versionDTO_Mobile.SponsoredDealerAd.PredictionData);
                    overview.LeadCTA = versionDTO_Mobile.SponsoredDealerAd.CTALinkText;
                    overview.ShowCampaignLink = versionDTO_Mobile.ShowCampaignLink;
                }
                overview.AdvantageAdData = versionDTO_Mobile.AdvantageAdData;
                overview.CarPriceOverview = Mapper.Map<PriceOverview, PriceOverviewDTOV2>(versionDTO_Mobile.CarPriceOverview);
                overview.LocationData = new Location { CityId = location.CityId, CityName = location.CityName };
                overview.PQPageId = 124;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return overview;
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
                callSlug.DealerName = callSlug.IsAdAvailable ? versionDTO_Mobile.SponsoredDealerAd.DealerName : string.Empty;
                callSlug.DealerMobile = callSlug.IsAdAvailable ? versionDTO_Mobile.SponsoredDealerAd.DealerMobile : string.Empty;
                callSlug.AdvantageAdData = versionDTO_Mobile.AdvantageAdData;
                callSlug.SponsoredDealerAd = versionDTO_Mobile.SponsoredDealerAd;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return callSlug;
        }

        private void SetCampaignsVM(VersionPageDTO_Mobile versionDTO, CarDataAdapterInputs inputParam)
        {
            try
            {
                versionDTO.SponsoredDealerAd = _campaign.GetSponsorDealerAd(inputParam.ModelDetails.ModelId, (int)Platform.CarwaleMobile, inputParam.CustLocation);

                var dealerDetails = _newCarDealer.NCDDetails(versionDTO.SponsoredDealerAd.ActualDealerId, versionDTO.SponsoredDealerAd.DealerId,
                    versionDTO.ModelDetails.MakeId, inputParam.CustLocation.CityId);

                if (dealerDetails.CampaignId > 0)
                {
                    versionDTO.DealerDetails = Mapper.Map<Entity.Dealers.DealerDetails, DTO.Dealers.DealerDetails>(dealerDetails);
                }

                if (versionDTO.SponsoredDealerAd == null || versionDTO.SponsoredDealerAd.DealerId < 1)
                {
                    versionDTO.ShowCampaignLink = _campaign.IsCityCampaignExist(inputParam.ModelDetails.ModelId, inputParam.CustLocation,
                        (int)Platform.CarwaleMobile, (int)Application.CarWale);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
    }
}
