using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.MobileAppAlerts
{
    public class GCMFormat
    {
        public MobileAppNotificationBase data { get; set; }
        public List<string> registration_ids { get; set; }
    }
}
