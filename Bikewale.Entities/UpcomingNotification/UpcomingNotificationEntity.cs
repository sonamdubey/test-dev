using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.UpcomingNotification
{
    
    /// <summary>
    /// Created by: Dhruv Joshi
    /// Dated: 8th Feb 2018
    /// Description: Entity for Upcoming Bikes Notification
    /// </summary>
    public class UpcomingBikeEntity
    {
        public ushort MakeId { get; set; }
        public ushort ModelId { get; set; }
        public string BikeName { get; set; }
    }
}
