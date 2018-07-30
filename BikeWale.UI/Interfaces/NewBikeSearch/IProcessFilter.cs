using Bikewale.Entities.NewBikeSearch;

namespace Bikewale.Interfaces.NewBikeSearch
{
    public interface IProcessFilter
    {
        FilterInput ProcessFilters(InputBaseEntity objInput);
    }
}
