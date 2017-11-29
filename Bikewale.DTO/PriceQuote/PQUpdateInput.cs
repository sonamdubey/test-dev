using Newtonsoft.Json;
using System;

namespace Bikewale.DTO.PriceQuote
{
    /// <summary>
    /// Author      :   Sumit Kate
    /// Created     :   16 Oct 2015
    /// Description :   PQ Update input DTO
    /// </summary>
    public class PQUpdateInput
    {
        [JsonProperty("pqId")]
        public UInt32 PQId { get; set; }
        [JsonProperty("versionId")]
        public UInt32 VersionId { get; set; }

    }
}
