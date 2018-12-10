using Carwale.Entity.Classified;
using Carwale.Entity.Stock;
using System.Collections.Generic;

namespace Carwale.Interfaces.Stock
{
    public interface IStockRecommendationsBL
    {
        List<StockBaseEntity> GetRecommendations(StockRecoParams stockRecoParams, int requestSource);
        List<StockBaseEntity> GetRecommendations(string profileId);
    }
}
