using AEPLCore.Cache;
using AEPLCore.Cache.Interfaces;
using Carwale.Entity;
using Carwale.Entity.PriceQuote;
using Carwale.Interfaces;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.Geolocation;
using Carwale.Interfaces.PriceQuote;
using Carwale.Notifications;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web;

namespace Carwale.Cache.PriceQuote
{
    public class PricesCacheRepository<T1, T2> : IPricesCacheRepository<T1, T2>
        where T1 : CarPriceQuote
        where T2 : VersionPriceQuote
    {
        private readonly ICacheManager _cacheProvider;
        private readonly IPricesRepository<T1, T2> _iPricesRepo;
        private readonly ICarDataRepository _iDataRepo;
        private readonly ICarModelCacheRepository _modelCacheRepo;
        private readonly IGeoCitiesCacheRepository _geoCacheRepo;
        private readonly IPrices _prices;
        private readonly string priceCacheKey = "PQ-{0}-ModelPrices-ModelId-{1}-CityId-{2}-{3}"; // Do this key versioning in PriceInsert Consumer till microservice is ready

        public PricesCacheRepository(ICacheManager cacheProvider, IPricesRepository<T1, T2> pricesRepo, ICarDataRepository iDataRepo,
            ICarModelCacheRepository modelCacheRepo, IGeoCitiesCacheRepository geoCacheRepo, IPrices prices)
        {
            _cacheProvider = cacheProvider;
            _iPricesRepo = pricesRepo;
            _iDataRepo = iDataRepo;
            _modelCacheRepo = modelCacheRepo;
            _geoCacheRepo = geoCacheRepo;
            _prices = prices;
        }

        public CarEntity GetVersionModel(int versionId)
        {
            return _cacheProvider.GetFromCache<CarEntity>(String.Format("PQ-V2-VersionModel-{0}", versionId),
                CacheRefreshTime.NeverExpire(), () => _iDataRepo.GetVersionModel(versionId));

        }

        public bool InvalidateCache(T1 pricesInput)
        {
            try
            {
                if(pricesInput.VersionPricesList == null)
                    return false;

                List<string> keys = new List<string>();
                List<string> priceKeys = new List<string>();
                CarEntity carMakeModel = null;

                if (pricesInput.VersionPricesList.Count > 0)
                {
                    carMakeModel = GetVersionModel(pricesInput.VersionPricesList[0].VersionId);                  
                    keys.Add(String.Format("PQ-VersionPrices-ModelId-{0}-CityId-{1}-{2}", pricesInput.ModelId, pricesInput.CityId, pricesInput.VersionPricesList[0].IsNew ? "new" : "discontinued")); 
                }

                var stateDetail = _geoCacheRepo.GetStateByCityId(pricesInput.CityId);

                if (carMakeModel != null )
                {
                    keys.Add(string.Format("CarModels_Site_{0}_v3", carMakeModel.MakeId));
                }

                priceKeys.Add(string.Format("model-versions-price-details-{0}-{1}", pricesInput.ModelId, pricesInput.CityId));
                priceKeys.Add(string.Format("model-price-details-{0}-{1}", pricesInput.ModelId, pricesInput.CityId));

                foreach (var item in pricesInput.VersionPricesList)
                {
                    keys.Add(string.Format("PQ-V2-VersionModel-{0}", item.VersionId));
                    keys.Add(string.Format("PQ-V1-VersionPrices-VersionId-{0}-CityId-{1}", item.VersionId, pricesInput.CityId));
                    keys.Add(string.Format("pq-car-details-of-version-{0}-v2", item.VersionId));
                    keys.Add(string.Format("pq-{0}-{1}", pricesInput.CityId, item.VersionId));
                    keys.Add(string.Format("pq-v1-{0}-{1}", pricesInput.CityId, item.VersionId));
                    keys.Add(string.Format("CompareCarData_Version_{0}_v3", item.VersionId));
                    priceKeys.Add(string.Format("version-price-details-{0}-{1}", item.VersionId, pricesInput.CityId));
                }

                keys.Add(string.Format("pq-model-prices-{0}-{1}", pricesInput.ModelId, pricesInput.CityId));
                keys.Add(string.Format("Model_Specs_Site_{0}", pricesInput.ModelId));
                keys.Add(string.Format("Model_Details_Site_{0}_v6", pricesInput.ModelId));
                keys.Add(string.Format("Version_Summary_Site_{0}_v3", pricesInput.ModelId));
                keys.Add(string.Format("pq-cities-by-modelid-v2-{0}", pricesInput.ModelId));
                keys.Add(string.Format("pq-states-by-modelid-{0}", pricesInput.ModelId));
                keys.Add(string.Format("pq-zones-{0}-{1}", pricesInput.ModelId, pricesInput.CityId));
                keys.Add(string.Format("pq-city-zones-by-modelid-{0}", pricesInput.ModelId));
                keys.Add(string.Format("pq-popular-cities-by-modelid-{0}", pricesInput.ModelId));
                keys.Add(string.Format("pq-cities-by-modelid-stateid{0}-{1}", pricesInput.ModelId, stateDetail.StateId));
                keys.Add(string.Format("pq-cust-city-{0}", pricesInput.CityId));
                keys.Add(string.Format("pq-zones-by-modelid-v2-{0}", pricesInput.ModelId));
                keys.Add(string.Format("pq-city-groups-by-modelid-v2-{0}", pricesInput.ModelId));
                keys.Add(string.Format("pq-model-avg-prices-{0}-" + (pricesInput.VersionPricesList[0].IsNew ? "new" : "discontinued"), pricesInput.ModelId));
                keys.Add(string.Format("Versions_new_{0}_v1", pricesInput.ModelId));
                keys.Add(string.Format("pq-version-details-{0}-{1}", pricesInput.ModelId, pricesInput.CityId));
                keys.Add(string.Format("Price_In_Other_Cities_Site_{0}_{1}_v2", pricesInput.ModelId, pricesInput.CityId));

                _prices.UpdateCache(priceKeys);
                _cacheProvider.ExpireMultipleCache(keys);
                _cacheProvider.ExpireCacheWithoutDelay(String.Format(priceCacheKey, "V4", pricesInput.ModelId, pricesInput.CityId, pricesInput.VersionPricesList[0].IsNew ? "new" : "discontinued"));
                _cacheProvider.ExpireCacheWithCriticalRead(string.Format("pq-model-avg-prices-{0}-" + (pricesInput.VersionPricesList[0].IsNew ? "new" : "discontinued"), pricesInput.ModelId));
                
                return true;
            }
            catch (Exception e)
            {
                string errString = HttpContext.Current.Request.ServerVariables["URL"] + JsonConvert.SerializeObject(pricesInput);
                var exception = new ExceptionHandler(e, errString);
                exception.LogException();
                return false;
            }
        }
    }
}
