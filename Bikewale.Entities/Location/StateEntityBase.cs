using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.Location
{
    /// <summary>
    /// Modified By : Ashwini Todkar on 24th Oct 2014
    /// Summary : added serializable attribute and json properties
    /// </summary>
    [Serializable,DataContract]
    public class StateEntityBase
    {
        [JsonProperty("stateId"),DataMember]
        public uint StateId { get; set; }

        [JsonProperty("stateName"), DataMember]
        public string StateName { get; set; }

        [JsonProperty("stateMaskingName")]
        public string StateMaskingName { get; set; }
    }
}
