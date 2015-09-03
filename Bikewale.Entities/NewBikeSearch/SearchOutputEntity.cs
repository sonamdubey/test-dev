using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Entities.Pager;

namespace Bikewale.Entities.NewBikeSearch
{
    public class SearchOutputEntity
    {
        public List<SearchOutputEntityBase> SearchResult { get; set; }
        public PagingUrl PageUrl { get; set; }
    }
}
