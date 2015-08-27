using System.Runtime.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Bikewale.DTO.Make
{
    [Serializable, DataContract]
    public class MakeBase
    {
        [JsonProperty(PropertyName = "makeId"), DataMember]
        public int MakeId { get; set; }

        [JsonProperty(PropertyName = "makeName"), DataMember]
        public string MakeName { get; set; }

        [JsonProperty(PropertyName = "maskingName"), DataMember]
        public string MaskingName { get; set; }
    }
}
