using Newtonsoft.Json;

namespace Bikewale.DTO.NewBikeSearch
{
    public class Pager
    {
        [JsonProperty("prevUrl")]
        public string PrevPageUrl { get; set; }

        [JsonProperty("nextUrl")]
        public string NextPageUrl { get; set; }
    }
}
