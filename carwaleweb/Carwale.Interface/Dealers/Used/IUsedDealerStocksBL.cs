using Carwale.Entity.Classified;
using Carwale.Entity.Dealers.Used;
using System.Collections.Generic;

namespace Carwale.Interfaces.Dealers.Used
{
    public interface IUsedDealerStocksBL
    {
        DealerStocksEntity GetDealerStocksEntity(int dealerId, int from, int size);
        Dictionary<string, string> ValidateDealerStocksApiInputs(int dealerId, int from, int size);
        IEnumerable<StockBaseEntity> GetDealerStocksForVirtualPage(int dealerId);
    }
}
