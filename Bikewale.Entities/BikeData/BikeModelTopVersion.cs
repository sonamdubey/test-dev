
using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.BikeData
{
    [Serializable, DataContract]
    public class BikeModelTopVersion : BikeModelEntityBase
    {
        [JsonProperty("topVersionId"), DataMember]
        public int TopVersionId{ get; set; }
    }
}
