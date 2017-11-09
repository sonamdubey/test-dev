using System;

namespace Bikewale.Entities.Models
{
    /// <summary>
    /// Created by : Ashutosh Sharma on 31 Oct 2017
    /// Description : Base Entity for AdSlot.
    /// </summary>
    [Serializable]
    public class AdSlotEntity
    {
        public string Name { get; set; }
        public bool Status { get; set; }
    }
}
