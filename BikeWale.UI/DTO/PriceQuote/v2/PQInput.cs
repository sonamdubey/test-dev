using Newtonsoft.Json;
using System;

namespace Bikewale.DTO.PriceQuote.v2
{
    /// <summary>
    /// 
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

        [JsonProperty("pQLeadId")]
        public ushort? PQLeadId { get; set; }

        [JsonProperty("deviceId")]
        public string DeviceId { get; set; }

        [JsonProperty("refPQId")]
        public UInt64? RefPQId { get; set; }

        [JsonProperty("isReload")]
        public bool IsReload { get; set; }

        [JsonProperty("isPersistance")]
        public bool IsPersistance { get; set; }
    }
}
