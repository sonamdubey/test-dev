using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppNotification.Entity
{
    public class GCMFormat
    {
        public MobileAppNotificationBase data { get; set; }
        public List<string> registration_ids { get; set; }
    }
}
