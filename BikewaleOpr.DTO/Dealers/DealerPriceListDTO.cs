using Newtonsoft.Json;
using System.Collections.Generic;

namespace BikewaleOpr.DTO.Dealers
{
    /// <summary>
    /// Created By  :   Vishnu Teja Yalakuntla on 02 Aug 2017
    /// Description :   Holds multiple versions and their prices of a dealer.
    /// </summary>
    public class DealerPriceListDTO
    {
        [JsonProperty("dealerId")]
        public IEnumerable<uint> DealerIds { get; set; }
        [JsonProperty("cityId")]
        public IEnumerable<uint> CityIds { get; set; }
        [JsonProperty("versionIds")]
        public IEnumerable<uint> VersionIds { get; set; }
        [JsonProperty("itemIds")]
        public IEnumerable<uint> ItemIds { get; set; }
        [JsonProperty("itemValues")]
        public IEnumerable<uint> ItemValues { get; set; }
        [JsonProperty("enteredBy")]
        public uint EnteredBy { get; set; }
        [JsonProperty("bikeModelIds")]
        public IEnumerable<uint> BikeModelIds { get; set; }
    }
}
