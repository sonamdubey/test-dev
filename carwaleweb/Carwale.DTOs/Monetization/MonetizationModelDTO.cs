using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Monetization
{
    public class MonetizationModelDTO
    {
        [JsonProperty("advantageAdUnit")]
        public AdvantageMonetizationDTO AdvantageAdUnit { get; set; }
        [JsonProperty("pqAdUnit")]
        public SponsoredDealerMonetizationDTO PQAdUnit { get; set; }
        [JsonProperty("sponsoredAdUnit")]
        public AppDTOV1 SponsoredAdUnit { get; set; }
    }
}
