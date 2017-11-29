using Newtonsoft.Json;
using System;

namespace Bikewale.DTO.BikeBooking.Area
{
    /// <summary>
    /// Bikebooking Area
    /// Author  :   Sumit Kate
    /// Date    :   20 Aug 2015
    /// </summary>
    public class BBAreaBase
    {
        [JsonProperty("areaId")]
        public UInt32 AreaId { get; set; }
        [JsonProperty("areaName")]
        public string AreaName { get; set; }
    }
}
