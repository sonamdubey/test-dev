using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Carwale.DTOs.Deals
{
    public class SimilarDealsRecommendationDTO
    {
        [JsonProperty("heading")]
        public string Heading { get; set; }
        [JsonProperty("dealsRecommendations")]
        public List<DealsSummaryDesktop_DTO> DealsRecommedations { get; set; }
        [JsonProperty("dealsCount")]
        public int DealsCount { get; set; }
    }
}