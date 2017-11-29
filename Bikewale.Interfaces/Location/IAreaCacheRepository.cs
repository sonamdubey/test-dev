using System.Collections.Generic;

namespace Bikewale.Interfaces.Location
{
    /// <summary>
    /// Created by  :   Sumit Kate on 25 Jan 2016
    /// Description :   Interface for AreaCacheRepository
    /// </summary>
    public interface IAreaCacheRepository
    {
        IEnumerable<Bikewale.Entities.Location.AreaEntityBase> GetAreaList(uint modelId, uint cityId);
    }
}
