﻿using Bikewale.Entities.UserReviews;
using Bikewale.Entities.UserReviews.Search;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Interfaces.UserReviews.Search;
using Bikewale.Notifications;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
        public SearchResult GetUserReviewsList(InputFilters inputFilters, string searchQuery)
        {
            SearchResult reviews = null;
            if (inputFilters != null && (!String.IsNullOrEmpty(inputFilters.Model) || !String.IsNullOrEmpty(inputFilters.Make)))
            {
                string key = "BW_UserReviews_MO_V1_" + inputFilters.Model;
                bool skipDataLimit = (inputFilters.PN * inputFilters.PS) > 24;
                try
                {                    
                    if (inputFilters.SO > 0)
                    {
                        key = string.Format("{0}_CAT_{1}", key, inputFilters.SO);
                    }

                    if (skipDataLimit)
                    {
                        key = string.Format("{0}_PN_{1}_PS_{2}", key, inputFilters.PN, inputFilters.PS);
                    }
                    else
                    {
                        key += "_PN_1_PS_24";
                    }

                    reviews = _cache.GetFromCache<SearchResult>(key, new TimeSpan((skipDataLimit ? 1 : 24), 0, 0), () => _objUserReviews.GetUserReviewsList(searchQuery));


                    if (reviews != null && reviews.Result != null && !skipDataLimit)
                    {
                        if (inputFilters.PN != 1)
                        {
                            reviews.Result = reviews.Result.Skip((inputFilters.PN - 1) * inputFilters.PS);
                        }

                        reviews.Result = reviews.Result.Take(inputFilters.PS);

                        if (inputFilters.SkipReviewId > 0)
                        {
                            reviews.Result = reviews.Result.Where(x => x.ReviewId != inputFilters.SkipReviewId);
                        }
                    }
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
                ErrorClass objErr = new ErrorClass(ex, "UserReviewsCacheRepository.GetReviewQuestionValuesByModel");
            }
            return objRatingsList;
        }

        public BikeRatingsReviewsInfo GetBikeRatingsReviewsInfo(uint modelId)
        {            
            BikeRatingsReviewsInfo reviews = null;
            string key = "BW_BikeRatingsReviewsInfo_MO_V1_" + modelId;
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
            string key = string.Format("BW_UserReviewDetails_V2_{0}", reviewId);
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
                htResult = _cache.GetFromCache<Hashtable>(key, new TimeSpan(30, 0, 0, 0), () => _objUserReviews.GetUserReviewsIdMapping());
            }
            catch (Exception ex)
            {
                ErrorClass objError = new ErrorClass(ex, "BikeMakesCacheRepository.GetUserReviewsIdMapping");
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
            catch(Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("BikeMakesCacheRepository.GetReviewsIdListByModel {0}", modelId));
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
                string[] keys;
                keys = new string[reviewIdList.Count()];
                dictIdKeys = new Dictionary<string, string>();
                foreach(var id in reviewIdList)
                {
                    dictIdKeys.Add(id.ToString(), string.Format("BW_UserReviewDetails_V2_{0}", id));
                }                
                Func<string, IEnumerable<UserReviewSummary>> fnCallback =_objUserReviews.GetUserReviewSummaryList;

                objSummaryList = _cache.GetListFromCache<UserReviewSummary>(dictIdKeys,
                    new TimeSpan(1, 0, 0),
                    fnCallback);                
            }
            catch(Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikeMakesCacheRepository.GetUserReviewSummaryList");
            }
            return objSummaryList;
        }
    }
}
