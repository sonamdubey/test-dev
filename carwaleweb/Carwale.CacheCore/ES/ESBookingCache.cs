using Carwale.Interfaces.ES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.Entity.ES;
using Carwale.Interfaces;
using AEPLCore.Cache;
using AEPLCore.Cache.Interfaces;

namespace Carwale.Cache.ES
{
    public class ESBookingCache : IBookingCache
    {
        private readonly IBookingRepository _bookingRepo;
        protected readonly ICacheManager _cacheProvider;

        public ESBookingCache(IBookingRepository bookingRepo, ICacheManager cacheProvider)
        {
            _bookingRepo = bookingRepo;
            _cacheProvider = cacheProvider;
        }
        public List<ESVersionColors> GetBookingModelData(int modelId)
        {
            return _cacheProvider.GetFromCache<List<ESVersionColors>>("booking_version_details_" + modelId,
                           CacheRefreshTime.EODExpire(),
                               () => _bookingRepo.GetBookingModelData(modelId));
        }
    }
}
