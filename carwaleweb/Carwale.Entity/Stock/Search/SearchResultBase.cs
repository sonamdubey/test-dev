using Carwale.Entity.Classified;
using System.Collections.Generic;

namespace Carwale.Entity.Stock.Search
{
    public class SearchResultBase
    {
        public SearchResultBase()
        {
            ResultData = new List<StockBaseEntity>();
        }
        public SearchResultBase(List<StockBaseEntity> resultData)
        {
            ResultData = resultData;
        }
        //Sonar qube: S4004 - make collection read only for avoiding replacing collection from client
        public List<StockBaseEntity> ResultData { get; private set; }
        public City PrimaryCity { get; set; }
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
