using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.PriceQuote.BikeQuotation
{
    /// <summary>
    /// Author      :   Sumit Kate
    /// Created By  :   08 Oct 2015
    /// Description :   Varients DTO
    /// </summary>
    public class OtherVersionInfoDTO
    {
        [JsonProperty("versionId")]
        public uint VersionId { get; set; }
        [JsonProperty("versionName")]
        public string VersionName { get; set; }
        [JsonProperty("onRoadPrice")]
        public ulong OnRoadPrice { get; set; }
        [JsonProperty("price")]
        public UInt32 Price { get; set; }
        [JsonProperty("rto")]
        public UInt32 RTO { get; set; }
        [JsonProperty("insurance")]
        public UInt32 Insurance { get; set; }
    }
}
