using Newtonsoft.Json;
using System.Collections.Generic;

namespace BikewaleOpr.DTO.Dealers
{
    /// <summary>
    /// Created By  :   Vishnu Teja Yalakuntla on 11 Aug 2017
    /// Description :   Entity holding dealerId, cityId and list of version Ids
    /// </summary>
    public class DealerCityVersionsDTO
    {
        [JsonProperty("dealerId")]
        public uint DealerId { get; set; }
        [JsonProperty("cityId")]
        public uint CityId { get; set; }
        [JsonProperty("bikeVersionIds")]
        public IEnumerable<uint> BikeVersionIds { get; set; }
        [JsonProperty("bikeModelIds")]
        public IEnumerable<uint> BikeModelIds { get; set; }
    }
}
