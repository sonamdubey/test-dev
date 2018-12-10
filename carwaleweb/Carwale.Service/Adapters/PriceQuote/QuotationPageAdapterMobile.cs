using AutoMapper;
using Carwale.BL.GeoLocation;
using Carwale.DTOs.Campaigns;
using Carwale.DTOs.CarData;
using Carwale.DTOs.Geolocation;
using Carwale.DTOs.OfferAndDealerAd;
using Carwale.DTOs.OffersV1;
using Carwale.DTOs.PageProperty;
using Carwale.DTOs.PriceQuote;
using Carwale.DTOs.Template;
using Carwale.Entity;
using Carwale.Entity.Campaigns;
using Carwale.Entity.CarData;
using Carwale.Entity.Enum;
using Carwale.Entity.Geolocation;
using Carwale.Entity.OffersV1;
using Carwale.Entity.Price;
using Carwale.Entity.PriceQuote;
using Carwale.Entity.Template;
using Carwale.Interfaces.Campaigns;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.Customer;
using Carwale.Interfaces.Geolocation;
using Carwale.Interfaces.NewCars;
using Carwale.Interfaces.Offers;
using Carwale.Interfaces.PriceQuote;
using Carwale.Interfaces.Prices;
using Carwale.Notifications.Logs;
using Carwale.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Carwale.Service.Adapters.PriceQuote
{
    public class QuotationPageAdapterMobile : IServiceAdapterV2
    {
        private readonly ICarVersionCacheRepository _versionCacheRepo;
        private readonly IPriceQuoteBL _priceQuoteBL;
        private readonly IPQGeoLocationBL _locationBL;
        private readonly ICarModelCacheRepository _modelCacheRepo;
        private readonly IGeoCitiesCacheRepository _citiesCacheRepo;
        private readonly IDealerAdProvider _dealerAdProviderBl;
        private readonly IPrices _pricesBl;
        private readonly ICustomerTracking _trackingBL;
        private readonly IOffersAdapter _offersAdapter;
        private readonly ITemplate _campaignTemplate;
        private readonly IEmiCalculatorBl _emiCalculatorBl;
        private readonly IQuotationAdapterCommon _quotationAdapterCommon;
        private readonly IEmiCalculatorAdapter _emiCalculatorAdapter;

        private readonly short NearByCitiesCount = 3;
        private static readonly int OptionalChargesDisplayCount = CustomParser.parseIntObject(ConfigurationManager.AppSettings["OptionalChargesDisplayCount"]);
        private readonly string _offersCtaText = ConfigurationManager.AppSettings["OffersCtaButtonText"] ?? string.Empty;
        private readonly string _leadSourcePrefix = "NewPq-";
        public QuotationPageAdapterMobile(ICarVersionCacheRepository versionCacheRepo, IPriceQuoteBL priceQuoteBL, IPQGeoLocationBL locationBL,
            ICarModelCacheRepository modelCacheRepo, IGeoCitiesCacheRepository citiesCacheRepo, IDealerAdProvider dealerAdProviderBl,
            IPrices pricesBl, ICustomerTracking trackingBL, ITemplate campaignTemplate, IEmiCalculatorBl emiCalculatorBl, IQuotationAdapterCommon quotationAdapterCommon,
            IOffersAdapter offersAdapter, IEmiCalculatorAdapter emiCalculatorAdapter)
        {
            _versionCacheRepo = versionCacheRepo;
            _priceQuoteBL = priceQuoteBL;
            _locationBL = locationBL;
            _modelCacheRepo = modelCacheRepo;
            _citiesCacheRepo = citiesCacheRepo;
            _dealerAdProviderBl = dealerAdProviderBl;
            _pricesBl = pricesBl;
            _trackingBL = trackingBL;
            _campaignTemplate = campaignTemplate;
            _emiCalculatorBl = emiCalculatorBl;
            _offersAdapter = offersAdapter;
            _quotationAdapterCommon = quotationAdapterCommon;
            _emiCalculatorAdapter = emiCalculatorAdapter;
        }

        public T Get<T, U>(U input)
        {
            return (T)Convert.ChangeType(GetPriceQuoteDetails<U>(input), typeof(T));
        }

        /// <summary>
        /// This function returns the price quote list for input model/version, city/area provided
        /// </summary>
        /// <typeparam name="U">List<QuotationPageMobileDTO></typeparam>
        /// <param name="priceQuoteInput">PriceQuoteInput</param>
        /// <returns></returns>
        private List<QuotationPageMobileDTO> GetPriceQuoteDetails<U>(U priceQuoteInput)
        {
            try
            {
                var input = (PriceQuoteInput)Convert.ChangeType(priceQuoteInput, typeof(U));

                #region Validations

                if (input == null)
                {
                    return null;
                }

                var modelDetails = (input.ModelId > 0) ? _modelCacheRepo.GetModelDetailsById(input.ModelId) : null;
                var versionDetails = input.VersionId > 0 ? _versionCacheRepo.GetVersionDetailsById(input.VersionId) : null;
                var cityDetails = (input.CityId > 0) ? _citiesCacheRepo.GetCityDetailsById(input.CityId) : null;

                if (!_quotationAdapterCommon.IsBasicInputValid(input, versionDetails))
                {
                    return null;
                }

                input = _quotationAdapterCommon.GetCompleteInput(input, versionDetails);

                if (input == null)
                {
                    return null;
                }

                modelDetails = modelDetails ?? _modelCacheRepo.GetModelDetailsById(input.ModelId);
                versionDetails = versionDetails ?? _versionCacheRepo.GetVersionDetailsById(input.VersionId);
                cityDetails = cityDetails ?? _citiesCacheRepo.GetCityDetailsById(input.CityId);

                if (!_quotationAdapterCommon.IsCompleteInputValid(modelDetails, versionDetails, cityDetails))
                {
                    return null;
                }

                #endregion

                var priceQuoteList = new List<QuotationPageMobileDTO>();
                var priceQuote = GetPriceQuote(input, versionDetails);

                if (priceQuote == null)
                {
                    return priceQuoteList;
                }
                priceQuoteList.Add(priceQuote);
                if (!input.HideCampaign && _priceQuoteBL.FetchCrossSell(priceQuote.PriceQuote, priceQuote.DealerAd))
                {
                    GetCrossSellPriceQuote(input, priceQuote.DealerAd, ref priceQuoteList);
                    priceQuoteList[0].DealerAd = null;
                    priceQuote.DealerAd = null;
                }

                TrackPriceQuoteImpression(input, priceQuote.DealerAd, priceQuote.PriceQuote, priceQuote.Source);

                if (input.IsCrossSellPriceQuote)
                {
                    var currentQuotation = priceQuoteList[0];
                    //This is hardcoded as current requirement says that only one cross sell will be there and at card 2 only
                    priceQuoteList[0].CardNo = 2;
                    //commented this code for hiding the emi calculator on version city change on cross sell card
                    //priceQuoteList[0].EmiCalculatorModelData.AddRange(GetEmiCalculatorModelData(currentQuotation.CarDetails,
                    //    currentQuotation.PriceQuote, currentQuotation.CardNo, currentQuotation.DealerAd));
                }
                else
                {
                    for (int emiModel = 0; emiModel < priceQuoteList.Count; emiModel++)
                    {
                        int cardNo = emiModel + 1;
                        priceQuoteList[emiModel].EmiCalculatorModelData.AddRange(GetEmiCalculatorModelData(priceQuoteList[emiModel].CarDetails,
                            priceQuoteList[emiModel].PriceQuote, cardNo, priceQuoteList[emiModel].DealerAd));


                        if (cardNo == 2 && (priceQuoteList[0].EmiCalculatorModelData.IsNotNullOrEmpty() || priceQuoteList[1].EmiCalculatorModelData.IsNotNullOrEmpty()))
                        {
                            priceQuoteList[1].HideCrossSellEmiSlug = priceQuoteList[0].EmiCalculatorModelData[0].IsEligibleForThirdPartyEmi
                                                                                || priceQuoteList[1].EmiCalculatorModelData[0].IsEligibleForThirdPartyEmi;
                        }
                    }
                }

                return priceQuoteList;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return null;
        }

        /// <summary>
        /// This function gets pricequote for input version city area provided
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private QuotationPageMobileDTO GetPriceQuote(PriceQuoteInput input, CarVersionDetails versionDetails)
        {
            try
            {

                var priceQuoteDTO = new QuotationPageMobileDTO();
                priceQuoteDTO.Source = BrowserUtils.IsAndroidWebView() ? Platform.CarwaleAndroid : BrowserUtils.IsIosWebView() ? Platform.CarwaleiOS : Platform.CarwaleMobile;

                priceQuoteDTO.CarDetails = Mapper.Map<PQCarDetails>(versionDetails);
                priceQuoteDTO.Versions.AddRange(_priceQuoteBL.GetVersions(priceQuoteDTO.CarDetails.ModelId, input.CityId));
                priceQuoteDTO.Location = _locationBL.GetCityDetails(input.CityId, 0, input.AreaId);
                GetPriceOrNearByCities(ref priceQuoteDTO, priceQuoteDTO.CarDetails, input.CityId, input.IsCrossSellPriceQuote);

                if (priceQuoteDTO.PriceQuote.Count > 0 && !input.HideCampaign)
                {
                    GetDealerAd(ref priceQuoteDTO);
                }
                if (input.IsCrossSellPriceQuote && _priceQuoteBL.FetchCrossSell(priceQuoteDTO.PriceQuote, priceQuoteDTO.DealerAd))
                {
                    priceQuoteDTO.DealerAd = null;
                }

                int pageId = priceQuoteDTO.Source == Platform.CarwaleAndroid ? (int)CwPages.QuotationAndroid : priceQuoteDTO.Source == Platform.CarwaleiOS ? (int)CwPages.QuotationIos : (int)CwPages.QuotationMsite;
                priceQuoteDTO.CampaignTemplates = _campaignTemplate.GetTemplatesByPage(GetCampaignTemplatesInput(priceQuoteDTO.Location.CityId, priceQuoteDTO.CarDetails.ModelId, priceQuoteDTO.CarDetails.MakeId, (short)priceQuoteDTO.Source, pageId), priceQuoteDTO);
                priceQuoteDTO.ShowSellCarLink = Carwale.BL.PresentationLogic.NewCar.ShowSellCarLinkOnPq(input.CityId);
                priceQuoteDTO.IsCrossSellPriceQuote = input.IsCrossSellPriceQuote;
                priceQuoteDTO.PageId = input.PageId;
                priceQuoteDTO.OptionalChargesDisplayCount = OptionalChargesDisplayCount;
                GetOffer(ref priceQuoteDTO);

                return priceQuoteDTO;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "MobileAdapter.GetPriceQuote() : modelId : " + input.ModelId + " VersionId : " + input.VersionId + " City : " + input.CityId);
            }

            return null;
        }

        private CampaignInputv2 GetCampaignTemplatesInput(int cityId, int modelId, int makeId, short platform, int pageId)
        {
            return new CampaignInputv2
            {
                MakeId = makeId,
                ModelId = modelId,
                PlatformId = platform,
                ApplicationId = (short)Application.CarWale,
                CityId = cityId,
                PageId = pageId
            };
        }

        private void GetOffer(ref QuotationPageMobileDTO priceQuoteDTO)
        {
            priceQuoteDTO.OfferAndDealerAd = new OfferAndDealerAdDTO();
            priceQuoteDTO.OfferAndDealerAd.Platform = "Msite";
            if (priceQuoteDTO.CarDetails != null && priceQuoteDTO.Location != null && priceQuoteDTO.PriceQuote.Count > 0)
            {
                priceQuoteDTO.OfferAndDealerAd.CarDetails = Mapper.Map<CarDetailsDTO>(priceQuoteDTO.CarDetails);
                SetOffers(priceQuoteDTO);
            }
        }

        private void SetOffers(QuotationPageMobileDTO priceQuoteDTO)
        {
            try
            {
                var offerInput = Mapper.Map<PQCarDetails, OfferInput>(priceQuoteDTO.CarDetails);
                offerInput.CityId = priceQuoteDTO.Location.CityId;
                priceQuoteDTO.OfferAndDealerAd.Offer = _offersAdapter.GetOffers(offerInput);

                if (priceQuoteDTO.OfferAndDealerAd.Offer != null && priceQuoteDTO.OfferAndDealerAd.Offer.OfferDetails != null)
                {
                    SetDealerAndLeadDetails(priceQuoteDTO);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        private void SetDealerAndLeadDetails(QuotationPageMobileDTO priceQuoteDTO)
        {
            priceQuoteDTO.OfferAndDealerAd.DealerAd = Mapper.Map<DealerAdDTO>(priceQuoteDTO.DealerAd);
            priceQuoteDTO.OfferAndDealerAd.Page = Pages.PriceQuote;
            if (priceQuoteDTO.DealerAd != null && priceQuoteDTO.DealerAd.CampaignType == CampaignAdType.Pq)
            {
                priceQuoteDTO.OfferAndDealerAd.CampaignDetailsVisible = false;
                SetLeadSource(priceQuoteDTO);
            }
            priceQuoteDTO.OfferAndDealerAd.Location = Mapper.Map<CityAreaDTO>(priceQuoteDTO.Location);
        }

        /// <summary>
        /// This function fetches prices for input version city provided
        /// If price is not available then fetches nearbycities list where price is available
        /// </summary>
        /// <param name="priceQuoteDTO"></param>
        /// <param name="input"></param>
        private void GetPriceOrNearByCities(ref QuotationPageMobileDTO priceQuoteDTO, PQCarDetails carDetails, int cityId, bool isCrossSellPriceQuote)
        {
            var priceList = _priceQuoteBL.GetVersionPrice(carDetails.ModelId, carDetails.VersionId, cityId);
            if (priceList != null)
            {
                priceQuoteDTO.PriceQuote.AddRange(priceList);
            }
            else
            {
                priceQuoteDTO.NearByCities = Mapper.Map<NearByCityDetailsDto>(_pricesBl.GetNearbyCities(carDetails.VersionId, cityId, NearByCitiesCount));
                priceQuoteDTO.NearByCities.VersionId = carDetails.VersionId;
                priceQuoteDTO.NearByCities.IsCrossSellPriceQuote = isCrossSellPriceQuote;
                priceQuoteDTO.NearByCities.Location = priceQuoteDTO.Location;
                priceQuoteDTO.NearByCities.CardNo = isCrossSellPriceQuote ? 2 : 1; // Only two cards allowed in M-Site. Some javascript logic is there based on card no. So required
            }
        }

        /// <summary>
        /// This function fetches the crosssell pricequote for input featured version, 
        /// city and area provided
        /// </summary>
        /// <param name="input"></param>
        /// <param name="dealerAd"></param>
        /// <param name="priceQuoteList"></param>
        private void GetCrossSellPriceQuote(PriceQuoteInput input, DealerAd dealerAd, ref List<QuotationPageMobileDTO> priceQuoteList)
        {
            var crossSellInput = new PriceQuoteInput
                            {
                                VersionId = dealerAd.FeaturedCarData.VersionId,
                                CityId = input.CityId,
                                AreaId = input.AreaId,
                                IsCrossSellPriceQuote = true,
                                HideCampaign = input.HideCampaign
                            };

            var crossSellPriceQuote = GetPriceQuote(crossSellInput, _versionCacheRepo.GetVersionDetailsById(crossSellInput.VersionId));

            if (crossSellPriceQuote != null && crossSellPriceQuote.PriceQuote.Count > 0)
            {
                crossSellPriceQuote.DealerAd = dealerAd;
                crossSellPriceQuote.IsCrossSellPriceQuote = true;
                crossSellPriceQuote.PageId = input.PageId;

                crossSellInput.ModelId = crossSellPriceQuote.CarDetails.ModelId;
                TrackPriceQuoteImpression(crossSellInput, dealerAd, crossSellPriceQuote.PriceQuote, crossSellPriceQuote.Source);

                priceQuoteList.Add(crossSellPriceQuote);
            }
        }

        /// <summary>
        /// This function fetches Dealer Ad for input model, city or area
        /// </summary>
        /// <param name="priceQuoteDTO"></param>
        /// <param name="input"></param>
        private void GetDealerAd(ref QuotationPageMobileDTO priceQuoteDTO)
        {
            var carDetails = Mapper.Map<PQCarDetails, CarIdEntity>(priceQuoteDTO.CarDetails);

            //27 implies PQ page for IOS
            var pageId = BrowserUtils.IsIosWebView() ? 27 : 0;
            priceQuoteDTO.DealerAd = _dealerAdProviderBl.GetDealerAd(carDetails, priceQuoteDTO.Location, Convert.ToInt32(priceQuoteDTO.Source), 0, 0, pageId);
            if (priceQuoteDTO.DealerAd != null)
            {
                priceQuoteDTO.DealerAd.LeadSource = GetLeadSources(priceQuoteDTO.DealerAd.CampaignType);
            }
        }

        private void TrackPriceQuoteImpression(PriceQuoteInput input, DealerAd dealerAd, List<Entity.Price.PriceQuote> priceList, Platform platform)
        {
            bool priceAvailable = priceList != null && priceList.Count > 0;

            CarDataTrackingEntity carDataObj = Mapper.Map<PriceQuoteInput, CarDataTrackingEntity>(input);
            carDataObj.Platform = (int)platform;
            carDataObj.CampaignType = dealerAd != null ? (int)dealerAd.CampaignType : 0;
            carDataObj.OnRoadPrice = _priceQuoteBL.CalculateOnRoadPrice(priceList);
            _trackingBL.TrackPriceQuoteImpression(carDataObj, dealerAd);
        }


        private List<EmiCalculatorModelData> GetEmiCalculatorModelData(PQCarDetails carDetails, List<Entity.Price.PriceQuote> priceQuoteList, int cardNo, DealerAd dealerAd)
        {
            try
            {
                List<EmiCalculatorModelData> EmiCalculatorModelDataList = new List<EmiCalculatorModelData>();

                if (priceQuoteList == null)
                {
                    return EmiCalculatorModelDataList;
                }

                for (int pq = 0; pq < priceQuoteList.Count; pq++)
                {
                    EmiCalculatorModelData emiCalculatorModelData = Mapper.Map<PQCarDetails, EmiCalculatorModelData>(carDetails);

                    if (emiCalculatorModelData == null)
                    {
                        return EmiCalculatorModelDataList;
                    }

                    var compulsoryPrices = priceQuoteList[pq].ChargeGroup.Where(x => x.Type == 1).ToList();

                    var downPaymentValues = _emiCalculatorBl.GetDownPaymentValues(compulsoryPrices);
                    emiCalculatorModelData.DownPaymentMinValue = downPaymentValues.Item1;
                    emiCalculatorModelData.DownPaymentMaxValue = priceQuoteList[pq].OnRoadPrice;
                    emiCalculatorModelData.DownPaymentDefaultValue = _emiCalculatorBl.CalculateDownPaymentDefaultValue(downPaymentValues.Item1, downPaymentValues.Item2);
                    emiCalculatorModelData.IsMetallic = priceQuoteList[pq].IsMetallic;
                    emiCalculatorModelData.UniqueKey = (CustomParser.parseStringObject(cardNo) + "_" + (priceQuoteList[pq].IsMetallic ? "m" : "s"));
                    emiCalculatorModelData.DealerAd = Mapper.Map<DealerAd, DealerAdDTO>(dealerAd);
                    emiCalculatorModelData.Page = CwPages.QuotationMsite;
                    if (dealerAd != null)
                    {
                        var template = _campaignTemplate.GetEmiCalculatorTemplate();
                        emiCalculatorModelData.DealerAd.PageProperty = new List<PagePropertyDTO> { 
                        new PagePropertyDTO { 
                            Template = Mapper.Map<TemplateDTO>(template) 
                            }
                        };

                        //TODO: This is a temporary fix. It will be removed when DealerAd.PageProperties is used
                        emiCalculatorModelData.CtaDetails = new LeadSourceDTO
                        {
                            LeadClickSourceId = dealerAd.CampaignType == CampaignAdType.Pq ? 366 : 368,
                            InquirySourceId = 36
                        };
                    }

                    var emiCalculatorDataThirdParty = _emiCalculatorAdapter.GetEmiSummary(carDetails.VersionId, emiCalculatorModelData.IsMetallic, (int)priceQuoteList[pq].OnRoadPrice);
                    emiCalculatorModelData.ThirdPartyEmiDetails = emiCalculatorDataThirdParty.ThirdPartyEmiDetails;
                    emiCalculatorModelData.IsEligibleForThirdPartyEmi = emiCalculatorDataThirdParty.IsEligibleForThirdPartyEmi;
                    emiCalculatorModelData.PageName = CwPages.QuotationMsite.ToString();
                    emiCalculatorModelData.Platform = "Msite";
                    EmiCalculatorModelDataList.Add(emiCalculatorModelData);
                }
                return EmiCalculatorModelDataList;
            }
            catch (Exception err)
            {
                Logger.LogException(err, "EmiCalculatorBl.GetEmiCalculatorModelData()");
                return null;
            }
        }


        #region LeadSources

        /// <summary>
        /// This function returns the list of leadsources for particular ad type
        /// </summary>
        /// <param name="adType"></param>
        /// <returns></returns>
        private List<LeadSource> GetLeadSources(CampaignAdType adType)
        {
            var leadSources = new List<LeadSource>();
            string campaignType = GetCampaignTypeInTemplate(adType);

            if (campaignType != null)
            {
                //Platform is fixed as mobile as App leads will also go with mobile lead sources but with App in platform
                leadSources.Add(BL.Campaigns.LeadSource.GetLeadSource(string.Format("{0}{1}Link", _leadSourcePrefix, campaignType), (int)adType, (int)Platform.CarwaleMobile));
                leadSources.Add(BL.Campaigns.LeadSource.GetLeadSource(string.Format("{0}{1}LinkReco", _leadSourcePrefix, campaignType), (int)adType, (int)Platform.CarwaleMobile));
                leadSources.Add(BL.Campaigns.LeadSource.GetLeadSource(string.Format("{0}{1}LinkSummaryCard", _leadSourcePrefix, campaignType), (int)adType, (int)Platform.CarwaleMobile));
                leadSources.Add(BL.Campaigns.LeadSource.GetLeadSource(string.Format("{0}{1}LinkSummaryCardReco", _leadSourcePrefix, campaignType), (int)adType, (int)Platform.CarwaleMobile));
                leadSources.Add(BL.Campaigns.LeadSource.GetLeadSource(string.Format("{0}{1}Template", _leadSourcePrefix, campaignType), (int)adType, (int)Platform.CarwaleMobile));
                leadSources.Add(BL.Campaigns.LeadSource.GetLeadSource(string.Format("{0}{1}TemplateReco", _leadSourcePrefix, campaignType), (int)adType, (int)Platform.CarwaleMobile));
                leadSources.Add(BL.Campaigns.LeadSource.GetLeadSource(string.Format("{0}{1}TemplateSummaryCard", _leadSourcePrefix, campaignType), (int)adType, (int)Platform.CarwaleMobile));
                leadSources.Add(BL.Campaigns.LeadSource.GetLeadSource(string.Format("{0}{1}TemplateSummaryCardReco", _leadSourcePrefix, campaignType), (int)adType, (int)Platform.CarwaleMobile));
                leadSources.Add(BL.Campaigns.LeadSource.GetLeadSource(string.Format("{0}{1}FixedLink", _leadSourcePrefix, campaignType), (int)adType, (int)Platform.CarwaleMobile));
                leadSources.Add(BL.Campaigns.LeadSource.GetLeadSource(string.Format("{0}{1}FixedLinkReco", _leadSourcePrefix, campaignType), (int)adType, (int)Platform.CarwaleMobile));
                leadSources.Add(BL.Campaigns.LeadSource.GetLeadSource(string.Format("{0}{1}TemplateEmiCalculator", _leadSourcePrefix, campaignType), (int)adType, (int)Platform.CarwaleMobile));
                leadSources.Add(BL.Campaigns.LeadSource.GetLeadSource(string.Format("{0}{1}TemplateEmiCalculatorReco", _leadSourcePrefix, campaignType), (int)adType, (int)Platform.CarwaleMobile));
                leadSources.ForEach(ls => ls.AdType = (short)adType);
            }

            return leadSources;
        }

        /// <summary>
        /// This function returns the CampaignType text used in leadsource 
        /// </summary>
        /// <param name="campaignType"></param>
        /// <returns></returns>
        private string GetCampaignTypeInTemplate(CampaignAdType campaignType)
        {
            switch (campaignType)
            {
                case CampaignAdType.Pq:
                    return "Pq";
                case CampaignAdType.PaidCrossSell:
                    return "PaidCS";
                case CampaignAdType.HouseCrossSell:
                    return "HouseCS";
            }

            return null;
        }

        private void SetLeadSource(QuotationPageMobileDTO priceQuoteDTO)
        {
            var leadSources = new List<LeadSource>();
            string campaignAdType = GetCampaignTypeInTemplate(priceQuoteDTO.DealerAd.CampaignType);
            var campaignType = (int)priceQuoteDTO.DealerAd.CampaignType;

            priceQuoteDTO.OfferAndDealerAd.LeadSource = new List<LeadSourceDTO>();

            if (campaignAdType != null)
            {
                //Platform is fixed as mobile as App leads will also go with mobile lead sources but with App in platform
                leadSources.Add(BL.Campaigns.LeadSource.GetLeadSource(string.Format("{0}{1}OffersButton", _leadSourcePrefix, campaignAdType), campaignType, (int)Platform.CarwaleMobile));
                leadSources.Add(BL.Campaigns.LeadSource.GetLeadSource(string.Format("{0}{1}OffersButtonReco", _leadSourcePrefix, campaignAdType), campaignType, (int)Platform.CarwaleMobile));
            }
            priceQuoteDTO.OfferAndDealerAd.LeadSource = Mapper.Map<List<LeadSource>, List<LeadSourceDTO>>(leadSources);
        }

        #endregion LeadSources
    }
}
