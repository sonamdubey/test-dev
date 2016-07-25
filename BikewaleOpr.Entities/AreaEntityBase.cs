using Newtonsoft.Json;
using System;

namespace BikewaleOpr.Entities
{
    public class AreaEntityBase
    {
        [JsonProperty("areaId"), DataMember]
        public UInt32 AreaId { get; set; }

        [JsonProperty("areaName"), DataMember]
        public string AreaName { get; set; }

        [JsonProperty("pinCode"), DataMember]
        public string PinCode { get; set; }

        [JsonProperty("longitude"), DataMember]
        public double Longitude { get; set; }

        [JsonProperty("latitude"), DataMember]
        public double Latitude { get; set; }

    }
}
