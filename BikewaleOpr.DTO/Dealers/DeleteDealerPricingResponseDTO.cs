using Newtonsoft.Json;

namespace BikewaleOpr.DTO.Dealers
{
    /// <summary>
    /// Created By  :   Vishnu Teja Yalakuntla on 18 Aug 2017
    /// Description :   Holds response of an API which deletes dealer pricing and bike availability
    /// </summary>
    public class DeleteDealerPricingResponseDTO
    {
        [JsonProperty("isPriceDeleted")]
        public bool IsPriceDeleted { get; set; }
        [JsonProperty("isAvailabilityDeleted")]
        public bool IsAvailabilityDeleted { get; set; }
    }
}
