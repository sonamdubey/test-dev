using Carwale.DTOs.CarData;
using Carwale.DTOs.PriceQuote;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Campaigns
{
    public class CampaignRecommendationDTO
    {
        [JsonProperty("carData")]
        public CarDetailsDTO CarData { get; set; }

        [JsonProperty("campaign")]
        public CampaignBaseDTO Campaign { get; set; }

        [JsonProperty("carPricesOverview")]
        public PriceOverviewDTOV2 PricesOverview { get; set; }
    }
}
