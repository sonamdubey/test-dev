using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppNotification.Entity
{
    [Serializable]
    public class MobileAppNotificationRegistration
    {
        public string IMEI { get; set; }
        public string Name { get; set; }
        public string EmailId { get; set; }
        public string ContactNo { get; set; }
        public int OsType { get; set; }
        public string GCMId { get; set; }
        public string SubsMasterId { get; set; }
    }
}
