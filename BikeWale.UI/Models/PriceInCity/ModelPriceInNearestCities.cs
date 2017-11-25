using Bikewale.Common;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.PriceQuote;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.Models.PriceInCity
{
    /// <summary>
    /// Created by Sajal Gupta on 24-03-2017
    /// This class provides data for ModelPriceInNearestCities widget (Desktop + Mobile)
    /// </summary>
    public class ModelPriceInNearestCities
    {
        private readonly IPriceQuoteCache _objCache;
        private uint _modelId, _cityId;
        private ushort _topCount;

        public ModelPriceInNearestCities(IPriceQuoteCache objCache, uint modelId, uint cityId, ushort topCount)
        {
            try
            {
                _modelId = modelId;
                _cityId = cityId;
                _topCount = topCount;
                _objCache = objCache;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.PriceInCity.ModelPriceInNearestCities.ModelPriceInNearestCities()");
            }
        }

        public PriceInTopCitiesWidgetVM GetData()
        {
            PriceInTopCitiesWidgetVM priceInTopCitiesWidgetVM = null;
            IEnumerable<PriceQuoteOfTopCities> cityPrices = null;

            try
            {
                cityPrices = _objCache.GetModelPriceInNearestCities(_modelId, _cityId, _topCount);

                if (cityPrices != null && cityPrices.Any())
                {
                    priceInTopCitiesWidgetVM = new PriceInTopCitiesWidgetVM();

                    priceInTopCitiesWidgetVM.PriceQuoteList = cityPrices;
                    priceInTopCitiesWidgetVM.BikeName = string.Format("{0} {1}", cityPrices.First().Make, cityPrices.First().Model);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.PriceInCity.ModelPriceInNearestCities.GetData()");
            }
            return priceInTopCitiesWidgetVM;
        }
    }
}