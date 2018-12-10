using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Classified.Stock
{
    public class ResultsPagerAndroid
    {
        [JsonProperty("stockResults")]
        public List<StockResultsAndroidBase> ResultsData { get; set; }

        [JsonProperty("sortCriteria")]
        public List<SortCriteriaAndroid> SortCriteria { get; set; }

        [JsonProperty("pagerData")]
        public PagerAndroidBase PagerData { get; set; }

    }
}
