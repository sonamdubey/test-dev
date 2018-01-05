using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.NewBikeSearch;
using Bikewale.Notifications;
using System;

namespace Bikewale.Cache.BikeSearch
{
    /// <summary>
    /// Created by  :   Sumit Kate on 05 Jan 2018
    /// Description :   Bike Search Cache Repository
    /// </summary>
    public class BikeSearchCacheRepository : IBikeSearchCacheRepository
    {
        private readonly ICacheManager _cache;
        private readonly ISearchResult _searchResult;
        public BikeSearchCacheRepository(ICacheManager cache, ISearchResult searchResult)
        {
            _cache = cache;
            _searchResult = searchResult;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 05 Jan 2018
        /// Description :   Returns the Budget ranges from Cache
        /// </summary>
        /// <returns></returns>
        public Entities.NewBikeSearch.BudgetFilterRanges GetBudgetRanges()
        {
            Entities.NewBikeSearch.BudgetFilterRanges budgets = null;
            string key = "BW_BudgetRanges";
            try
            {
                budgets = _cache.GetFromCache<Entities.NewBikeSearch.BudgetFilterRanges>(key, new TimeSpan(1, 0, 0, 0), () => _searchResult.GetBudgetRanges());
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikeMakesCacheRepository.GetMakesByType");

            }
            return budgets;
        }
    }
}
