using System;
using System.Collections.Generic;
using Bikewale.Entities.PriceQuote;

namespace Bikewale.Entities.NewBikeSearch
{
    [Serializable]
    public class SearchOutputEntity
    {
        public List<SearchOutputEntityBase> SearchResult { get; set; }
        public PagingUrl PageUrl { get; set; }
        public int TotalCount { get; set; }
        public int CurrentPageNo { get; set; }

        public PQSourceEnum PqSource { get; set; }
    }
}
