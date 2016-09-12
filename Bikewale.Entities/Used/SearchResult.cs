using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Entities.NewBikeSearch;

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
        public uint TotalCount { get; set; }
        public int CurrentPageNo { get; set; }
    }
}
