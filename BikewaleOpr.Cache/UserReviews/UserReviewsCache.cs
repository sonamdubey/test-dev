using Bikewale.Interfaces.Cache.Core;
using Bikewale.Notifications;
using BikewaleOpr.Entity.UserReviews;
using BikewaleOpr.Interface.UserReviews;
using System;
using System.Collections.Generic;

namespace BikewaleOpr.Cache.UserReviews
{
    /// <summary>
    /// Created By Ashish G. Kamble on 18 Apr 2017
    /// </summary>
    public class UserReviewsCache : IUserReviewsCache
    {
        private readonly ICacheManager _cache;
        private readonly IUserReviewsRepository _userReviewsRepo;

        /// <summary>
        /// Constructor to initialize the dependencies
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="userReviewsRepo"></param>
        public UserReviewsCache(ICacheManager cache, IUserReviewsRepository userReviewsRepo)
        {
            _cache = cache;
            _userReviewsRepo = userReviewsRepo;
        }

        /// <summary>
        /// Written By : Ashish G. Kamble on 18 Apr 2017
        /// Summary : Function to save user reviews discard/rejection reasons data to cache and return it from cache.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DiscardReasons> GetUserReviewsDiscardReasons()
        {
            IEnumerable<DiscardReasons> objReasons = null;

            try
            {
                objReasons = _cache.GetFromCache<IEnumerable<DiscardReasons>>("BW_UserReviewsDiscardReasons", new TimeSpan(1, 0, 0, 0), () => _userReviewsRepo.GetUserReviewsDiscardReasons());
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.Cache.UserReviews.UserReviewsCache");
            }

            return objReasons;

        }   // End of GetUserReviewsDiscardReasons

    }   // class
}   // namespace
