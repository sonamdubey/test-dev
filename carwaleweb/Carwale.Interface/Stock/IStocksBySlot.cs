using Carwale.Entity.Classified;
using Carwale.Entity.Enum;
using System.Collections.Generic;

namespace Carwale.Interfaces.Stock
{
    public interface IStocksBySlot
    {
        List<StockBaseEntity> GetStocksAccordingToSlot(IDictionary<CwBasePackageId, List<StockBaseEntity>> mappingOfPackagesAndStocks, IEnumerable<Slot> slots);
    }
}
