﻿using Bikewale.Entities.BikeData;
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
    public class BikeVersionsCacheRepository<T, U> : IBikeVersionCacheRepository<T, U>
    {
        private readonly ICacheManager _cache;
        private readonly IBikeVersions<T, U> _objVersions;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="objMakes"></param>
        public BikeVersionsCacheRepository(ICacheManager cache, IBikeVersions<T, U> objVersions)
        {
            _cache = cache;
            _objVersions = objVersions;
        }


        /// <summary>
        /// Created by  :    Sushil Kumar on 28th June 2016
        /// Summary     :   Gets the Similar Bikes  list
        /// Modified by : Ashutosh Sharma on 03 Oct 2017
        /// Description : Changed key from 'BW_SimilarBikes_' to 'BW_SimilarBikes_V1_'.
        /// </summary>
        /// <param name="makeType">Type of make</param>
        /// <returns></returns>
        public IEnumerable<Entities.BikeData.SimilarBikeEntity> GetSimilarBikesList(U versionId, uint topCount, uint cityid)
        {
            IEnumerable<Entities.BikeData.SimilarBikeEntity> versions = null;
            string key = String.Format("BW_SimilarBikes_V1_{0}_Cnt_{1}_{2}", versionId, topCount, cityid);
            try
            {
                TimeSpan cacheTime = new TimeSpan(3, 0, 0);
                if (cityid == 0)
                {
                    cacheTime = new TimeSpan(23, 0, 0);
                }
                versions = _cache.GetFromCache<IEnumerable<Entities.BikeData.SimilarBikeEntity>>(key, cacheTime, () => _objVersions.GetSimilarBikesList(versionId, topCount, cityid));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikeMakesCacheRepository.GetSimilarBikesList");

            }
            return versions;
        }

        public IEnumerable<Entities.BikeData.SimilarBikeEntity> GetSimilarBikesByModel(U modelId, uint topCount, uint cityid)
        {
            IEnumerable<Entities.BikeData.SimilarBikeEntity> bikelist = null;
            string key = String.Format("BW_SimilarBikes_M1_{0}_Cnt_{1}_{2}", modelId, topCount, cityid);
            try
            {
                TimeSpan cacheTime = new TimeSpan(3, 0, 0);
                if (cityid == 0)
                {
                    cacheTime = new TimeSpan(23, 0, 0);
                }
                bikelist = _cache.GetFromCache<IEnumerable<Entities.BikeData.SimilarBikeEntity>>(key, cacheTime, () => _objVersions.GetSimilarBikesByModel(modelId, topCount, cityid));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikeMakesCacheRepository.GetSimilarBikesList");

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
                versions = _cache.GetFromCache<List<BikeVersionsListEntity>>(key, new TimeSpan(timeSpan, 0, 0), () => _objVersions.GetVersionsByType(requestType, modelId, cityId));
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
                versionDetails = _cache.GetFromCache<T>(key, new TimeSpan(1, 0, 0), () => _objVersions.GetById(versionId));
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
        public List<BikeVersionMinSpecs> GetVersionMinSpecs(uint modelId, bool isNew)
        {
            List<BikeVersionMinSpecs> versions = null;
            string key = String.Format("BW_VersionMinSpecs_V1_{0}_New_{1}", modelId, isNew);
            try
            {
                versions = _cache.GetFromCache<List<BikeVersionMinSpecs>>(key, new TimeSpan(1, 0, 0), () => _objVersions.GetVersionMinSpecs(modelId, isNew));
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
                versionColors = _cache.GetFromCache<IEnumerable<BikeColorsbyVersion>>(key, new TimeSpan(1, 0, 0), () => _objVersions.GetColorsbyVersionId(versionId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("BikeMakesCacheRepository.GetColorsbyVersionId: {0}", versionId));

            }
            return versionColors;
        }
    }
}
