using System;
using System.Collections.Generic;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Compare;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Compare;
using Bikewale.Notifications;

namespace Bikewale.Cache.Compare
{
    /// <summary>
    /// Created By : Lucky Rathore on 06 Nov. 2015.
    /// Description : Use for caching Popular compare bike Widget.
    /// Modified By :   Sumit Kate on 22 Jan 2016
    /// Description :   Implemented the newly added method of IBikeCompareCacheRepository
    /// Modified By :   Sushil Kumar on 2nd Feb 2017
    /// Description :   Implemented the newly added method of BikeCompareEntity DoCompare(string versions, uint cityId)
    /// Modified By :Snehal Dange on 25th Oct 2017
    /// Description : Added method GetSimilarBikesForComparisions()
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
                ErrorClass.LogError(ex, "BikeCompareCacheRepository.CompareList");

            }
            return topBikeComapareBase;
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 25 Apr 2017
        /// Summary    : Get list of comparisons of popular bikes
        /// Modified by :Adit Srivastava on 5 June 2017
        /// Summary     : Changed cache key
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SimilarCompareBikeEntity> GetPopularCompareList(uint cityId)
        {
            IEnumerable<SimilarCompareBikeEntity> compareBikeList = null;
            string key = string.Format("BW_PopularSimilarBikes_CityId_v2_{0}", cityId);
            try
            {
                compareBikeList = _cache.GetFromCache<IEnumerable<SimilarCompareBikeEntity>>(key, new TimeSpan(12, 0, 0, 0), () => _compareRepository.GetPopularCompareList(cityId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikeCompareCacheRepository.GetPopularCompareList- CityId : {0}", cityId));
            }
            return compareBikeList;
        }
        /// <summary>
        /// Modified By : Sushil Kumar on 2nd Dec 2016
        /// Description : changed timespan for cache to 1 hour
        /// Modified by : Aditi Srivastava on 18 May 2017
        /// Summary     : Changed cache key due to change in entity 
        /// </summary>
        /// <param name="versions"></param>
        /// <returns></returns>
        public BikeCompareEntity DoCompare(string versions)
        {
            BikeCompareEntity compareEntity = null;
            string key = string.Empty;
            try
            {
                key = "BW_Compare_Bikes_v2" + versions.Replace(',', '_');
                compareEntity = _cache.GetFromCache<BikeCompareEntity>(key, new TimeSpan(1, 0, 0), () => _compareRepository.DoCompare(versions));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikeCompareCacheRepository.DoCompare");

            }
            return compareEntity;
        }

        /// <summary>
        /// Created By : Sushil Kumar on 2nd Dec 2016
        /// Description : Cache layer to similar cache comaprisions bikes
        /// Modified by: Ashutosh Sharma on 02 Oct 2017
        /// Description : Changed key form 'BW_SimilarCompareBikes_' to 'BW_SimilarCompareBikes_V1_'.
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
                if (!String.IsNullOrEmpty(versionList))
                {
                    key = string.Format("BW_SimilarCompareBikes_V1_{0}_City_{1}_Count_{2}", versionList.Replace(',', '_'), cityid, topCount);
                    compareEntity = _cache.GetFromCache<ICollection<SimilarCompareBikeEntity>>(key, new TimeSpan(1, 0, 0), () => _compareRepository.GetSimilarCompareBikes(versionList, topCount, cityid));
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikeCompareCacheRepository_GetSimilarCompareBikes_{0}_Cnt_{1}_City_{2}", versionList, topCount, cityid));
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
                ErrorClass.LogError(ex, string.Format("BikeCompareCacheRepository_GetSimilarCompareBikeSponsored_{0}_Cnt_{1}_SP_{2}_City_{3}", versionList, topCount, sponsoredVersionId, cityid));

            }
            return compareEntity;
        }

        /// <summary>
        /// Created By :   Sushil Kumar on 2nd Feb 2017
        /// Description :   To cache bikecomparision data
        /// Modified by : Aditi Srivastava on 18 May 2017
        /// Summary     : Changed cache key due to change in entity
        /// </summary>
        /// <param name="versions"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public BikeCompareEntity DoCompare(string versions, uint cityId)
        {
            BikeCompareEntity compareEntity = null;
            try
            {
                string key = string.Format("BW_Compare_Bikes_{0}_City_{1}", versions.Replace(',', '_'), cityId);
                compareEntity = _cache.GetFromCache(key, new TimeSpan(1, 0, 0), () => _compareRepository.DoCompare(versions, cityId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikeCompareCacheRepository.DoCompare - {0} - {1}", versions, cityId));
            }
            return compareEntity;
        }
        /// <summary>
        /// Created By :- Subodh Jain 10 March 2017
        /// Summary :- Populate Compare ScootersList
        /// </summary>
        public IEnumerable<TopBikeCompareBase> ScooterCompareList(uint topCount)
        {
            IEnumerable<TopBikeCompareBase> topScootersComapareBase = null;
            string key = string.Format("BW_CompareScooters_Count_{0}", topCount);
            try
            {
                topScootersComapareBase = _cache.GetFromCache<IEnumerable<TopBikeCompareBase>>(key, new TimeSpan(1, 0, 0), () => _compareRepository.ScooterCompareList(topCount));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikeCompareCacheRepository.ScooterCompareList topCount:{0}", topCount));

            }
            return topScootersComapareBase;
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 2 June 2017
        /// Summary    : Get list of comparisons of popular scooters
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SimilarCompareBikeEntity> GetScooterCompareList(uint cityId)
        {
            IEnumerable<SimilarCompareBikeEntity> compareBikeList = null;
            string key = string.Format("BW_PopularScootersComparison_CityId_v2_{0}", cityId);
            try
            {
                compareBikeList = _cache.GetFromCache<IEnumerable<SimilarCompareBikeEntity>>(key, new TimeSpan(12, 0, 0, 0), () => _compareRepository.GetScooterCompareList(cityId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikeCompareCacheRepository.GetScooterCompareList- CityId : {0}", cityId));
            }
            return compareBikeList;
        }

        /// <summary>
        /// Created By : Snehal Dange on 24th Oct 2017.
        /// Decsription : Cache method created for similar bikes for comparison on compare bikes page.
        /// </summary>
        /// <param name="versionList"></param>
        /// <param name="topCount"></param>
        /// <returns></returns>
        public SimilarBikeComparisonWrapper GetSimilarBikes(string modelList, ushort topCount)
        {
            SimilarBikeComparisonWrapper similarbikecomparison = null;
            string key = string.Empty;
            try
            {

                if (!string.IsNullOrEmpty(modelList))
                {
                    key = string.Format("BW_SimilarCompareBikes_{0}_Cnt_{1}", modelList.Replace(',', '_'), topCount);
                    similarbikecomparison = _cache.GetFromCache<SimilarBikeComparisonWrapper>(key, new TimeSpan(3, 0, 0), () => _compareRepository.GetSimilarBikes(modelList, topCount));
                }

            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, string.Format("BikeCompareCacheRepository.GetSimilarBikes_{0}_Cnt_{1}", modelList, topCount));
            }
            return similarbikecomparison;
        }
    }
}
