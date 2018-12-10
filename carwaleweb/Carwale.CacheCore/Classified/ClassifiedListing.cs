using AEPLCore.Cache;
using Carwale.Entity.Classified;
using Carwale.Interfaces.Classified;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Cache.Classified
{
    public class ClassifiedListing : IClassifiedListing
    {
        protected readonly IStockRepository _stockRepo;

        public ClassifiedListing(IStockRepository stockRepo)
        {
            _stockRepo = stockRepo;
        }

        public List<StockSummary> GetSimilarUsedModels(int modelId)
        {
            var data = new CacheManager();
            return data.GetFromCache<List<StockSummary>>("UsedCarSuggestions-" + modelId, CacheRefreshTime.OneDayExpire(), () => _stockRepo.GetSimilarUsedModels(modelId));
        }

        public List<StockSummary> GetLuxuryCarRecommendations(int carId, int dealerId, int pageId)
        {
            var data = new CacheManager();
            return data.GetFromCache<List<StockSummary>>("LuxuryCarRecommendations-" + carId + "-" + dealerId + "-" + pageId, CacheRefreshTime.OneDayExpire(), () => _stockRepo.GetLuxuryCarRecommendations(carId, dealerId, pageId));
        }
    }
}
