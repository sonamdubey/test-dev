using BikewaleOpr.Entity;
using System.Collections.Generic;

namespace BikewaleOpr.Models.AdSlot
{
    /// <summary>
    /// Created by : Ashutosh Sharma on 30 Oct 2017
    /// Description : View Model for AdSlot.
    /// </summary>
    public class AdSlotVM
    {
        public int UserId { get; set; }
        public IEnumerable<AdSlotEntity> AdSlotList{ get; set; }
    }
}
