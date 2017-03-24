using Bikewale.Cache.Core;
using Bikewale.Cache.PriceQuote;
using Bikewale.Common;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.PriceQuote;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;

namespace Bikewale.Models.PriceInCity
{
    public class ModelPriceInNearestCities
    {
        private IPriceQuoteCache _objCache;
        private uint _modelId, _cityId;
        private ushort _topCount;

        public ModelPriceInNearestCities(uint modelId, uint cityId, ushort topCount)
        {
            try
            {
                _modelId = modelId;
                _cityId = cityId;
                _topCount = topCount;

                using (IUnityContainer container = new UnityContainer())
                {

                    container.RegisterType<IPriceQuote, Bikewale.BAL.PriceQuote.PriceQuote>()
                             .RegisterType<ICacheManager, MemcacheManager>()
                             .RegisterType<IPriceQuoteCache, PriceQuoteCache>();

                    _objCache = container.Resolve<IPriceQuoteCache>();
                }
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