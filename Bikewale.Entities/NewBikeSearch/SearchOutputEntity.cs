using System;
using System.Collections.Generic;

namespace Bikewale.Entities.NewBikeSearch
{
    [Serializable]
    public class SearchOutputEntity
    {
        public List<SearchOutputEntityBase> SearchResult { get; set; }
        public PagingUrl PageUrl { get; set; }
        public int TotalCount { get; set; }
        public int CurrentPageNo { get; set; }
    }
}
