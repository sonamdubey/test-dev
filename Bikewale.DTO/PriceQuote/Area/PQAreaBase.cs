using Newtonsoft.Json;
using System;

namespace Bikewale.DTO.PriceQuote.Area
{
    /// <summary>
    /// Price Quote Area base
    /// Author  :   Sumit Kate
    /// Date    :   20 Aug 2015
    /// </summary>
    public class PQAreaBase
    {
        [JsonProperty("areaId")]
        public UInt32 AreaId { get; set; }
        [JsonProperty("areaName")]
        public string AreaName { get; set; }
        [JsonProperty("MaskingName")]
        public string MaskingName { get; set; }
        [JsonProperty("pinCode")]
        public string PinCode { get; set; }
        [JsonProperty("longitude")]
        public double Longitude { get; set; }
        [JsonProperty("latitude")]
        public double Latitude { get; set; }
    }
}
