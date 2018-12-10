using AutoMapper;
using Carwale.DTOs.Campaigns;
using Carwale.DTOs.CarData;
using Carwale.DTOs.Geolocation;
using Carwale.DTOs.NewCars;
using Carwale.DTOs.PriceQuote;
using Carwale.Entity.AdapterModels;
using Carwale.Entity.Campaigns;
using Carwale.Entity.CarData;
using Carwale.Entity.CMS;
using Carwale.Entity.CMS.Photos;
using Carwale.Entity.CMS.URIs;
using Carwale.Entity.Enum;
using Carwale.Entity.Geolocation;
using Carwale.Entity.PriceQuote;
using Carwale.Interfaces.Campaigns;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.CMS;
using Carwale.Interfaces.CMS.Photos;
using Carwale.Interfaces.Dealers;
using Carwale.Interfaces.Deals;
using Carwale.Interfaces.Geolocation;
using Carwale.Interfaces.NewCars;
using Carwale.Interfaces.PriceQuote;
using Carwale.Notifications.Logs;
using Carwale.Utility;
using Microsoft.Practices.Unity;
using ProtoBufClass.Campaigns;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Carwale.BL.NewCars
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
        public ModelPageAdapterMobile(IPhotos photos, IVideosBL videos,ITemplate campaignTemplate, IGeoCitiesCacheRepository geoCitiesCacheRepository,
            ICarMileage carMileage,ICarModels carModelBl,ICarModelCacheRepository carModelCacheRepo,ICarPriceQuoteAdapter priceQuoteAdapter,
           ICarVersions carVersionsBl,IDeals carDeals,INewCarDealers newCarDealersBl,IMediaBL mediaBl,ICampaign campaign)
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
                _carMileage = carMileage;

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

                if (modelDetails.MakeId > 0)
                {
                    var modelPageDTO = new ModelPageDTO_Mobile();
                    modelPageDTO.ModelDetails = modelDetails;
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

                            SetDefaultVersion(newCarVersions, modelPageDTO, inputParam.ModelDetails.VersionId);

                            SetCampaignsVM(modelPageDTO, inputParam);

                            modelPageDTO.IsRenaultLeadCampaign = modelPageDTO.ModelDetails.ModelId.Equals(CWConfiguration.RenaultLeadFormModelId);
                            modelPageDTO.MileageData = _carMileage.GetMileageData(versionDetailsList);

                        }

                        modelPageDTO.UpgradedModelDetails = _carModelsCacheRepo.GetUpgradedModel(modelDetails.ModelId);
                        modelPageDTO.AdvantageAdData = _carDeals.IsShowDeals(inputParam.CustLocation.CityId, true) ? _carDeals.GetAdvantageAdContent(inputParam.ModelDetails.ModelId,
                            (inputParam.CustLocation.CityId > 0 ? inputParam.CustLocation.CityId : 0), modelPageDTO.ModelDetails.SubSegmentId) : null;
                    }
                    else if (modelPageDTO.ModelDetails.Futuristic)
                    {
                        modelPageDTO.UpcomingCarDetails = _carModelsCacheRepo.GetUpcomingCarDetails(inputParam.ModelDetails.ModelId);
                        GetUpcomingCarVm(modelPageDTO, inputParam);
                    }

                    modelPageDTO.SimilarCars = _carModelsBL.GetSimilarCarVM(modelDetails.ModelId, modelDetails.MakeName, modelDetails.ModelName, modelDetails.MaskingName, inputParam.CustLocation.CityId, WidgetSource.ModelPageAlternativeWidgetCompareCarLinkMobile, orp: true, isMobile: true, isFuturistic: modelDetails.Futuristic);
                    modelPageDTO.SelectedVersions = SetComparisionCarVM(inputParam, newCarVersions);
                    modelPageDTO.ModelMenu = new ModelMenuDTO() { ActiveSection = ModelMenuEnum.Overview };
                    modelPageDTO.MobileOverviewDetails = ModelOverview(modelPageDTO, inputParam.ABTest, inputParam.CustLocation);
                    modelPageDTO.CallSlugInfo = SetCallSlugVM(modelPageDTO);
                    GetPageMetaTags(modelPageDTO);
                    var queryString = new ModelPhotosBycountURI
                    {
                        ApplicationId = (ushort)CMSAppId.Carwale,
                        CategoryIdList = "8,10",
                        ModelId = modelPageDTO.ModelDetails.ModelId,
                        PlatformId = Platform.CarwaleDesktop.ToString("D")
                    };
                    if (modelPageDTO.ModelDetails.New)
                    {
                        var processVersionData = _carVersionsBL.ProcessVersionsData(inputParam.ModelDetails.ModelId, newCarVersions);
                        modelPageDTO.Summary = _carModelsBL.Summary(modelPageDTO.PageMetaTags.Summary, modelPageDTO.ModelDetails.MakeName, modelPageDTO.ModelDetails.ModelName,
                            processVersionData, newCarVersions, inputParam.IsCityPage, inputParam.CustLocation.CityName);
                        SetVariantList(modelPageDTO);
                    }
                    List<ModelImage> modelPhotos = _photos.GetModelPhotosByCount(queryString);
                    modelPageDTO.ModelDetails.PhotoCount = modelPhotos.IsNotNullOrEmpty() ? modelPhotos.Count : 0;
                    modelPageDTO.ModelPhotosListCarousel = _media.GetModelCarouselImages(modelPhotos);
                    modelPageDTO.ModelPhotosList = _media.GetModelImages(modelPhotos);
                    List<Video> modelVideos = _videos.GetVideosByModelId(modelPageDTO.ModelDetails.ModelId, CMSAppId.Carwale, 1, -1);
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
                         && DateTimeUtility.ShowReplaceModel(modelPageDTO.ModelDetails.ModelLaunchDate, false))
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
                    modelPageDTO.CityDetails = Mapper.Map<CitiesDTO>(_geoCitiesCacheRepository.GetCityDetailsById(inputParam.CustLocation.CityId));
                    return modelPageDTO;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return new ModelPageDTO_Mobile();
        }

        private void SetCampaignsVM(ModelPageDTO_Mobile modelPageDTO, CarDataAdapterInputs inputParam)
        {
            try
            {
                modelPageDTO.SponsoredDealerAd = _campaign.GetSponsorDealerAd(inputParam.ModelDetails.ModelId, (int)Platform.CarwaleMobile, inputParam.CustLocation);

                var dealerDetails = _newCarDealersBL.NCDDetails(modelPageDTO.SponsoredDealerAd.ActualDealerId, modelPageDTO.SponsoredDealerAd.DealerId,
                    modelPageDTO.ModelDetails.MakeId, inputParam.CustLocation.CityId);

                if (dealerDetails.CampaignId > 0)
                {
                    modelPageDTO.DealerDetails = AutoMapper.Mapper.Map<Entity.Dealers.DealerDetails, DTO.Dealers.DealerDetails>(dealerDetails);
                }

                if (modelPageDTO.SponsoredDealerAd == null || modelPageDTO.SponsoredDealerAd.DealerId < 1)
                {
                    modelPageDTO.ShowCampaignLink = _campaign.IsCityCampaignExist(inputParam.ModelDetails.ModelId, inputParam.CustLocation,
                        (int)Platform.CarwaleMobile, (int)Application.CarWale);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        private List<CarVersionDetails> SetComparisionCarVM(CarDataAdapterInputs inputParam, List<NewCarVersionsDTO> newCarVersions)
        {
            var selectedVersions = _carVersionsBL.GetSelectedVersionDetails();

            if (selectedVersions.IsNotNullOrEmpty())
            {
                foreach (var comparecar in selectedVersions)
                {
                    if (comparecar.ModelId > 0)
                    {
                        if (comparecar.ModelId == inputParam.ModelDetails.ModelId && newCarVersions != null)
                        {
                            var version = newCarVersions.First(x => x.Id == comparecar.VersionId);
                            if(version.CarPriceOverview != null)
                                comparecar.MinPrice = version.CarPriceOverview.Price;
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
                overview.MinPrice = modelDetailsObj.MinPrice;
                overview.MaxPrice = modelDetailsObj.MaxPrice;
                overview.MinAvgPrice = modelDetailsObj.MinAvgPrice;
                overview.MaskingName = modelDetailsObj.MaskingName;
                overview.EMI = modelPageDTO_Mobile.CarPriceOverview != null ? Calculation.Calculation.CalculateEmi(modelPageDTO_Mobile.CarPriceOverview.Price) : string.Empty;
                overview.PhotoCount = modelPageDTO_Mobile.ModelPhotosList != null ? modelPageDTO_Mobile.ModelPhotosList.Count : 0;
                overview.IsVersionPage = false;
                overview.MaxDiscount = modelPageDTO_Mobile.AdvantageAdData != null ? modelPageDTO_Mobile.AdvantageAdData.Savings : 0;
                overview.StockMaskingName = modelPageDTO_Mobile.AdvantageAdData != null ? modelPageDTO_Mobile.AdvantageAdData.Model.MaskingName : string.Empty;
                overview.DealsCityId = modelPageDTO_Mobile.AdvantageAdData != null ? modelPageDTO_Mobile.AdvantageAdData.City.CityId : 0;
                overview.DealsModelName = modelPageDTO_Mobile.AdvantageAdData != null ? modelPageDTO_Mobile.AdvantageAdData.Model.ModelName : string.Empty;
                overview.AdAvailable = modelPageDTO_Mobile.SponsoredDealerAd != null && modelPageDTO_Mobile.SponsoredDealerAd.DealerId > 1;
                overview.AdvantageAdData = modelPageDTO_Mobile.AdvantageAdData;
                overview.CarPriceOverview = modelPageDTO_Mobile.CarPriceOverview != null ? AutoMapper.Mapper.Map<PriceOverview, PriceOverviewDTOV2>(modelPageDTO_Mobile.CarPriceOverview) : null;
                if (modelPageDTO_Mobile.CarVersion != null)
                {
                    overview.VersionId = modelPageDTO_Mobile.CarVersion.ID;
                    overview.VersionName = modelPageDTO_Mobile.CarVersion.Name;
                }
                overview.PredictionData = modelPageDTO_Mobile.SponsoredDealerAd != null ? AutoMapper.Mapper.Map<PredictionData>(modelPageDTO_Mobile.SponsoredDealerAd.PredictionData)
                    : new PredictionData();
                overview.LeadCTA = modelPageDTO_Mobile.SponsoredDealerAd != null ? modelPageDTO_Mobile.SponsoredDealerAd.CTALinkText : string.Empty;
                overview.LocationData = location;
                if (modelDetailsObj.Futuristic && modelPageDTO_Mobile.UpcomingCarDetails != null)
                {
                    overview.ExpectedLaunch = modelPageDTO_Mobile.UpcomingCarDetails.ExpectedLaunch;
                    if (modelPageDTO_Mobile.UpcomingCarDetails.Price != null)
                        overview.EstimatedPrice = Format.GetUpcomingFormatPrice(modelPageDTO_Mobile.UpcomingCarDetails.Price.MinPrice, modelPageDTO_Mobile.UpcomingCarDetails.Price.MinPrice);
                }
                overview.PQPageId = 124;
                overview.ShowCampaignLink = modelPageDTO_Mobile.ShowCampaignLink;
                return overview;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return null;
            }
        }

        public CallSlugDTO SetCallSlugVM(ModelPageDTO_Mobile modelPageDTO_Mobile)
        {
            CallSlugDTO callSlug = new CallSlugDTO();
            var modelDetailsObj = modelPageDTO_Mobile.ModelDetails;

            callSlug.CarName = modelDetailsObj.MakeName + "_" + modelDetailsObj.ModelName;
            callSlug.ModelId = modelDetailsObj.ModelId;
            callSlug.VersionId = 0;
            callSlug.IsAdAvailable = modelPageDTO_Mobile.SponsoredDealerAd != null && modelPageDTO_Mobile.SponsoredDealerAd.DealerId > 1;
            callSlug.DealerName = modelPageDTO_Mobile.SponsoredDealerAd != null && callSlug.IsAdAvailable ? modelPageDTO_Mobile.SponsoredDealerAd.DealerName : string.Empty;
            callSlug.DealerMobile = modelPageDTO_Mobile.SponsoredDealerAd != null && callSlug.IsAdAvailable ? modelPageDTO_Mobile.SponsoredDealerAd.DealerMobile : string.Empty;
            callSlug.AdvantageAdData = modelPageDTO_Mobile.AdvantageAdData;
            callSlug.SponsoredDealerAd = modelPageDTO_Mobile.SponsoredDealerAd;
            return callSlug;
        }
		private void SetDefaultVersion(IEnumerable<NewCarVersionsDTO> carVersionsDTOList, ModelPageDTO_Mobile modelPageDTO, int versionId)
		{
			try
			{
				if (carVersionsDTOList.IsNotNullOrEmpty())
				{
					var defaultVersion = carVersionsDTOList.FirstOrDefault(x => x.Id == versionId) ?? carVersionsDTOList.ElementAt(0);
					modelPageDTO.CarVersion = new CarVersionsDTO {
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
                mobilepageDTO.VariantList.SponsoredDealerAd = mobilepageDTO.SponsoredDealerAd;
                mobilepageDTO.VariantList.ModelDetails = mobilepageDTO.ModelDetails;
                mobilepageDTO.VariantList.MobileOverviewDetails = mobilepageDTO.MobileOverviewDetails;
                mobilepageDTO.VariantList.NewCarVersions = mobilepageDTO.NewCarVersions;
                mobilepageDTO.VariantList.ShowCampaignLink = mobilepageDTO.ShowCampaignLink;
                mobilepageDTO.VariantList.PageId = (int)CwPages.ModelMsite;
            }
            catch (Exception ex )
            {
                Logger.LogException(ex);
            }
        }
	}
}
