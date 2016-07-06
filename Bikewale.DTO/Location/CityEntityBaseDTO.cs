using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace Bikewale.DTO.Location
{
    /// <summary>
    /// Modified By : Vivek Gupta on 06-07-2016
    /// Summary : created dtofor the response of api
    /// </summary>
    [Serializable, DataContract]
    public class CityEntityBaseDTO
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
