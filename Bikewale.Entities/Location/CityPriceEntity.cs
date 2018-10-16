
using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.Location
{
    [Serializable, DataContract]
    public class CityPriceEntity
    {
        [JsonProperty("cityId"), DataMember]
        public uint CityId { get; set; }

        [JsonProperty("cityName"), DataMember]
        public string CityName { get; set; }

        [JsonProperty("cityMaskingName"), DataMember]
        public string CityMaskingName { get; set; }

        [JsonProperty("OnRoadPrice"), DataMember]
        public uint OnRoadPrice { get; set; }

        [JsonProperty("latitude"), DataMember]
        public float Latitude { get; set; }

        [JsonProperty("longitude"), DataMember]
        public float Longitude { get; set; }
    }
}
