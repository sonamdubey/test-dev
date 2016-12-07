
using Bikewale.Entities.Location;
using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;
namespace Bikewale.Entities.service
{
    [Serializable, DataContract]
    public class ServiceCityEntity : CityEntityBase
    {
        [JsonProperty("latitude"), DataMember]
        public string Lattitude { get; set; }

        [JsonProperty("longitude"), DataMember]
        public string Longitude { get; set; }

        [JsonProperty("dealerCount"), DataMember]
        public uint ServiceCenterCountCity { get; set; }

        [JsonProperty("id"), DataMember]
        public uint Id { get; set; }

        [JsonProperty("link"), DataMember]
        public string Link { get; set; }

        [JsonProperty("cityName"), DataMember]
        public string CityName { get; set; }

        [JsonProperty("citymasking"), DataMember]
        public string CityMaskingName { get; set; }

        [JsonProperty("stateId"), DataMember]
        public uint stateId { get; set; }
    }
}
