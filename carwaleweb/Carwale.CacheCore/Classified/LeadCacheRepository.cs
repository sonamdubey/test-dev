using AEPLCore.Cache.Interfaces;
using Carwale.Entity.Classified.UsedLeads;
using Carwale.Interfaces;
using Carwale.Interfaces.Classified.Leads;
using System;

namespace Carwale.Cache.Classified
{
    public class LeadCacheRepository : ILeadCacheRepository
    {
        protected readonly ICacheManager _cacheProvider;
        private readonly IUsedLeadsRepository _usedLeadsRepo;

        public LeadCacheRepository(ICacheManager cacheProvider, IUsedLeadsRepository usedLeadsRepo)
        {
            _cacheProvider = cacheProvider;
            _usedLeadsRepo = usedLeadsRepo;
        }

        public DealerLeadsCount GetLeadsCountForCurrentMonth(int dealerId)
        {
            DateTime currentDate = DateTime.Now;
            string currentMonthKey = "uclc-" + dealerId + "-" + currentDate.ToString("dd");
            return _cacheProvider.GetFromCache<DealerLeadsCount>(currentMonthKey, new TimeSpan(24, 0, 0),
                () => _usedLeadsRepo.GetDealerLeadsCount(dealerId, currentDate.Month, currentDate.Year));
        }

        public DealerLeadsCount GetLeadsCountForLastMonth(int dealerId)
        {
            DateTime lastMonthDate = DateTime.Now.AddMonths(-1);
            string key = "uclc-" + dealerId + "-" + lastMonthDate.ToString("MM-yyyy");
            return _cacheProvider.GetFromCache<DealerLeadsCount>(key, new TimeSpan(60, 0, 0, 0),
                () => _usedLeadsRepo.GetDealerLeadsCount(dealerId, lastMonthDate.Month, lastMonthDate.Year));
        }

        public DealerLeadsCount GetLeadsCountForSecondLastMonth(int dealerId)
        {
            DateTime secLastMonthDate = DateTime.Now.AddMonths(-2);
            string key = "uclc-" + dealerId + "-" + secLastMonthDate.ToString("MM-yyyy");
            return _cacheProvider.GetFromCache<DealerLeadsCount>(key, new TimeSpan(30, 0, 0, 0),
                () => _usedLeadsRepo.GetDealerLeadsCount(dealerId, secLastMonthDate.Month, secLastMonthDate.Year));
        }
    }
}
