using Bikewale.DTO.NewBikeSearch;
using Bikewale.Entities.NewBikeSearch;

namespace Bikewale.Interfaces.NewBikeSearch
{
    public interface IBikeSearchResult
    {
        SearchOutput GetSearchResult(FilterInput filterInputs, InputBaseEntity input);
    }
}
