using Carwale.Entity.Classified;
using Nest;

namespace Carwale.Interfaces.Dealers.Used
{
    public interface IUsedDealerStocksRepository
    {
        ISearchResponse<StockBaseEntity> GetDealerFranchiseStocks(int dealerId, int from, int size);
    }
}
