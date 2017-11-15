using Bikewale.Entities.Models;
using System.Collections.Generic;

namespace Bikewale.Interfaces.AdSlot
{
    /// <summary>
    /// Created by : Ashutosh Sharma on 31 Oct 2017
    /// Description : Provide DAL methods for ad slots.
    /// </summary>
    public interface IAdSlotRepository
    {
        IEnumerable<AdSlotEntity> GetAdSlotStatus();
    }
}
