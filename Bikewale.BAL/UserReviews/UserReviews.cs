using Bikewale.DAL.UserReviews;
using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Utility.LinqHelpers;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


namespace Bikewale.BAL.UserReviews
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 12 June 2014
    /// Summary : Class have business logic for the user reviews
    /// </summary>
    public class UserReviews : IUserReviews
    {
        private readonly IUserReviews userReviewsRepository = null;
        private readonly IUserReviewsCache _userReviewsCache = null;

        public UserReviews()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IUserReviews, UserReviewsRepository>();
                container.RegisterType<IUserReviewsCache, Bikewale.Cache.UserReviews.UserReviewsCacheRepository>();
                userReviewsRepository = container.Resolve<IUserReviews>();
                _userReviewsCache = container.Resolve<IUserReviewsCache>();
            }
        }

        public List<ReviewTaggedBikeEntity> GetMostReviewedBikesList(ushort totalRecords)
        {
            List<ReviewTaggedBikeEntity> objTaggedBikes = null;

            objTaggedBikes = userReviewsRepository.GetMostReviewedBikesList(totalRecords);

            return objTaggedBikes;
        }

        public List<ReviewTaggedBikeEntity> GetReviewedBikesList()
        {
            List<ReviewTaggedBikeEntity> objTaggedBikes = null;

            objTaggedBikes = userReviewsRepository.GetReviewedBikesList();

            return objTaggedBikes;
        }

        public List<ReviewsListEntity> GetMostReadReviews(ushort totalRecords)
        {
            List<ReviewsListEntity> objReviews = null;

            objReviews = userReviewsRepository.GetMostReadReviews(totalRecords);

            return objReviews;
        }

        public List<ReviewsListEntity> GetMostHelpfulReviews(ushort totalRecords)
        {
            List<ReviewsListEntity> objReviews = null;

            objReviews = userReviewsRepository.GetMostHelpfulReviews(totalRecords);

            return objReviews;
        }

        public List<ReviewsListEntity> GetMostRecentReviews(ushort totalRecords)
        {
            List<ReviewsListEntity> objReviews = null;

            objReviews = userReviewsRepository.GetMostRecentReviews(totalRecords);

            return objReviews;
        }

        public List<ReviewsListEntity> GetMostRatedReviews(ushort totalRecords)
        {
            List<ReviewsListEntity> objReviews = null;

            objReviews = userReviewsRepository.GetMostRatedReviews(totalRecords);

            return objReviews;
        }

        public ReviewRatingEntity GetBikeRatings(uint modelId)
        {
            ReviewRatingEntity objRating = null;

            objRating = userReviewsRepository.GetBikeRatings(modelId);

            return objRating;
        }

        public ReviewListBase GetBikeReviewsList(uint startIndex, uint endIndex, uint modelId, uint versionId, FilterBy filter)
        {
            ReviewListBase objReviewList = null;

            objReviewList = userReviewsRepository.GetBikeReviewsList(startIndex, endIndex, modelId, versionId, filter);

            return objReviewList;
        }

        public ReviewDetailsEntity GetReviewDetails(uint reviewId)
        {
            ReviewDetailsEntity objReview = null;

            objReview = userReviewsRepository.GetReviewDetails(reviewId);

            return objReview;
        }

        public bool UpdateViews(uint reviewId)
        {
            bool isViesUpdated = false;

            isViesUpdated = userReviewsRepository.UpdateViews(reviewId);

            return isViesUpdated;
        }

        public bool AbuseReview(uint reviewId, string comment, string userId)
        {
            bool isReviewAbused = false;

            isReviewAbused = userReviewsRepository.AbuseReview(reviewId, comment, userId);

            return isReviewAbused;
        }

        public bool UpdateReviewUseful(uint reviewId, bool isHelpful)
        {
            bool isUpdated = false;

            isUpdated = userReviewsRepository.UpdateReviewUseful(reviewId, isHelpful);

            return isUpdated;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public UserReviewsData GetUserReviewsData()
        {
            return _userReviewsCache.GetUserReviewsData();
        }

        /// <summary>
        /// Created by  :   Sajal Gupta on 10-04-2017
        /// Description :   Returns models according to filters
        /// </summary>
        /// <returns></returns>
        private IEnumerable<UserReviewQuestion> GetQuestions(UserReviewsInputEntity inputParams)
        {
            IEnumerable<UserReviewQuestion> objQuestions = null; bool isAsc;
            try
            {

                var objUserReviewQuestions = _userReviewsCache.GetUserReviewsData();

                if (objUserReviewQuestions != null && objUserReviewQuestions.Questions != null)
                {

                    // objQuestions = objQuestions.Sort(ProcessOrderBy(sortBy, out isAsc), isAsc);
                    objQuestions = objQuestions.Where(ProcessInputFilter(inputParams));

                    if (objQuestions != null && objQuestions.Count() > 0)
                    {
                        //objQuestions = objQuestions.Page(inputParams.PageNo, inputParams.PageSize);
                    }

                }

            }
            catch (Exception ex)
            {
                //ErrorClass objErr = new ErrorClass(ex, "Bikewale.BAL.BikeData.Upcoming.GetModels");
            }
            return objQuestions;
        }

        private Func<UserReviewQuestion, bool> ProcessInputFilter(UserReviewsInputEntity filters)
        {
            Expression<Func<UserReviewQuestion, bool>> filterExpression = PredicateBuilder.True<UserReviewQuestion>();
            if (filters != null)
            {
                if (filters.DisplayType > 0)
                {
                    filterExpression = filterExpression.And(m => m.DisplayType == filters.DisplayType);
                }
                if (filters.Type > 0)
                {
                    filterExpression = filterExpression.And(m => m.Type == filters.Type);
                }
                //if (filters.BodyStyleId > 0)
                //{
                //    filterExpression = filterExpression.And(m => m.BodyStyleId == filters.BodyStyleId);
                //}
                //if (filters.Year > 0)
                //{
                //    filterExpression = filterExpression.And(m => m.ExpectedLaunchedDate.Year == filters.Year);
                //}
            }
            return filterExpression.Compile();
        }


    }   // Class
}   // Namespace
