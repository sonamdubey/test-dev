using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Bikewale.DTO.AutoComplete
{
    [Serializable,DataContract]
    public class Payload
    {
        [JsonProperty("makeId"), DataMember]
        public int MakeId { get; set; }

        [JsonProperty("modelId"), DataMember]
        public int ModelId { get; set; }
    }
}
