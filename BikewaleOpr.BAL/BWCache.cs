using Bikewale.Cache.Core;
using Bikewale.Interfaces.Cache.Core;
using BikewaleOpr.Interface;
using System.Collections.Generic;
namespace BikewaleOpr.BAL
{
    /// <summary>
    /// Created by  :   Sumit Kate on 08 May 2018
    /// Description :   Business Layer BW Cache
    /// </summary>
    public class BWCache : IBWCache
    {
        private readonly ICacheManager _cache;
        public BWCache(ICacheManager cache)
        {
            _cache = cache;
        }
        public void Clear(Entity.CacheContents cacheContent)
        {
            switch (cacheContent)
            {
                case BikewaleOpr.Entity.CacheContents.Make:
                    break;
                case BikewaleOpr.Entity.CacheContents.Model:
                    break;
                case BikewaleOpr.Entity.CacheContents.Version:
                    break;
                case BikewaleOpr.Entity.CacheContents.Dealer:
                    break;
                case BikewaleOpr.Entity.CacheContents.DealerCampaign:
                    IEnumerable<string> keys = GetDealerListMakeCityCacheKey();
                    _cache.RefreshCache(keys);
                    break;
                case BikewaleOpr.Entity.CacheContents.ESCampaign:
                    break;
                default:
                    break;
            }
        }

        private IEnumerable<string> GetDealerListMakeCityCacheKey()
        {
            List<string> keys = new List<string>();
            for (int i = 1; i < 100; i++)
            {
                for (int j = 1; j < 1500; j++)
                {
                    keys.Add(string.Format("BW_DealerList_Make_{0}_City_{1}_v1", i, j));
                }
            }
            return keys;
        }

        public void Clear(System.Collections.Generic.IEnumerable<Entity.CacheContents> cacheContents)
        {

        }
    }
}
