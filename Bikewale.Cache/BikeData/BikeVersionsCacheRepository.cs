using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Cache.BikeData
{

    /// <summary>
    /// Created by  :    Sushil Kumar on 28th June 2016
    /// Description :   Bike Versions Repository Cache
    /// </summary>
    public class BikeVersionsCacheRepository<T, U> : IBikeVersionCacheRepository<U>
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
    }
}
