using Bikewale.Entities.PriceQuote;
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

        public PQSourceEnum PqSource { get; set; }

        public ICollection<SearchBudgetLink> BudgetLinks { get; set; }
    }

    public class SearchBudgetLink
    {
        public Tuple<String, String, String, uint> Link { get; set; }
    }
}
