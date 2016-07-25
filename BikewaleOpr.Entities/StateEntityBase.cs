using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BikewaleOpr.Entities
{
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
