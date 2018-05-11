using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;

namespace Bikewale.Cache.BikeData
{

    /// <summary>
    /// Created by  :    Sushil Kumar on 28th June 2016
    /// Description :   Bike Versions Repository Cache
    /// </summary>
    public class BikeVersionsCacheRepository<T, U> : IBikeVersionCacheRepository<T, U> where T : BikeVersionEntity, new()
    {
        private readonly ICacheManager _cache;
        private readonly IBikeVersionsRepository<T, U> _objVersionsRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="objMakes"></param>
        public BikeVersionsCacheRepository(ICacheManager cache, IBikeVersionsRepository<T, U> objVersionRepository)
        {
            _cache = cache;
            _objVersionsRepository = objVersionRepository;
        }


        /// <summary>
        /// Created by  :    Sushil Kumar on 28th June 2016
        /// Summary     :   Gets the Similar Bikes  list
        /// Modified by : Ashutosh Sharma on 03 Oct 2017
        /// Description : Changed key from 'BW_SimilarBikes_' to 'BW_SimilarBikes_V1_'.
        /// </summary>
        /// <param name="makeType">Type of make</param>
        /// <returns></returns>
        public IEnumerable<SimilarBikeEntity> GetSimilarBikesList(U versionId, uint topCount, uint cityid)
        {
            IEnumerable<SimilarBikeEntity> versions = null;
            try
            {
                string key = String.Format("BW_SimilarBikes_{0}_Cnt_{1}_{2}", versionId, topCount, cityid);
                TimeSpan cacheTime;
                if (cityid == 0)
                {
                    cacheTime = new TimeSpan(23, 0, 0);
                }
                else
                {
                    cacheTime = new TimeSpan(3, 0, 0);
                }
                versions = _cache.GetFromCache(key, cacheTime, () => _objVersionsRepository.GetSimilarBikesList(versionId, topCount, cityid));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikeMakesCacheRepository.GetSimilarBikesList");

            }
            return versions;
        }

        public IEnumerable<SimilarBikeEntity> GetSimilarBikesByModel(U modelId, uint topCount, uint cityid)
        {
            IEnumerable<SimilarBikeEntity> bikelist = null;
            try
            {
                string key = String.Format("BW_SimilarBikes_V1_M1_{0}_Cnt_{1}_{2}", modelId, topCount, cityid);
                TimeSpan cacheTime;
                if (cityid == 0)
                {
                    cacheTime = new TimeSpan(23, 0, 0);
                }
                else
                {
                    cacheTime = new TimeSpan(3, 0, 0);
                }
                bikelist = _cache.GetFromCache(key, cacheTime, () => _objVersionsRepository.GetSimilarBikesByModel(modelId, topCount, cityid));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikeMakesCacheRepository.GetSimilarBikesByModel ModelId:{0}", modelId));
            }
            return bikelist;
        }
        public IEnumerable<SimilarBikeEntity> GetSimilarBikesByMinPriceDiff(U modelId, uint topCount, uint cityid)
        {
            IEnumerable<SimilarBikeEntity> bikelist = null;
            try
            {
                string key = String.Format("BW_SimilarBikesMinDiff_V1_M1_{0}_Cnt_{1}_{2}", modelId, topCount, cityid);
                TimeSpan cacheTime;
                if (cityid == 0)
                {
                    cacheTime = new TimeSpan(23, 0, 0);
                }
                else
                {
                    cacheTime = new TimeSpan(3, 0, 0);
                }
                bikelist = _cache.GetFromCache(key, cacheTime, () => _objVersionsRepository.GetSimilarBudgetBikes(modelId, topCount, cityid));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikeMakesCacheRepository.GetSimilarBikesByMinPriceDiff ModelId:{0}", modelId));
            }
            return bikelist;
        }


        /// <summary>
        /// Created by  :    Sushil Kumar on 28th June 2016
        /// Summary     :   Gets the versions by type and modelid and cityId
        /// Modified by :   Sumit Kate on 16 Feb 2017
        /// Description :   Consider City Id for memcache key
        /// </summary>
        /// <param name="makeType">Type of make</param>
        /// <returns></returns>
        public List<BikeVersionsListEntity> GetVersionsByType(EnumBikeType requestType, int modelId, int? cityId = null)
        {
            List<BikeVersionsListEntity> versions = null;
            int timeSpan = 24;

            string key = String.Format("BW_VersionsByType_{0}_MO_{1}", (int)requestType, modelId);

            if (cityId.HasValue && cityId.Value > 0)
            {
                key = string.Format("{0}_CI_{1}", key, cityId);
                timeSpan = 1;
            }
            try
            {
                versions = _cache.GetFromCache<List<BikeVersionsListEntity>>(key, new TimeSpan(timeSpan, 0, 0), () => _objVersionsRepository.GetVersionsByType(requestType, modelId, cityId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikeMakesCacheRepository.GetVersionsByType");

            }
            return versions;
        }

        /// <summary>
        /// Created by  : Sushil Kumar on 28th June 2016
        /// Summary     : Gets the version details
        /// Modified By: Snehal Dange on 13th Oct 2017
        /// Description : - Versioned the cache key
        /// </summary>
        /// <param name="makeType">Type of make</param>
        /// <returns></returns>
        public T GetById(U versionId)
        {
            T versionDetails = default(T);
            string key = String.Format("BW_VersionDetails_{0}_V1", versionId);
            try
            {
                versionDetails = _cache.GetFromCache<T>(key, new TimeSpan(1, 0, 0), () => _objVersionsRepository.GetById(versionId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikeMakesCacheRepository.GetById");

            }
            return versionDetails;
        }

        /// <summary>
        /// Created by  : Sushil Kumar on 28th June 2016
        /// Summary     : Gets version minspecs
        /// Modified by : Ashutosh Sharma on 03 Oct 2017
        /// Description :  Changed key from 'BW_VersionMinSpecs_' to 'BW_VersionMinSpecs_V1_'.
        /// </summary>
        /// <param name="makeType">Type of make</param>
        /// <returns></returns>
        public IEnumerable<BikeVersionMinSpecs> GetVersionMinSpecs(uint modelId, bool isNew)
        {
            IEnumerable<BikeVersionMinSpecs> versions = null;
            try
            {
                string key = String.Format("BW_VersionMinSpecs_{0}_New_{1}", modelId, isNew);
                versions = _cache.GetFromCache(key, new TimeSpan(1, 0, 0), () => _objVersionsRepository.GetVersionMinSpecs(modelId, isNew));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikeMakesCacheRepository.GetVersionMinSpecs");
            }
            return versions;
        }
        /// <summary>
        /// Created By: Aditi Srivastava on 17 Oct 2016
        /// Summary :   Get version colors
        /// </summary>
        /// <param name="versionId"></param>
        /// <returns></returns>
        public IEnumerable<BikeColorsbyVersion> GetColorsbyVersionId(uint versionId)
        {
            IEnumerable<BikeColorsbyVersion> versionColors = null;
            string key = String.Format("BW_VersionColor_{0}", versionId);
            try
            {
                versionColors = _cache.GetFromCache<IEnumerable<BikeColorsbyVersion>>(key, new TimeSpan(1, 0, 0), () => _objVersionsRepository.GetColorsbyVersionId(versionId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("BikeVersionsCacheRepository.GetColorsbyVersionId: {0}", versionId));

            }
            return versionColors;
        }

        /// <summary>
        /// Gets the dealer versions by model.
        /// </summary>
        /// <param name="dealerId">The dealer identifier.</param>
        /// <param name="modelId">The model identifier.</param>
        /// <returns></returns>
        public IEnumerable<BikeVersionWithMinSpec> GetDealerVersionsByModel(uint dealerId, uint modelId)
        {
            IEnumerable<BikeVersionWithMinSpec> versions = null;
            string key = String.Format("BW_Versions_Dealer_{0}_Model_{1}", dealerId, modelId);
            try
            {
                versions = _cache.GetFromCache<IEnumerable<BikeVersionWithMinSpec>>(key, new TimeSpan(0, 30, 0), () => _objVersionsRepository.GetDealerVersionsByModel(dealerId, modelId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikeMakesCacheRepository.GetDealerVersionsByModel: DealerId: {0}, ModelId: {1}", dealerId, modelId));

            }
            return versions;
        }

        /// <summary>
        /// Author  : Kartik Rathod on 11 May 2018
        /// Desc    : Get similar bikes based on road price for emi page in finance 
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="topcount"></param>
        /// <param name="cityId"></param>
        /// <returns>SimilarBikesForEMIEntityList</returns>
        public IEnumerable<SimilarBikesForEMIEntity> GetSimilarBikesForEMI(int versionId, short topcount, int cityId)
        {
            IEnumerable<SimilarBikesForEMIEntity> objBikes = null;
            
            try
            {
                string key = String.Format("BW_SimilarBikesForEMI_{0}_{1}_cnt_{2}", versionId, cityId, topcount);
                objBikes = _cache.GetFromCache<IEnumerable<SimilarBikesForEMIEntity>>(key, new TimeSpan(1, 0, 0, 0), () => _objVersionsRepository.GetSimilarBikesForEMI(versionId, topcount, cityId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikeMakesCacheRepository.GetSimilarBikesForEMI: versionId: {0}, cityId: {1}", versionId, cityId));

            }
            return objBikes;
        }
    }
}