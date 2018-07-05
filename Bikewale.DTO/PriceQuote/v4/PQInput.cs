using Newtonsoft.Json;
using System;

namespace Bikewale.DTO.PriceQuote.v4
{
    /// <summary>
    /// Modifier    : Kartik on 20 jun 2018 for price quote changes modified string RefPQId 
    /// </summary>
    public class PQInput
    {
        /// <summary>
        /// City Id
        /// </summary>
        [JsonProperty("cityId")]
        public uint CityId { get; set; }
        /// <summary>
        /// Area Id
        /// </summary>
        [JsonProperty("areaId")]
        public uint AreaId { get; set; }
        /// <summary>
        /// Bike model id
        /// </summary>
        [JsonProperty("modelId")]
        public uint ModelId { get; set; }
        /// <summary>
        /// Client IP
        /// </summary>
        [JsonProperty("clientIP")]
        public string ClientIP { get; set; }
        /// <summary>
        /// PQ Source Id.
        /// </summary>
        [JsonProperty("sourceType")]
        public PQSources SourceType { get; set; }
        /// <summary>
        /// Bike version Id
        /// </summary>
        [JsonProperty("versionId")]
        public uint VersionId { get; set; }
        //Added By  : Sadhana Upadhyay on 29 Dec 2015
        /// <summary>
        /// PQ Lead Id
        /// </summary>
        [JsonProperty("pQLeadId")]
        public ushort? PQLeadId { get; set; }
        /// <summary>
        /// Device ID. 
        /// For Desk and Mobile site this is bwc cookie value.
        /// For Android, This is IMEI number
        /// </summary>
        [JsonProperty("deviceId")]
        public string DeviceId { get; set; }
        /// <summary>
        /// Dealer ID
        /// </summary>
        [JsonProperty("dealerId")]
        public uint? DealerId { get; set; }

        [JsonProperty("refPQId")]
        public string RefPQId { get; set; }
    }
}
