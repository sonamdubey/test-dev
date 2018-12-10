using AEPLCore.Cache;
using AEPLCore.Cache.Interfaces;
using Carwale.DAL.Accessories.Tyres;
using Carwale.Entity.Accessories.Tyres;
using Carwale.Interfaces;
using Carwale.Interfaces.Accessories.Tyres;
using System.Collections.Generic;

namespace Carwale.Cache.Accessories.Tyres
{
    public class TyresCacheRepository : TyresRepository, ITyresRepository
    {
        private readonly ICacheManager _cacheProvider;
        public TyresCacheRepository(ICacheManager cacheProvider)
        {
            _cacheProvider = cacheProvider;
        }

        public new VersionTyres GetTyresByVersionId(int carVersionId)
        {
            var cacheKey = string.Format("TyresByCar_{0}", carVersionId);

            var cacheObj = _cacheProvider.GetFromCache<VersionTyres>(cacheKey);

            if(cacheObj == null)
            {
                cacheObj = base.GetTyresByVersionId(carVersionId);

                if(cacheObj != null)
                {
                    return _cacheProvider.GetFromCache<VersionTyres>(cacheKey, CacheRefreshTime.DefaultRefreshTime(), () => cacheObj);
                }
            }

            return cacheObj;
        }

        public new List<TyreSummary> GetTyresByModels(string carModelIds)
        {
            var cacheKey = string.Format("TyresByModels_{0}", carModelIds);
            var cacheObj = _cacheProvider.GetFromCache<List<TyreSummary>>(cacheKey);

            if(cacheObj == null)
            {
                cacheObj = base.GetTyresByModels(carModelIds);

                if(cacheObj!=null)
                {
                    return _cacheProvider.GetFromCache<List<TyreSummary>>(cacheKey, CacheRefreshTime.DefaultRefreshTime(), () => cacheObj);
                }   
            }

            return cacheObj;
        }
        public int GetBrandIdFromMaskingName(string tyreMaskingName)
        {
            return _cacheProvider.GetFromCache(string.Format("TyresBrandIdByName_{0}", tyreMaskingName),
                CacheRefreshTime.DefaultRefreshTime(), () => base.GetBrandIdFromMaskingName(tyreMaskingName));
        }

        public List<TyreSummary> GetTyresByBrandId(int brandId)
        {
            return _cacheProvider.GetFromCache(string.Format("TyresByBrandId_{0}", brandId),
                CacheRefreshTime.DefaultRefreshTime(), () => base.GetTyresByBrandId(brandId));
        }
    }
}
