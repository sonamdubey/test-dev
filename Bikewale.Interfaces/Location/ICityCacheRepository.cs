using Bikewale.Entities.Location;
using System.Collections.Generic;

namespace Bikewale.Interfaces.Location
{
    /// <summary>
    /// Created By : Vivek Gupta on 24 june 2016    
    /// </summary>
    public interface ICityCacheRepository
    {
        IEnumerable<Entities.Location.CityEntityBase> GetPriceQuoteCities(uint modelId);
        DealerStateCities GetDealerStateCities(uint makeId, uint stateId);
    }
}
