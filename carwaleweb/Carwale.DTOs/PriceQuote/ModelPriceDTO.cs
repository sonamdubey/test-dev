using Carwale.Entity.CarData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.PriceQuote
{
    public class ModelPriceDTO
    {
        [JsonProperty("minPrice")]
        public int MinPrice { get; set; }
        [JsonProperty("maxPrice")]
        public int MaxPrice { get; set; }
        [JsonProperty("versions")]
        public IEnumerable<VersionPricesDTO> Versions { get; set; }  
    }
}
