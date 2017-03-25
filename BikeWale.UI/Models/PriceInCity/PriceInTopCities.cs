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
    /// This Class provides data for price in top cities widget (Desktop + Mobile)
    /// </summary>
    public class PriceInTopCities
    {
        private uint _modelId, _topCount;
        private readonly IPriceQuoteCache _objCache;

        public PriceInTopCities(IPriceQuoteCache objCache, uint modelId, uint topCount)
        {
            try
            {
                _modelId = modelId;
                _topCount = topCount;
                _objCache = objCache;
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.PriceInCity.PriceInTopCities.PriceInTopCities()");
            }
        }

        public PriceInTopCitiesWidgetVM GetData()
        {
            PriceInTopCitiesWidgetVM objData = null;
            try
            {
                IEnumerable<PriceQuoteOfTopCities> prices = null;

                if (_objCache != null)
                    prices = _objCache.FetchPriceQuoteOfTopCitiesCache(_modelId, _topCount);

                objData = new PriceInTopCitiesWidgetVM();

                if (prices != null && prices.Count() > 0)
                {
                    objData.PriceQuoteList = prices;
                    objData.BikeName = string.Format("{0} {1}", prices.First().Make, prices.First().Model);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.PriceInCity.PriceInTopCities.GetData()");
            }
            return objData;
        }
    }
}