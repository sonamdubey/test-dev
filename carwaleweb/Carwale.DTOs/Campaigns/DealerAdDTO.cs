using Carwale.DTOs.CarData;
using Carwale.DTOs.Dealer;
using Carwale.DTOs.PageProperty;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Carwale.DTOs.Campaigns
{
    public class DealerAdDTO
    {
        [JsonProperty("campaign")]
        public CampaignDTO Campaign { get; set; }
        [JsonProperty("dealerDetails")]
        public DealerDTO DealerDetails { get; set; }
        [JsonProperty("pageProperty")]
        public List<PagePropertyDTO> PageProperty { get; set; }
        [JsonProperty("featuredCarData", NullValueHandling=NullValueHandling.Ignore)]
        public CarIdWithImageDto FeaturedCarData { get; set; }
        [JsonProperty("campaignType")]
        public int CampaignType { get; set; }
    }
}
