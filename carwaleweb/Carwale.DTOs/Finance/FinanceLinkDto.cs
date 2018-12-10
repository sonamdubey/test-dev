using Newtonsoft.Json;

namespace Carwale.DTOs.Finance
{
    public class FinanceLinkDto
    {
        [JsonProperty("clientId")]
        public int ClientId { get; set; }
        [JsonProperty("linkText")]
        public string LinkText { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("appendQs")]
        public bool AppendQS { get; set; }
        [JsonProperty("isAd")]
        public bool IsAd { get; set; }
        [JsonProperty("adText")]
        public string AdText { get; set; }
    }
}
