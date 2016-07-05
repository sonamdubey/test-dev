using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using BikewaleOpr.Entities;

namespace BikewaleOpr.Entities
{
    /// <summary>
    /// Modified By : Lucky Rathore
    /// Summary : HostUrl and LogoUrl Added
    /// Modified by :   Sumit Kate on 03 Mar 2016
    /// Description :   Added PopularityIndex
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

        [JsonProperty("popularityIndex"), DataMember]
        public ushort PopularityIndex { get; set; }
    }
}
