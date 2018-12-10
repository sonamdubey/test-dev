using Carwale.Entity.Stock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.Stock
{
    public interface IStockConditionCacheRepository
    {
        List<StockConditionItems> GetCarConditionParts();
    }
}
