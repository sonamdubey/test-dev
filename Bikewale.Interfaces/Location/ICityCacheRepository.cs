using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entities.Used;
using System.Collections.Generic;

namespace Bikewale.Interfaces.Location
{
    /// <summary>
    /// Created By : Vivek Gupta on 24 june 2016    
    /// </summary>
    public interface ICityCacheRepository
    {
        IEnumerable<CityEntityBase> GetPriceQuoteCities(uint modelId);
        IEnumerable<CityEntityBase> GetAllCities(EnumBikeType requestType);
        DealerStateCities GetDealerStateCities(uint makeId, uint stateId);
        IEnumerable<UsedBikeCities> GetUsedBikeByCityWithCount();
    }
}
