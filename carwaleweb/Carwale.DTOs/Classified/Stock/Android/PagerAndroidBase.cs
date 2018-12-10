using Carwale.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Classified.Stock
{
    public class PagerAndroidBase
    {
        [JsonProperty("pageDetails")]
        public List<PagerUrlList> PagesDetail { get; set; }

        [JsonProperty("recordRange")]
        public string RecordRange { get; set; }

        [JsonProperty("recordCount")]
        public int RecordCount { get; set; }

        [JsonProperty("totalCount")]
        public int TotalCount { get; set; }
    }
}
