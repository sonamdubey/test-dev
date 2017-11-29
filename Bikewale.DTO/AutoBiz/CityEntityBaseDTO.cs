using Newtonsoft.Json;
using System;

namespace BikeWale.DTO.AutoBiz
{
    /// <summary>
    /// Created By : Sadhana Upadhyay on 24th Oct 2014
    /// Summary : Entity for City
    /// </summary>
    public class CityEntityBaseDTO
    {
        [JsonProperty("cityId")]
        public UInt32 CityId { get; set; }

        [JsonProperty("cityName")]
        public string CityName { get; set; }

        [JsonProperty("cityMaskingName")]
        public string CityMaskingName { get; set; }
    }
}
