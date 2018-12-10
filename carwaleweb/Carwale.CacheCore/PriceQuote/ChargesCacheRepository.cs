using Carwale.Interfaces.PriceQuote;
using System.Collections.Generic;
using Carwale.Interfaces;
using AEPLCore.Cache;
using Carwale.Entity.Price;
using AEPLCore.Cache.Interfaces;

namespace Carwale.Cache.PriceQuote
{
    public class ChargesCacheRepository : IChargesCacheRepository
    {
        private readonly ICacheManager _cacheRepo;
        private readonly IChargesRepository _chargesRepo;

        public ChargesCacheRepository(ICacheManager cacheRepo, IChargesRepository chargesRepo)
        {
            _cacheRepo = cacheRepo;
            _chargesRepo = chargesRepo;
        }

        public Dictionary<int, Charge> GetCharges()
        {
            return _cacheRepo.GetFromCache("charges_v1", CacheRefreshTime.NeverExpire(), () => _chargesRepo.GetCharges());
        }

        public List<int> GetComponents(int chargeId)
        {
            return _cacheRepo.GetFromCache(string.Format("charge-components-{0}", chargeId), CacheRefreshTime.NeverExpire(),
                () => _chargesRepo.GetComponents(chargeId));
        }
    }
}
