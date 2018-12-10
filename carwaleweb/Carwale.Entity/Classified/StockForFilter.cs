using Carwale.Entity.Enum;
using System.Collections.Generic;

namespace Carwale.Entity.Classified
{
    public class StockForFilter
    {
        public StockForFilter(IDictionary<CwBasePackageId, List<StockBaseEntity>> stockPackageMap)
        {
            StockPackageMap = stockPackageMap;
        }
        public StockForFilter()
        {

        }
        public IDictionary<CwBasePackageId, List<StockBaseEntity>> StockPackageMap { get; private set; }
        public int Count { get; set; }
    }
}
