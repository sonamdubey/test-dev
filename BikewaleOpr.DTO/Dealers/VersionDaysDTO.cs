using Newtonsoft.Json;
using System.Collections.Generic;

namespace BikewaleOpr.DTO.Dealers
{
    /// <summary>
    /// Created By  :   Vishnu Teja Yalakuntla on 11 Aug 2017
    /// Description :   Entity holds dealer versions along with their availabilities
    /// </summary>
    public class VersionDaysDTO
    {
        [JsonProperty("dealerId")]
        public uint DealerId { get; set; }
        [JsonProperty("bikeVersionIds")]
        public IEnumerable<uint> BikeVersionIds { get; set; }
        [JsonProperty("numberOfDays")]
        public IEnumerable<uint> NumberOfDays { get; set; }
    }
}
