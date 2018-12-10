﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Classified.Stock
{
    public class StockResultsAndroid
    {
        [JsonProperty("stockResults")]
        public List<StockResultsAndroidBase> StockResults { get; set; }

        [JsonProperty("sortCriteria")]
        public List<SortCriteriaAndroid> SortCriteria { get; set; }

        [JsonProperty("nextPageUrl")]
        public string NextPageUrl { get; set; }
    }
}
