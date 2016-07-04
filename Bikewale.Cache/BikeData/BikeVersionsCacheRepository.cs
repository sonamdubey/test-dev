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
        /// </summary>
        /// <param name="makeType">Type of make</param>
        /// <returns></returns>
        public IEnumerable<Entities.BikeData.SimilarBikeEntity> GetSimilarBikesList(U versionId, uint topCount, uint percentDeviation)
        {
            IEnumerable<Entities.BikeData.SimilarBikeEntity> versions = null;
            string key = String.Format("BW_SimilarBikes_{0}_Cnt_{1}", versionId, topCount);
            try
            {
                versions = _cache.GetFromCache<IEnumerable<Entities.BikeData.SimilarBikeEntity>>(key, new TimeSpan(1, 0, 0), () => _objVersions.GetSimilarBikesList(versionId, topCount, percentDeviation));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikeMakesCacheRepository.GetSimilarBikesList");
                objErr.SendMail();
            }
            return versions;
        }

        /// <summary>
        /// Created by  :    Sushil Kumar on 28th June 2016
        /// Summary     :   Gets the versions by type and modelid and cityId
        /// </summary>
        /// <param name="makeType">Type of make</param>
        /// <returns></returns>
        public List<BikeVersionsListEntity> GetVersionsByType(EnumBikeType requestType, int modelId, int? cityId = null)
        {
            List<BikeVersionsListEntity> versions = null;
            string key = String.Format("BW_VersionsByType_{0}_MO_{1}", (int)requestType, modelId, cityId.HasValue && cityId.Value > 0 ? "_CI_" + cityId : "");
            try
            {
                versions = _cache.GetFromCache<List<BikeVersionsListEntity>>(key, new TimeSpan(1, 0, 0), () => _objVersions.GetVersionsByType(requestType, modelId, cityId));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikeMakesCacheRepository.GetVersionsByType");
                objErr.SendMail();
            }
            return versions;
        }

        /// <summary>
        /// Created by  : Sushil Kumar on 28th June 2016
        /// Summary     : Gets the version details
        /// </summary>
        /// <param name="makeType">Type of make</param>
        /// <returns></returns>
        public T GetById(U versionId)
        {
            T versionDetails = default(T);
            string key = String.Format("BW_VersionDetails_{0}", versionId);
            try
            {
                versionDetails = _cache.GetFromCache<T>(key, new TimeSpan(1, 0, 0), () => _objVersions.GetById(versionId));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikeMakesCacheRepository.GetById");
                objErr.SendMail();
            }
            return versionDetails;
        }

        /// <summary>
        /// Created by  : Sushil Kumar on 28th June 2016
        /// Summary     : Gets version minspecs
        /// </summary>
        /// <param name="makeType">Type of make</param>
        /// <returns></returns>
        public List<BikeVersionMinSpecs> GetVersionMinSpecs(uint modelId, bool isNew)
        {
            List<BikeVersionMinSpecs> versions = null;
            string key = String.Format("BW_VersionMinSpecs_{0}_New_{1}", modelId, isNew);
            try
            {
                versions = _cache.GetFromCache<List<BikeVersionMinSpecs>>(key, new TimeSpan(1, 0, 0), () => _objVersions.GetVersionMinSpecs(modelId, isNew));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikeMakesCacheRepository.GetVersionMinSpecs");
                objErr.SendMail();
            }
            return versions;
        }

    }
}
