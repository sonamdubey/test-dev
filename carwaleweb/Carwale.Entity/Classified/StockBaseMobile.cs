using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Classified
{
    public class StockBaseMobile
    {
        public List<StockBaseEntity> ListStocks { get; set; }
        public int LastNonFeaturedRank { get; set; }
        public int LastDealerFeaturedRank { get; set; }
        public int LastIndividualFeaturedRank { get; set; }
        public int NearbyCityId { get; set; }
        public string NearbyCityIds { get; set; }
        public string NearbyCityIdsStockCount { get; set; }
        public bool IsAllStocksFetched { get; set; }
        public int TotalStockCount { get; set; }

        public StockBaseMobile()
        {
            ListStocks = new List<StockBaseEntity>();
        }
    }
}
