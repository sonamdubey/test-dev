using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Notifications;
using System;
using System.Collections;
using System.Collections.Generic;

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
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikeMakesCacheRepository.GetBikeReviewsList");
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

            string key = "BW_UserReviewsRatings_V2";
            try
            {
                reviewsRatings = _cache.GetFromCache<UserReviewsData>(key, new TimeSpan(24, 0, 0), () => _objUserReviews.GetUserReviewsData());
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikeMakesCacheRepository.GetUserReviewsData");
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
                ErrorClass.LogError(ex, "BikeMakesCacheRepository.GetUserReviewsData");
            }
            return reviews;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public BikeReviewsInfo GetBikeReviewsInfo(uint modelId)
        {
            BikeReviewsInfo reviews = null;
            string key = "BW_BikeReviewsInfo_MO_" + modelId;

            try
            {
                reviews = _cache.GetFromCache<BikeReviewsInfo>(key, new TimeSpan(1, 0, 0), () => _objUserReviews.GetBikeReviewsInfo(modelId, 0));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikeMakesCacheRepository.GetUserReviewsData");
            }
            return reviews;
        }

        /// <summary>
        /// Created by Sajal Gupta on 14-07-2017
        /// Description : Added caching logic for dal call GetReviewQuestionValuesByModel
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public QuestionsRatingValueByModel GetReviewQuestionValuesByModel(uint modelId)
        {
            QuestionsRatingValueByModel objRatingsList = null;
            string key = "BW_ReviewQuestionsValue_MO_" + modelId;

            try
            {
                objRatingsList = _cache.GetFromCache<QuestionsRatingValueByModel>(key, new TimeSpan(1, 0, 0), () => _objUserReviews.GetReviewQuestionValuesByModel(modelId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "UserReviewsCacheRepository.GetReviewQuestionValuesByModel");
            }
            return objRatingsList;
        }

        public BikeRatingsReviewsInfo GetBikeRatingsReviewsInfo(uint modelId)
        {
            BikeRatingsReviewsInfo reviews = null;
            string key = "BW_BikeRatingsReviewsInfo_MO_V4_" + modelId;
            try
            {
                reviews = _cache.GetFromCache<BikeRatingsReviewsInfo>(key, new TimeSpan(24, 0, 0), () => _objUserReviews.GetBikeRatingsReviewsInfo(modelId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikeMakesCacheRepository.GetUserReviewsData");
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
            string key = string.Format("BW_UserReviewDetails_V4_{0}", reviewId);
            try
            {
                objUserReviewSummary = _cache.GetFromCache<UserReviewSummary>(key, new TimeSpan(1, 0, 0), () => _objUserReviews.GetUserReviewSummaryWithRating(reviewId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikeMakesCacheRepository.GetUserReviewSummaryWithRating {0}", reviewId));
            }
            return objUserReviewSummary;
        }

        /// <summary>
        /// Created by Sajal gupta on 11-05-2017
        /// Descriptio : Creates hash table for reviews id mapping
        /// </summary>
        /// <returns></returns>
        public Hashtable GetUserReviewsIdMapping()
        {
            Hashtable htResult = null;
            string key = "BW_UserReviewIdMapping";
            try
            {
                htResult = _cache.GetFromCache<Hashtable>(key, new TimeSpan(1, 0, 0, 0), () => _objUserReviews.GetUserReviewsIdMapping());
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikeMakesCacheRepository.GetUserReviewsIdMapping");
            }
            return htResult;
        }

        /// <summary>
        /// Created by Sajal Gupta on 12-06-2017
        /// Description : this gets all review id list for particular model from dal
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public BikeReviewIdListByCategory GetReviewsIdListByModel(uint modelId)
        {
            BikeReviewIdListByCategory objReviewIdList = null;
            try
            {
                string key = "BW_ReviewIdList_V1_" + modelId;
                objReviewIdList = _cache.GetFromCache<BikeReviewIdListByCategory>(key, new TimeSpan(6, 0, 0), () => _objUserReviews.GetReviewsIdListByModel(modelId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikeMakesCacheRepository.GetReviewsIdListByModel {0}", modelId));
            }
            return objReviewIdList;
        }

        /// <summary>
        /// Created by Sajal Gupta on 12-06-2017
        /// Description : this gets all review id summary list from dal
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public IEnumerable<UserReviewSummary> GetUserReviewSummaryList(IEnumerable<uint> reviewIdList)
        {

            IEnumerable<UserReviewSummary> objSummaryList = null;
            Dictionary<string, string> dictIdKeys = null;
            try
            {
                if(reviewIdList != null)
                {
                    dictIdKeys = new Dictionary<string, string>();
                    foreach (var id in reviewIdList)
                    {
                        dictIdKeys.Add(id.ToString(), string.Format("BW_UserReviewDetails_V4_{0}", id));
                    }
                    Func<string, IEnumerable<UserReviewSummary>> fnCallback = _objUserReviews.GetUserReviewSummaryList;

                    objSummaryList = _cache.GetListFromCache<UserReviewSummary>(dictIdKeys,
                        new TimeSpan(1, 0, 0),
                        fnCallback);
                }
                
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikeMakesCacheRepository.GetUserReviewSummaryList");
            }
            return objSummaryList;
        }

        /// <summary>
        /// Created by Sajal Gupta on 02-08-2017
        /// Description : Cache layer to cache recent reviews data
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RecentReviewsWidget> GetRecentReviews()
        {
            IEnumerable<RecentReviewsWidget> objList = null;
            try
            {
                string key = "BW_RecentReviews";
                objList = _cache.GetFromCache<IEnumerable<RecentReviewsWidget>>(key, new TimeSpan(6, 0, 0), () => _objUserReviews.GetRecentReviews());
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikeMakesCacheRepository.GetRecentReviews");
            }
            return objList;
        }

        /// <summary>
        /// Created by: Vivek Singh Tomar On 11th Aug 2017
        /// Summary: Cache layer to cache user reviews winners
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RecentReviewsWidget> GetUserReviewsWinners()
        {
            IEnumerable<RecentReviewsWidget> objReviewsWinnersList = null;
            try
            {
                string key = "BW_UserReviewsWinners";
                objReviewsWinnersList = _cache.GetFromCache<IEnumerable<RecentReviewsWidget>>(key, new TimeSpan(6, 0, 0, 0), () => _objUserReviews.GetUserReviewsWinners());
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Cache.UserReviews.UserReviewsCacheRepository.GetUserReviewsWinners");
            }
            return objReviewsWinnersList;
        }

        /// <summary>
        /// Creaetd by sajal Gupta to cache top rated bikes for 1 day
        /// </summary>
        /// <param name="topCount"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public IEnumerable<TopRatedBikes> GetTopRatedBikes(uint topCount, uint cityId)
        {
            IEnumerable<TopRatedBikes> objTopRatedBikesWidget = null;
            string key = string.Empty;
            try
            {
                key = string.Format("BW_TopRatedBikes_Cnt_{0}_city_{1}", topCount, cityId);
                objTopRatedBikesWidget = _cache.GetFromCache<IEnumerable<TopRatedBikes>>(key, new TimeSpan(1, 0, 0), () => (IEnumerable<TopRatedBikes>)_objUserReviews.GetTopRatedBikes(topCount, cityId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("UserReviewsCacheRepository.GetTopRatedBikes: topCount: {0}, CityId {1}", topCount, cityId));
            }
            return objTopRatedBikesWidget;
        }

        public IEnumerable<TopRatedBikes> GetTopRatedBikes(uint topCount)
        {
            IEnumerable<TopRatedBikes> objTopRatedBikesWidget = null;
            string key = string.Empty;
            try
            {
                key = string.Format("BW_TopRatedBikes_Cnt_{0}", topCount);
                objTopRatedBikesWidget = _cache.GetFromCache<IEnumerable<TopRatedBikes>>(key, new TimeSpan(1, 0, 0), () => (IEnumerable<TopRatedBikes>)_objUserReviews.GetTopRatedBikes(topCount));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("UserReviewsCacheRepository.GetTopRatedBikes: topCount: {0}", topCount));
            }
            return objTopRatedBikesWidget;
        }

        /// <summary>
        /// Created By : Sushil Kumar on 11th Oct 2017
        /// Description : To cache popular bikes with expert reviews count
        /// </summary>
        /// <param name="topCount"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public IEnumerable<PopularBikesWithExpertReviews> GetPopularBikesWithExpertReviews(ushort topCount)
        {
            IEnumerable<PopularBikesWithExpertReviews> objTopRatedBikesWidget = null;
            string key = string.Empty;
            try
            {
                key = "BW_PopularBikesWithExpertReviews_V1_Cnt_" + topCount;
                objTopRatedBikesWidget = _cache.GetFromCache<IEnumerable<PopularBikesWithExpertReviews>>(key, new TimeSpan(24, 0, 0), () => (IEnumerable<PopularBikesWithExpertReviews>)_objUserReviews.GetPopularBikesWithExpertReviews(topCount));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("UserReviewsCacheRepository.GetPopularBikesWithExpertReviews: topCount: {0}", topCount));
            }
            return objTopRatedBikesWidget;
        }

        /// <summary>
        /// Created By : Sushil Kumar on 11th Oct 2017
        /// Description : To cache popular bikes with expert reviews count by city
        /// </summary>
        /// <param name="topCount"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public IEnumerable<PopularBikesWithExpertReviews> GetPopularBikesWithExpertReviewsByCity(ushort topCount, uint cityId)
        {
            IEnumerable<PopularBikesWithExpertReviews> objTopRatedBikesWidget = null;
            string key = string.Empty;
            try
            {
                key = string.Format("BW_PopularBikesWithExpertReviews_V1_Cnt_{0}_City_{1}", topCount, cityId);
                objTopRatedBikesWidget = _cache.GetFromCache<IEnumerable<PopularBikesWithExpertReviews>>(key, new TimeSpan(6, 0, 0), () => (IEnumerable<PopularBikesWithExpertReviews>)_objUserReviews.GetPopularBikesWithExpertReviewsByCity(topCount, cityId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("UserReviewsCacheRepository.GetPopularBikesWithExpertReviewsByCity: topCount: {0}, CityId {1}", topCount, cityId));
            }
            return objTopRatedBikesWidget;
        }


        /// <summary>
        /// Created By : Sushil Kumar on 11th Oct 2017
        /// Description : To cache popular bikes with expert reviews count by city
        /// </summary>
        /// <param name="topCount"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public IEnumerable<PopularBikesWithUserReviews> GetPopularBikesWithUserReviewsByMake(uint makeId)
        {
            IEnumerable<PopularBikesWithUserReviews> objBikes = null;
            try
            {
                string key = string.Format("BW_PopularBikesWithUserReviews_Make_{0}", makeId);
                objBikes = _cache.GetFromCache<IEnumerable<PopularBikesWithUserReviews>>(key, new TimeSpan(24, 0, 0), () => (IEnumerable<PopularBikesWithUserReviews>)_objUserReviews.GetPopularBikesWithUserReviewsByMake(makeId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("UserReviewsCacheRepository.GetPopularBikesWithUserReviewsByMake: Make: {0}", makeId));
            }
            return objBikes;
        }

        /// <summary>
        /// Created BY: Snehal Dange on 20th Sep 2017
        /// Descrption: To cache most recent and helpful reviews by make
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public IEnumerable<BikesWithReviewByMake> GetBikesWithReviewsByMake(uint makeId)
        {
            IEnumerable<BikesWithReviewByMake> objBikes = null;
            try
            {
                if (makeId > 0 && _objUserReviews!=null)
                {
                    string key = string.Format("BW_PopularBikesWithRecentAndHelpfulReviews_Make_{0}", makeId);
                    objBikes = _cache.GetFromCache<IEnumerable<BikesWithReviewByMake>>(key, new TimeSpan(24, 0, 0), () => (IEnumerable<BikesWithReviewByMake>)_objUserReviews.GetBikesWithReviewsByMake(makeId));

                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, string.Format("UserReviewsCacheRepository.GetBikesWithReviewsByMake: Make: {0}", makeId));
            }
            return objBikes;
        }
    }
}
