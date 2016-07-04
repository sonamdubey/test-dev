using Newtonsoft.Json;
using System;

namespace BikeWale.DTO.AutoBiz
{
    /// <summary>
    /// Created By : Sadhana Upadhyay on 24th Oct 2014
    /// Summary : Entity for Area
    /// </summary>
    public class AreaEntityBaseDTO
    {
        [JsonProperty("areaId")]
        public UInt32 AreaId { get; set; }

        [JsonProperty("areaName")]
        public string AreaName { get; set; }

        [JsonProperty("pinCode")]
        public string PinCode { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("latitude")]
        public double Latitude { get; set; }
    }
}
