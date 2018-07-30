using Newtonsoft.Json;

namespace Bikewale.DTO.PriceQuote.DealerPriceQuote
{
    /// <summary>
    /// Dealer price quote version base
    /// Author  :   Sumit Kate
    /// Date    :   21 Aug 2015
    /// </summary>
    public class DPQVersionBase
    {
        [JsonProperty("versionId")]
        public int VersionId { get; set; }
        [JsonProperty("versionName")]
        public string VersionName { get; set; }
    }
}
