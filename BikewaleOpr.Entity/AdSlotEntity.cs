using System;

namespace BikewaleOpr.Entity
{
    /// <summary>
    /// Created by : Ashutosh Sharma on 30 Oct 2017
    /// Description : Entity for AdSlot.
    /// </summary>
    public class AdSlotEntity
    {
        public uint AdId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
    }
}
