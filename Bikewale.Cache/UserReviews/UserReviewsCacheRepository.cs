using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Notifications;
using System;

namespace Bikewale.Cache.UserReviews
{

    /// <summary>
    /// Created by  : Sushil Kumar on 28th June 2016
    /// Description : Bike User Reviews Repository Cache
    /// </summary>
    public class UserReviewsCacheRepository : IUserReviewsCache
    {
        private readonly ICacheManager _cache;
        private readonly IUserReviewsRepository _objUserReviews;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="objUserReviews"></param>
        public UserReviewsCacheRepository(ICacheManager cache, IUserReviewsRepository objUserReviews)
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
        public ReviewListBase GetBikeReviewsList(uint startIndex, uint endIndex, uint modelId, uint versionId, FilterBy filter)
        {
            ReviewListBase reviews = null;

            string key = String.Format("BW_UserReviews_Model_{0}_SEI_{1}_{2}_SO_{3}", modelId, startIndex, endIndex, (int)filter);
            try
            {
                reviews = _cache.GetFromCache<ReviewListBase>(key, new TimeSpan(1, 0, 0), () => _objUserReviews.GetBikeReviewsList(startIndex, endIndex, modelId, versionId, filter));
                //reviews = _cache.GetFromCache<IEnumerable<ReviewEntity>>(key, new TimeSpan(1, 0, 0), () => _objUserReviews.GetBikeReviewsList(startIndex, endIndex, modelId, versionId, filter, out totalReviews));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikeMakesCacheRepository.GetBikeReviewsList");
            }
            return reviews;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public UserReviewsData GetUserReviewsData()
        {
            UserReviewsData reviewsRatings = null;

            string key = "BW_UserReviewsRatings";
            try
            {
                reviewsRatings = _cache.GetFromCache<UserReviewsData>(key, new TimeSpan(24, 0, 0), () => _objUserReviews.GetUserReviewsData());
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikeMakesCacheRepository.GetUserReviewsData");
            }
            return reviewsRatings;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 26 Apr 2017
        /// Description :   Returns the User Reviews by calling DAL
        /// </summary>
        /// <returns></returns>
        public ReviewListBase GetUserReviews()
        {
            ReviewListBase reviews = null;
            string key = "BW_UserReviews";
            try
            {
                reviews = _cache.GetFromCache<ReviewListBase>(key, new TimeSpan(24, 0, 0), () => _objUserReviews.GetUserReviews());
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikeMakesCacheRepository.GetUserReviewsData");
            }
            return reviews;
        }
    }
}
