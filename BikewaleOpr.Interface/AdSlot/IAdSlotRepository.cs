using BikewaleOpr.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.Interface.AdSlot
{
    /// <summary>
    /// Created by : Ashutosh Sharma on 30 Oct 2017
    /// Description : Provide methods to get data for ad slots.
    /// </summary>
    public interface IAdSlotRepository
    {
        IEnumerable<AdSlotEntity> GetAdSlots();
        bool ChangeStatus(uint AdId, uint UserId);
    }
}
