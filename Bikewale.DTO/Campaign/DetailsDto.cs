using Newtonsoft.Json;

namespace Bikewale.DTO.Campaign
{
    public class DetailsDto
    {
        [JsonProperty("dealer", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DealerCampaignBase Dealer { get; set; }
        [JsonProperty("esCampaign", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public PreRenderCampaignBase EsCamapign { get; set; }
    }
}
