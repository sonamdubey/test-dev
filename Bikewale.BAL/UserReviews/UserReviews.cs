using Bikewale.Cache.Core;
using Bikewale.DAL.UserReviews;
using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Notifications;
using Bikewale.Utility.LinqHelpers;
using Microsoft.Practices.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


namespace Bikewale.BAL.UserReviews
{
    /// <summary>
    /// Created By : Sushil Kumar on 16th April 2017
    /// Summary : Class have business logic for the user reviews
    /// </summary>
    public class UserReviews : IUserReviews
    {
        private readonly IUserReviewsCache _userReviewsCache = null;

        public UserReviews()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<ICacheManager, MemcacheManager>();
                container.RegisterType<IUserReviewsRepository, UserReviewsRepository>();
                container.RegisterType<IUserReviewsCache, Bikewale.Cache.UserReviews.UserReviewsCacheRepository>();
                _userReviewsCache = container.Resolve<IUserReviewsCache>();
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar on 16th April 2017
        /// Description : To get all user reviews questions,ratings,overall ratings and price range data
        /// </summary>
        /// <returns></returns>
        public UserReviewsData GetUserReviewsData()
        {
            return _userReviewsCache.GetUserReviewsData();
        }

        /// <summary>
        /// Created By : Sushil Kumar on 16th April 2017
        /// Description : To get all user reviews questions filtered with inputs
        /// </summary>
        /// <param name="inputParams"></param>
        /// <returns></returns>
        public IEnumerable<UserReviewQuestion> GetUserReviewQuestions(UserReviewsInputEntity inputParams)
        {
            IEnumerable<UserReviewQuestion> objQuestions = null;
            try
            {
                UserReviewsData objUserReviewData = _userReviewsCache.GetUserReviewsData();
                objQuestions = GetUserReviewQuestions(inputParams, objUserReviewData);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.BAL.UserReviews.UserReviews.GetUserReviewQuestions(inputParams) ");
            }

            return objQuestions;
        }

        /// <summary>
        /// Created By : Sushil Kumar on 16th April 2017
        /// Description : To get all user reviews questions filtered with inputs
        /// </summary>
        /// <param name="inputParams"></param>
        /// <param name="objUserReviewQuestions"></param>
        /// <returns></returns>
        public IEnumerable<UserReviewQuestion> GetUserReviewQuestions(UserReviewsInputEntity inputParams, UserReviewsData objUserReviewQuestions)
        {
            IEnumerable<UserReviewQuestion> objQuestions = null;
            try
            {
                if (objUserReviewQuestions != null && objUserReviewQuestions.Questions != null)
                {

                    objQuestions = objUserReviewQuestions.Questions.Where(ProcessInputFilter(inputParams));

                    if (objQuestions != null)
                    {
                        var objQuestionratings = objUserReviewQuestions.Ratings.GroupBy(x => x.QuestionId);

                        foreach (var question in objQuestions)
                        {
                            foreach (var rating in objQuestionratings)
                            {
                                if (rating.Key == question.Id)
                                {
                                    question.Rating = rating;
                                    break;
                                }
                            }
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.BAL.UserReviews.UserReviews.GetUserReviewQuestions(inputParams,objUserReviewQuestions)");
            }
            return objQuestions;
        }

        /// <summary>
        /// Created By : Sushil Kumar on 16th April 2017
        /// Description : Create function to be executed for user reviews linq filters
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        private Func<UserReviewQuestion, bool> ProcessInputFilter(UserReviewsInputEntity filters)
        {
            Expression<Func<UserReviewQuestion, bool>> filterExpression = PredicateBuilder.True<UserReviewQuestion>();
            if (filters != null)
            {
                if (filters.Type > 0)
                {
                    filterExpression = filterExpression.And(m => m.Type == filters.Type);
                }
                if (filters.DisplayType > 0)
                {
                    filterExpression = filterExpression.And(m => m.DisplayType == filters.DisplayType);
                }
                if (filters.IsRequired)
                {
                    filterExpression = filterExpression.And(m => m.IsRequired == filters.IsRequired);
                }
            }
            return filterExpression.Compile();
        }



    }   // Class
}   // Namespace
