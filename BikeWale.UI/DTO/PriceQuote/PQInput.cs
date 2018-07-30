using Newtonsoft.Json;
using System;

namespace Bikewale.DTO.PriceQuote
{
    /// <summary>
    /// Price Quote input entity
    /// Author  :   Sumit Kate
    /// Date    :   21 Aug 2015
    /// Modified by :   Sumit Kate on 08 Feb 2016
    /// Description :   Added Dealer Id as new property
    /// Modified by :   Lucky Rathore on 20 April 2016
    /// Description :   Added RefPQId as new property
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
        public UInt64? RefPQId { get; set; }
    }
}
