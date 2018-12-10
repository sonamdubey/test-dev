using AEPLCore.Cache;
using AEPLCore.Cache.Interfaces;
using Carwale.Entity.CarData;
using Carwale.Entity.Common;
using Carwale.Entity.Dealers;
using Carwale.Interfaces;
using Carwale.Interfaces.Dealers;
using System;
using System.Collections.Generic;

namespace Carwale.Cache.Dealers
{
    public class DealerSponsoredAdCache : IDealerSponsoredAdCache
    {
        protected readonly IDealerSponsoredAdRespository _repo;
        protected readonly ICacheManager _cacheProvider;

        public DealerSponsoredAdCache(IDealerSponsoredAdRespository repo, ICacheManager cacheProvider)
        {
            _repo = repo;
            _cacheProvider = cacheProvider;
        }

         public List<NewCarDealersList> GetNewCarDealersByMakeAndCityId(int makeId, int cityId)
        {
            return _cacheProvider.GetFromCache<List<NewCarDealersList>>(String.Format("NewCarDealersByMakeCity-{0}-{1}", makeId, cityId), CacheRefreshTime.OneDayExpire(),
                () => _repo.GetNewCarDealersByMakeAndCityId(makeId, cityId));
        }
    }
}
