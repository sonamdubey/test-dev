using Bikewale.Entities.Used.Search;

namespace Bikewale.Interfaces.Used.Search
{
    /// <summary>
    /// Created by : Ashish G. Kamble on 11 sept 2016
    /// </summary>
    public interface ISearchFilters
    {
        ProcessedInputFilters ProcessFilters(InputFilters objFilters);
    }
}
