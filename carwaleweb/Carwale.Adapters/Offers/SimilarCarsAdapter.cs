using AutoMapper;
using Carwale.Adapters.Offers;
using Carwale.DAL.ApiGateway;
using Carwale.DAL.ApiGateway.ApiGatewayHelper;
using Carwale.DTOs.CarData;
using Carwale.DTOs.OffersV1;
using Carwale.Entity.CarData;
using Carwale.Entity.Enum;
using Carwale.Entity.Offers;
using Carwale.Entity.OffersV1;
using Carwale.Entity.PriceQuote;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.PriceQuote;
using Carwale.Notifications.Logs;
using Carwale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Carwale.Adapters.NewCars
{
    public class SimilarCarsAdapter : ApiGatewayWidgetAdapterBase<SimilarCarVmRequest, SimilarCarsDTO>
    {
        private readonly ICarModels _carModelsBL;
        private readonly ICarPriceQuoteAdapter _prices;
        private readonly ICarModelCacheRepository _modelsCacheRepo;
        private readonly OfferAvailabilityAdapter _offerAvailability;
        private SimilarCarVmRequest _similarCarVmInput;
        private List<int> _modelList;

        public SimilarCarsAdapter(ICarModels carModelsBL, ICarPriceQuoteAdapter prices, ICarModelCacheRepository modelsCacheRepo)
        {
            _offerAvailability = new OfferAvailabilityAdapter();
            _carModelsBL = carModelsBL;
            _prices = prices;
            _modelsCacheRepo = modelsCacheRepo;
        }

        /// <summary>
        /// To fetch similars along with prices and Offers
        /// </summary>
        protected override void AddApiGatewayCallsForWidget(IApiGatewayCaller caller, SimilarCarVmRequest similarCarVmInput)
        {
            _similarCarVmInput = similarCarVmInput;
            _modelList = similarCarVmInput.SimilarCarModelList != null ? similarCarVmInput.SimilarCarModelList.Select(x => x.ModelId).ToList() : new List<int>();
            OfferAvailabilityInput offerAvailabilityInput = new OfferAvailabilityInput() { ModelIds = _modelList, CityId = _similarCarVmInput.CityId, StateId = _similarCarVmInput.StateId };
            _offerAvailability.AddApiGatewayCallWithCallback(caller, offerAvailabilityInput);
        }

        protected override SimilarCarsDTO BuildResponse(IApiGatewayCaller caller)
        {
            if (_similarCarVmInput != null)
            {
                try
                {
                    SimilarCarsDTO similarCars = new SimilarCarsDTO
                    {
                        SourceModelName = _similarCarVmInput.ModelName,
                        SourceModelId = _similarCarVmInput.ModelId,
                        WidgetPageSource = (int)_similarCarVmInput.WidgetSource,
                        PageName = _similarCarVmInput.PageName
                    };
                    IDictionary<int, PriceOverview> similarModelPrices = _prices.GetModelsCarPriceOverview(_modelList, _similarCarVmInput.CityId, true);
                    Tuple<int, string> modelTuple = new Tuple<int, string>(_similarCarVmInput.ModelId, (Format.FormatSpecial(_similarCarVmInput.MakeName) + '-' + _similarCarVmInput.MaskingName));
                    _similarCarVmInput.SimilarCarModelList?.ForEach(x =>
                    {
                        var priceOverview = similarModelPrices[x.ModelId];
                        x.PricesOverview = (priceOverview ?? new PriceOverview());
                        if (!_similarCarVmInput.IsFuturistic)
                        {
                            List<Tuple<int, string>> compareCarsTupleList = new List<Tuple<int, string>> {
                                modelTuple,
                                new Tuple<int, string>(x.ModelId, (Format.FormatSpecial(x.MakeName)+'-'+ x.MaskingName))
                            };
                            string formatCompareUrl = Format.GetCompareUrl(compareCarsTupleList);
                            x.CompareCarUrl = string.Format("{0}/comparecars/{1}",
                                (_similarCarVmInput.IsMobile ? "/m" : string.Empty), !string.IsNullOrWhiteSpace(formatCompareUrl) ? formatCompareUrl + "/" : string.Empty);
                        }
                    });
                    similarCars.SimilarCarModels = Mapper.Map<List<SimilarCarModels>, List<SimilarCarModelsDTOV2>>(_similarCarVmInput.SimilarCarModelList);
                    similarCars.SimilarUpcomingCar = _modelsCacheRepo.GetSimilarUpcomingCarModel(_similarCarVmInput.ModelId) ?? new UpcomingCarModel();
                    if (_offerAvailability.Output != null)
                    {
                        Dictionary<int, bool> modelOfferDict = new Dictionary<int, bool>();
                        foreach (var model in _offerAvailability.Output)
                        {
                            if (!modelOfferDict.ContainsKey(model.ModelId))
                            {
                                modelOfferDict.Add(model.ModelId, model.IsOfferAvailable);
                            }
                        }
                        foreach (var model in similarCars.SimilarCarModels)
                        {
                            var isOfferPresent = false;
                            modelOfferDict.TryGetValue(model.ModelId, out isOfferPresent);
                            model.OfferDetails = new OfferLinkDto {
                                IsOfferAvailable = isOfferPresent,
                                MakeName = model.MakeName,
                                ModelName = model.ModelName,
                                Url = ManageCarUrl.CreateModelUrl(model.MakeName, model.MaskingName),
                                PageName = _similarCarVmInput.PageName
                            };
                        }
                    }

                    return similarCars;
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                }
            }
            return new SimilarCarsDTO();
        }

    }
}
