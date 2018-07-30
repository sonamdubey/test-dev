using Newtonsoft.Json;

namespace Bikewale.DTO.PriceQuote
{
    /// <summary>
    /// Mobile Verification input
    /// Author  :   Sumit Kate
    /// Date    :   21 Aug 2015
    /// </summary>
    public class DPQMobileVerifiedInput
    {
        [JsonProperty("pqId")]
        public uint PQId { get; set; }
    }
}
