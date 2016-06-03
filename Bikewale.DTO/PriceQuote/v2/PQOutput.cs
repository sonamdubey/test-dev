using Newtonsoft.Json;

namespace Bikewale.DTO.PriceQuote.v2
{
    public class PQOutput
    {
        [JsonProperty("quoteId")]
        public ulong PQId { get; set; }
        [JsonProperty("dealerId")]
        public uint DealerId { get; set; }
        [JsonProperty("versionId")]
        public uint VersionId { get; set; }
        [JsonProperty("isDealerAvailable")]
        public bool IsDealerAvailable { get; set; }
    }
}
