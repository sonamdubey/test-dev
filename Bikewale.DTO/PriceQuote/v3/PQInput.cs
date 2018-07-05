using Newtonsoft.Json;
using System;

namespace Bikewale.DTO.PriceQuote.v3
{
    /// <summary>
    /// Modifier    : Kartik on 20 jun 2018 for price quote changes modified RefPQId(string)
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
        public string RefPQId { get; set; }

        [JsonProperty("isReload")]
        public bool IsReload { get; set; }

        [JsonProperty("isPersistance")]
        public bool IsPersistance { get; set; }
    }
}
