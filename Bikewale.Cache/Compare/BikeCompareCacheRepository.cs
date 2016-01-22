using Bikewale.Entities.Compare;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Compare;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;

namespace Bikewale.Cache.Compare
{
    /// <summary>
    /// Created By : Lucky Rathore on 06 Nov. 2015.
    /// Description : Use for caching Popular compare bike Widget.
    /// Modified By :   Sumit Kate on 22 Jan 2016
    /// Description :   Implemented the newly added method of IBikeCompareCacheRepository
    /// </summary>
    public class BikeCompareCacheRepository : IBikeCompareCacheRepository
    {
        private readonly ICacheManager _cache;
        private readonly IBikeCompare _compareRepository;

        public BikeCompareCacheRepository(ICacheManager cache,IBikeCompare compareRepository)
        {
            _cache = cache;
            _compareRepository = compareRepository;
        }

        /// <summary>
        /// Created by : Lucky Rathore on 06 Nov. 2015.
        /// Description : For getting information of popular bikes.
        /// </summary>
        /// <param name="topCount"></param>
        /// <returns>Top pouplar bikes comparision.</returns>
        public IEnumerable<TopBikeCompareBase> CompareList(uint topCount)
        {
            IEnumerable<TopBikeCompareBase> topBikeComapareBase = null;
            string key = string.Empty; 
            try
            {
                key = "BW_CompareBikes";
                topBikeComapareBase = _cache.GetFromCache<IEnumerable<TopBikeCompareBase>>(key, new TimeSpan(1, 0, 0, 0), () => _compareRepository.CompareList(topCount));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikeCompareCacheRepository.CompareList");
                objErr.SendMail();    
            }
            return topBikeComapareBase;
        }


        public BikeCompareEntity DoCompare(string versions)
        {
            BikeCompareEntity compareEntity = null;
            string key = string.Empty;
            try
            {
                key = "BW_Compare_Bikes_" + versions.Replace(',','_');
                compareEntity = _cache.GetFromCache<BikeCompareEntity>(key, new TimeSpan(0, 30, 0), () => _compareRepository.DoCompare(versions));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikeCompareCacheRepository.DoCompare");
                objErr.SendMail();
            }
            return compareEntity;
        }
    }
}
