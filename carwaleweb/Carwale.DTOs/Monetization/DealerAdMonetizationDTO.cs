using Carwale.DTOs.Campaigns;
using Newtonsoft.Json;

namespace Carwale.DTOs.Monetization
{
    public class DealerAdMonetizationDTO : MonetizationDataPriorityDTO
    {
        [JsonProperty("dealerAd")]
        public DealerAdDTO dealerAd { get; set; }
    }
}
