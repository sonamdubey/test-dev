using Carwale.Entity.Stock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.Stock
{
    public interface IStockScoreRepository
    {
        StockSortScore GetStockScore(string profileId);
        bool UpdateStockScore(string profileId, StockSortScore stockSortScore);
    }
}
