
using Bikewale.Entities.NewBikeSearch;
namespace Bikewale.Interfaces.NewBikeSearch
{
    public interface IBikeSearch
    {
        BikeSearchOutputEntity GetBikeSearch(SearchFilters filters);
    }
}
