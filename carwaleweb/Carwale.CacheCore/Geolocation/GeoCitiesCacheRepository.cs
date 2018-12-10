using AEPLCore.Cache;
using AEPLCore.Cache.Interfaces;
using Carwale.Entity.Common;
using Carwale.Entity.Geolocation;
using Carwale.Interfaces.Geolocation;
using System;
using System.Collections.Generic;

namespace Carwale.Cache.Geolocation
{
    /// <summary>
    /// Created by : Kirtan Shetty
    /// Date       : September 5, 2014
    /// </summary>
    public class GeoCitiesCacheRepository : IGeoCitiesCacheRepository
    {
        private readonly IGeoCitiesRepository _igcr;
        private readonly ICacheManager _cacheProvider;
        private const string _citiesKeyPrefix = "cities_";

        public GeoCitiesCacheRepository(IGeoCitiesRepository igcr, ICacheManager cacheProvider)
        {
            _igcr = igcr;
            _cacheProvider = cacheProvider;
        }

        public IEnumerable<Cities> GetCities(Modules module = Modules.Default, bool? isPopular = null)
        {
            return _cacheProvider.GetFromCache<IEnumerable<Cities>>(_citiesKeyPrefix + module + (isPopular.HasValue ? "_" + isPopular.Value : null)
                , CacheRefreshTime.NeverExpire(), () => _igcr.GetCities(module, isPopular));
        }

        /// <summary>
        /// Created by : ashish Verma
        /// Date       : September 24, 2014
        /// </summary>
        public List<City> GetPQCitiesByModelId(int modelId)
        {
            var cacheKey = "pq-cities-by-modelid-v2-" + modelId;
            return _cacheProvider.GetFromCache<List<City>>(cacheKey, CacheRefreshTime.OneDayExpire(), () => _igcr.GetPQCitiesByModelId(modelId));
        }

        /// <summary>
        /// Created by : sanjay soni
        /// Date       : July 16, 2015
        /// </summary>
        [Obsolete("Please use GetPQZones and GetPQCityGroups instead")]
        public List<Zone> GetPQCityZonesList(int modelId)
        {
            var cacheKey = "pq-city-zones-by-modelid-" + modelId;
            return _cacheProvider.GetFromCache<List<Zone>>(cacheKey, CacheRefreshTime.OneDayExpire(), () => _igcr.GetPQCityZonesList(modelId));
        }

        public List<States> GetStates()
        {
            var cacheKey = "statesData";
            return _cacheProvider.GetFromCache<List<States>>(cacheKey, CacheRefreshTime.OneDayExpire(), () => _igcr.GetStates());
        }

        /// <summary>
        /// Created by : ashish Verma
        /// Date       : September 24, 2014
        /// </summary>
        public List<States> GetPQStatesByModelId(int modelId)
        {
            var cacheKey = "pq-states-by-modelid-" + modelId;
            return _cacheProvider.GetFromCache<List<States>>(cacheKey, CacheRefreshTime.OneDayExpire(), () => _igcr.GetPQStatesByModelId(modelId));
        }

        public List<City> GetCitiesByStateId(int stateId)
        {
            var cacheKey = "citiesByState-" + stateId;
            return _cacheProvider.GetFromCache<List<City>>(cacheKey, CacheRefreshTime.OneDayExpire(), () => _igcr.GetCitiesByStateId(stateId));
        }

        /// <summary>
        /// Created by : ashish Verma
        /// Date       : September 24, 2014
        /// </summary>
        public List<Zones> GetPQCityZones(int cityId, int modelId)
        {
            var cacheKey = "pq-zones-" + modelId +"-"+cityId;
            return _cacheProvider.GetFromCache<List<Zones>>(cacheKey, CacheRefreshTime.OneDayExpire(), () => _igcr.GetPQCityZones(cityId,modelId));
        }

        /// <summary>
        /// Created by : ashish Verma
        /// Date       : September 24, 2014
        /// </summary>
        public List<PopularCity> GetPQPopularCities(int modelId)
        {
            var cacheKey = "pq-popular-cities-by-modelid-" + modelId;
            return _cacheProvider.GetFromCache<List<PopularCity>>(cacheKey, CacheRefreshTime.OneDayExpire(), () => _igcr.GetPQPopularCities(modelId));
        }

        /// <summary>
        /// Created by : ashish Verma
        /// Date       : September 24, 2014
        /// </summary>
        public List<City> GetPQCitiesByStateIdAndModelId(int modelId, int stateId)
        {
            var cacheKey = "pq-cities-by-modelid-stateid" + modelId +"-"+stateId;
            return _cacheProvider.GetFromCache<List<City>>(cacheKey, CacheRefreshTime.OneDayExpire(), () => _igcr.GetPQCitiesByStateIdAndModelId(modelId, stateId));
        }


        public CustLocation GetCustLocation(int cityId, string zoneId)
        {
            var cacheKey = string.IsNullOrEmpty(zoneId) ? "pq-cust-city-" + cityId : "pq-cust-city-" + cityId + "-zone-" + zoneId;

            return _cacheProvider.GetFromCache<CustLocation>(cacheKey, CacheRefreshTime.OneDayExpire(), () => _igcr.GetCustLocation(cityId, zoneId));
        }

        public string GetCityNameById(string cityId)
        {
            return _cacheProvider.GetFromCache<string>("CityName_" + cityId, CacheRefreshTime.NeverExpire(), () => _igcr.GetCityNameById(cityId));
        }

        public States GetStateByCityId(int cityId)
        {
            return _cacheProvider.GetFromCache<States>("StateByCity_" + cityId, CacheRefreshTime.NeverExpire(), () => _igcr.GetStateByCityId(cityId));
        }

        public List<City> GetNearestCities(int cityId, short count = 20)
        {
            return _cacheProvider.GetFromCache<List<City>>("NearestCity_v1_" + cityId + "_" + count, CacheRefreshTime.NeverExpire(), () => _igcr.GetNearestCities(cityId,count));
        }

        public StateAndAllCities GetStateAndAllCities(int cityId)
        {
            return _cacheProvider.GetFromCache<StateAndAllCities>("StateAndAllCities_" + cityId, CacheRefreshTime.OneDayExpire(), () => _igcr.GetStateAndAllCities(cityId));
        }

        public List<Zone> GetPQZones(int modelId)
        {
            var cacheKey = string.Format("pq-zones-by-modelid-v2-{0}",modelId);
            return _cacheProvider.GetFromCache<List<Zone>>(cacheKey, CacheRefreshTime.OneDayExpire(), () => _igcr.GetPQZones(modelId));
        }

        public List<City> GetPQGroupCities(int modelId)
        {
            var cacheKey = string.Format("pq-city-groups-by-modelid-v2-{0}", modelId);
            return _cacheProvider.GetFromCache<List<City>>(cacheKey, CacheRefreshTime.OneDayExpire(), () => _igcr.GetPQGroupCities(modelId));
        }

        public bool IsAreaAvailable(int cityid)
        {
            var cacheKey = string.Format("areaexists-{0}", cityid);
            return _cacheProvider.GetFromCache<bool>(cacheKey, CacheRefreshTime.NeverExpire(), () => _igcr.IsAreaAvailable(cityid));
        }

        public List<City> GetClassifiedPopularCities()
        {
            var cacheKey = "classifiedPopularCitiesData";
            return _cacheProvider.GetFromCache<List<City>>(cacheKey, CacheRefreshTime.NeverExpire(), () => _igcr.GetClassifiedPopularCities());
        }

        public Cities GetCityDetailsById(int cityId)
        {
            return _cacheProvider.GetFromCache<Cities>(string.Format("Cw_CityDetails_V4_{0}",cityId), CacheRefreshTime.EODExpire(), () => _igcr.GetCityDetailsById(cityId));
        }

        public IEnumerable<Zone> GetZonesByCity(int id)
        {
            return _cacheProvider.GetFromCache<IEnumerable<Zone>>(string.Format("zone-by-city-{0}", id), CacheRefreshTime.NeverExpire(), () => _igcr.GetZonesByCity(id));
        }

        public List<AreaCode> GetAreaCodeByCity(int cityId)
        {
            return _cacheProvider.GetFromCache(string.Format("City_AreaCode_{0}", cityId), CacheRefreshTime.OneDayExpire(), () => _igcr.GetAreaCodeByCity(cityId));
        }


        public List<Zone> GetZonesByState(int stateId)
        {
            return _cacheProvider.GetFromCache(string.Format("zones-by-state-{0}", stateId), CacheRefreshTime.NeverExpire(), () => _igcr.GetZonesByState(stateId));
        }

        public List<City> GetMasterGroupCities()
        {
            return _cacheProvider.GetFromCache("MasterGroupCities", CacheRefreshTime.NeverExpire(), () => _igcr.GetMasterGroupCities());
        }
        public List<City> GetAllGroupCities()
        {
            return _cacheProvider.GetFromCache("AllGroupCities", CacheRefreshTime.NeverExpire(), () => _igcr.GetAllGroupCities());

        }

        public City GetCityDetailsByMaskingName(string maskingName)
        {
            return _cacheProvider.GetFromCache(string.Format("Cw_CityDetails_V3_{0}", maskingName), CacheRefreshTime.NeverExpire(), () => _igcr.GetCityDetailsByMaskingName(maskingName));

        }
    }
}
