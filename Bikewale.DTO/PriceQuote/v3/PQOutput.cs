using Newtonsoft.Json;

namespace Bikewale.DTO.PriceQuote.v3
{
    /// <summary>
    /// Created By  : Pratibha Verma on 19 June 2018
    /// Description : New PQOutput version for  api/v5/onroadprice
    /// </summary>
    public class PQOutput
    {
        [JsonProperty("quoteId")]
        public string PQId { get; set; }
        [JsonProperty("dealerId")]
        public uint DealerId { get; set; }
        [JsonProperty("versionId")]
        public uint VersionId { get; set; }
        [JsonProperty("isDealerAvailable")]
        public bool IsDealerAvailable { get; set; }
        [JsonProperty("makeName")]
        public string MakeName { get; set; }
        [JsonProperty("modelName")]
        public string ModelName { get; set; }
    }
}
