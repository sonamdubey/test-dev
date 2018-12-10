using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Classified.Stock
{
    public class ResultsFiltersPagerAndroid
    {
        [JsonProperty("stockResults")]
        public List<StockResultsAndroidBase> ResultsData { get; set; }

        [JsonProperty("pagerData")]
        public PagerAndroidBase PagerData { get; set; }

        [JsonProperty("filtersData")]
        public FiltersCountAndroidBase FiltersData { get; set; }
    }
}
