using Carwale.Entity.Common;
using Carwale.Entity.Geolocation;
using Carwale.Entity.Geolocation.LatLongURI;
using System.Collections.Generic;

namespace Carwale.Interfaces.Geolocation
{
    public interface IGeoCitiesRepository
    {
        IEnumerable<Cities> GetCities(Modules module = Modules.Default, bool? isPopular = null);
        List<City> GetPQCitiesByStateIdAndModelId(int stateId, int modelId);
        List<States> GetPQStatesByModelId(int modelId);
        List<City> GetCitiesByType(string type);
        List<City> GetPQCitiesByModelId(int modelId);
        List<Zones> GetPQCityZones(int cityId, int modelId);
        List<PopularCity> GetPQPopularCities(int modelId);
        string GetCityNameById(string cityId);
        List<States> GetStates();
        List<City> GetCitiesByStateId(int stateId);
        CustLocation GetCustLocation(int cityId, string zoneId);
        List<Zone> GetPQCityZonesList(int modelId);
        City GetCityDetailsByLatLong(LatLongURI querystring);
        States GetStateByCityId(int cityId);
        List<City> GetNearestCities(int cityId, short count = 20);
        StateAndAllCities GetStateAndAllCities(int cityId);
        List<Zone> GetPQZones(int modelId);
        List<City> GetPQGroupCities(int modelId);
        bool IsAreaAvailable(int cityId);
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
