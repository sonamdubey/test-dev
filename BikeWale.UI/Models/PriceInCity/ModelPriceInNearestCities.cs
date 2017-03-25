using Bikewale.Common;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.PriceQuote;
using System;
using System.Collections.Generic;

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
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.PriceInCity.ModelPriceInNearestCities.ModelPriceInNearestCities()");
            }
        }

        public IEnumerable<PriceQuoteOfTopCities> GetData()
        {
            IEnumerable<PriceQuoteOfTopCities> objList = null;
            try
            {
                objList = _objCache.GetModelPriceInNearestCities(_modelId, _cityId, _topCount);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.PriceInCity.ModelPriceInNearestCities.GetData()");
            }
            return objList;
        }
    }
}