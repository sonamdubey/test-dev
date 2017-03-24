using Bikewale.Cache.Core;
using Bikewale.Cache.PriceQuote;
using Bikewale.Common;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.PriceQuote;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.Models.PriceInCity
{
    public class PriceInTopCities
    {
        private uint _modelId, _topCount;
        private IPriceQuoteCache _objCache;

        public PriceInTopCities(uint modelId, uint topCount)
        {
            try
            {
                _modelId = modelId;
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
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.PriceInCity.PriceInTopCities.PriceInTopCities()");
            }
        }

        public PriceInTopCitiesWidgetVM GetData()
        {
            PriceInTopCitiesWidgetVM objData = null;
            try
            {
                IEnumerable<PriceQuoteOfTopCities> prices = null;
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