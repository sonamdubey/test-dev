using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Bikewale.DTO.NewBikeSearch
{
    public class SearchOutput
    {
        [JsonProperty("searchResult")]
        public List<SearchOutputBase> SearchResult { get; set; }

        [JsonProperty("pageUrl")]
        public Pager PageUrl { get; set; }

        [JsonProperty("totalCount")]
        public int TotalCount { get; set; }

        [JsonProperty("curPageNo")]
        public int CurrentPageNo { get; set; }
    }
}
