using Bikewale.DTO.BikeBooking;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.Used.Search
{
    public class SearchResult
    {
        [JsonProperty("result")]
        public IEnumerable<UsedBikeBase> Result { get; set; }
        [JsonProperty("pageUrl")]
        public PagingUrl PageUrl { get; set; }
        [JsonProperty("totalCount")]
        public uint TotalCount { get; set; }
        [JsonProperty("currentPageNo")]
        public int CurrentPageNo { get; set; }
    }
}
