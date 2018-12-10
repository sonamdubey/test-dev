using Carwale.Entity.Common;
using Carwale.Entity.Geolocation;
using System.Collections.Generic;

namespace Carwale.Interfaces.Geolocation
{
    /// <summary>
    /// Created by: Kirtan Shetty
    /// Date      : September 5, 2014
    /// </summary>
    public interface IGeoCitiesCacheRepository
    {
        IEnumerable<Cities> GetCities(Modules module = Modules.Default, bool? isPopular = null);
        List<City> GetPQCitiesByModelId(int modelId);
        List<States> GetPQStatesByModelId(int modelId);
        List<Zones> GetPQCityZones(int cityId, int modelId);
        List<Zone> GetPQCityZonesList(int modelId);
        List<PopularCity> GetPQPopularCities(int modelId);
        List<City> GetPQCitiesByStateIdAndModelId(int modelId, int stateId);
        CustLocation GetCustLocation(int cityId, string zoneId);
        List<States> GetStates();
        List<City> GetCitiesByStateId(int stateId);
        string GetCityNameById(string cityId);
        States GetStateByCityId(int cityId);
        List<City> GetNearestCities(int cityId, short count = 20);
        StateAndAllCities GetStateAndAllCities(int cityId);
        List<Zone> GetPQZones(int modelId);
        List<City> GetPQGroupCities(int modelId);
        bool IsAreaAvailable(int cityid);
        List<City> GetClassifiedPopularCities();
        Cities GetCityDetailsById(int cityId);
        IEnumerable<Zone> GetZonesByCity(int id);
        List<AreaCode> GetAreaCodeByCity(int cityId);
        List<Zone> GetZonesByState(int stateId);
        List<City> GetMasterGroupCities();
        List<City> GetAllGroupCities();
        City GetCityDetailsByMaskingName(string maskingName);
    }
}
