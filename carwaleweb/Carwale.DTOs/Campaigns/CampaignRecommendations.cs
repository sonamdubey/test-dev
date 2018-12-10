using Newtonsoft.Json;
using System.Collections.Generic;

namespace Carwale.DTOs.Campaigns
{
    public class CampaignRecommendations
    {
        [JsonProperty("recommendations")]
        public List<CampaignDealsRecommendationDTO> Recommendations { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }
}
