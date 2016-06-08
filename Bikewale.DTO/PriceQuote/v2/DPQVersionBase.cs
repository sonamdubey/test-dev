using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.PriceQuote.v2
{
    /// <summary>
    /// Created By : Sumit Kate
    /// Created on : 3rd June 2016
    /// Description : New DPQVersionBase version for api/dealerversionprices and api/v2/onroadprice
    /// </summary>
    public class DPQVersionBase
    {
        [JsonProperty("priceList")]
        public IEnumerable<DPQ_Price> PriceList { get; set; }
        [JsonProperty("versionId")]
        public uint VersionId { get; set; }
        [JsonProperty("versionName")]
        public string VersionName { get; set; }
        [JsonProperty("otherPriceList")]
        public IEnumerable<DPQ_Price> OtherPriceList { get; set; }
    }
}
