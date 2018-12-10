using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.Interfaces;
using Carwale.Entity.Geolocation;
using Carwale.DAL.Geolocation;
using AEPLCore.Cache;
using Carwale.Entity;
using AEPLCore.Cache.Interfaces;

namespace Carwale.Cache.Geolocation
{
    public class GeoCitiesCache : GeoCities,IRepository<Cities>
    {
        private readonly ICacheManager _cacheProvider;
        public GeoCitiesCache(ICacheManager cacheProvider)
        {
            _cacheProvider = cacheProvider;
        }

        public IEnumerable<Cities> GetAll()
        {
            var cacheKey = "allGeoCities";
            return _cacheProvider.GetFromCache<IEnumerable<Cities>>(cacheKey, CacheRefreshTime.NeverExpire(), () => base.GetAll());
        }

        public IEnumerable<Cities> GetAllById(int id)
        {
            throw new NotImplementedException();
        }

        public PagedResult<Cities> Find(SearchQuery<Cities> query, int pageSize, int pageNumber)
        {
            throw new NotImplementedException();
        }

        public Cities GetById(int id)
        {
            throw new NotImplementedException();
        }
        public int Create(Cities entity)
        {
            throw new NotImplementedException();
        }
        public bool Update(Cities entity)
        {
            throw new NotImplementedException();
        }
        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
