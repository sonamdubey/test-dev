using Bikewale.Entities.NewBikeSearch;
using System.Collections.Generic;

namespace Bikewale.Entities.Used.Search
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 11 Sept 2016
    /// Summary : Class to hold the data for the used bikes search result
    /// </summary>
    public class SearchResult
    {
        public IEnumerable<UsedBikeBase> Result { get; set; }

        public PagingUrl PageUrl { get; set; }
        public int TotalCount { get; set; }
        public int CurrentPageNo { get; set; }
    }
}
