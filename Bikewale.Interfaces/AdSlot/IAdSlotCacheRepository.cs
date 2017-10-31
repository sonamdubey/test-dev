using Bikewale.Entities.Models;
using System.Collections.Generic;

namespace Bikewale.Interfaces.AdSlot
{
    /// <summary>
    /// Created by : Ashutosh Sharma on 31 Oct 2017
    /// Description : Provide cache repository methods for ad slots.
    /// </summary>
    public interface IAdSlotCacheRepository
    {
        IEnumerable<AdSlotEntity> GetAdSlotStatus();
    }
}
