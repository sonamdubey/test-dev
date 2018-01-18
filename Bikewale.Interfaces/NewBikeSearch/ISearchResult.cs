using Bikewale.Entities.NewBikeSearch;

namespace Bikewale.Interfaces.NewBikeSearch
{
    public interface ISearchResult
    {
        SearchOutputEntity GetSearchResult(FilterInput filterInputs, InputBaseEntity input);
        BudgetFilterRanges GetBudgetRanges();
    }
}
