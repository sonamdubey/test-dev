using AEPLCore.Cache;
using AEPLCore.Cache.Interfaces;
using Carwale.DAL.IPToLocation;
using Carwale.Entity.IPToLocation;
using Carwale.Interfaces;
using Carwale.Interfaces.IPToLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Cache.IPToLocation
{
    public class IPToLocationCacheRepository : IIPToLocationCacheRepository
    {
        private readonly ICacheManager _cacheProvider;
        private readonly IIPToLocationRepository _ipToLocationRepo;

        public IPToLocationCacheRepository(ICacheManager cacheProvider, IPToLocationRepository ipToLocationRepo)
        {
            _cacheProvider = cacheProvider;
            _ipToLocationRepo = ipToLocationRepo;
        }

        public IPToLocationEntity GetIPToLocation(ulong ipNumber)
        {
            return _cacheProvider.GetFromCache<IPToLocationEntity>("IpToLocation_" + ipNumber,
                    CacheRefreshTime.OneHourWithEOD(), () => _ipToLocationRepo.GetIPToLocation(ipNumber));
        }
    }
}
