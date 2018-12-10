using Carwale.Entity.Classified;
using Carwale.Entity.Elastic;
using System.Collections.Generic;

namespace Carwale.Interfaces.Classified.ElasticSearch
{
    public interface IESStockQueryRepository
    {
        IEnumerable<StockBaseEntity> GetFrachiseCars(string[] cities, int size);
        StockForFilter GetStocksForSearchResults(ElasticOuptputs filterInputs);
        string GetDetailsPageUrlByRegistrationNumber(string regNo);
        int GetTotalStockCount(ElasticOuptputs filterInputs);
    }
}
