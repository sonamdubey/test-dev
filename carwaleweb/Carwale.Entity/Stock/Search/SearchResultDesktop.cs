using Carwale.Entity.Classified;
using System.Collections.Generic;

namespace Carwale.Entity.Stock.Search
{
    public class SearchResultDesktop : SearchResultBase
    {
        public SearchResultDesktop()
        {
            NearByCitiesWithCount = new List<City>();
        }

        public SearchResultDesktop(List<StockBaseEntity> resultData) : base(resultData)
        {

        }
        public CountData FiltersData { get; set; }
        public List<City> NearByCitiesWithCount { get; private set; }
        public string ExcludeStocks { get; set; }

    }
}
