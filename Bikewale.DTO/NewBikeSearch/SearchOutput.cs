using Newtonsoft.Json;
using System;
using System.Collections.Generic;

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
        [JsonProperty("pqSource")]
        public int PqSource { get; set; }

        [JsonIgnore]
        public IEnumerable<Tuple<String, String, uint>> BudgetLinks { get; set; }
    }
}
