using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace BikeWaleOpr.Entities
{
    /// <summary>
    /// Created By : Sadhana Upadhyay on 24th Oct 2014
    /// Summary : Entity for City
    /// </summary>
    [Serializable, DataContract]
    public class CityEntityBase
    {
        [JsonProperty("cityId"), DataMember]
        public UInt32 CityId { get; set; }

        [JsonProperty("cityName"), DataMember]
        public string CityName { get; set; }

        [JsonProperty("cityMaskingName"), DataMember]
        public string CityMaskingName { get; set; }
    }
}
