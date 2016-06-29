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
        private readonly IBikeModelsRepository<T, U> _modelRepository;

        /// <summary>
        /// Intitalize the references for the cache and BL
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="objModels"></param>
        public BikeModelsCacheRepository(ICacheManager cache, IBikeModels<T, U> objModels, IBikeModelsRepository<T, U> modelRepository)
        {
            _cache = cache;
            _objModels = objModels;
            _modelRepository = modelRepository;
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


        /// <summary>
        /// Written By : Sushil Kumar on 28th June 2016
        /// Summary : Function to get the upcoming bikes. If data is not available in the cache it will return data from BL.
        /// </summary>
        /// <param name="sortBy"></param>
        /// <param name="pageSize"></param>
        /// <param name="makeId"></param>
        /// <param name="modelId"></param>
        /// <param name="curPageNo"></param>
        /// <returns>Returns List<UpcomingBikeEntity></returns>
        public IEnumerable<UpcomingBikeEntity> GetUpcomingBikesList(EnumUpcomingBikesFilter sortBy, int pageSize, int? makeId = null, int? modelId = null, int? curPageNo = null)
        {
            IEnumerable<UpcomingBikeEntity> objUpcoming = null;
            string key = "BW_UpcomingBikes_Cnt_" + pageSize;

            if (makeId.HasValue && makeId.Value > 0)
                key += "_Make_" + makeId;

            if (modelId.HasValue && modelId.Value > 0)
                key += "_Model_" + modelId;

            try
            {
                objUpcoming = _cache.GetFromCache<IEnumerable<UpcomingBikeEntity>>(key, new TimeSpan(1, 0, 0), () => _objModels.GetUpcomingBikesList(sortBy, pageSize, makeId, modelId, curPageNo));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikeModelsCacheRepository.GetUpcomingBikesList");
                objErr.SendMail();
            }

            return objUpcoming;
        }

        /// <summary>
        /// Written By : Sushil Kumar on 28th June 2016
        /// Summary : Function to get the popular bikes by make . If data is not available in the cache it will return data from BL.
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns>Returns BikeModelPageEntity</returns>
        public IEnumerable<MostPopularBikesBase> GetMostPopularBikesByMake(int makeId)
        {
            IEnumerable<MostPopularBikesBase> objBikes = null; 
            string key = "BW_PopularBikesByMake_" + makeId;

            try
            {
                objBikes = _cache.GetFromCache<IEnumerable<MostPopularBikesBase>>(key, new TimeSpan(1, 0, 0), () => _modelRepository.GetMostPopularBikesByMake(makeId));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikeModelsCacheRepository.GetModelPageDetails");
                objErr.SendMail();
            }

            return objBikes;
        }

    }
}
