using Bikewale.DTO.BikeBooking;
using Bikewale.Entities.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
