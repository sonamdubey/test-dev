using Carwale.Entity.Elastic;
using Carwale.Entity.Stock.Search;

namespace Carwale.Interfaces.Stock
{
    public interface IStockManager
    {
        SearchResultBase GetStocks(ElasticOuptputs filterInputs);
    }
}
