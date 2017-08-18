using Newtonsoft.Json;

namespace BikewaleOpr.DTO.Dealers
{
    public class DeleteDealerPricingResponseDTO
    {
        [JsonProperty("isPriceDeleted")]
        public bool IsPriceDeleted { get; set; }
        [JsonProperty("isAvailabilityDeleted")]
        public bool IsAvailabilityDeleted { get; set; }
    }
}
