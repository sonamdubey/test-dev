using Newtonsoft.Json;
using System.Collections.Generic;

namespace BikewaleOpr.DTO.Dealers
{
    /// <summary>
    /// Created By  :   Vishnu Teja Yalakuntla on 11 Aug 2017
    /// Description :   Entity holds dealer version prices and their availabilities
    /// </summary>
    public class DealerVersionPricesAvailabilityDTO
    {
        [JsonProperty("dealerVersionPrices")]
        public DealerPriceListDTO DealerVersionPrices { get; set; }
        [JsonProperty("dealerVersionAvailabilities")]
        public VersionDaysDTO DealerVersionAvailabilities { get; set; }
        [JsonProperty("makeId")]
        public uint MakeId { get; set; }
        [JsonProperty("bikeModelIds")]
        public IEnumerable<uint> BikeModelIds { get; set; }
        [JsonProperty("bikeModelNames")]
        public IEnumerable<string> BikeModelNames { get; set; }
    }
}
