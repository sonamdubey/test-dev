using AEPLCore.Cache;
using AEPLCore.Cache.Interfaces;
using Carwale.DAL.Accessories.Tyres;
using Carwale.Entity.Accessories.Tyres;
using Carwale.Interfaces;
using Carwale.Interfaces.Accessories.Tyres;

namespace Carwale.Cache.Accessories.Tyres
{
    public class AccessoryCacheRepository : IAccessoryCache, IAccessoryRepo
    {
        private readonly ICacheManager _cacheProvider;
        private readonly IAccessoryRepo _accessoryRepo;
        public AccessoryCacheRepository(ICacheManager cacheProvider, IAccessoryRepo accessoryRepo)
        {
            _cacheProvider = cacheProvider;
            _accessoryRepo = accessoryRepo;
        }

        public ItemData GetAccessoryDataByItemId(int itemId)
        {
            string cacheKey = string.Format("ItemDataById_{0}", itemId);

            var cacheObj = _cacheProvider.GetFromCache<ItemData>(cacheKey);

            if(cacheObj == null)
            {
                cacheObj = _accessoryRepo.GetAccessoryDataByItemId(itemId);

                if (cacheObj != null)
                {
                    return _cacheProvider.GetFromCache<ItemData>(cacheKey, CacheRefreshTime.DefaultRefreshTime(), () => cacheObj);
                }   
            }
            
            return cacheObj;
        }
    }
}