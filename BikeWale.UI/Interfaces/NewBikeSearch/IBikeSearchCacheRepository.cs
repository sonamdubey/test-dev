
using Bikewale.Entities.NewBikeSearch;
namespace Bikewale.Interfaces.NewBikeSearch
{
    /// <summary>
    /// Created by  :   Sumit Kate on 05 Jan 2018
    /// Description :   Bike Search Cache Repo interface
    /// </summary>
    public interface IBikeSearchCacheRepository
    {
        BudgetFilterRanges GetBudgetRanges();
    }
}
