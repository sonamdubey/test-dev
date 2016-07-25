using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BikewaleOpr.Entities
{
    public class CityEntityBase
    {
        [JsonProperty("cityId"), DataMember]
        public uint CityId { get; set; }

        [JsonProperty("cityName"), DataMember]
        public string CityName { get; set; }

        [JsonProperty("cityMaskingName"), DataMember]
        public string CityMaskingName { get; set; }

        [JsonProperty("isPopular"), DataMember]
        public bool IsPopular { get; set; }

        [JsonProperty("hasAreas"), DataMember]
        public bool HasAreas { get; set; }
    }
}
