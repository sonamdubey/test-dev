using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace BikeWaleOpr.Entities
{
    /// <summary>
    /// Created By : Sadhana Upadhyay on 24th Oct 2014
    /// Summary : Entity for State
    /// </summary>
    [Serializable]
    public class StateEntityBase
    {
        [JsonProperty("stateId")]
        public uint StateId { get; set; }

        [JsonProperty("stateName")]
        public string StateName { get; set; }
    }
}
