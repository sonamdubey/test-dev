using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.Upcoming
{
    public class UpcomingNotificationEntity
    {
        public string UserId { get; set; }
        public string EmailId { get; set; }
        public string NotificationId { get; set; }
        public string MakeId { get; set; }
        public string ModelId { get; set; }
        public DateTime EntryDate { get; set; }
        public bool IsActive { get; set; }
        public string NotificationTypeId { get; set; }
        public string NotificationType { get; set; }

    }
}
