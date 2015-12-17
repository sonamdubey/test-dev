using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Interfaces.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Cache.Core;
using Bikewale.DAL.BikeData;
using Bikewale.Notifications;
using Bikewale.Entities.CMS;
using Bikewale.Entities.CMS.Photos;

namespace Bikewale.Cache.BikeData
{
    /// <summary>
    /// Written By : Ashish G. Kamble on 7 Oct 2015
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public class BikeModelsCacheRepository<T, U> : IBikeModelsCacheRepository<U>
    {
        private readonly ICacheManager _cache;
        private readonly IBikeModels<T, U> _objModels;

        /// <summary>
        /// Intitalize the references for the cache and BL
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="objModels"></param>
        public BikeModelsCacheRepository(ICacheManager cache, IBikeModels<T, U> objModels)
        {
            _cache = cache;
            _objModels = objModels;
        }

        /// <summary>
        /// Written By : Ashish G. Kamble on 8 Oct 2015
        /// Summary : Function to get the model page details from the cache. If data is not available in the cache it will return data from BL.
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns>Returns BikeModelPageEntity</returns>
        public BikeModelPageEntity GetModelPageDetails(U modelId)
        {
            BikeModelPageEntity objModelPage = null;
            string key = "BW_ModelDetails_" + modelId;

            try
            {                
                objModelPage = _cache.GetFromCache<BikeModelPageEntity>(key, new TimeSpan(1, 0, 0), () => _objModels.GetModelPageDetails(modelId));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikeModelsCacheRepository.GetModelPageDetails");
                objErr.SendMail(); 
            }

            return objModelPage;
        }
    }
}
