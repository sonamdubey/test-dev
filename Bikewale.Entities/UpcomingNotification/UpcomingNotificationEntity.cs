using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.UpcomingNotification
{
    
    /// <summary>
    /// Created by: Dhruv Joshi on 7th Feb 2018
    /// Description: Entity for Upcoming Bikes Notification
    /// </summary>
    public class UpcomingNotificationEntity
    {
        public string EmailId { get; set; }
        public ushort NotificationId { get; set; }
        public ushort MakeId { get; set; }
        public ushort ModelId { get; set; }
        public string BikeName { get; set; }
        public DateTime EntryDate { get; set; }
        public bool IsActive { get; set; }
        public ushort NotificationTypeId { get; set; }

    }
}
