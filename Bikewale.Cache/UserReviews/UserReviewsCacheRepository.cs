using Bikewale.Entities.UserReviews;
using Bikewale.Entities.UserReviews.Search;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Interfaces.UserReviews.Search;
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
        private readonly IUserReviewsSearch _objUserReviewSearch;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="objUserReviews"></param>
        public UserReviewsCacheRepository(ICacheManager cache, IUserReviewsRepository objUserReviews, IUserReviewsSearch objUserReviewSearch)
        {
            _cache = cache;
            _objUserReviews = objUserReviews;
            _objUserReviewSearch = objUserReviewSearch;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputFilters"></param>
        /// <returns></returns>
        public SearchResult GetUserReviewsList(InputFilters inputFilters)
        {
            SearchResult reviews = null;
            if (inputFilters != null && (!String.IsNullOrEmpty(inputFilters.Model) || !String.IsNullOrEmpty(inputFilters.Make)))
            {
                string key = "BW_UserReviews_MO_" + inputFilters.Model;

                try
                {
                    if (!string.IsNullOrEmpty(inputFilters.CAT))
                    {

                    }

                    reviews = _cache.GetFromCache<SearchResult>(key, new TimeSpan(24, 0, 0), () => _objUserReviewSearch.GetUserReviewsList(inputFilters));
                }
                catch (Exception ex)
                {
                    ErrorClass objErr = new ErrorClass(ex, "BikeMakesCacheRepository.GetUserReviewsData");
                }
            }
            return reviews;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public BikeReviewsInfo GetBikeReviewsInfo(uint modelId, uint? skipReviewId)
        {
            BikeReviewsInfo reviews = null;
            string key = "BW_BikeReviewsInfo_MO_" + modelId;

            if (skipReviewId.HasValue)
                key = key + "_RId" + skipReviewId.Value;

            try
            {
                reviews = _cache.GetFromCache<BikeReviewsInfo>(key, new TimeSpan(24, 0, 0), () => _objUserReviews.GetBikeReviewsInfo(modelId, skipReviewId));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikeMakesCacheRepository.GetUserReviewsData");
            }
            return reviews;
        }

        public BikeRatingsReviewsInfo GetBikeRatingsReviewsInfo(uint modelId)
        {
            BikeRatingsReviewsInfo reviews = null;
            string key = "BW_BikeRatingsReviewsInfo_MO_" + modelId;
            try
            {
                reviews = _cache.GetFromCache<BikeRatingsReviewsInfo>(key, new TimeSpan(24, 0, 0), () => _objUserReviews.GetBikeRatingsReviewsInfo(modelId));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikeMakesCacheRepository.GetUserReviewsData");
            }
            return reviews;
        }

        /// <summary>
        /// Created by Sajal Gupta on 05-05-2017
        /// Description : Return user review summary by calling dal
        /// </summary>
        /// <param name="reviewId"></param>
        /// <returns></returns>
        public UserReviewSummary GetUserReviewSummaryWithRating(uint reviewId)
        {
            UserReviewSummary objUserReviewSummary = null;
            string key = string.Format("BW_UserReviewDetails_{0}", reviewId);
            try
            {
                objUserReviewSummary = _cache.GetFromCache<UserReviewSummary>(key, new TimeSpan(1, 0, 0), () => _objUserReviews.GetUserReviewSummaryWithRating(reviewId));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("BikeMakesCacheRepository.GetUserReviewSummaryWithRating {0}", reviewId));
            }
            return objUserReviewSummary;
        }
    }
}
