using Newtonsoft.Json;

namespace Carwale.DTOs.Dealer
{
    public class CTDealerDTO
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }
    }
}
