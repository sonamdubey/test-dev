using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

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
