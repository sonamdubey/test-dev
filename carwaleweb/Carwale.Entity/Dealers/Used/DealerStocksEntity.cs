using Carwale.Entity.Classified;
using System.Collections.Generic;

namespace Carwale.Entity.Dealers.Used
{
    public class DealerStocksEntity
    {
        public IEnumerable<StockBaseEntity> Stocks { get; set; }
        public string NextPageUrl { get; set; }
    }
}
