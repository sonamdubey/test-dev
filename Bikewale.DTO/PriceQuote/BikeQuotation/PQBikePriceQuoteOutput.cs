using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.PriceQuote.BikeQuotation
{
    /// <summary>
    /// BikeWale Price Quote output
    /// Author  :   Sumit Kate
    /// Date    :   20 Aug 2015
    /// </summary>
    public class PQBikePriceQuoteOutput
    {
        [JsonProperty("priceQuoteId")]
        public ulong PriceQuoteId { get; set; }

        [JsonProperty("exShowroomPrice")]
        public ulong ExShowroomPrice { get; set; }

        [JsonProperty("rto")]
        public uint RTO { get; set; }

        [JsonProperty("insurance")]
        public uint Insurance { get; set; }

        [JsonProperty("onRoadPrice")]
        public ulong OnRoadPrice { get; set; }

        [JsonProperty("makeName")]
        public string MakeName { get; set; }

        [JsonProperty("modelName")]
        public string ModelName { get; set; }

        [JsonProperty("versionName")]
        public string VersionName { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("area")]
        public string Area { get; set; }

        [JsonProperty("versionId")]
        public uint VersionId { get; set; }

        [JsonProperty("varients")]
        public IEnumerable<OtherVersionInfoDTO> Varients { get; set; }
    }
}
