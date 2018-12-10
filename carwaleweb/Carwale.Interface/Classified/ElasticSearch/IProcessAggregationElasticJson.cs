using Carwale.Entity.Classified;
using Nest;

namespace Carwale.Interfaces.Classified.ElasticSearch
{
    public interface IProcessAggregationElasticJson
    {
        SellerType GetSellerTypeCount(ISearchResponse<StockBaseEntity> searchResponse);
        CountData GetAllFilterCount(ISearchResponse<StockBaseEntity> searchResponse);
    }
}
