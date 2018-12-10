using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Classified.Stock.Ios
{
    public class StockResultIos
    {
        [JsonProperty("usedCars")]
        public List<StockResultsIosBase> StockResults { get; set; }

        [JsonProperty("sortCriterias")]
        public List<SortCriteriaIos> SortCriteria { get; set; }

        [JsonProperty("nextPageUrl")]
        public string NextPageUrl { get; set; }
    }
}
