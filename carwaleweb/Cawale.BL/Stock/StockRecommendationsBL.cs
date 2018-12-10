using AutoMapper;
using Carwale.BL.Tracking;
using Carwale.Entity.Classified;
using Carwale.Entity.Stock;
using Carwale.Interfaces.Elastic;
using Carwale.Interfaces.Stock;
using Carwale.Utility;
using Carwale.Utility.Classified;
using System.Collections.Generic;
using System.Linq;

namespace Carwale.BL.Stock
{
    public class StockRecommendationsBL : IStockRecommendationsBL
    {
        private readonly IElasticSearchManager _searchManager;
        private readonly IStockRecommendationRepository _stockRecommendationRepository;
        private readonly BhriguTracker _bhrigutrack;

        public StockRecommendationsBL(IElasticSearchManager searchManager, IStockRecommendationRepository stockRecommendationRepository, BhriguTracker bhrigutrack)
        {
            _searchManager = searchManager;
            _stockRecommendationRepository = stockRecommendationRepository;
            _bhrigutrack = bhrigutrack;
        }

        public List<StockBaseEntity> GetRecommendations(StockRecoParams stockRecoParams, int requestSource)
        {
            var recommendations = _stockRecommendationRepository.GetRecommendations(Mapper.Map<StockRecoParams, StockRecoParamsData>(stockRecoParams)).ToList();
            int count = recommendations == null ? 0 : recommendations.Count;
            TrackRecommendationsCount(count);
            return recommendations;
        }

        public List<StockBaseEntity> GetRecommendations(string profileId)
        {
            return _searchManager.SearchIndexProfileRecommendation<List<StockBaseEntity>, string>(profileId, Constants.AppRecommendationCount);
        }

        public static string GetSimilarCarsUrl(string profileId, int rootId, int cityId, int deliveryCity, int price, int versionSubSegmentId)//for view response
        {
            return string.Format("/m/search/getsimilarcars/?profileId={0}&rootid={1}&cityid={2}&price={3}&recommendationsCount={4}&versionSubSegmentId={5}", 
                profileId, 
                rootId, 
                (deliveryCity > 0 ? deliveryCity : cityId), 
                price, 
                Constants.WebAndMobileRecommendationCount, 
                versionSubSegmentId
            );
        }

        public static string GetStockRecommendationsUrl(string profileId, int rootId, int cityId, int deliveryCity, int price, int versionSubSegmentId)//for JSON response
        {
            return string.Format("/api/stockrecommendations/?profileId={0}&rootid={1}&cityid={2}&price={3}&recommendationsCount={4}&versionSubSegmentId={5}", 
                profileId, 
                rootId, 
                (deliveryCity > 0 ? deliveryCity : cityId), 
                price, 
                Constants.WebAndMobileRecommendationCount,
                versionSubSegmentId
            );
        }

        public static int GetMinPriceForRecommendations(int price)
        {
            return (int)(price - (price * Constants.PriceInterval * Constants.IterationCount));
        }

        public static int GetMaxPriceForRecommendations(int price)
        {
            return (int)(price + (price * Constants.PriceInterval * Constants.IterationCount));
        }

        private void TrackRecommendationsCount(int count)
        {
            var label = new Dictionary<string, string>()
                        {
                            {"RecommendationCount" , count.ToString()}
                        };
            _bhrigutrack.Track(Constants.TrackingCatForRecommendations, Constants.TrackingActionForRecommendations, label);
        }
    }
}
