using Newtonsoft.Json;

namespace BikewaleOpr.DTO.Dealers
{
    public class SaveDealerPricingResponseDTO
    {
        [JsonProperty("isPriceSaved")]
        public bool IsPriceSaved { get; set; }
        [JsonProperty("isAvailabilitySaved")]
        public bool IsAvailabilitySaved { get; set; }
        [JsonProperty("rulesUpdatedModelNames")]
        public string RulesUpdatedModelNames { get; set; }
    }
}
