using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace Bikewale.DTO.BikeData
{
    [Serializable, DataContract]
    public class BikeDiscription
    {
        [JsonProperty(PropertyName = "smallDisc"), DataMember]
        public string SmallDescription { get; set; }

        [JsonProperty(PropertyName = "fullDisc"), DataMember]
        public string FullDescription { get; set; }
    }
}
