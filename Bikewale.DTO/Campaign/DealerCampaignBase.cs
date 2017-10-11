using Bikewale.DTO.DealerLocator;
using Newtonsoft.Json;
using System;

namespace Bikewale.DTO.Campaign
{
    public class DealerCampaignBase : CampaignDetailsBase
    {
        [JsonProperty("secondaryDealerCount", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public UInt16 SecondaryDealerCount { get; set; }

        [JsonProperty("isPremium", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool IsPremium { get; set; }

        [JsonProperty("primaryDealer", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DealerBase PrimaryDealer { get; set; }
    }
}
