using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CarData
{
    [Serializable]
    public class CarPrice
    {
        [JsonProperty("minPrice")]
        public double MinPrice { get; set; }
        [JsonProperty("maxPrice")]
        public double MaxPrice { get; set; }
        [JsonProperty("avgPrice")]
        public int AvgPrice { get; set; }
    }
}
