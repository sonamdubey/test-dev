using AEPLCore.Cache;
using AEPLCore.Cache.Interfaces;
using Carwale.Entity.Classified;
using Carwale.Interfaces.Classified.Slots;
using System.Collections.Generic;

namespace Carwale.Cache.Classified.Slots
{
    public class SlotsCacheRepository : ISlotsCacheRepository
    {
        private readonly ICacheManager _cacheProvider;
        private const string _cacheKeyPrefix = "used_featured_slots_city_";
        private readonly ISlotsRepository _slotsRepository;
        public SlotsCacheRepository(ICacheManager cacheProvider, ISlotsRepository slotsRepository)
        {
            _cacheProvider = cacheProvider;
            _slotsRepository = slotsRepository;
        }

        public IEnumerable<Slot> GetSlotsByCityId(int cityId)
        {
            return _cacheProvider.GetFromCache<IEnumerable<Slot>>($"{ _cacheKeyPrefix }{ cityId }", CacheRefreshTime.NeverExpire(), () => _slotsRepository.GetSlotsByCityId(cityId));
        }
    }
}
