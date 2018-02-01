using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.UpcomingNotification
{
    public class UpcomingNotificationEntity
    {
        public int UserId { get; set; }
        public string EmailId { get; set; }
        public int NotificationId { get; set; }
        public int MakeId { get; set; }
        public string ModelId { get; set; }
        public DateTime EntryDate { get; set; }
        public bool IsActive { get; set; }
        public string NotificationTypeId { get; set; }

    }
}
