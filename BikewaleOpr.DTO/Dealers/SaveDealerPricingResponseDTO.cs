using Newtonsoft.Json;

namespace BikewaleOpr.DTO.Dealers
{
    /// <summary>
    /// Created By  :   Vishnu Teja Yalakuntla on 18 Aug 2017
    /// Description :   Holds reponse of an API which saves or updates dealer pricing and bike availability
    /// </summary>
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
