using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Bikewale.Entities.MobileAppAlert
{
    public class FCMNotifications
    {

    }

    [Serializable]
    public class MapResponse
    {
        [JsonProperty("results")]
        public List<Mapping> Results { get; set; }
    }

    [Serializable]
    public class Mapping
    {
        [JsonProperty("apns_token")]
        public string ApnsToken { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("registration_token")]
        public string RegistrationToken { get; set; }
    }

    [Serializable]
    public class SubscriptionRequest
    {
        [JsonProperty("to")]
        public string To { get; set; }

        [JsonProperty("registration_tokens")]
        public List<string> RegistrationTokens { get; set; }
    }


    [Serializable]
    public class SubscriptionResponse
    {
        [JsonProperty("results")]
        public List<SubscriptionResult> Results { get; set; }
    }

    [Serializable]
    public class SubscriptionResult
    {
        [JsonProperty("error")]
        public string Error { get; set; }
    }

}
