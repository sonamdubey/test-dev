using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entities.Used;
using System.Collections.Generic;

namespace Bikewale.Interfaces.Location
{
    /// <summary>
    /// Created By : Vivek Gupta on 24 june 2016
    /// Modified by : Sajal Gupta on 8-11-2016
    /// Desc : Added GetCityDetails() method.
    /// Modified By:-Subodh Jain 29 dec 2016
    /// Summary :- Get Used Bike By Make City With Count
    /// </summary>
    public interface ICityCacheRepository
    {
        IEnumerable<CityEntityBase> GetPriceQuoteCities(uint modelId);
        IEnumerable<CityEntityBase> GetAllCities(EnumBikeType requestType);
        DealerStateCities GetDealerStateCities(uint makeId, uint stateId);
        IEnumerable<UsedBikeCities> GetUsedBikeByCityWithCount();
        IEnumerable<UsedBikeCities> GetUsedBikeByMakeCityWithCount(uint makeid);
        CityEntityBase GetCityDetails(string cityMasking);
		IEnumerable<CityEntityBase> GetModelPriceCities(uint modelId, uint popularCityCount);

	}
}
