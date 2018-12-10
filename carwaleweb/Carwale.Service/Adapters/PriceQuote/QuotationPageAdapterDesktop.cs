using AutoMapper;
using Bhrigu;
using Carwale.BL.Experiments;
using Carwale.BL.PresentationLogic;
using Carwale.DAL.ApiGateway;
using Carwale.DAL.ApiGateway.Extensions.CarData;
using Carwale.DAL.ApiGateway.Extensions.Mmv;
using Carwale.DAL.ApiGateway.Extensions.Offers;
using Carwale.DAL.ApiGateway.Extensions.SimilarCar;
using Carwale.DAL.Campaigns;
using Carwale.DTOs.Campaigns;
using Carwale.DTOs.CarData;
using Carwale.DTOs.Geolocation;
using Carwale.DTOs.LeadForm;
using Carwale.DTOs.OfferAndDealerAd;
using Carwale.DTOs.OffersV1;
using Carwale.DTOs.PriceQuote;
using Carwale.DTOs.Template;
using Carwale.Entity;
using Carwale.Entity.Campaigns;
using Carwale.Entity.CarData;
using Carwale.Entity.Common;
using Carwale.Entity.Enum;
using Carwale.Entity.Geolocation;
using Carwale.Entity.OffersV1;
using Carwale.Entity.Price;
using Carwale.Entity.PriceQuote;
using Carwale.Entity.Template;
using Carwale.Entity.VehicleData;
using Carwale.Interfaces.Campaigns;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.Geolocation;
using Carwale.Interfaces.NewCars;
using Carwale.Interfaces.Offers;
using Carwale.Interfaces.PriceQuote;
using Carwale.Notifications.Logs;
using Carwale.Utility;
using Microsoft.Practices.Unity;
using MMV.Service.ProtoClass;
using Offers.Protos.ProtoFiles;
using ProtoBufClass.Campaigns;
using System;
using System.Collections.Generic;
using System.Linq;
using VehicleData.Service.ProtoClass;

namespace Carwale.Service.Adapters.PriceQuote
{
    public class QuotationPageAdapterDesktop : IServiceAdapterV2
    {
        readonly IOfferBL _offersBL;
        readonly ICarVersionCacheRepository _versionCacheRepo;
        readonly ICarModelCacheRepository _modelCacheRepo;
        readonly IGeoCitiesCacheRepository _citiesCacheRepo;
        readonly IPQGeoLocationBL _locationBL;
        readonly IDealerAdProvider _dealerAdProviderBl;
        readonly IUnityContainer _unityContainer;
        readonly IPrices _pricesBL;
        readonly ICarPriceQuoteAdapter _prices;
        readonly IQuotationAdapterCommon _quotationAdapterCommon;
        readonly IPriceQuoteBL _priceQuoteBl;
        readonly IOffersAdapter _offersAdapter;
        readonly ITemplate _template;
        readonly int _widgetCardCount = 3;

        private readonly short NearByCitiesCount = 3;
        public QuotationPageAdapterDesktop(IOfferBL offersBL, ICarVersionCacheRepository versionCacheRepo, ICarModelCacheRepository modelCacheRepo,
            IGeoCitiesCacheRepository citiesCacheRepo, IPQGeoLocationBL locationBL, ICarPriceQuoteAdapter prices,
            IDealerAdProvider dealerAdProviderBl, IPrices pricesBL, IQuotationAdapterCommon quotationAdapterCommon, IOffersAdapter offersAdapter,
            IUnityContainer unityContainer, IPriceQuoteBL priceQuoteBl, ITemplate template)
        {
            _offersBL = offersBL;
            _offersAdapter = offersAdapter;
            _modelCacheRepo = modelCacheRepo;
            _versionCacheRepo = versionCacheRepo;
            _citiesCacheRepo = citiesCacheRepo;
            _locationBL = locationBL;
            _dealerAdProviderBl = dealerAdProviderBl;
            _pricesBL = pricesBL;
            _quotationAdapterCommon = quotationAdapterCommon;
            _unityContainer = unityContainer;
            _prices = prices;
            _priceQuoteBl = priceQuoteBl;
            _template = template;
        }

        public T Get<T, U>(U input)
        {
            return (T)Convert.ChangeType(GetPriceQuoteDetails<U>(input), typeof(T));
        }

        private QuotationPageDesktopDTO GetPriceQuoteDetails<U>(U priceQuoteInput)
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

            if (!_quotationAdapterCommon.IsCompleteInputValid(modelDetails, versionDetails, cityDetails))
            {
                return null;
            }

            #endregion

            var priceQuoteDTO = new QuotationPageDesktopDTO();
            priceQuoteDTO.CarDetails = Mapper.Map<CarDetailsDTO>(versionDetails);
            BindBreadcrumb(ref priceQuoteDTO);
            var location = _locationBL.GetCityDetails(input.CityId, 0, input.AreaId);
            priceQuoteDTO.Location = Mapper.Map<PQCustLocationDTO>(location);
            SimilarCarRequest similarCarRequest = GetSimliarCarRequest(input.ModelId);

            var apiGatewayCallerStep1 = CallApiGatewayStep1(input, similarCarRequest, priceQuoteDTO);
            
            try 
	        {	        
	        	var recommendationResponse = apiGatewayCallerStep1.GetResponse<RecommendationResponse>(1)?.Result?.ToList();
                GetSimilarModels(recommendationResponse, input.CityId, priceQuoteDTO);
	        }
	        catch (Exception e)
	        {
	        	Logger.LogException(e);
	        }

            GetLinkCampaignTemplates(apiGatewayCallerStep1, input.PageId, priceQuoteDTO);

            var dealerAd = new DealerAd();
            if(!input.HideCampaign)
            {
                GetDealerAd(ref priceQuoteDTO, location, out dealerAd);
            }
            
            CityAreaDTO userLocation = Mapper.Map<CityAreaDTO>(location);

            GetOffer(apiGatewayCallerStep1, priceQuoteDTO);
            GetVersionList(apiGatewayCallerStep1, input, ref priceQuoteDTO);
            GetPriceList(ref priceQuoteDTO, input.ModelId,input.VersionId,userLocation);

            var carDetails = Mapper.Map<PQCarDetails>(versionDetails);
            GetNearByCities(ref priceQuoteDTO, carDetails, input.CityId, location);

            GetEmiCalculatorModelData(ref priceQuoteDTO,carDetails,dealerAd,userLocation);
            
            ShowSellCarSlug(ref priceQuoteDTO, input.CityId);
            priceQuoteDTO.AlternateCars = new AlternateCarsDTO
            {
                SimilarModelList = priceQuoteDTO.SimilarModelList,
                MakeName = priceQuoteDTO.CarDetails.CarMake.MakeName,
                ModelName = priceQuoteDTO.CarDetails.CarModel.ModelName,
                Location = priceQuoteDTO.Location,
                WidgetCardCount = _widgetCardCount
            };
            SetLeadFormData(priceQuoteDTO , location);
            return priceQuoteDTO;
        }

        #region EmiCalculator

        private void GetEmiCalculatorModelData(ref QuotationPageDesktopDTO priceQuoteDTO, PQCarDetails carDetails, DealerAd dealerAd, CityAreaDTO location)
        {
            if (carDetails == null && dealerAd == null)
            {
                return;
            }
            try
            {

                var priceQuoteList = new List<Entity.Price.PriceQuote>();
                if (priceQuoteDTO.MetallicCompulsoryList != null && priceQuoteDTO.MetallicCompulsoryList.PriceQuote != null)
                {
                    priceQuoteList.Add(priceQuoteDTO.MetallicCompulsoryList.PriceQuote);
                }
                if (priceQuoteDTO.SolidCompulsoryList != null && priceQuoteDTO.SolidCompulsoryList.PriceQuote != null)
                {
                    priceQuoteList.Add(priceQuoteDTO.SolidCompulsoryList.PriceQuote);
                }

                priceQuoteDTO.EmiCalculatorModelLink = GetEmiCalculatorData(carDetails, priceQuoteList, location, dealerAd);
            }
            catch (Exception e)
            {
                Logger.LogException(e);
            }
        }

        private List<EmiCalculatorModelLink> GetEmiCalculatorData(PQCarDetails carDetails, List<Entity.Price.PriceQuote> priceQuoteList, CityAreaDTO userLocation, DealerAd dealerAd)
        {
            List<EmiCalculatorModelLink> emiCalculatorLinkList = new List<EmiCalculatorModelLink>();
            var emiCalculatorDataList = _quotationAdapterCommon.GetEmiCalculatorModelData(carDetails, priceQuoteList, 0, null);
            EmiCalculatorDealerAdDto emiCalculatorDealerDataDto = new EmiCalculatorDealerAdDto();
            if (dealerAd != null && dealerAd.Campaign != null)
            {
                emiCalculatorDealerDataDto.AdAvailable = true;
                emiCalculatorDealerDataDto.ModelId = carDetails.ModelId;
                emiCalculatorDealerDataDto.CampaignDealerId = String.Format("{0}_{1}", dealerAd.Campaign.Id, dealerAd.Campaign.DealerId);
                Templates template = _template.GetEmiCalculatorTemplate();
                Dictionary<int, IdName> templateDictionary = new Dictionary<int, IdName>();
                _template.AddTemplatesToDict(template, (int)PageProperties.EmiCalculator, (object)null, ref templateDictionary);
                emiCalculatorDealerDataDto.CampaignTemplates = templateDictionary;
                emiCalculatorDealerDataDto.UserLocation = userLocation;
            }

            if (emiCalculatorDataList == null)
            {
                return emiCalculatorLinkList;
            }

            foreach (var emiCalculatorData in emiCalculatorDataList)
            {
                EmiCalculatorModelLink emiCalculatorLink = new EmiCalculatorModelLink();
                emiCalculatorLink.EmiCalculatorModelData = emiCalculatorData;
                emiCalculatorLink.EmiCalculatorModelData.Page = CwPages.QuotationDesktopNew;
                emiCalculatorLink.EmiCalculatorModelData.PageName = CwPages.QuotationDesktopNew.ToString();
                emiCalculatorLink.EmiCalculatorModelData.Platform = "Desktop";
                emiCalculatorLink.DealerData = emiCalculatorDealerDataDto;
                if (!emiCalculatorLink.EmiCalculatorModelData.IsEligibleForThirdPartyEmi)
                {
                    emiCalculatorLink.ShowLinkOnBottomLeft = ProductExperiments.ShowLinkOnBottomLeft();
                    emiCalculatorLink.ShowToolTip = ProductExperiments.ShowToolTip();
                    emiCalculatorLink.ShowRedSolidButton = ProductExperiments.ShowRedSolidButton();
                    emiCalculatorLink.ShowChangeTextLink = ProductExperiments.ShowChangeTextLinkOnDesk();
                    emiCalculatorLink.ShowGetEmiOffersButton = ProductExperiments.ShowGetEmiOffersButton();
                }

                emiCalculatorLinkList.Add(emiCalculatorLink);
            }
            return emiCalculatorLinkList;


        }

        #endregion

        private void GetVersionList(IApiGatewayCaller apiGatewayCallerStep1, PriceQuoteInput input, ref QuotationPageDesktopDTO priceQuoteDTO)
        {
           try 
	       {	        
	       	   var versions = apiGatewayCallerStep1.GetResponse<VersionList>(0)?.Versions?.ToList();

               var apiGatewayCallerStep2 = CallApiGatewayStep2(input, versions);
           
               var versionSpecs = apiGatewayCallerStep2.GetResponse<VersionItemsDataResponse>(0)?.VersionItemsDataList?.ToList();
               var modelPrices = _pricesBL.GetOnRoadPrice(input.ModelId, input.CityId);

               priceQuoteDTO.VersionList = GetVersionDropdownDetails(versions, versionSpecs, modelPrices);
	       }
	       catch (Exception e)
	       {
	       	   Logger.LogException(e);
	       }
        }

        private void ShowSellCarSlug(ref QuotationPageDesktopDTO priceQuoteDTO, int cityId)
        {
            try
            {
                if (NewCar.ShowSellCarLinkOnPq(cityId))
                {
                    priceQuoteDTO.ShowSellCarTwoThirdSlug = (!priceQuoteDTO.IsCampaignPresent && priceQuoteDTO.IsOfferPresent) ||
                                                        (!priceQuoteDTO.IsSolidCompulsoryPricePresent && !priceQuoteDTO.IsMetallicCompulsoryPricePresent);
                    priceQuoteDTO.ShowSellCarOneThirdSlug = !priceQuoteDTO.ShowSellCarTwoThirdSlug;
                }
            }
            catch (Exception e)
            {
                Logger.LogException(e);
            }
        }

        private void GetPriceList(ref QuotationPageDesktopDTO priceQuoteDTO, int modelId, int versionId, CityAreaDTO userLocation)
        {
            try
            {
                var priceQuoteList = _priceQuoteBl.GetVersionPrice(modelId, versionId, userLocation.CityId);
                if (priceQuoteList != null)
                {
                    foreach (var price in priceQuoteList)
                    {
                        var compPriceQuoteList = new List<Entity.Price.ChargeGroupPrice>();
                        var optionalPriceQuoteList = new List<Entity.Price.ChargeGroupPrice>();

                        foreach (var charge in price.ChargeGroup)
                        {
                            if (charge.Type == (int)ChargeGroupType.Compulsory)
                            {
                                compPriceQuoteList.Add(charge);
                            }

                            if (charge.Type == (int)ChargeGroupType.Optional)
                            {
                                optionalPriceQuoteList.Add(charge);
                            }
                        }

                        var compulsoryObj = new Entity.Price.PriceQuote();
                        compulsoryObj.IsMetallic = price.IsMetallic;
                        compulsoryObj.OnRoadPrice = price.OnRoadPrice;
                        compulsoryObj.ChargeGroup.AddRange(compPriceQuoteList);

                        var optionalObj = new Entity.Price.PriceQuote();
                        optionalObj.IsMetallic = price.IsMetallic;
                        optionalObj.OnRoadPrice = price.OnRoadPrice;
                        optionalObj.ChargeGroup.AddRange(optionalPriceQuoteList);

                        if (!price.IsMetallic)
                        {
                            priceQuoteDTO.SolidCompulsoryList.PriceQuote = compulsoryObj;
                            priceQuoteDTO.IsSolidCompulsoryPricePresent = compulsoryObj != null && compulsoryObj.ChargeGroup.IsNotNullOrEmpty();

                            priceQuoteDTO.SolidOptionalList.PriceQuote = optionalObj;
                            priceQuoteDTO.SolidOptionalList.MakeName = priceQuoteDTO.CarDetails.CarMake.MakeName;
                            priceQuoteDTO.SolidOptionalList.ModelName = priceQuoteDTO.CarDetails.CarModel.ModelName;
                            priceQuoteDTO.SolidOptionalList.IsCampaignPresent = priceQuoteDTO.IsCampaignPresent;
                            priceQuoteDTO.SolidOptionalList.DealerAd = priceQuoteDTO.DealerAd;
                            priceQuoteDTO.SolidOptionalList.UserLocation = userLocation;
                            priceQuoteDTO.IsSolidOptionalPricePresent = optionalObj != null && optionalObj.ChargeGroup.IsNotNullOrEmpty();
                        }
                        else
                        {
                            priceQuoteDTO.MetallicCompulsoryList.PriceQuote = compulsoryObj;
                            priceQuoteDTO.IsMetallicCompulsoryPricePresent = compulsoryObj != null && compulsoryObj.ChargeGroup.IsNotNullOrEmpty();

                            priceQuoteDTO.MetallicOptionalList.PriceQuote = optionalObj;
                            priceQuoteDTO.MetallicOptionalList.MakeName = priceQuoteDTO.CarDetails.CarMake.MakeName;
                            priceQuoteDTO.MetallicOptionalList.ModelName = priceQuoteDTO.CarDetails.CarModel.ModelName;
                            priceQuoteDTO.MetallicOptionalList.IsCampaignPresent = priceQuoteDTO.IsCampaignPresent;
                            priceQuoteDTO.MetallicOptionalList.DealerAd = priceQuoteDTO.DealerAd;
                            priceQuoteDTO.MetallicOptionalList.UserLocation = userLocation;
                            priceQuoteDTO.IsMetallicOptionalPricePresent = optionalObj != null && optionalObj.ChargeGroup.IsNotNullOrEmpty();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logger.LogException(e);
            }
        }

        private void GetNearByCities(ref QuotationPageDesktopDTO priceQuoteDTO, PQCarDetails carDetails, int cityId, Location location)
        {
            try
            {
                if (!priceQuoteDTO.IsSolidCompulsoryPricePresent && !priceQuoteDTO.IsMetallicCompulsoryPricePresent)
                {
                    priceQuoteDTO.NearByCities = Mapper.Map<NearByCityDetailsDto>(_pricesBL.GetNearbyCities(carDetails.VersionId, cityId, NearByCitiesCount));
                    priceQuoteDTO.NearByCities.VersionId = carDetails.VersionId;
                    priceQuoteDTO.NearByCities.Location = location;
                    priceQuoteDTO.NearByCities.CardNo = 0;
                }
            }
            catch (Exception e)
            {
                Logger.LogException(e);
            }
        }

        #region Alternate cars

        private SimilarCarRequest GetSimliarCarRequest(int modelId)
        {
            SimilarCarRequest similarCarRequest = new SimilarCarRequest
            {
                CarId = modelId,
                IsVersion = false,
                Count = 15,
                UserIdentifier = UserTracker.GetSessionCookie(),
                IsBoost = true
            };
            return similarCarRequest;
        }

        private void GetSimilarModels(List<int> similarCarModelIdList, int cityId, QuotationPageDesktopDTO priceQuoteDTO)
        {
            List<SimilarCarModelsDtoV3> similarModelsListDto = new List<SimilarCarModelsDtoV3>();
            IDictionary<int, PriceOverview> similarModelsPrice = _prices.GetModelsCarPriceOverview(similarCarModelIdList, cityId, true);
            if (similarModelsPrice.IsNotNullOrEmpty())
            {
                foreach (KeyValuePair<int, PriceOverview> model in similarModelsPrice)
                {
                    AddToSimilarModelList(model, similarModelsListDto);
                }
            }
            priceQuoteDTO.SimilarModelList = similarModelsListDto;
        }

        private void AddToSimilarModelList(KeyValuePair<int, PriceOverview> model, List<SimilarCarModelsDtoV3> similarModelsListDto)
        {
            if (model.Value != null && model.Value.PriceStatus == (int)PriceBucket.HaveUserCity && model.Value.Price > 0)
            {
                CarModelDetails modelDetails = _modelCacheRepo.GetModelDetailsById(model.Key);
                if (modelDetails != null)
                {
                    var similarModel = Mapper.Map<CarModelDetails, SimilarCarModelsDtoV3>(modelDetails);
                    similarModel.PriceOverview = Mapper.Map<PriceOverviewDtoV3>(model.Value);

                    var formattedPrice = Format.GetFormattedPriceV2(similarModel.PriceOverview.PriceForSorting.ToString());
                    if (similarModel.PriceOverview.PriceForSorting > 0)
                    {
                        formattedPrice = "\u20B9" + formattedPrice;
                    }
                    similarModel.PriceOverview.FormattedFullPrice = formattedPrice;
                    similarModelsListDto.Add(similarModel);
                }
            }
        }

        #endregion

        #region Version List
        private List<VersionSpecPriceDto> GetVersionDropdownDetails(List<MmvVersion> versions, List<VersionItemsData> versionSpecs, ModelPriceDTO modelPrices)
        {
            if (versions == null || versionSpecs == null || modelPrices == null)
            {
                return null;
            }

            var versionList = new List<VersionSpecPriceDto>();

            foreach (var version in versions)
            {
                if (version == null || version.Status != MmvStatus.New)
                {
                    continue;
                }

                List<VehicleData.Service.ProtoClass.ItemData> currentVersionSpecs = versionSpecs.Where(x => x.Id == version.Id).FirstOrDefault()?.ItemList?.ToList();

                var transmissionType = ExtractSpecValue(currentVersionSpecs, Items.Transmission_Type);
                var fuelType = ExtractSpecValue(currentVersionSpecs, Items.Fuel_Type);
                var price = modelPrices?.Versions?.Where(x => x.VersionId == version.Id).FirstOrDefault()?.OnRoadPrice;
                int validPrice = (price == null) ? 0 : (int)price;
                string formattedPrice = (validPrice < 1) ? "N/A" : "\u20B9" + Format.FormatNumericCommaSep(price.ToString());
                
                versionList.Add(new VersionSpecPriceDto
                {
                    Id = version.Id,
                    Name = version.Name,
                    FuelType = fuelType,
                    Transmission = transmissionType,
                    FormattedPrice = formattedPrice,
                    Price = validPrice
                });
            }
            
            //sort the versionList on the basis of price 
            //e.g. {1, 0, 5, 4} will get sorted as {1, 4, 5, 0}
           versionList.Sort(ComparatorVersionSpecPriceDto);

            return versionList;
        }

        private int ComparatorVersionSpecPriceDto(VersionSpecPriceDto first, VersionSpecPriceDto second)
        {
            if (first.Price == second.Price)
            {
                return first.Name.CompareTo(second.Name);
            }

            return first.Price == 0 || second.Price == 0 ? second.Price.CompareTo(first.Price) : first.Price.CompareTo(second.Price);
        }

        private string ExtractSpecValue(List<VehicleData.Service.ProtoClass.ItemData> currentVersionSpecs, Items item)
        {
            return currentVersionSpecs.Where(x => x.ItemId == (int)item).FirstOrDefault()?.Value;
        }

        #endregion

        private void BindBreadcrumb(ref QuotationPageDesktopDTO quotationPageDesktopDTO)
        {
            List<BreadcrumbEntity> breadcrumbEntitylist = new List<BreadcrumbEntity>();
            var makeName = quotationPageDesktopDTO.CarDetails.CarMake.MakeName;
            var modelName = quotationPageDesktopDTO.CarDetails.CarModel.ModelName;
            var modelMaskingName = quotationPageDesktopDTO.CarDetails.CarModel.MaskingName;
            breadcrumbEntitylist.Add(new BreadcrumbEntity
            {
                Title = modelName,
                Link = ManageCarUrl.CreateModelUrl(makeName, modelMaskingName),
                Text = modelName
            });
            breadcrumbEntitylist.Add(new BreadcrumbEntity
            {
                Text = "On-Road Price Quote"
            });
            quotationPageDesktopDTO.BreadcrumbEntitylist = breadcrumbEntitylist;
        }

        #region API Gateway steps
        private IApiGatewayCaller CallApiGatewayStep1(PriceQuoteInput input, SimilarCarRequest similarCarRequest, QuotationPageDesktopDTO priceQuoteDTO)
        {
            IApiGatewayCaller apiGatewayCaller = _unityContainer.Resolve<IApiGatewayCaller>();

            apiGatewayCaller.AggregateGetVersionsByModelId(input.ModelId);
            apiGatewayCaller.GenerateSimilarCarCallerRequest(similarCarRequest);
            AddCampaignTemplateCall(apiGatewayCaller, input, priceQuoteDTO);
            AddOfferCall(apiGatewayCaller, priceQuoteDTO);
            apiGatewayCaller.Call();

            return apiGatewayCaller;
        }

        private IApiGatewayCaller CallApiGatewayStep2(PriceQuoteInput input, List<MmvVersion> versions)
        {
            IApiGatewayCaller apiGatewayCaller = _unityContainer.Resolve<IApiGatewayCaller>();

            var versionIds = versions.Select(x => x.Id).ToList();
            apiGatewayCaller.GenerateVersionsDataByItemIdsRequest(versionIds, new List<int> { (int)Items.Fuel_Type, (int)Items.Transmission_Type });
            apiGatewayCaller.Call();

            return apiGatewayCaller;
        }
        #endregion

        #region Campaign and Lead form

        private void GetDealerAd(ref QuotationPageDesktopDTO priceQuoteDTO, Location location, out DealerAd dealerAd)
        {
            try
            {
                var carDetails = Mapper.Map<CarDetailsDTO, CarIdEntity>(priceQuoteDTO.CarDetails);
                var dealerAdList = _dealerAdProviderBl.GetDealerAdList(carDetails, location, (int)Platform.CarwaleDesktop, 0, 0, (int)Pages.PriceQuote);
                if (dealerAdList.IsNotNullOrEmpty() && dealerAdList[0] != null)
                {
                    if (dealerAdList[0].CampaignType == Carwale.Entity.Enum.CampaignAdType.Pq)
                    {
                        priceQuoteDTO.DealerAd = Mapper.Map<DealerAdDTO>(dealerAdList[0]);
                        priceQuoteDTO.IsCampaignPresent = true;
                        SetDealerDetails(priceQuoteDTO);
                        dealerAd = dealerAdList[0];
                    }
                    else
                    {
                        GetCrossSellDetails(ref priceQuoteDTO, dealerAdList);
                        dealerAd = null;
                    }
                }
                else
                {
                    dealerAd = null;
                }
            }
            catch (Exception e)
            {
                dealerAd = null;
                Logger.LogException(e);
            }
        }

        private void GetCrossSellDetails(ref QuotationPageDesktopDTO priceQuoteDTO, List<DealerAd> dealerAdList)
        {
            if (dealerAdList.IsNotNullOrEmpty())
            {
                priceQuoteDTO.CrossSellList = new List<CrossSellDto>();
                int cityId = priceQuoteDTO.Location.CityId;
                foreach (var dealerAd in dealerAdList)
                {
                    if (dealerAd != null && dealerAd.FeaturedCarData != null && dealerAd.Campaign != null)
                    {
                        var onRoadPrice = _pricesBL.GetVersionOnRoadPrice(dealerAd.FeaturedCarData.ModelId, dealerAd.FeaturedCarData.VersionId, cityId);
                        if (onRoadPrice > 0)
                        {
                            var crossSell = new CrossSellDto()
                            {
                                DealerAd = Mapper.Map<DealerAdDTO>(dealerAd),
                                OnRoadPrice = string.Format("\u20B9 {0}", Format.GetFormattedPriceV2(onRoadPrice.ToString()))
                            };
                            priceQuoteDTO.CrossSellList.Add(crossSell);
                        }
                    }
                }
            }
        }

        private void SetLeadFormData(QuotationPageDesktopDTO priceQuoteDTO, Location userLocation)
        {
            if (priceQuoteDTO.DealerAd != null || (priceQuoteDTO.CrossSellList != null))
            {
                priceQuoteDTO.LeadFormModelData = new LeadFormModelData();
                priceQuoteDTO.LeadFormModelData.CustLocation = userLocation;
                List<DealerAdDTO> dealerAdList = new List<DealerAdDTO>();
                if (priceQuoteDTO.DealerAd != null)
                {
                    dealerAdList.Add(priceQuoteDTO.DealerAd);
                }
                else
                {
                    foreach (var crossSell in priceQuoteDTO.CrossSellList)
                    {
                        dealerAdList.Add(crossSell.DealerAd);
                    }
                }
                priceQuoteDTO.LeadFormModelData.DealerAd = dealerAdList;
            }
        }

        #endregion

        #region Offers

        private OfferInput GetOfferInput(QuotationPageDesktopDTO priceQuoteDTO)
        {
            var offerInput = new OfferInput
            {
                ApplicationId = (int)Carwale.Entity.Enum.Application.CarWale,
                CityId = priceQuoteDTO.Location.CityId,
                MakeId = priceQuoteDTO.CarDetails.CarMake.MakeId,
                ModelId = priceQuoteDTO.CarDetails.CarModel.ModelId,
                VersionId = priceQuoteDTO.CarDetails.CarVersion.Id
            };
            return offerInput;
        }

        private void AddOfferCall(IApiGatewayCaller apiGatewayCaller, QuotationPageDesktopDTO priceQuoteDTO)
        {
            priceQuoteDTO.OfferAndDealerAd = new OfferAndDealerAdDTO();
            priceQuoteDTO.OfferAndDealerAd.Platform = "Desktop";
            if (priceQuoteDTO.CarDetails != null && priceQuoteDTO.Location != null)//&& priceQuoteDTO.PriceQuote.Count > 0)
            {
                priceQuoteDTO.OfferAndDealerAd.CarDetails = Mapper.Map<CarDetailsDTO>(priceQuoteDTO.CarDetails);
                var offerInput = GetOfferInput(priceQuoteDTO);
                if (_offersBL.ValidateOfferInput(offerInput))
                {
                    apiGatewayCaller.AggregateGetOffersOnCriteria(offerInput);
                }
            }
        }

        private void SetDealerDetails(QuotationPageDesktopDTO priceQuoteDTO)
        {
            priceQuoteDTO.OfferAndDealerAd.DealerAd = Mapper.Map<DealerAdDTO>(priceQuoteDTO.DealerAd);
            priceQuoteDTO.OfferAndDealerAd.Page = Pages.PriceQuote;
            priceQuoteDTO.OfferAndDealerAd.Location = Mapper.Map<CityAreaDTO>(priceQuoteDTO.Location);
        }

        private void GetOffer(IApiGatewayCaller apiGatewayCallerStep1, QuotationPageDesktopDTO priceQuoteDTO)
        {
            try
            {
                var offersList = apiGatewayCallerStep1.GetResponse<OfferWithCategoryDetailList>(3);
                if (offersList != null)
                {
                    if (offersList != null && offersList.ListOfferWithCategoryDetail != null && offersList.ListOfferWithCategoryDetail.Count > 0)
                    {
                        var offer = Mapper.Map<Carwale.Entity.OffersV1.Offer>(offersList.ListOfferWithCategoryDetail[0]);
                        priceQuoteDTO.OfferAndDealerAd.Offer = Mapper.Map<OfferDto>(offer);
                        var offerDto = priceQuoteDTO.OfferAndDealerAd.Offer;
                        if (offerDto != null && offerDto.OfferDetails != null)
                        {
                            SetDealerDetails(priceQuoteDTO);
                            priceQuoteDTO.OfferAndDealerAd.Offer.OfferDetails.ValidityText = _offersAdapter.GetValidityText(offer.OfferDetails.IsExtended, offerDto.OfferDetails);
                            priceQuoteDTO.IsOfferPresent = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        #endregion

        #region Link Template

        private void AddCampaignTemplateCall(IApiGatewayCaller apiGatewayCaller, PriceQuoteInput input, QuotationPageDesktopDTO priceQuoteDTO)
        {
            if (priceQuoteDTO.CarDetails != null && priceQuoteDTO.Location != null)//&& priceQuoteDTO.PriceQuote.Count > 0)
            {
                var template = GetLinkCampaignTemplatesInput(input, priceQuoteDTO);
                apiGatewayCaller.GetAllTemplatesByPage(template);
            }
        }

        TemplateInput GetLinkCampaignTemplatesInput(PriceQuoteInput input, QuotationPageDesktopDTO priceQuoteDTO)
        {
            TemplateInput templateInput = new TemplateInput()
            {
                ApplicationId = (int)Carwale.Entity.Enum.Application.CarWale,
                CityId = priceQuoteDTO.Location.CityId,
                MakeId = priceQuoteDTO.CarDetails.CarMake.MakeId,
                ModelId = priceQuoteDTO.CarDetails.CarModel.ModelId,
                PlatformId = (int)Platform.CarwaleDesktop,
                PageId = (short)CwPages.QuotationDesktopNew
            };
            return templateInput;
        }

        private void GetLinkCampaignTemplates(IApiGatewayCaller apiGatewayCallerStep1, int pageId, QuotationPageDesktopDTO priceQuoteDTO)
        {
            try
            {

                
                var response = apiGatewayCallerStep1.GetResponse<PropertyTemplates>(2);
            	if (response != null)
            	{
                    List<PageTemplates> pageTemplates = Mapper.Map<List<PageTemplates>>(response.PageTemplates);
                    var templateDict = _template.AddTemplatesToDict(pageTemplates, priceQuoteDTO, pageId);
                    priceQuoteDTO.CampaignTemplates = templateDict;
                    priceQuoteDTO.SolidCompulsoryList.CampaignTemplates = templateDict;
                    priceQuoteDTO.MetallicCompulsoryList.CampaignTemplates = templateDict;
                }
            }
            catch (Exception e)
            {
                Logger.LogException(e);
            }
        }

        #endregion
    }
}
