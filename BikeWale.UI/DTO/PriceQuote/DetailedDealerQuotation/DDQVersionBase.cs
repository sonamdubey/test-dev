using Newtonsoft.Json;

namespace Bikewale.DTO.PriceQuote.DetailedDealerQuotation
{
    /// <summary>
    /// Detailed Dealer Quotation Version base
    /// Author  :   Sumit Kate
    /// Date    :   24 Aug 2015
    /// </summary>
    public class DDQVersionBase
    {
        [JsonProperty("versionId")]
        public int VersionId { get; set; }
        [JsonProperty("versionName")]
        public string VersionName { get; set; }
    }
}
