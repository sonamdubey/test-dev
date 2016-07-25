using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.Location
{
    /// <summary>
    /// Modified By : Ashwini Todkar on 24th Oct 2014
    /// Summary : added serializable attribute and json properties
    /// Modified By :   Sumit Kate on 12 Jan 2016
    /// Summary     :   Added new property HasAreas
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

        [JsonProperty("isPopular"), DataMember]
        public bool IsPopular { get; set; }

        [JsonProperty("hasAreas"), DataMember]
        public bool HasAreas { get; set; }
    }
}
