using Carwale.Entity.Classified;
using System.Collections.Generic;

namespace Carwale.Entity.Stock.Search
{
    public class SearchResultMobile : SearchResultBase
    {
        public SearchResultMobile()
        {

        }
        public SearchResultMobile(List<StockBaseEntity> resultData) : base(resultData)
        {

        }
    }
}
