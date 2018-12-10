using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Classified
{
    public class CountDataMobile
    {
        [JsonProperty("stockcount")]
        public StockCount StockCount { get; set; }

        [JsonProperty("city")]
        public List<City> CityCount { get; set; }
    }
}
