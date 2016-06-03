using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.PriceQuote.v2
{
    public class DPQVersionBase
    {
        [JsonProperty("priceList")]
        public IEnumerable<DPQ_Price> PriceList { get; set; }
        [JsonProperty("versionId")]
        public uint VersionId { get; set; }
        [JsonProperty("versionName")]
        public string VersionName { get; set; }
    }
}
