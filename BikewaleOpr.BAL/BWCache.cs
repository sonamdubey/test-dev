using Bikewale.Cache.Core;
using Bikewale.Interfaces.Cache.Core;
using BikewaleOpr.Entity.Dealers;
using BikewaleOpr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
namespace BikewaleOpr.BAL
{
    /// <summary>
    /// Created by  :   Sumit Kate on 08 May 2018
    /// Description :   Business Layer BW Cache
    /// </summary>
    public class BWCache : IBWCache
    {
        private readonly ICacheManager _cache;
        public readonly IDealers _dealerRepo;
        public BWCache(ICacheManager cache, IDealers dealerRepo)
        {
            _cache = cache;
            _dealerRepo = dealerRepo;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 15 May 2018
        /// Description :   Clears memcache
        /// </summary>
        /// <param name="cacheContent"></param>
        /// <param name="keyValues"></param>
        public void Clear(Entity.CacheContents cacheContent, IDictionary<string, string> keyValues)
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
                    uint cityId = 0, dealerid = 0;
                    if (keyValues != null && keyValues.Any())
                    {

                        if ((UInt32.TryParse(keyValues["cityId"], out cityId) && cityId > 0) && (UInt32.TryParse(keyValues["dealerId"], out dealerid) && dealerid > 0))
                        {
                            IEnumerable<string> keys = GetDealerListInCityCacheKey(dealerid, cityId);
                            if (keys != null && keys.Any())
                            {
                                _cache.RefreshCache(keys);
                            }
                        }
                    }
                    break;
                case BikewaleOpr.Entity.CacheContents.DealerCampaign:
                    break;
                case BikewaleOpr.Entity.CacheContents.ESCampaign:
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 15 May 2018
        /// Description :   Returns dealer in city cache key for refresh
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        private IEnumerable<string> GetDealerListInCityCacheKey(uint dealerId, uint cityId)
        {
            IEnumerable<DealerMakeEntity> dealerMakes = _dealerRepo.GetDealersByCity(cityId);
            IEnumerable<uint> makeIds = dealerMakes != null && dealerMakes.Any(m => m.DealerId == dealerId) ? dealerMakes.Select(m => m.MakeId).Distinct().ToList() : null;
            if (makeIds != null && makeIds.Any())
            {
                ICollection<string> keys = new List<string>();
                foreach (var makeId in makeIds)
                {
                    keys.Add(String.Format("BW_DealerList_Make_{0}_City_{1}_v1", makeId, cityId));
                }

                return keys;
            }
            return null;
        }
    }
}
