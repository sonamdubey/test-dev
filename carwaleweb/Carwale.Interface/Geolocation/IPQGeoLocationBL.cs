using Carwale.DTOs.Geolocation;
using Carwale.Entity.Geolocation;
using Carwale.Entity.Geolocation.LatLongURI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.Geolocation
{
    /// <summary>
    /// Created by: Rohan Sapkal
    /// Date      : 20-03-2015
    /// </summary>
    public interface IPQGeoLocationBL
    {
        StatesAndPopularCities GetPQStatesAndPopularCities(int modelId);
        City GetCityDetailsByLatLong(LatLongURI querystring);
        City GetCityById(int id);
        List<CityZones> GetPQCityZonesList(int modelId);
        PQCityDTO GetPQCitiesZones(int modelId, int cityId, int stateId);
        bool GetPriceAvailability(int modelId, int cityId);
        PQCityDTOV2 GetPQCitiesZonesV2(int modelId, int cityId, int stateId);
        Location GetCityDetails(int cityId, int zoneId, int areaId);
        LocationV2 GetCityDetailsV2(int cityId, int v, int areaId);
        Dictionary<string, CustLocation> MultiGetCityNameFromCache(List<int> cityIds);
        List<CityZonesDTOV2> GetZonesbyState(int stateId);
        List<GroupCityDTO> GetGroupCities(int stateId);
    }
}