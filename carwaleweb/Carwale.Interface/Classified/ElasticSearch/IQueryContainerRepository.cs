using Carwale.Entity.Elastic;
using Nest;

namespace Carwale.Interfaces.Classified.ElasticSearch
{
    public interface IQueryContainerRepository<T> where T: class
    {
        QueryContainer GetCommonQueryContainerForSearchPage(ElasticOuptputs filterInputs, string carsWithPhotos, QueryContainerDescriptor<T> queryContainerDescriptor);
        QueryContainer GetCommonQueryContainerForSearchPage(ElasticOuptputs filterInputs, string carsWithPhotos, QueryContainerDescriptor<T> queryContainerDescriptor, bool isNearByCity);
    }
}
