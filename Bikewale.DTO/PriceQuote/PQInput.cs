using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.PriceQuote
{
    /// <summary>
    /// Price Quote input entity
    /// Author  :   Sumit Kate
    /// Date    :   21 Aug 2015
    /// </summary>
    public class PQInput
    {
        [JsonProperty("cityId")]
        public uint CityId { get; set; }
        [JsonProperty("areaId")]
        public uint AreaId { get; set; }
        [JsonProperty("modelId")]
        public uint ModelId { get; set; }
        [JsonProperty("clientIP")]
        public string ClientIP { get; set; }
        [JsonProperty("sourceType")]
        public PQSources SourceType { get; set; }
        [JsonProperty("versionId")]
        public uint VersionId { get; set; }
        //Added By  : Sadhana Upadhyay on 29 Dec 2015
        [JsonProperty("pQLeadId")]
        public ushort? PQLeadId { get; set; }
        [JsonProperty("deviceId")]
        public string DeviceId { get; set; }
    }
}
