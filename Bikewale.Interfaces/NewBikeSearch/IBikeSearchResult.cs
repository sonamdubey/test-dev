using Bikewale.DTO.NewBikeSearch;
using Bikewale.Entities.NewBikeSearch;

namespace Bikewale.Interfaces.NewBikeSearch
{
    /// <summary>
    /// Created by : Vivek Singh Tomar on 16th Nov 2017
    /// Summary : Interface for Bike Search Result
    /// </summary>
    public interface IBikeSearchResult
    {
        SearchOutput GetSearchResult(FilterInput filterInputs, InputBaseEntity input);
    }
}
