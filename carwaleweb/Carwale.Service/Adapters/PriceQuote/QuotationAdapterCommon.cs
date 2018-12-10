using AutoMapper;
using Carwale.BL.GeoLocation;
using Carwale.DTOs.Campaigns;
using Carwale.DTOs.PageProperty;
using Carwale.DTOs.PriceQuote;
using Carwale.DTOs.Template;
using Carwale.Entity.Campaigns;
using Carwale.Entity.CarData;
using Carwale.Entity.Enum;
using Carwale.Entity.Geolocation;
using Carwale.Entity.Price;
using Carwale.Entity.PriceQuote;
using Carwale.Interfaces.Campaigns;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.Geolocation;
using Carwale.Interfaces.PriceQuote;
using Carwale.Interfaces.Prices;
using Carwale.Notifications.Logs;
using Carwale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Carwale.Service.Adapters.PriceQuote
{
    public class QuotationAdapterCommon : IQuotationAdapterCommon
    {
        private readonly ICarVersionCacheRepository _versionCacheRepo;
        private readonly IGeoCitiesCacheRepository _citiesCacheRepo;
        private readonly IEmiCalculatorBl _emiCalculatorBl;
        private readonly ITemplate _template;
        private readonly IEmiCalculatorAdapter _emiCalculatorAdapter;

        public QuotationAdapterCommon(ICarVersionCacheRepository versionCacheRepo,
             IGeoCitiesCacheRepository citiesCacheRepo, IEmiCalculatorBl emiCalculatorBl,
             ITemplate template, IEmiCalculatorAdapter emiCalculatorAdapter)
        {
            _versionCacheRepo = versionCacheRepo;
            _citiesCacheRepo = citiesCacheRepo;
            _emiCalculatorBl = emiCalculatorBl;
            _template = template;
            _emiCalculatorAdapter = emiCalculatorAdapter;
        }

        #region Validations
        public bool IsBasicInputValid(PriceQuoteInput input, CarVersionDetails versionDetails)
        {
            try
            {
                var location = Mapper.Map<Area, Location>(new ElasticLocation().GetLocation(input.AreaId));

                if (!IsSufficientInputsAvailable(input) || !IsRequiredAreaAvailable(input))
                {
                    return false;
                }

                if (!IsInputsMatch(input, versionDetails, location))
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return false;
            }

            return true;
        }

        public PriceQuoteInput GetCompleteInput(PriceQuoteInput input, CarVersionDetails versionDetails)
        {
            try
            {
                var location = Mapper.Map<Area, Location>(new ElasticLocation().GetLocation(input.AreaId));

                GetModelIdVersionId(ref input, versionDetails);
                GetCityId(ref input, location);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return null;
            }

            return input;
        }

        public bool IsCompleteInputValid(CarModelDetails modelDetails, CarVersionDetails versionDetails, Cities cityDetails)
        {
            try
            {
                if (modelDetails == null || versionDetails == null || cityDetails == null)
                {
                    return false;
                }

                if (!IsModelVersionDetailsValid(modelDetails, versionDetails))
                {
                    return false;
                }

                if (!IsCityDetailsValid(cityDetails))
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return false;
            }

            return true;
        }


        private static void GetCityId(ref PriceQuoteInput input, Location location)
        {
            if (input.CityId < 1)
            {
                input.CityId = location.CityId;
            }
        }

        private bool IsSufficientInputsAvailable(PriceQuoteInput input)
        {
            //From Model-Version, atleast one is available
            //From City-Area, atleast one is available
            if ((input.ModelId <= 0 && input.VersionId <= 0) || (input.CityId <= 0 && input.AreaId <= 0))
            {
                return false;
            }
            return true;
        }

        private bool IsRequiredAreaAvailable(PriceQuoteInput input)
        {
            //Area should be available for Cities that have area feasibility
            if (input.CityId > 0 && input.AreaId <= 0 && _citiesCacheRepo.IsAreaAvailable(input.CityId))
            {
                return false;
            }
            return true;
        }


        private bool IsInputsMatch(PriceQuoteInput input, CarVersionDetails versionDetails, Location location)
        {
            //Model-Version both are available but are a mismatch
            if (input.ModelId > 0 && input.VersionId > 0 && versionDetails != null && !input.ModelId.Equals(versionDetails.ModelId))
            {
                return false;
            }

            //City-Area both are available but are a mismatch
            if (input.CityId > 0 && input.AreaId > 0 && !input.CityId.Equals(location.CityId))
            {
                return false;
            }

            return true;
        }

        private bool IsCityDetailsValid(Cities cityDetails)
        {
            if (!cityDetails.IsDeleted)
            {
                return true;
            }
            return false;
        }

        private bool IsModelVersionDetailsValid(CarModelDetails model, CarVersionDetails version)
        {
            if (model.New && !model.IsDeleted && version.New == 1 && !version.IsDeleted)
            {
                return true;
            }
            return false;
        }

        private void GetModelIdVersionId(ref PriceQuoteInput input, CarVersionDetails versionDetails)
        {
            if (input.VersionId < 1)
            {
                input.VersionId = _versionCacheRepo.GetDefaultVersionId(input.CityId, input.ModelId);
            }

            if (input.ModelId < 1)
            {
                input.ModelId = versionDetails.ModelId;
            }
        }
        #endregion
        public List<EmiCalculatorModelData> GetEmiCalculatorModelData(PQCarDetails carDetails, List<Entity.Price.PriceQuote> priceQuoteList, int cardNo, DealerAd dealerAd)
        {
            try
            {
                List<EmiCalculatorModelData> emiCalculatorModelDataList = new List<EmiCalculatorModelData>();
                if (priceQuoteList == null)
                {
                    return emiCalculatorModelDataList;
                }
                for (int pq = 0; pq < priceQuoteList.Count; pq++)
                {
                    EmiCalculatorModelData emiCalculatorModelData = Mapper.Map<PQCarDetails, EmiCalculatorModelData>(carDetails);

                    if (emiCalculatorModelData == null)
                    {
                        return emiCalculatorModelDataList;
                    }

                    var compulsoryPrices = priceQuoteList[pq].ChargeGroup.Where(x => x.Type == (int)ChargeGroupType.Compulsory).ToList();
                    var downPaymentValues = _emiCalculatorBl.GetDownPaymentValues(compulsoryPrices);
                    emiCalculatorModelData.DownPaymentMinValue = downPaymentValues.Item1;
                    emiCalculatorModelData.DownPaymentMaxValue = priceQuoteList[pq].OnRoadPrice;
                    emiCalculatorModelData.DownPaymentDefaultValue = _emiCalculatorBl.CalculateDownPaymentDefaultValue(downPaymentValues.Item1, downPaymentValues.Item2);
                    emiCalculatorModelData.IsMetallic = priceQuoteList[pq].IsMetallic;
                    if (cardNo == 0)
                    {
                        emiCalculatorModelData.UniqueKey = (priceQuoteList[pq].IsMetallic ? "m" : "s");
                    }
                    else
                    {
                        emiCalculatorModelData.UniqueKey = (CustomParser.parseStringObject(cardNo) + "_" + (priceQuoteList[pq].IsMetallic ? "m" : "s"));
                    }
                    emiCalculatorModelData.DealerAd = Mapper.Map<DealerAd, DealerAdDTO>(dealerAd);
                    if (dealerAd != null)
                    {
                        var template = _template.GetEmiCalculatorTemplate();
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
                    emiCalculatorModelData.IsEligibleForThirdPartyEmi = emiCalculatorDataThirdParty.IsEligibleForThirdPartyEmi;

                    if (emiCalculatorModelData.IsEligibleForThirdPartyEmi)
                    {
                        emiCalculatorModelData.ThirdPartyEmiDetails = emiCalculatorDataThirdParty.ThirdPartyEmiDetails;
                    }

                    emiCalculatorModelData.MakeName = carDetails.MakeName;
                    emiCalculatorModelData.ModelName = carDetails.ModelName;

                    emiCalculatorModelDataList.Add(emiCalculatorModelData);
                }
                return emiCalculatorModelDataList;
            }
            catch (Exception err)
            {
                Logger.LogException(err, "EmiCalculatorBl.GetEmiCalculatorModelData()");
                return null;
            }
        }
    }
}

