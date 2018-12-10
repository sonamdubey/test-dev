using Carwale.Entity.Classified;
using Carwale.Entity.Elastic;
using Nest;

namespace Carwale.Interfaces.Classified.ElasticSearch
{
    public interface ISortDescriptorRepository
    {
        SortDescriptor<StockBaseEntity> GetSortDescriptorForNonFeaturedStocks(ElasticOuptputs filterInputs, string sortField, SortDescriptor<StockBaseEntity> sortDescriptor);
    }
}
