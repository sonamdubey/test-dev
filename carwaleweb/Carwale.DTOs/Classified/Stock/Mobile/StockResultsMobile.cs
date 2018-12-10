using Carwale.Entity.Classified;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Classified.Stock
{
    public class StockResultsMobile
    {
        public IList<StockBaseEntity> ResultData { get; set; }
        public Carwale.Entity.Classified.City PrimaryCity { get; set; }
        public int LastNonFeaturedSlotRank { get; set; }
        public int LastDealerFeaturedSlotRank { get; set; }
        public int LastIndividualFeaturedSlotRank { get; set; }
        public int NearbyCityId { get; set; }
        public string NearbyCityIds { get; set; }
        public string NearbyCityIdsStockCount { get; set; }
        public bool IsAllStocksFetched { get; set; }
        public int TotalStockCount { get; set; }
        public string NextPageUrl { get; set; }
    }
}
