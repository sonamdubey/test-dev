using Newtonsoft.Json;
using System;

/// <summary>
/// Summary description for FCMPushNotificationStatus
/// </summary>
namespace Bikewale.Entities.MobileAppAlert
{

    [Serializable]
    public class MobilePushNotificationData
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("smallPicUrl")]
        public string SmallPicUrl { get; set; }

        [JsonProperty("detailUrl")]
        public string DetailUrl { get; set; }

        [JsonProperty("alertTypeId")]
        public int AlertTypeId { get; set; }

        [JsonProperty("alertId")]
        public int AlertId { get; set; }

        [JsonProperty("isFeatured")]
        public bool IsFeatured { get; set; }

        [JsonProperty("largePicUrl")]
        public string LargePicUrl { get; set; }

        [JsonProperty("publishDate")]
        public string PublishDate { get; set; }

        [JsonProperty("payLoad")]
        public string PayLoad { get; set; }
    }

    [Serializable]
    public class MobilePushNotificationAttr
    {
        [JsonProperty("sound")]
        public string Sound { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }
    }

    [Serializable]
    public class NotificationBase
    {
        [JsonProperty("to")]
        public string To { get; set; }

        [JsonProperty("data")]
        public MobilePushNotificationData Data { get; set; }

        [JsonProperty("time_to_live")]
        public int TimeToLive { get; set; }
    }

    [Serializable]
    public class NotificationResponse
    {
        [JsonProperty("message_id")]
        public string MessageId { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }
    }

    [Serializable]
    public class FCMPushNotificationStatus
    {
        public bool Successful { get; set; }

        public NotificationResponse Response { get; set; }

        public Exception Error { get; set; }
    }
}

