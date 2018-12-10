using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.MobileAppAlerts
{
    public class MobileAppAlert
    {
        public string title { get; set; }
        public string smallPicUrl { get; set; }
        public string detailUrl { get; set; }

        public List<string> UserTokenIds { get; set; }
        public List<string> NotificationServiceList { get; set; }
    }
}
