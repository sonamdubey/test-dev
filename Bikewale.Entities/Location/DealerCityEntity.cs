﻿
using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;
namespace Bikewale.Entities.Location
{
    /// <summary>
    /// Created By vivek gupta on 24 june 2016
    /// Desc : to add lat long dealer count
    /// </summary>
    [Serializable, DataContract]
    public class DealerCityEntity : CityEntityBase
    {
        [JsonProperty("latitude"), DataMember]
        public string Lattitude { get; set; }

        [JsonProperty("longitude"), DataMember]
        public string Longitude { get; set; }

        [JsonProperty("dealerCount"), DataMember]
        public uint DealersCount { get; set; }

        [JsonProperty("id"), DataMember]
        public uint Id { get; set; }

        [JsonProperty("link"), DataMember]
        public string Link { get; set; }

        [JsonProperty("cityname"), DataMember]
        public string CityName { get; set; }

        [JsonProperty("citymasking"), DataMember]
        public string CityMaskingName { get; set; }



    }
}
