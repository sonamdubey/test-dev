using Carwale.Entity.Classified;
using Carwale.Entity.Elastic;

namespace Carwale.Interfaces.Classified.ElasticSearch
{
    public interface IAggregationsRepository
    {
        SellerType GetSellerTypeCount(FilterInputs filterInputs);
        CountData GetAllFilterCount(ElasticOuptputs elasticInputs);
    }
}
