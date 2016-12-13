using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.Entities.MobileAppAlert
{
    public class FCMNotifications
    {
        public SubscriptionRequest Request { get; set; }
        public SubscriptionResponse Response { get; set; }
        public SubscriptionResult Result { get; set; }
    }

    public class SubscriptionRequest
    {
        [JsonProperty("to")]
        public string To { get; set; }

        [JsonProperty("registration_tokens")]
        public List<string> RegistrationTokens { get; set; }
    }

    public class SubscriptionResponse
    {
        [JsonProperty("results")]
        public List<SubscriptionResult> Results { get; set; }
    }

    public class SubscriptionResult
    {
        [JsonProperty("error")]
        public string Error { get; set; }
    }

}
