using System.Collections.Generic;
using Carwale.Interfaces.PriceQuote;
using Carwale.Interfaces;
using AEPLCore.Cache;
using Carwale.Entity.Price;
using AEPLCore.Cache.Interfaces;

namespace Carwale.Cache.PriceQuote
{
    public class ChargeGroupsCacheRepository : IChargeGroupsCacheRepository
    {
        private readonly ICacheManager _cacheRepo;
        private readonly IChargeGroupsRepository _chargeGroupsRepo;

        public ChargeGroupsCacheRepository(ICacheManager cacheRepo, IChargeGroupsRepository chargeGroupsRepo)
        {
            _cacheRepo = cacheRepo;
            _chargeGroupsRepo = chargeGroupsRepo;
        }

        public Dictionary<int, ChargeGroup> GetChargeGroups()
        {
            return _cacheRepo.GetFromCache("charge-groups_v1", CacheRefreshTime.NeverExpire(),
                () => _chargeGroupsRepo.GetChargeGroups());
        }
    }
}
