using Carwale.BL.CMS;
using Carwale.DTOs.Campaigns;
using Carwale.DTOs.CarData;
using Carwale.DTOs.CMS.Photos;
using Carwale.DTOs.NewCars;
using Carwale.DTOs.PriceQuote;
using Carwale.Entity.AdapterModels;
using Carwale.Entity.CarData;
using Carwale.Entity.Classified;
using Carwale.Entity.CMS;
using Carwale.Entity.CMS.Articles;
using Carwale.Entity.CMS.Photos;
using Carwale.Entity.CMS.URIs;
using Carwale.Entity.Enum;
using Carwale.Entity.Geolocation;
using Carwale.Entity.PriceQuote;
using Carwale.Interfaces.Campaigns;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.Classified;
using Carwale.Interfaces.CMS;
using Carwale.Interfaces.CMS.Articles;
using Carwale.Interfaces.CMS.Photos;
using Carwale.Interfaces.Dealers;
using Carwale.Interfaces.Deals;
using Carwale.Interfaces.NewCars;
using Carwale.Interfaces.PriceQuote;
using Carwale.Notifications.Logs;
using Carwale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Carwale.DTOs.CMS.ThreeSixtyView;
using Carwale.Interfaces.Template;
using AutoMapper;

namespace Carwale.BL.NewCars
{

    public class PriceInCityAdapter : IServiceAdapterV2
    {
        private readonly IUnityContainer _unityContainer;
        public ICarModelCacheRepository _carModelsCacheRepo;
        public ICarVersions _carVersionsBL;
        public ICarModels _carModelsBL;
        private readonly IDeals _carDeals;
        private readonly ICarPriceQuoteAdapter _iPrices;
        private readonly INewCarDealers _newCarDealersBL;
        private readonly IMediaBL _media;
        private readonly IPhotos _photos;
        private readonly IStockCountCacheRepository _stockCountCacheRepo;
        private readonly IClassifiedListing _classifiedListing;
        private readonly ICMSContent _cmsContentCacheRepo;
        private readonly IVideosBL _videos;
        private readonly ICampaign _campaignBL;
        private readonly ITemplate _campaignTemplate;
        private readonly ITemplatesCacheRepository _tempCache;
        private readonly string[] _carMakeVariantList = (ConfigurationManager.AppSettings["PicPageVersionListMakeIds"] ?? String.Empty).Split(new char[] { ',' });
        public PriceInCityAdapter(IUnityContainer unityContainer, IPhotos photos, IVideosBL videos, ITemplate campaignTemplate, ITemplatesCacheRepository tempCache)
        {
            _unityContainer = unityContainer;
            _carModelsBL = _unityContainer.Resolve<ICarModels>();
            _carModelsCacheRepo = _unityContainer.Resolve<ICarModelCacheRepository>();
            _iPrices = _unityContainer.Resolve<ICarPriceQuoteAdapter>();
            _carVersionsBL = _unityContainer.Resolve<ICarVersions>();
            _carDeals = _unityContainer.Resolve<IDeals>();
            _newCarDealersBL = _unityContainer.Resolve<INewCarDealers>();
            _cmsContentCacheRepo = _unityContainer.Resolve<ICMSContent>();
            _campaignBL = _unityContainer.Resolve<ICampaign>();
            _media = _unityContainer.Resolve<IMediaBL>();
            _photos = _unityContainer.Resolve<IPhotos>();
            _stockCountCacheRepo = _unityContainer.Resolve<IStockCountCacheRepository>();
            _classifiedListing = _unityContainer.Resolve<IClassifiedListing>();
            _videos = videos;
            _campaignTemplate = campaignTemplate;
            _tempCache = tempCache;
		}

        public T Get<T, U>(U input)
        {
            return (T)Convert.ChangeType(GetPICPageDTOForMobile(input), typeof(T));
        }

        private PriceInCityPageDTO GetPICPageDTOForMobile<U>(U input)
        {
            try
            {
                CarDataAdapterInputs inputParam = (CarDataAdapterInputs)Convert.ChangeType(input, typeof(U));

                var modelDetails = _carModelsCacheRepo.GetModelDetailsById(inputParam.ModelDetails.ModelId);

                if (modelDetails != null && modelDetails.MakeId > 0)
                {
                    var modelPageDTO = new PriceInCityPageDTO();
                    modelPageDTO.ModelDetails = modelDetails;
                    modelPageDTO.DealersList = _newCarDealersBL.NewCarDealerListByCityMake(modelPageDTO.ModelDetails.MakeId, inputParam.ModelDetails.ModelId, inputParam.CustLocation.CityId);

                    //Check if is DealerAvailable
                    if (modelPageDTO.DealersList == null || (modelPageDTO.DealersList != null && modelPageDTO.DealersList.NewCarDealers != null && modelPageDTO.DealersList.NewCarDealers.Count <= 0))
                    {
                        return modelPageDTO;
                    }
                    else
                    {
                        modelPageDTO.NewCarDealersCount = modelPageDTO.DealersList.NewCarDealers.Count;
                    }

                    List<NewCarVersionsDTO> newCarVersions = new List<NewCarVersionsDTO>();
                    if (modelPageDTO.ModelDetails.New || (!modelPageDTO.ModelDetails.Futuristic)) //new or discontinue car
                    {
                        if (modelPageDTO.ModelDetails.New)
                        {
                            //get version for model and map to respective DTO
                            newCarVersions = _carVersionsBL.MapCarVersionDtoWithCarVersionEntity(inputParam.ModelDetails.ModelId, inputParam.CustLocation.CityId);
                            if (newCarVersions != null)
                            {
                                if (newCarVersions.Count > 0 && newCarVersions[0].CarPriceOverview.Price > 0)
                                {
                                    modelPageDTO.ModelDetails.MinPrice = newCarVersions.Where(c => c.CarPriceOverview.Price > 0).Min(x => x.CarPriceOverview.Price);
                                    modelPageDTO.ModelDetails.MaxPrice = newCarVersions.Max(x => x.CarPriceOverview.Price);
                                }
                                IDictionary<int, PriceOverview> modelPrice;
                                NewCarVersionsDTO defaultVersionInfo = inputParam.ModelDetails.VersionId > 0 ? newCarVersions.FirstOrDefault(x => x.Id == inputParam.ModelDetails.VersionId) : null;
                                if (defaultVersionInfo != null)
                                {
                                    modelPrice = _iPrices.GetVersionsPriceForSameModel(
                                                                    inputParam.ModelDetails.ModelId,
                                                                    new List<int> { inputParam.ModelDetails.VersionId },
                                                                    inputParam.CustLocation.CityId,
                                                                    true
                                                                    );
                                    modelPageDTO.CarPriceOverview = modelPrice.ContainsKey(inputParam.ModelDetails.VersionId)
                                                                    && modelPrice[inputParam.ModelDetails.VersionId] != null ?
                                                                    modelPrice[inputParam.ModelDetails.VersionId] : new PriceOverview();
                                    modelPageDTO.CarVersion = new CarVersionsDTO
                                    {
                                        ID = inputParam.ModelDetails.VersionId,
                                        Name = defaultVersionInfo.Version
                                    };
                                }
                                else
                                {
                                    modelPrice = _iPrices.GetModelsCarPriceOverview(
                                                                    new List<int> { inputParam.ModelDetails.ModelId },
                                                                    inputParam.CustLocation.CityId,
                                                                    true
                                                                    );
                                    modelPageDTO.CarPriceOverview = modelPrice.ContainsKey(inputParam.ModelDetails.ModelId)
                                                                    && modelPrice[inputParam.ModelDetails.ModelId] != null ?
                                                                    modelPrice[inputParam.ModelDetails.ModelId] : new PriceOverview();
                                    modelPageDTO.CarVersion = new CarVersionsDTO
                                    {
                                        ID = newCarVersions.Count > 0 ? newCarVersions[0].Id : 0,
                                        Name = newCarVersions.Count > 0 ? newCarVersions[0].Version : String.Empty
                                    };
                                }
                                modelPageDTO.NewCarVersions = Mapper.Map<List<NewCarVersionsDTO>, List<NewCarVersionsDTOV2>>(newCarVersions);
                                VariantListDTO variantListDto = new VariantListDTO();
                                variantListDto.FuelTypes = newCarVersions.Equals(null) ? new List<string>() : newCarVersions.Select(x => x.CarFuelType).Distinct().ToList();
                                variantListDto.TransmissionTypes = newCarVersions.Equals(null) ? new List<string>() : newCarVersions.Select(x => x.TransmissionType).Distinct().ToList();
                                modelPageDTO.VariantList = variantListDto;
                            }

                            SetCampaignsVM(modelPageDTO, inputParam);
                            modelPageDTO.IsRenaultLeadCampaign = modelPageDTO.ModelDetails.ModelId.Equals(CWConfiguration.RenaultLeadFormModelId);

                            modelPageDTO.SimilarCars = _carModelsBL.GetSimilarCarVM(modelDetails.ModelId, modelDetails.MakeName, modelDetails.ModelName,
                       modelDetails.MaskingName, inputParam.CustLocation.CityId,
                       (inputParam.IsMobile ? WidgetSource.PICPageAlternativeWidgetCompareCarLinkMobile : WidgetSource.PICPageAlternativeWidgetCompareCarLinkDesktop), "CityPage", true, inputParam.IsMobile);

                        }
                        modelPageDTO.AdvantageAdData = _carDeals.IsShowDeals(
                                        inputParam.CustLocation.CityId, true) ? _carDeals.GetAdvantageAdContent(inputParam.ModelDetails.ModelId
                                            , (inputParam.CustLocation.CityId > 0 ? inputParam.CustLocation.CityId : 0)
                                            , modelPageDTO.ModelDetails.SubSegmentId) : null;
                    }


                    modelPageDTO.SelectedVersions = SetComparisionCarVM(inputParam, newCarVersions);

                    modelPageDTO.CallSlugInfo = SetCallSlugVM(modelPageDTO);
                    modelPageDTO.PageMetaTags = GetPageMetaTags(modelPageDTO, inputParam.CustLocation.CityName, inputParam.CustLocation.CityMaskingName);
                    var queryString = new ModelPhotosBycountURI
                    {
                        ApplicationId = (ushort)CMSAppId.Carwale,
                        CategoryIdList = "8,10",
                        ModelId = modelPageDTO.ModelDetails.ModelId,
                        PlatformId = Platform.CarwaleDesktop.ToString("D")
                    };
                    List<ModelImage> modelPhotos = _photos.GetModelPhotosByCount(queryString);
                    modelPageDTO.ModelDetails.PhotoCount = modelPhotos.IsNotNullOrEmpty() ? modelPhotos.Count : 0;
                    modelPageDTO.ModelPhotosListCarousel = _media.GetModelCarouselImages(modelPhotos);


                    List<VideoDTO> modelVideos = AutoMapper.Mapper.Map<List<Video>, List<VideoDTO>>(_videos.GetVideosByModelId(modelPageDTO.ModelDetails.ModelId, CMSAppId.Carwale, 1, -1));
                    modelPageDTO.ModelVideos = modelVideos;
                    modelPageDTO.ModelDetails.VideoCount = modelVideos != null ? modelVideos.Count : 0;
                    var processVersionData = _carVersionsBL.ProcessVersionsData(inputParam.ModelDetails.ModelId, newCarVersions);
                    modelPageDTO.Summary = _carModelsBL.Summary(modelPageDTO.PageMetaTags.Summary
                                , modelPageDTO.ModelDetails.MakeName
                                , modelPageDTO.ModelDetails.ModelName
                                , processVersionData
                                , newCarVersions
                                , true
                                , inputParam.CustLocation.CityName);

                    modelPageDTO.BreadcrumbEntitylist = BindBreadCrumb(modelPageDTO.ModelDetails, inputParam.CustLocation.CityName, inputParam.IsMobile);
                    modelPageDTO.VersionWidgetSource = inputParam.IsMobile ? (int)WidgetSource.PICPageVariantListCompareCarLinkMobile : (int)WidgetSource.PICPageVariantListCompareCarLinkDesktop;
                    modelPageDTO.ModelDetails.ModelColors = _carModelsCacheRepo.GetModelColorsByModel(modelPageDTO.ModelDetails.ModelId);
                    modelPageDTO.ModelDetails.IsModelColorPhotosAvailable = _media.IsModelColorPhotosPresent(modelPageDTO.ModelDetails.ModelColors);

                    if (!inputParam.IsMobile)
                    {
                        modelPageDTO.SubNavigation = BindModelQuickMenu(modelPageDTO, inputParam);
                        modelPageDTO.DesktopOverviewDetails = DesktopModelOverview(modelPageDTO, inputParam, newCarVersions);
                        SetUsedCarsVM(modelPageDTO, inputParam);
                    }
                    else
                    {
                        modelPageDTO.ModelMenu = new ModelMenuDTO{ ActiveSection = ModelMenuEnum.PriceInCity };
                        modelPageDTO.MobileOverviewDetails = MobileModelOverview(modelPageDTO, inputParam.ABTest, inputParam.CustLocation);
                        inputParam.CampaignInput.MakeId = modelPageDTO.ModelDetails.MakeId;
                        if (modelPageDTO.ModelDetails.New && !inputParam.IsAmp)
                        {
                            modelPageDTO.MobileOverviewDetails.CampaignTemplates = _campaignTemplate.GetTemplatesByPage(inputParam.CampaignInput, modelPageDTO.MobileOverviewDetails);
                        }
                    }
                    if (inputParam.IsAmp)
                    {
                        modelPageDTO.NewCarVersions = AutoMapper.Mapper.Map<List<NewCarVersionsDTO>, List<NewCarVersionsDTOV2>>(newCarVersions);
                    }
                    if (modelPageDTO.ModelDetails.New)
                    {
                        SetVariantList(modelPageDTO);
                    }
                    return modelPageDTO;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return new PriceInCityPageDTO();
        }

        private void SetUsedCarsVM(PriceInCityPageDTO modelPageDTO, CarDataAdapterInputs inputParam)
        {
            try
            {
                modelPageDTO.UsedCarsCount = _stockCountCacheRepo.GetUsedCarsCount(modelPageDTO.ModelDetails.RootId, inputParam.CustLocation.CityId) ?? new UsedCarCount();
                modelPageDTO.UsedCarsCount.ModelId = modelPageDTO.ModelDetails.ModelId;
                modelPageDTO.UsedLuxuryCars = _classifiedListing.GetLuxuryCarRecommendations(inputParam.ModelDetails.ModelId, 3849, 2);
            }
            catch (Exception ex)
            {

                Logger.LogException(ex);
            }

        }

        private SubNavigationDTO BindModelQuickMenu(PriceInCityPageDTO modelDTO, CarDataAdapterInputs modelInput)
        {
            SubNavigationDTO subNavigation = null;
            try
            {
                string label = string.Format("make:{0}|model:{1}|city:{2}", modelDTO.ModelDetails.MakeName, modelDTO.ModelDetails.ModelName,
               (modelInput.IsCityPage ? modelInput.CustLocation.CityName : modelInput.CustLocation.ZoneId.ToString()));
                var articleURI = new ArticleRecentURI()
                {
                    ApplicationId = (ushort)CMSAppId.Carwale,
                    ContentTypes = ((int)CMSContentType.RoadTest).ToString(),
                    TotalRecords = Convert.ToUInt16(ConfigurationManager.AppSettings["ExpertReviewsDesktop_Count"]),
                    ModelId = modelDTO.ModelDetails.ModelId,
                };
                List<ArticleSummary> expertReviews = _cmsContentCacheRepo.GetMostRecentArticles(articleURI);
                subNavigation = _carModelsBL.GetModelQuickMenu(modelDTO.ModelDetails, null,
                    !modelInput.IsCityPage, (expertReviews != null && expertReviews.Count > 0), (modelInput.IsCityPage ? "CityPage" : "ModelPage"), label);
                subNavigation.PQPageId = modelInput.IsCityPage ? 45 : 30;
                subNavigation.Page = Pages.PIC;
                subNavigation.ShowColorsLink = CMSCommon.IsModelColorPhotosPresent(modelDTO.ModelDetails.ModelColors);
                subNavigation.Default360Category = CMSCommon.Get360DefaultCategory(AutoMapper.Mapper.Map<ThreeSixtyAvailabilityDTO>(modelDTO.ModelDetails));
            }
            catch (Exception ex)
            {

                Logger.LogException(ex);

            }
            return subNavigation;
        }

        private void SetCampaignsVM(PriceInCityPageDTO modelPageDTO, CarDataAdapterInputs inputParam)
        {
            try
            {
                modelPageDTO.SponsoredDealerAd = _campaignBL.GetSponsorDealerAd(inputParam.ModelDetails.ModelId,
                    inputParam.IsMobile ? (int)Platform.CarwaleMobile : (int)Platform.CarwaleDesktop, inputParam.CustLocation);

                var dealerDetails = _newCarDealersBL.NCDDetails(modelPageDTO.SponsoredDealerAd.ActualDealerId,
                    modelPageDTO.SponsoredDealerAd.DealerId, modelPageDTO.ModelDetails.MakeId, inputParam.CustLocation.CityId);
                if (dealerDetails != null && dealerDetails.CampaignId > 0)
                {
                    modelPageDTO.DealerDetails = AutoMapper.Mapper.Map<Entity.Dealers.DealerDetails, DTO.Dealers.DealerDetails>(dealerDetails);
                }
                if (modelPageDTO.SponsoredDealerAd != null && modelPageDTO.SponsoredDealerAd.DealerId > 0)
                {
                    modelPageDTO.SponsoredDealerAd.MakeName = modelPageDTO.ModelDetails.MakeName;
                    int templateId = _campaignTemplate.GetCampaignGroupTemplateId(modelPageDTO.SponsoredDealerAd.AssignedTemplateId,
                        modelPageDTO.SponsoredDealerAd.AssignedGroupId, (int)Platform.CarwaleDesktop);
                    var template = _tempCache.GetById(templateId);
                    var sponsordealerObj = modelPageDTO.SponsoredDealerAd;
                    _campaignTemplate.ResolveTemplate(template, ref sponsordealerObj);
                }
                else
                {
                    modelPageDTO.ShowCampaignLink = _campaignBL.IsCityCampaignExist(inputParam.ModelDetails.ModelId, inputParam.CustLocation, (int)Platform.CarwaleDesktop, (int)Application.CarWale);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        private List<CarVersionDetails> SetComparisionCarVM(CarDataAdapterInputs inputParam, List<NewCarVersionsDTO> newCarVersions)
        {
            List<CarVersionDetails> selectedVersions = null;
            try
            {
                selectedVersions = _carVersionsBL.GetSelectedVersionDetails();

                if (selectedVersions.IsNotNullOrEmpty())
                {
                    foreach (var comparecar in selectedVersions)
                    {
                        if (comparecar.ModelId > 0)
                        {
                            if (comparecar.ModelId == inputParam.ModelDetails.ModelId && newCarVersions != null)
                            {
                                comparecar.MinPrice = newCarVersions.First(x => x.Id == comparecar.VersionId).CarPriceOverview.Price;
                            }
                            else
                            {
                                var versionList = new List<int>();
                                versionList.Add(comparecar.VersionId);
                                var verPrices = _iPrices.GetVersionsPriceForSameModel(comparecar.ModelId, versionList, inputParam.CustLocation.CityId, true);
                                comparecar.MinPrice = (verPrices != null && verPrices[comparecar.VersionId] != null) ? verPrices[comparecar.VersionId].Price : 0;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                Logger.LogException(ex);

            }

            return selectedVersions;
        }


        private PageMetaTags GetPageMetaTags(PriceInCityPageDTO modelDetails, string cityName ,string cityMaskingName)
        {
            PageMetaTags pageMetaTags = new PageMetaTags();
            try
            {
                pageMetaTags = _carModelsCacheRepo.GetModelPageMetaTags(modelDetails.ModelDetails.ModelId, Convert.ToInt16(Pages.ModelPageId));
                pageMetaTags.Title = GetTitle(modelDetails.ModelDetails, cityName);
                pageMetaTags.Description = GetDescription(modelDetails, cityName);
                pageMetaTags.Heading = _carModelsBL.Heading(pageMetaTags.Heading ?? "", modelDetails.ModelDetails.MakeName, modelDetails.ModelDetails.ModelName);
                pageMetaTags.Canonical = ManageCarUrl.CreatePriceInCityUrl(modelDetails.ModelDetails.MakeName, modelDetails.ModelDetails.MaskingName, cityMaskingName, true);
            }
            catch (Exception ex)
            {

                Logger.LogException(ex);

            }

            return pageMetaTags;
        }

        private string GetTitle(CarModelDetails modelDetails, string cityName)
        {
            try
            {
                string title = string.Empty;

                var modelMinPrice = Format.GetFormattedPriceV2(modelDetails.MinPrice > 0 ? modelDetails.MinPrice.ToString() : null);
                var modelMaxPrice = Format.GetFormattedPriceV2(modelDetails.MaxPrice > 0 ? modelDetails.MaxPrice.ToString() : null);
                title = string.Format("{0} {1} price (GST Rates) in {2} - ₹ {3}{4}",
                    modelDetails.MakeName, modelDetails.ModelName, cityName, modelMinPrice, ((modelMinPrice != modelMaxPrice) ? " to ₹ " + modelMaxPrice : string.Empty));

                return System.Text.RegularExpressions.Regex.Replace(title, "maruti suzuki", "Maruti", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            }
            catch (Exception ex)
            {

                Logger.LogException(ex);
                return null;

            }

        }

        private string GetDescription(PriceInCityPageDTO modelPageDTO_Mobile, string globalCityName)
        {
            try
            {
                var modelDetails = modelPageDTO_Mobile.ModelDetails;

                var formatFullPrice = Format.GetFormattedPriceV2((modelDetails.MinPrice <= 0 ? null : modelDetails.MinPrice.ToString()),
                                                                                        (modelDetails.MaxPrice <= 0 ? null : modelDetails.MaxPrice.ToString()));
                if (modelPageDTO_Mobile.CarPriceOverview != null && modelPageDTO_Mobile.CarPriceOverview.PriceStatus == 1)
                {
                    return string.Format("{0} {1} Price (GST Rates) in {2} - ₹ {3}. Get its detailed on road price in {2}. Check your nearest {0} {1} Dealer in {2}",
                            modelDetails.MakeName, modelDetails.ModelName, globalCityName, formatFullPrice);
                }
                else
                {
                    string dealerNames = string.Empty;

                    var dealerList = modelPageDTO_Mobile.DealersList.NewCarDealers;
                    dealerNames = dealerList.Count > 1 ? (dealerList[0].Name + ";" + dealerList[1].Name) : dealerList[0].Name;


                    var description = string.Format("{0} {1} can be bought in {2} from {3}.", modelDetails.MakeName, modelDetails.ModelName, globalCityName, dealerNames);
                    return !(modelDetails.New || modelDetails.Futuristic) ? description :
                        string.Format("{0} {1} {2} can be bought for ₹ {3} in {4}.", description, modelDetails.MakeName, modelDetails.ModelName, formatFullPrice, globalCityName);
                }
            }
            catch (Exception ex)
            {

                Logger.LogException(ex);
                return null;

            }

        }

        public CarOverviewDTOV2 MobileModelOverview(PriceInCityPageDTO modelPageDTO_Mobile, int abTest, Location location)
        {
            CarOverviewDTOV2 overview = new CarOverviewDTOV2();
            try
            {
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
                overview.EMI = modelPageDTO_Mobile.CarPriceOverview != null ? Calculation.Calculation.CalculateEmi(modelPageDTO_Mobile.CarPriceOverview.Price) : string.Empty;
                overview.PhotoCount = modelPageDTO_Mobile.ModelPhotosListCarousel != null ? modelPageDTO_Mobile.ModelPhotosListCarousel.Count : 0;
                overview.IsVersionPage = false;
                overview.MaxDiscount = modelPageDTO_Mobile.AdvantageAdData != null ? modelPageDTO_Mobile.AdvantageAdData.Savings : 0;
                overview.StockMaskingName = modelPageDTO_Mobile.AdvantageAdData != null ? modelPageDTO_Mobile.AdvantageAdData.Model.MaskingName : string.Empty;
                overview.DealsCityId = modelPageDTO_Mobile.AdvantageAdData != null ? (int)modelPageDTO_Mobile.AdvantageAdData.City.CityId : 0;
                overview.DealsModelName = modelPageDTO_Mobile.AdvantageAdData != null ? modelPageDTO_Mobile.AdvantageAdData.Model.ModelName : string.Empty;
                overview.AdAvailable = modelPageDTO_Mobile.SponsoredDealerAd != null && modelPageDTO_Mobile.SponsoredDealerAd.DealerId > 1;
                overview.AdvantageAdData = modelPageDTO_Mobile.AdvantageAdData;
                overview.CarPriceOverview = modelPageDTO_Mobile.CarPriceOverview != null ? AutoMapper.Mapper.Map<PriceOverview, PriceOverviewDTOV2>(modelPageDTO_Mobile.CarPriceOverview) : null;
                overview.ShowCampaignLink = modelPageDTO_Mobile.ShowCampaignLink;
                if (modelPageDTO_Mobile.CarVersion != null)
                {
                    overview.VersionId = modelPageDTO_Mobile.CarVersion.ID;
                    overview.VersionName = modelPageDTO_Mobile.CarVersion.Name;
                }
                else
                {
                    overview.VersionId = -1;
                    overview.VersionName = string.Empty;
                }

                overview.PredictionData = modelPageDTO_Mobile.SponsoredDealerAd != null ?
                    AutoMapper.Mapper.Map<PredictionData>(modelPageDTO_Mobile.SponsoredDealerAd.PredictionData) : new PredictionData();
                overview.LeadCTA = modelPageDTO_Mobile.SponsoredDealerAd != null ? modelPageDTO_Mobile.SponsoredDealerAd.CTALinkText : string.Empty;
                overview.LocationData = location;
                overview.PQPageId = 135;
            }
            catch (Exception ex)
            {

                Logger.LogException(ex);


            }

            return overview;
        }

        public CarOverviewDTO DesktopModelOverview(PriceInCityPageDTO modelDTO, CarDataAdapterInputs inputparam, List<NewCarVersionsDTO> newCarVersions)
        {
            CarOverviewDTO overview = new CarOverviewDTO();
            try
            {
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
                overview.IsUsedCarAvial = modelDTO.UsedCarsCount != null && (modelDTO.UsedCarsCount.LiveListingCount > 0 && modelDTO.UsedCarsCount.MinLiveListingPrice > 0);
                overview.MaskingName = modelDetailsObj.MaskingName;
                overview.LiveListingCount = modelDTO.UsedCarsCount != null ? modelDTO.UsedCarsCount.LiveListingCount : 0;
                overview.MinLiveListingPrice = modelDTO.UsedCarsCount != null ? modelDTO.UsedCarsCount.MinLiveListingPrice : 0;
                overview.MakeId = modelDetailsObj.MakeId;
                overview.ModelId = modelDetailsObj.ModelId;
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
                if (modelDTO.SponsoredDealerAd != null)
                {
                    overview.AdAvailable = modelDTO.SponsoredDealerAd.ActualDealerId > 0;
                    overview.DealerId = modelDTO.SponsoredDealerAd.DealerId;
                    overview.LeadCTA = modelDTO.SponsoredDealerAd.CTALinkText;
                    overview.PredictionData = AutoMapper.Mapper.Map<PredictionData>(modelDTO.SponsoredDealerAd.PredictionData);
                }

                overview.AdvantageAdData = modelDTO.AdvantageAdData;
                overview.CarPriceOverview = modelDTO.CarPriceOverview;
                overview.RootName = modelDTO.ModelDetails.RootName;
                overview.EMI = Calculation.Calculation.CalculateEmi(inputparam.IsCityPage ? (int)modelDetailsObj.MinPrice : (modelDTO.CarPriceOverview != null ? modelDTO.CarPriceOverview.Price : 0));

                overview.Is360ExteriorAvailable = modelDetailsObj.Is360ExteriorAvailable;
                overview.Is360OpenAvailable = modelDetailsObj.Is360OpenAvailable;
                overview.Is360InteriorAvailable = modelDetailsObj.Is360InteriorAvailable;
                overview.ImageUrl360Slug = CMSCommon.Get360DefaultCategory(AutoMapper.Mapper.Map<ThreeSixtyAvailabilityDTO>(modelDetailsObj)) == ThreeSixtyViewCategory.Interior ? CMSCommon.Get360ModelCarouselLinkageImageUrl(modelDetailsObj, true) : CMSCommon.Get360ModelCarouselLinkageImageUrl(modelDetailsObj);
                overview.CarName = string.Format("{0} {1}", modelDetailsObj.MakeName, modelDetailsObj.ModelName);
                int pageId = 31;
                if (modelDetailsObj.New && inputparam.IsCityPage)
                {
                    overview.BucketType = 2;
                    pageId = 55;
                }
                overview.PageId = pageId;
                overview.PhotoCount = modelDTO.ModelDetails.PhotoCount;
                overview.ShowColours = CMSCommon.IsModelColorPhotosPresent(modelDTO.ModelDetails.ModelColors);
                overview.ColoursCount = modelDTO.ModelDetails.ModelColors != null ? modelDTO.ModelDetails.ModelColors.Count : 0;
                overview.ModelPhotosListCarousel = AutoMapper.Mapper.Map<List<ModelImage>, List<ModelImageDTO>>(modelDTO.ModelPhotosListCarousel);
                overview.VideosCount = modelDTO.ModelDetails.VideoCount;
                overview.ShowCampaignLink = modelDTO.ShowCampaignLink;
                if (modelDTO.ModelDetails.VideoCount > 0)
                {
                    overview.VideoUrl = modelDTO.ModelVideos[0].VideoTitleUrl;
                }
                overview.NewCarVersions = AutoMapper.Mapper.Map<List<NewCarVersionsDTO>, List<NewCarVersionsDTOV2>>(newCarVersions);
            }
            catch (Exception ex)
            {

                Logger.LogException(ex);


            }
            return overview;
        }

        public CallSlugDTO SetCallSlugVM(PriceInCityPageDTO modelPageDTO_Mobile)
        {
            CallSlugDTO callSlug = new CallSlugDTO();
            try
            {
                var modelDetailsObj = modelPageDTO_Mobile.ModelDetails;

                callSlug.CarName = modelDetailsObj.MakeName + "_" + modelDetailsObj.ModelName;
                callSlug.ModelId = modelDetailsObj.ModelId;
                callSlug.VersionId = 0;
                callSlug.IsAdAvailable = modelPageDTO_Mobile.SponsoredDealerAd != null && modelPageDTO_Mobile.SponsoredDealerAd.DealerId > 1;
                callSlug.DealerName = callSlug.IsAdAvailable ? modelPageDTO_Mobile.SponsoredDealerAd.DealerName : string.Empty;
                callSlug.DealerMobile = callSlug.IsAdAvailable ? modelPageDTO_Mobile.SponsoredDealerAd.DealerMobile : string.Empty;
                callSlug.AdvantageAdData = modelPageDTO_Mobile.AdvantageAdData;
                callSlug.SponsoredDealerAd = modelPageDTO_Mobile.SponsoredDealerAd;
            }
            catch (Exception ex)
            {

                Logger.LogException(ex);


            }
            return callSlug;
        }

        private List<Entity.BreadcrumbEntity> BindBreadCrumb(CarModelDetails modelDetails, string cityName, bool isMobile)
        {

            List<Carwale.Entity.BreadcrumbEntity> _BreadcrumbEntitylist = new List<Carwale.Entity.BreadcrumbEntity>();
            try
            {
                string makeName = Format.FormatSpecial(modelDetails.MakeName);
                _BreadcrumbEntitylist.Add(new Entity.BreadcrumbEntity
                { Title = string.Format("{0} Cars", modelDetails.MakeName), Link = string.Format((isMobile ? "/m" : string.Empty) + "/{0}-cars/", makeName), Text = modelDetails.MakeName });

                _BreadcrumbEntitylist.Add(new Entity.BreadcrumbEntity
                { Title = modelDetails.ModelName, Link = ManageCarUrl.CreateModelUrl(makeName, modelDetails.MaskingName), Text = modelDetails.ModelName });
                _BreadcrumbEntitylist.Add(new Entity.BreadcrumbEntity { Text = string.Format("Price in {0}", cityName) });
            }
            catch (Exception ex)
            {

                Logger.LogException(ex);


            }

            return _BreadcrumbEntitylist;
        }
        private void SetVariantList(PriceInCityPageDTO mobilepageDTO)
        {
            try
            {
                mobilepageDTO.VariantList.Summary = mobilepageDTO.Summary;
                mobilepageDTO.VariantList.SponsoredDealerAd = mobilepageDTO.SponsoredDealerAd;
                mobilepageDTO.VariantList.ModelDetails = mobilepageDTO.ModelDetails;
                mobilepageDTO.VariantList.MobileOverviewDetails = mobilepageDTO.MobileOverviewDetails;
                mobilepageDTO.VariantList.NewCarVersions = mobilepageDTO.NewCarVersions;
                mobilepageDTO.VariantList.ShowCampaignLink = mobilepageDTO.ShowCampaignLink;
                mobilepageDTO.VariantList.PageId = (int)CwPages.PicMsite;
                mobilepageDTO.VariantList.ShowVariantList = _carMakeVariantList.Contains(mobilepageDTO.ModelDetails.MakeId.ToString());
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
    }
}
