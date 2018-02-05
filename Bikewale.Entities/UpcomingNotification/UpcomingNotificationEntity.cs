using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.UpcomingNotification
{
    public class UpcomingNotificationEntity
    {
        public string EmailId { get; set; }
        public int NotificationId { get; set; }
        public int MakeId { get; set; }
        public int ModelId { get; set; }
        public string BikeName { get; set; }
        public DateTime EntryDate { get; set; }
        public bool IsActive { get; set; }
        public int NotificationTypeId { get; set; }

    }
}
