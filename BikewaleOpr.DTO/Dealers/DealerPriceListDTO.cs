using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.DTO.Dealers
{
    /// <summary>
    /// Created By  :   Vishnu Teja Yalakuntla on 02 Aug 2017
    /// Description :   Holds multiple versions and their prices of a dealer.
    /// </summary>
    public class DealerPriceListDTO
    {
        [JsonProperty("dealerId")]
        public uint DealerId { get; set; }
        [JsonProperty("cityId")]
        public uint CityId { get; set; }
        public IEnumerable<uint> VersionIds { get; set; }
        public IEnumerable<uint> ItemIds { get; set; }
        public IEnumerable<uint> ItemValues { get; set; }
        [JsonProperty("enteredBy")]
        public uint EnteredBy { get; set; }
    }
}
