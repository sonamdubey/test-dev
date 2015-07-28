using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Bikewale.Entities.BikeData
{
    [Serializable, DataContract]
    public class BikeMakeEntityBase
    {
        [DataMember]
        [JsonProperty(PropertyName = "makeId")]
        public int MakeId { get; set; }

        [DataMember]
        [JsonProperty(PropertyName = "makeName")]
        public string MakeName { get; set; }

        [DataMember]
        [JsonProperty(PropertyName = "maskingName")]
        public string MaskingName { get; set; }
    }
}
