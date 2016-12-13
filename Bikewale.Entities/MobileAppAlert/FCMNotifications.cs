using System.Collections.Generic;

namespace Bikewale.Entities.MobileAppAlert
{
    public class MapResponse
    {
        public List<Mapping> Results { get; set; }
    }

    public class Mapping
    {
        public string ApnsToken { get; set; }
        public string Status { get; set; }
        public string RegistrationToken { get; set; }
    }

    public class SubscriptionRequest
    {
        public string To { get; set; }
        public string RegistrationTokens { get; set; }
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
