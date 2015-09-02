using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace BikeWaleOpr.Entities
{
    /// <summary>
    /// Created By : Sadhana Upadhyay on 24th Oct 2014
    /// Summary : Entity for State
    /// </summary>
    [Serializable,DataContract]
    public class StateEntityBase
    {
        [JsonProperty("stateId"),DataMember]
        public uint StateId { get; set; }

        [JsonProperty("stateName"),DataMember]
        public string StateName { get; set; }
    }
}
