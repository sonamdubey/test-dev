using Bikewale.DTO.City;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entities.Used;
using System.Collections;
using System.Collections.Generic;

namespace Bikewale.Interfaces.Location
{
    /// <summary>
    /// Created By : Ashish G. Kamble
    /// Modified By : Ashish G. Kamble on 7 June 2016
    /// Modified By Vive Gupta on 24 june 2016
    /// Desc : added description for GetDealerStateCities
    /// Modified By:-Subodh Jain 29 dec 2016
    /// Summary :- Get Used Bike By Make City With Count
    /// Modified By: Kartik Rathod on 8 jun 2018 added GetCitiesByStateName
    /// </summary>
    public interface ICity
    {
        List<CityEntityBase> GetPriceQuoteCities(uint modelId);
        List<CityEntityBase> GetAllCities(EnumBikeType requestType);
        List<CityEntityBase> GetCities(string stateId, EnumBikeType requestType);
        Hashtable GetMaskingNames();
        Hashtable GetOldMaskingNames();
        DealerStateCities GetDealerStateCities(uint makeId, uint stateId);
        IEnumerable<UsedBikeCities> GetUsedBikeByCityWithCount();
        IEnumerable<UsedBikeCities> GetUsedBikeByMakeCityWithCount(uint makeid);
        IEnumerable<CityBase> GetCitiesByStateName(string stateName);
    }
}
