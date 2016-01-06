using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppNotification.Entity
{
    [Serializable]
    public class MobileAppNotifications : MobileAppNotificationBase
    {
        public List<string> GCMList { get; set; }
        public List<string> ApnsList { get; set; }
    }
}
