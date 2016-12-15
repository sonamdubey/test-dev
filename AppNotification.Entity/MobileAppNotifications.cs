using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AppNotification.Entity
{
    public class MobileAppNotifications : MobileAppNotificationBase
    {
        [JsonProperty("gCMList")]
        public List<string> GCMList { get; set; }
        [JsonProperty("apnsList")]
        public List<string> ApnsList { get; set; }
    }


    public class NotificationResponse
    {
        public string Error { get; set; }
    }

    public class FCMPushNotificationStatus
    {
        public bool Successful { get; set; }

        public NotificationResponse Response { get; set; }

        public Exception Error { get; set; }
    }

    public class SubscriptionResponse
    {
        public List<SubscriptionResult> Results { get; set; }
    }

    public class SubscriptionResult
    {
        public string Error { get; set; }
    }
}
