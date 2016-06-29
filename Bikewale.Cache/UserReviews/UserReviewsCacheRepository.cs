using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Cache.UserReviews
{

    /// <summary>
    /// Created by  : Sushil Kumar on 28th June 2016
    /// Description : Bike User Reviews Repository Cache
    /// </summary>
    public class UserReviewsCacheRepository : IUserReviewsCache
    {
        private readonly ICacheManager _cache;
        private readonly IUserReviews _objUserReviews;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="objUserReviews"></param>
        public UserReviewsCacheRepository(ICacheManager cache, IUserReviews objUserReviews)
        {
            _cache = cache;
            _objUserReviews = objUserReviews;
        }

        /// <summary>
        /// Created by  : Sushil Kumar on 29th June 2016
        /// Summary     : Gets the User Reviews for Bikes
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="modelId"></param>
        /// <param name="versionId"></param>
        /// <param name="filter"></param>
        /// <param name="totalReviews"></param>
        /// <returns></returns>
        public IEnumerable<ReviewEntity> GetBikeReviewsList(uint startIndex, uint endIndex, uint modelId, uint versionId, FilterBy filter, out uint totalReviews)
        {
            IEnumerable<ReviewEntity> reviews = null;
            totalReviews = 10;
            //Func<IEnumerable<ReviewEntity>> func = delegate() { return _objUserReviews.GetBikeReviewsList(startIndex, endIndex, modelId, versionId, filter, out totalReviews); };
            
            string key = String.Format("BW_BikeReviews_Cnt_{0}_Model_{1}_Version_{2}_Filter_{3}", totalReviews, modelId, versionId, filter);
            try
            {
                //reviews = _cache.GetFromCache<IEnumerable<ReviewEntity>>(key, new TimeSpan(1, 0, 0), () => _objUserReviews.GetBikeReviewsList(startIndex, endIndex, modelId, versionId, filter, out totalReviews));
               //reviews = _cache.GetFromCache<IEnumerable<ReviewEntity>>(key, new TimeSpan(1, 0, 0), () => _objUserReviews.GetBikeReviewsList(startIndex, endIndex, modelId, versionId, filter, out totalReviews));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikeMakesCacheRepository.GetBikeReviewsList");
                objErr.SendMail();
            }
            return reviews;
        }
    }
}
