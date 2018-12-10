using Carwale.DTOs.Campaigns;
using Carwale.DTOs.Deals;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs
{
    public class CampaignDealsRecommendationDTO
    {
        [JsonProperty("dealsRecommendation")]
        public DealsRecommendationDTO DealsRecommendation { get; set; }

        [JsonProperty("campaignRecommendation")]
        public CampaignRecommendationEntity CampaignRecommendation { get; set; }
    }
}
