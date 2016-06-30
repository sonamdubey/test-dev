using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.Location
{
    /// <summary>
    /// Modified By : Ashwini Todkar on 24th Oct 2014
    /// Summary : added serializable attribute and json properties
    /// </summary>
    [Serializable, DataContract]
    public class StateEntityBase
    {
        [JsonProperty("stateId"), DataMember]
        public uint StateId { get; set; }

        [JsonProperty("stateName"), DataMember]
        public string StateName { get; set; }

        [JsonProperty("stateMaskingName"), DataMember]
        public string StateMaskingName { get; set; }
    }
}
