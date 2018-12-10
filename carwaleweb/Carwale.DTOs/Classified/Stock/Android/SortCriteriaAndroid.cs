using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Classified.Stock
{
    public class SortCriteriaAndroid
    {
        [JsonProperty("sortText")]
        public string SortText { get; set; }

        [JsonProperty("sortFor")]
        public string SortFor { get; set; }

        [JsonProperty("sortOrder")]
        public string SortOrder { get; set; }

        [JsonProperty("sortUrl")]
        public string SortUrl { get; set; }
    }
}
