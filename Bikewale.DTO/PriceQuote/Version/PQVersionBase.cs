using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.PriceQuote.Version
{
    /// <summary>
    /// Price Quote Version base
    /// Author  :   Sumit Kate
    /// Date    :   21 Aug 2015
    /// </summary>
    public class PQVersionBase
    {
        [JsonProperty("versionId")]
        public uint VersionId { get; set; }
        [JsonProperty("versionName")]
        public string VersionName { get; set; }
        [JsonProperty("modelName")]
        public string ModelName { get; set; }
        [JsonProperty("price")]
        public UInt64 Price { get; set; }
        [JsonProperty("maskingName")]
        public string MaskingName { get; set; }
    }
}
