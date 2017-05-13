using Bikewale.Entities.NewBikeSearch;
using System;
using System.Collections.Generic;

namespace Bikewale.Entities.UserReviews.Search
{
    [Serializable]
    public class SearchResult
    {
        public IEnumerable<ReviewEntity> Result { get; set; }
        public PagingUrl PageUrl { get; set; }
        public int TotalCount { get; set; }
        public int CurrentPageNo { get; set; }
    }
}
