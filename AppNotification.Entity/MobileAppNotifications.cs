using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AppNotification.Entity
{
    public class MobileAppNotifications : MobileAppNotificationBase
    {
        [JsonProperty("gCMList")]
        public List<string> GCMList { get; set; }
        [JsonProperty("apnsList")]
        public List<string> ApnsList { get; set; }
    }
}
