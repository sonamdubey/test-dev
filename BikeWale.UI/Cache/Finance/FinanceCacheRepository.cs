
using Bikewale.Entities.Finance.CapitalFirst;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Finance;
using Bikewale.Interfaces.Finance.CapitalFirst;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
namespace Bikewale.Cache.Finance
{
    /// <summary>
    /// Created by : Snehal Dange on 25th May 2018
    /// Description : Cache repository for finance
    /// </summary>
    public class FinanceCacheRepository : IFinanceCacheRepository
    {
        private readonly ICacheManager _cache;
        private readonly IFinanceRepository _financeobj;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="financeobj"></param>
        public FinanceCacheRepository(ICacheManager cache, IFinanceRepository financeobj)
        {
            _cache = cache;
            _financeobj = financeobj;
        }

        public IEnumerable<CityPanMapping> GetCapitalFirstPanCityMapping()
        {
            IEnumerable<CityPanMapping> panCityMapping = null;
            string key = String.Format("BW_CapitalFirst_PanCityMapping");
            try
            {
                panCityMapping = _cache.GetFromCache<IEnumerable<CityPanMapping>>(key, new TimeSpan(1, 0, 0, 0), () => _financeobj.GetCapitalFirstPanCityMapping());
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Cache.Finance.FinanceCacheRepository.GetCapitalFirstPanCityMapping()");

            }
            return panCityMapping;
        }

    }
}
