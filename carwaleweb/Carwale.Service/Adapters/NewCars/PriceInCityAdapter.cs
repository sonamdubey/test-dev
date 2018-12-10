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
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Carwale.DTOs.CMS.ThreeSixtyView;
using Carwale.Interfaces.Template;
using Carwale.Entity.OffersV1;
using AutoMapper;
using Carwale.DTOs.OffersV1;
using Carwale.Interfaces.Offers;
using Carwale.Entity;
using Carwale.Entity.Dealers;
using Carwale.DTOs.Dealer;
using Carwale.DTOs.OfferAndDealerAd;
using Carwale.DTOs.Geolocation;
using Carwale.DTOs.Template;
using Carwale.Interfaces.Prices;
using Carwale.DTOs.LeadForm;
using Carwale.BL.Experiments;

namespace Carwale.BL.NewCars
{

    public class PriceInCityAdapter : IServiceAdapterV2
    {
        private readonly ICarModelCacheRepository _carModelsCacheRepo;
        private readonly ICarVersions _carVersionsBl;
        private readonly ICarModels _carModelsBl;
        private readonly IDeals _carDeals;
        private readonly ICarPriceQuoteAdapter _iPrices;
        private readonly INewCarDealers _newCarDealersBl;
        private readonly IMediaBL _media;
        private readonly IPhotos _photos;
        private readonly IStockCountCacheRepository _stockCountCacheRepo;
        private readonly IClassifiedListing _classifiedListing;
        private readonly ICMSContent _cmsContentCacheRepo;
        private readonly IVideosBL _videos;
        private readonly ICampaign _campaignBl;
        private readonly ITemplate _campaignTemplate;
        private readonly ITemplatesCacheRepository _tempCache;
        private readonly IDealerAdProvider _dealerAdProviderBl;
        private readonly IOffersAdapter _offersAdapter;
        private readonly ITemplateRender _template;
        private readonly IEmiCalculatorAdapter _emiCalculatorAdapter;

        public PriceInCityAdapter(ICarModels carModelsBl, ICarModelCacheRepository carModelsCacheRepo, ICarPriceQuoteAdapter iPrices,
            ICarVersions carVersionsBl, IDeals carDeals, INewCarDealers newCarDealersBl, ICMSContent cmsContentCacheRepo, ICampaign campaignBl,
            IMediaBL media, IPhotos photos, IStockCountCacheRepository stockCountCacheRepo, IClassifiedListing classifiedListing, IVideosBL videos,
            ITemplate campaignTemplate, ITemplatesCacheRepository tempCache, IDealerAdProvider dealerAdProviderBl, IOffersAdapter offersAdapter, ITemplateRender template
            , IEmiCalculatorAdapter emiCalculatorAdapter)
        {
            _carModelsBl = carModelsBl;
            _carModelsCacheRepo = carModelsCacheRepo;
            _iPrices = iPrices;
            _carVersionsBl = carVersionsBl;
            _carDeals = carDeals;
            _newCarDealersBl = newCarDealersBl;
            _cmsContentCacheRepo = cmsContentCacheRepo;
            _campaignBl = campaignBl;
            _media = media;
            _photos = photos;
            _stockCountCacheRepo = stockCountCacheRepo;
            _classifiedListing = classifiedListing;
            _videos = videos;
            _campaignTemplate = campaignTemplate;
            _tempCache = tempCache;
            _dealerAdProviderBl = dealerAdProviderBl;
            _offersAdapter = offersAdapter;
            _template = template;
            _emiCalculatorAdapter = emiCalculatorAdapter;
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
                    modelPageDTO.IsRenaultLeadCampaign = modelPageDTO.ModelDetails.ModelId.Equals(CWConfiguration.RenaultLeadFormModelId);

                    modelPageDTO.DealersList = _newCarDealersBl.NewCarDealerListByCityMake(modelPageDTO.ModelDetails.MakeId, inputParam.ModelDetails.ModelId, inputParam.CustLocation.CityId, modelPageDTO.ModelDetails.New);
                    
                    //Check if is DealerAvailable
                    if (modelPageDTO.DealersList == null || (modelPageDTO.DealersList != null && modelPageDTO.DealersList.NewCarDealers != null && modelPageDTO.DealersList.NewCarDealers.Count <= 0))
                    {
                        return modelPageDTO;
                    }
                    else
                    {
                        modelPageDTO.DealersList.CustLocation = inputParam.CustLocation;
                        modelPageDTO.NewCarDealersCount = modelPageDTO.DealersList.NewCarDealers.Count;
                    }

                    List<NewCarVersionsDTO> newCarVersions = new List<NewCarVersionsDTO>();
                    if (modelPageDTO.ModelDetails.New || (!modelPageDTO.ModelDetails.Futuristic)) //new or discontinue car
                    {
                        if (modelPageDTO.ModelDetails.New)
                        {
                            //get version for model and map to respective DTO
                            newCarVersions = _carVersionsBl.MapCarVersionDtoWithCarVersionEntity(inputParam.ModelDetails.ModelId, inputParam.CustLocation.CityId);
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

                            modelPageDTO.OfferAndDealerAd = new OfferAndDealerAdDTO();
                            modelPageDTO.OfferAndDealerAd.Platform = inputParam.IsMobile ? "Msite" : "Desktop";

                            modelPageDTO.OfferAndDealerAd.CarDetails = Mapper.Map<CarDetailsDTO>(modelDetails);

                            if (modelPageDTO.ModelDetails != null && modelPageDTO.CarVersion != null && inputParam.CustLocation != null
                                    && modelPageDTO.CarPriceOverview.Price > 0 && inputParam.IsMobile && !inputParam.IsAmp)
                            {
                                SetOffers(modelPageDTO, inputParam);
                            }
                            SetLeadSource(modelPageDTO);
                            modelPageDTO.LeadFormModelData = new LeadFormModelData();
                            modelPageDTO.LeadFormModelData.DealerAd = new List<DealerAdDTO>();
                            SetCampaignsVM(modelPageDTO, inputParam);
                            modelPageDTO.OfferAndDealerAd.Location = Mapper.Map<CityAreaDTO>(inputParam.CustLocation);
                            var similarCarVmRequest = new SimilarCarVmRequest
                            {
                                ModelId = modelDetails.ModelId,
                                MakeName = modelDetails.MakeName,
                                ModelName = modelDetails.ModelName,
                                MaskingName = modelDetails.MaskingName,
                                CityId = inputParam.CustLocation.CityId,
                                WidgetSource = (inputParam.IsMobile ? WidgetSource.PICPageAlternativeWidgetCompareCarLinkMobile : WidgetSource.PICPageAlternativeWidgetCompareCarLinkDesktop),
                                PageName = "CityPage",
                                IsMobile = inputParam.IsMobile,
                                CwcCookie = inputParam.CwcCookie
                            };

                            modelPageDTO.SimilarCars = _carModelsBl.GetSimilarCarVm(similarCarVmRequest);

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
                    var processVersionData = _carVersionsBl.ProcessVersionsData(inputParam.ModelDetails.ModelId, newCarVersions);
                    modelPageDTO.Summary = _carModelsBl.Summary(modelPageDTO.PageMetaTags.Summary
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
                        foreach (var version in modelPageDTO.DesktopOverviewDetails.NewCarVersions)
                        {
                            CarOverviewDTOV2 overview = new CarOverviewDTOV2();
                            overview.VersionId = version.Id;
                            overview.VersionName = version.Version;
                            overview.ModelId = version.ModelId;
                            overview.MakeName = modelPageDTO.ModelDetails.MakeName;
                            overview.ModelName = modelPageDTO.ModelDetails.ModelName;
                            overview.MakeId = modelPageDTO.ModelDetails.MakeId;
                            version.EmiCalculatorModelData = _emiCalculatorAdapter.GetEmiData(overview, null, null,
                                                                version.CarPriceOverview.PriceForSorting, inputParam.CustLocation.CityId);
                        }
                    }
                    else
                    {
                        modelPageDTO.ModelMenu = new ModelMenuDTO { ActiveSection = ModelMenuEnum.PriceInCity };
                        modelPageDTO.MobileOverviewDetails = MobileModelOverview(modelPageDTO, inputParam.AbTest, inputParam.CustLocation);

                        if (modelPageDTO.ModelDetails.New && !inputParam.IsAmp)
                        {
                            inputParam.CampaignInput.MakeId = modelPageDTO.ModelDetails.MakeId;
                            modelPageDTO.MobileOverviewDetails.CampaignTemplates = _campaignTemplate.GetTemplatesByPage(inputParam.CampaignInput, modelPageDTO.MobileOverviewDetails);
                            modelPageDTO.MobileOverviewDetails.IsToolTipCampaign = modelPageDTO.MobileOverviewDetails.CampaignTemplates != null
                               && modelPageDTO.MobileOverviewDetails.CampaignTemplates.ContainsKey((int)PageProperties.ToolTip);
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
                    if (modelPageDTO.ModelDetails.New)
                    {
                        SetLeadFormData(modelPageDTO, inputParam.CustLocation);    
                    }
                    modelPageDTO.ShowEmiCalculator = modelPageDTO.ModelDetails.New && !modelPageDTO.ModelDetails.Futuristic;
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
                subNavigation = _carModelsBl.GetModelQuickMenu(modelDTO.ModelDetails, null,
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

        private void SetLeadSource(PriceInCityPageDTO modelPageDTO)
        {
            try
            {
                var leadSources = new List<LeadSource>();
                leadSources.Add(Carwale.BL.Campaigns.LeadSource.GetLeadSource("OfferCampaignSlugWithOffer", (short)LeadSources.PriceInCity, (short)Platform.CarwaleMobile));
                leadSources.Add(Carwale.BL.Campaigns.LeadSource.GetLeadSource("OfferCampaignSlugWithOfferReco", (short)LeadSources.PriceInCity, (short)Platform.CarwaleMobile));
                leadSources.Add(Carwale.BL.Campaigns.LeadSource.GetLeadSource("OfferCampaignSlugWithoutOffer", (short)LeadSources.PriceInCity, (short)Platform.CarwaleMobile));
                leadSources.Add(Carwale.BL.Campaigns.LeadSource.GetLeadSource("OfferCampaignSlugWithoutOfferReco", (short)LeadSources.PriceInCity, (short)Platform.CarwaleMobile));

                modelPageDTO.OfferAndDealerAd.LeadSource.AddRange(Mapper.Map<List<LeadSourceDTO>>(leadSources));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        private void SetOffers(PriceInCityPageDTO modelPageDTO, CarDataAdapterInputs inputParam)
        {
            try
            {
                var offerInput = new OfferInput
                {
                    ApplicationId = (int)Application.CarWale,
                    CityId = inputParam.CustLocation.CityId,
                    StateId = inputParam.CustLocation.StateId,
                    MakeId = modelPageDTO.ModelDetails.MakeId,
                    ModelId = modelPageDTO.ModelDetails.ModelId,
                    VersionId = modelPageDTO.CarVersion.ID
                };

                modelPageDTO.OfferAndDealerAd.Offer = _offersAdapter.GetOffers(offerInput);
                modelPageDTO.OfferAndDealerAd.Page = Pages.PIC;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        private void SetCampaignsVM(PriceInCityPageDTO modelPageDTO, CarDataAdapterInputs inputParam)
        {
            try
            {
                //Fetching campaign
                var carDetails = new CarIdEntity { MakeId = modelPageDTO.ModelDetails.MakeId, ModelId = inputParam.ModelDetails.ModelId };
                var persistedCampaignId = _campaignBl.GetPersistedCampaign(inputParam.ModelDetails.ModelId, inputParam.CustLocation);
                var dealerAd = _dealerAdProviderBl.GetDealerAd(carDetails, inputParam.CustLocation, inputParam.IsMobile ? (int)Platform.CarwaleMobile : (int)Platform.CarwaleDesktop,
                            (int)CampaignAdType.Pq, persistedCampaignId, (int)Pages.ModelPageId);

                if (dealerAd != null && dealerAd.Campaign != null)
                {
                    var dealerAdDto = Mapper.Map<DealerAdDTO>(dealerAd);
                    modelPageDTO.DealerAd = dealerAdDto;
                    modelPageDTO.OfferAndDealerAd.DealerAd = dealerAdDto;

                    modelPageDTO.LeadFormModelData.DealerAd.Add(dealerAdDto);
                                       
                        //Getting dealer details
                    var dealerDetails = _newCarDealersBl.NCDDetails(modelPageDTO.DealerAd.Campaign.DealerId, modelPageDTO.DealerAd.Campaign.Id, modelPageDTO.ModelDetails.MakeId, inputParam.CustLocation.CityId);

                        if (dealerDetails.CampaignId > 0)
                        {
                            modelPageDTO.OfferAndDealerAd.DealerAd.Campaign.DealerShowroom = AutoMapper.Mapper.Map<Entity.Dealers.DealerDetails, DealersDTO>(dealerDetails);
                        }
                    
                }
                else
                {
                    //Handling case when only city is set and campaign is available for atleast one area
                    modelPageDTO.ShowCampaignLink = _campaignBl.IsCityCampaignExist(inputParam.ModelDetails.ModelId, inputParam.CustLocation,
                        inputParam.IsMobile ? (int)Platform.CarwaleMobile : (int)Platform.CarwaleDesktop, (int)Application.CarWale);
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
                selectedVersions = _carVersionsBl.GetSelectedVersionDetails(inputParam.CompareVersionsCookie);

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


        private PageMetaTags GetPageMetaTags(PriceInCityPageDTO modelDetails, string cityName, string cityMaskingName)
        {
            PageMetaTags pageMetaTags = new PageMetaTags();
            try
            {
                pageMetaTags = _carModelsCacheRepo.GetModelPageMetaTags(modelDetails.ModelDetails.ModelId, Convert.ToInt16(Pages.PIC));
                pageMetaTags.Title = GetTitle(modelDetails.ModelDetails, cityName, pageMetaTags);
                pageMetaTags.Description = GetDescription(modelDetails, cityName);
                pageMetaTags.Heading = _carModelsBl.Heading(pageMetaTags.Heading ?? "", modelDetails.ModelDetails.MakeName, modelDetails.ModelDetails.ModelName);
                pageMetaTags.Canonical = ManageCarUrl.CreatePriceInCityUrl(modelDetails.ModelDetails.MakeName, modelDetails.ModelDetails.MaskingName, cityMaskingName, true);
            }
            catch (Exception ex)
            {

                Logger.LogException(ex);

            }

            return pageMetaTags;
        }

        private string GetTitle(CarModelDetails modelDetails, string cityName, PageMetaTags pageMetaTags)
        {
            try
            {
                string title = string.Empty;
                var modelMinPrice = Format.GetFormattedPriceV2(modelDetails.MinPrice > 0 ? modelDetails.MinPrice.ToString() : null);
                var modelMaxPrice = Format.GetFormattedPriceV2(modelDetails.MaxPrice > 0 ? modelDetails.MaxPrice.ToString() : null);
                var price = string.Format("₹ {0}{1}", modelMinPrice, ((modelMinPrice != modelMaxPrice) ? " to ₹ " + modelMaxPrice : string.Empty));
                if (!string.IsNullOrWhiteSpace(pageMetaTags.Title))
                {
                    string templateName = string.Format("{0}-{1}-title", pageMetaTags.Id, pageMetaTags.UpdatedOn.ToString());
                    title = _template.Render<MetaDataTemplateDto>(templateName, new MetaDataTemplateDto { CityName = cityName, Price = price }, pageMetaTags.Title);
                }
                else
                {
                    title = string.Format("{0} {1} price (GST Rates) in {2} - {3}", modelDetails.MakeName, modelDetails.ModelName, cityName, price);
                }
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
                var formatFullPrice = Format.GetFormattedPriceV2((modelDetails.MinPrice <= 0 ? null :
                    modelDetails.MinPrice.ToString()), (modelDetails.MaxPrice <= 0 ? null : modelDetails.MaxPrice.ToString()));
                if (modelPageDTO_Mobile.PageMetaTags != null && !string.IsNullOrWhiteSpace(modelPageDTO_Mobile.PageMetaTags.Description))
                {
                    string templateName = string.Format("{0}-{1}-description", modelPageDTO_Mobile.PageMetaTags.Id, modelPageDTO_Mobile.PageMetaTags.UpdatedOn.ToString());
                    string description = _template.Render<MetaDataTemplateDto>(templateName, new MetaDataTemplateDto
                    { CityName = globalCityName, Price = formatFullPrice }, modelPageDTO_Mobile.PageMetaTags.Description);
                    return description;
                }
                else if (modelPageDTO_Mobile.CarPriceOverview != null && modelPageDTO_Mobile.CarPriceOverview.PriceStatus == 1)
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
                overview.AdvantageAdData = modelPageDTO_Mobile.AdvantageAdData;
                overview.CarPriceOverview = modelPageDTO_Mobile.CarPriceOverview != null ? AutoMapper.Mapper.Map<PriceOverview, PriceOverviewDTOV2>(modelPageDTO_Mobile.CarPriceOverview) : null;
                overview.ShowCampaignLink = modelPageDTO_Mobile.ShowCampaignLink;
                overview.DealerAd = modelPageDTO_Mobile.DealerAd;
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

                if (modelPageDTO_Mobile.DealerAd != null && modelPageDTO_Mobile.DealerAd.Campaign != null)
                {
                    overview.CampaignDealerId = String.Format("{0}_{1}", overview.DealerAd.Campaign.Id, overview.DealerAd.Campaign.DealerId);
                    overview.AdAvailable = modelPageDTO_Mobile.DealerAd.Campaign.Id > 1;
                    overview.PredictionData = AutoMapper.Mapper.Map<PredictionData>(modelPageDTO_Mobile.DealerAd.Campaign.PredictionData);
                    overview.LeadCTA = modelPageDTO_Mobile.DealerAd.Campaign.ActionText;
                    overview.CampaignLeadCTA = modelPageDTO_Mobile.DealerAd.Campaign.ActionText;
                }
                else
                {
                    overview.PredictionData = new PredictionData();
                }
                overview.ShowCustomiseYourEmiLink = true;

                overview.UserLocation = Mapper.Map<CityAreaDTO>(location);
                overview.PQPageId = 135;
                overview.PageId = (int)CwPages.PicMsite;
                if (overview.New && !overview.Futuristic)
                {
                    var dealerAd = modelPageDTO_Mobile.OfferAndDealerAd != null ? modelPageDTO_Mobile.OfferAndDealerAd.DealerAd : null;
                    var price = modelPageDTO_Mobile.CarPriceOverview != null ? modelPageDTO_Mobile.CarPriceOverview.Price : 0;
                    overview.EmiCalculatorModelData = _emiCalculatorAdapter.GetEmiData(overview, dealerAd,
                              new LeadSourceDTO { LeadClickSourceId = 396, InquirySourceId = 155 }, price, location.CityId);
                    if (overview.EmiCalculatorModelData != null)
                    {
                        overview.EmiCalculatorModelData.Page = CwPages.PicMsite;
                        overview.EmiCalculatorModelData.PageName = CwPages.PicMsite.ToString();
                        overview.EmiCalculatorModelData.Platform = "Msite";
                    }
                }
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
                if (modelDTO.DealerAd != null && modelDTO.DealerAd.Campaign != null)
                {
                    overview.AdAvailable = modelDTO.DealerAd.Campaign.DealerId > 0;
                    overview.DealerId = modelDTO.DealerAd.Campaign.Id;
                    overview.ActualDealerId = modelDTO.DealerAd.Campaign.DealerId;
                    overview.LeadCTA = modelDTO.DealerAd.Campaign.ActionText;
                    overview.CampaignLeadCTA = modelDTO.DealerAd.Campaign.ActionText;
                    overview.PredictionData = modelDTO.DealerAd.Campaign.PredictionData;
                }
                else
                {
                    overview.PredictionData = new PredictionData();
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
                overview.IsPicSnippetExperiment = ProductExperiments.IsPicTableFormat(inputparam.ModelDetails.ModelId,inputparam.CustLocation.CityId);
                var dealerAd = modelDTO.OfferAndDealerAd != null ? modelDTO.OfferAndDealerAd.DealerAd : null;
                var price = modelDTO.CarPriceOverview != null ? modelDTO.CarPriceOverview.Price : 0;

                if (overview.CarPriceOverview != null)
                {
                    overview.EmiCalculatorModelData = _emiCalculatorAdapter.GetEmiData(Mapper.Map<CarOverviewDTO, CarOverviewDTOV2>(overview), null, null,
                        overview.CarPriceOverview.Price, inputparam.CustLocation.CityId);
                }

                if (overview.EmiCalculatorModelData != null)
                {
                    overview.EmiCalculatorModelData.PageName = CwPages.PicDesktop.ToString();
                    overview.EmiCalculatorModelData.Platform = "Desktop";
                }
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
                callSlug.IsAdAvailable = modelPageDTO_Mobile.DealerAd != null && modelPageDTO_Mobile.DealerAd.Campaign.Id > 1;
                callSlug.DealerName = callSlug.IsAdAvailable ? modelPageDTO_Mobile.DealerAd.Campaign.ContactName : string.Empty;
                callSlug.DealerMobile = callSlug.IsAdAvailable ? modelPageDTO_Mobile.DealerAd.Campaign.ContactNumber : string.Empty;
                callSlug.AdvantageAdData = modelPageDTO_Mobile.AdvantageAdData;
                callSlug.DealerAd = modelPageDTO_Mobile.DealerAd;
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
                _BreadcrumbEntitylist.Add(new Entity.BreadcrumbEntity { Title = string.Format("{0} Cars", modelDetails.MakeName), Link = string.Format((isMobile ? "/m" : string.Empty) + "/{0}-cars/", makeName), Text = modelDetails.MakeName });

                _BreadcrumbEntitylist.Add(new Entity.BreadcrumbEntity { Title = modelDetails.ModelName, Link = ManageCarUrl.CreateModelUrl(makeName, modelDetails.MaskingName), Text = modelDetails.ModelName });
                _BreadcrumbEntitylist.Add(new Entity.BreadcrumbEntity { Text = string.Format("{0} price in {1}",modelDetails.ModelName, cityName) });
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
                mobilepageDTO.VariantList.DealerAd = mobilepageDTO.DealerAd;
                mobilepageDTO.VariantList.ModelDetails = mobilepageDTO.ModelDetails;
                mobilepageDTO.VariantList.MobileOverviewDetails = mobilepageDTO.MobileOverviewDetails;
                mobilepageDTO.VariantList.NewCarVersions = mobilepageDTO.NewCarVersions;
                mobilepageDTO.VariantList.ShowCampaignLink = mobilepageDTO.ShowCampaignLink;
                mobilepageDTO.VariantList.PageId = (int)CwPages.PicMsite;
                mobilepageDTO.VariantList.ShowVariantList = (mobilepageDTO.ModelDetails.New && mobilepageDTO.NewCarVersions != null && mobilepageDTO.NewCarVersions.Count > 0);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
        private void SetLeadFormData(PriceInCityPageDTO picDTO, Location location)
        {
            try
            {
                picDTO.LeadFormModelData.CustLocation = Mapper.Map<Location>(location);
                if (picDTO.DealersList != null && picDTO.DealersList.NewCarDealers != null)
                {
                    var modelDetails = Mapper.Map<CarIdWithImageDto>(picDTO.ModelDetails);
                    foreach (var dealer in picDTO.DealersList.NewCarDealers)
                    {
                        if (dealer.CampaignId > 0)
                        {
                            if (!picDTO.LeadFormModelData.DealerAd.Exists(x => x.Campaign.Id == dealer.CampaignId && x.Campaign.DealerId == dealer.DealerId))
                            {
                                var dealerad = Mapper.Map<DealerAdDTO>(dealer);
                                dealerad.FeaturedCarData = modelDetails;
                                dealerad.CampaignType = (int)CampaignAdType.DealerLocator;
                                picDTO.LeadFormModelData.DealerAd.Add(dealerad);
                            }
                        }
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
