using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Bikewale.Entities.Location
{
    /// <summary>
    /// Modified By : Ashwini Todkar on 24th Oct 2014
    /// Summary : added serializable attribute and json properties
    /// </summary>
    [Serializable, DataContract]
    public class CityEntityBase
    {
        [JsonProperty("cityId"), DataMember]
        public uint CityId { get; set; }

        [JsonProperty("cityName"), DataMember]
        public string CityName { get; set; }

        [JsonProperty("cityMaskingName"), DataMember]
        public string CityMaskingName { get; set; }
    }
}
