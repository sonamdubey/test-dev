using Carwale.Entity.Classified;
using Carwale.Entity.Stock;
using System.Collections.Generic;

namespace Carwale.Interfaces.Stock
{
    public interface IStockRecommendationRepository
    {
        IEnumerable<StockBaseEntity> GetRecommendations(StockRecoParamsData stockRecoParams);
    }
}
