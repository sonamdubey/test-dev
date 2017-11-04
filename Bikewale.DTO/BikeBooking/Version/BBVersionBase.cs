using Newtonsoft.Json;
using System;

namespace Bikewale.DTO.BikeBooking.Version
{
    /// <summary>
    /// Bikebooking version base
    /// Author  :   Sumit Kate
    /// Date    :   20 Aug 2015
    /// </summary>
    public class BBVersionBase
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
