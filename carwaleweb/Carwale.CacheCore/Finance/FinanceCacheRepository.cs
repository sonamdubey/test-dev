using System;
using System.Collections.Generic;
using Carwale.Entity.Campaigns;
using Carwale.Interfaces;
using Carwale.Interfaces.Campaigns;
using AEPLCore.Cache;
using Carwale.Interfaces.Finance;
using Carwale.Entity.Common;
using Carwale.Entity.Enum;
using AEPLCore.Cache.Interfaces;

namespace Carwale.Cache.Campaigns
{
    public class FinanceCacheRepository : IFinanceCacheRepository
    {
        private readonly ICacheManager _cacheProvider;
        private readonly IFinanceOperations _financeRepo;

        public FinanceCacheRepository(ICacheManager cacheProvider, IFinanceOperations financeRepo)
        {
            _cacheProvider = cacheProvider;
            _financeRepo = financeRepo;
        }


        public List<IdName> GetFinanceCompanyList(int clientId)
        {
            return _cacheProvider.GetFromCache<List<IdName>>("FinanceCompanyList_" + clientId,
                   CacheRefreshTime.NeverExpire(),
                   () => _financeRepo.GetFinanceCompanyListRepo(clientId));
        }
    }
}
