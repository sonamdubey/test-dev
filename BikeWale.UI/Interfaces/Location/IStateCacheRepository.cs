using Bikewale.Entities.DealerLocator;
using Bikewale.Entities.Location;
using System.Collections.Generic;

namespace Bikewale.Interfaces.Location
{
    /// Created By : Vivek Gupta 
    /// Date : 24 june 2016    
    /// </summary>
    public interface IStateCacheRepository
    {
        IEnumerable<DealerStateEntity> GetDealerStates(uint makeId);
        DealerLocatorList GetDealerStatesCities(uint makeId);
        StateMaskingResponse GetStateMaskingResponse(string maskingName);
    }
}
