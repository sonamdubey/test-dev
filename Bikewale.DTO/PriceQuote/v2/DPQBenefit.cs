
using Newtonsoft.Json;
namespace Bikewale.DTO.PriceQuote.v2
{
    public class DPQBenefit
    {
        [JsonProperty("CategoryId")]
        public int CategoryId { get; set; }
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
