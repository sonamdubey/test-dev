using Newtonsoft.Json;
using System;

namespace Bikewale.DTO.Campaign
{
    public class DealerCampaignBase : CampaignDetailsBase
    {
        [JsonProperty("secondaryDealerCount")]
        public UInt16 SecondaryDealerCount { get; set; }
        [JsonProperty("isPremium")]
        public bool IsPremium { get; set; }
    }
}
