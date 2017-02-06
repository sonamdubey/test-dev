using Bikewale.Entities.BikeData;
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
    /// Modified By :   Sushil Kumar on 2nd Feb 2017
    /// Description :   Implemented the newly added method of BikeCompareEntity DoCompare(string versions, uint cityId)
    /// </summary>
    public class BikeCompareCacheRepository : IBikeCompareCacheRepository
    {
        private readonly ICacheManager _cache;
        private readonly IBikeCompare _compareRepository;

        public BikeCompareCacheRepository(ICacheManager cache, IBikeCompare compareRepository)
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
            string key = "BW_CompareBikes_Cnt_" + topCount;
            try
            {
                topBikeComapareBase = _cache.GetFromCache<IEnumerable<TopBikeCompareBase>>(key, new TimeSpan(1, 0, 0, 0), () => _compareRepository.CompareList(topCount));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikeCompareCacheRepository.CompareList");

            }
            return topBikeComapareBase;
        }

        /// <summary>
        /// Modified By : Sushil Kumar on 2nd Dec 2016
        /// Description : changed timespan for cache to 1 hour
        /// </summary>
        /// <param name="versions"></param>
        /// <returns></returns>
        public BikeCompareEntity DoCompare(string versions)
        {
            BikeCompareEntity compareEntity = null;
            string key = string.Empty;
            try
            {
                key = "BW_Compare_Bikes_" + versions.Replace(',', '_');
                compareEntity = _cache.GetFromCache<BikeCompareEntity>(key, new TimeSpan(1, 0, 0), () => _compareRepository.DoCompare(versions));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikeCompareCacheRepository.DoCompare");

            }
            return compareEntity;
        }

        /// <summary>
        /// Created By : Sushil Kumar on 2nd Dec 2016
        /// Description : Cache layer to similar cache comaprisions bikes
        /// </summary>
        /// <param name="versionList"></param>
        /// <param name="topCount"></param>
        /// <param name="cityid"></param>
        /// <returns></returns>
        public ICollection<SimilarCompareBikeEntity> GetSimilarCompareBikes(string versionList, ushort topCount, int cityid)
        {
            ICollection<SimilarCompareBikeEntity> compareEntity = null;
            string key = string.Empty;
            try
            {
                key = string.Format("BW_SimilarCompareBikes_{0}_City_{1}_Cnt_{2}", versionList.Replace(',', '_'), cityid, topCount);
                compareEntity = _cache.GetFromCache<ICollection<SimilarCompareBikeEntity>>(key, new TimeSpan(1, 0, 0), () => _compareRepository.GetSimilarCompareBikes(versionList, topCount, cityid));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("BikeCompareCacheRepository_GetSimilarCompareBikes_{0}_Cnt_{1}_City_{2}", versionList, topCount, cityid));

            }
            return compareEntity;
        }

        /// <summary>
        /// Created By : Sushil Kumar on 2nd Dec 2016
        /// Description : Cache layer to similar cache comaprisions bikes with sponsored comparision 
        /// </summary>
        /// <param name="versionList"></param>
        /// <param name="topCount"></param>
        /// <param name="cityid"></param>
        /// <param name="sponsoredVersionId"></param>
        /// <returns></returns>
        public ICollection<SimilarCompareBikeEntity> GetSimilarCompareBikeSponsored(string versionList, ushort topCount, int cityid, uint sponsoredVersionId)
        {
            ICollection<SimilarCompareBikeEntity> compareEntity = null;
            string key = string.Empty;
            try
            {
                key = string.Format("BW_SimilarCompareBikes_Sponsored_{0}_City_{1}_Cnt_{2}", versionList.Replace(',', '_'), cityid, topCount);
                compareEntity = _cache.GetFromCache<ICollection<SimilarCompareBikeEntity>>(key, new TimeSpan(1, 0, 0), () => _compareRepository.GetSimilarCompareBikeSponsored(versionList, topCount, cityid, sponsoredVersionId));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("BikeCompareCacheRepository_GetSimilarCompareBikeSponsored_{0}_Cnt_{1}_SP_{2}_City_{3}", versionList, topCount, sponsoredVersionId, cityid));

            }
            return compareEntity;
        }

        /// <summary>
        /// Created By :   Sushil Kumar on 2nd Feb 2017
        /// Description :   To cache bikecomparision data
        /// </summary>
        /// <param name="versions"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public BikeCompareEntity DoCompare(string versions, uint cityId)
        {
            BikeCompareEntity compareEntity = null;
            string key = string.Empty;
            try
            {
                key = string.Format("BW_Compare_Bikes_{0}_City_{1}", versions.Replace(',', '_'), cityId);
                compareEntity = _cache.GetFromCache<BikeCompareEntity>(key, new TimeSpan(1, 0, 0), () => _compareRepository.DoCompare(versions, cityId));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("BikeCompareCacheRepository.DoCompare- {0} - {1}", versions, cityId));

            }
            return compareEntity;
        }
    }
}
