using Bikewale.DTO.BikeBooking;
using Bikewale.DTO.UserReviews.v2;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.UserReviews.Search
{
    public class SearchResult
    {
        [JsonProperty("result")]
        public IEnumerable<Review> Result { get; set; }
        [JsonProperty("pageUrl")]
        public PagingUrl PageUrl { get; set; }
        [JsonProperty("totalCount")]
        public uint TotalCount { get; set; }
        [JsonProperty("currentPageNo")]
        public int CurrentPageNo { get; set; }
    }
}
