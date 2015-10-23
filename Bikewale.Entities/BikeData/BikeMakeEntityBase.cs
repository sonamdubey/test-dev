using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Modified By : Lucky Rathore
    /// Summary : HostUrl and LogoUrl Added
    /// </summary>
    [Serializable, DataContract]
    public class BikeMakeEntityBase
    {
        [JsonProperty(PropertyName = "makeId"), DataMember]        
        public int MakeId { get; set; }

        [JsonProperty(PropertyName = "makeName"), DataMember]
        public string MakeName { get; set; }

        [JsonProperty(PropertyName = "maskingName"), DataMember]
        public string MaskingName { get; set; }
        
        [JsonProperty(PropertyName = "hostUrl"), DataMember]
        public string HostUrl { get; set; }
        
        [JsonProperty(PropertyName = "logoUrl"), DataMember]
        public string LogoUrl { get; set; }        
    }
}
