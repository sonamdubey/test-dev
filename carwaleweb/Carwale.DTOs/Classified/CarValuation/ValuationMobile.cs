using Carwale.Entity.Classified.CarValuation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Classified.CarValuation
{
    public class ValuationMobileDTO
    {
        [JsonProperty("valuationResults")]
        public ValuationResultsDTO ValuationResults { get; set; }

        [JsonProperty("valuationRecommendations")]
        public List<ValuationRecommendationDTO> ValuationRecommendations { get; set; }
    }
}
