using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace BikewaleOpr.Entities
{
    [Serializable, DataContract]
    public class StateEntityBase
    {
        [JsonProperty("stateId"), DataMember]
        public uint StateId { get; set; }

        [JsonProperty("stateName"), DataMember]
        public string StateName { get; set; }

        [JsonProperty("stateMaskingName")]
        public string StateMaskingName { get; set; }
    }
}
