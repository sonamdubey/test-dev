using Newtonsoft.Json;

namespace Carwale.DTOs.Monetization
{
    public class MonetizationModelDTOV1
    {
        [JsonProperty("advantageAdUnit")]
        public AdvantageMonetizationDTO AdvantageAdUnit { get; set; }
        [JsonProperty("dealerAdUnit")]
        public DealerAdMonetizationDTO dealerAdUnit { get; set; }
        [JsonProperty("sponsoredAdUnit")]
        public AppDTOV1 SponsoredAdUnit { get; set; }
    }
}
