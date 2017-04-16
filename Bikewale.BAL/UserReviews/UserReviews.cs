using Bikewale.Entities.Customer;
using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.Customer;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Notifications;
using Bikewale.Utility.LinqHelpers;
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
        private readonly IUserReviewsRepository _userReviewsRepo = null;
        private readonly ICustomer<CustomerEntity, UInt32> _objCustomer = null;
        private readonly ICustomerRepository<CustomerEntity, UInt32> _objCustomerRepo = null;

        // container.RegisterType<IUserReviewsCache, Bikewale.Cache.UserReviews.UserReviewsCacheRepository>();

        public UserReviews(IUserReviewsCache userReviewsCache, IUserReviewsRepository userReviewsRepo, ICustomer<CustomerEntity, UInt32> objCustomer,
            ICustomerRepository<CustomerEntity, UInt32> objCustomerRepo)
        {
            _userReviewsCache = userReviewsCache;
            _userReviewsRepo = userReviewsRepo;
            _objCustomer = objCustomer;
            _objCustomerRepo = objCustomerRepo;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="overAllrating"></param>
        /// <param name="ratingQuestionAns"></param>
        /// <param name="userName"></param>
        /// <param name="emailId"></param>
        /// <returns></returns>
        public bool SaveUserRatings(string overAllrating, string ratingQuestionAns, string userName, string emailId)
        {
            bool isSaved = false;
            CustomerEntity objCust = null;
            //check for user registration
            //Check if Customer exists
            objCust = _objCustomer.GetByEmail(emailId);

            if (objCust != null && objCust.CustomerId > 0)
            {
                //If exists update the mobile number and name
                _objCustomerRepo.UpdateCustomerMobileNumber("", emailId, userName);
                //set customer id for further use
            }
            else
            {
                //if not registered register and get customerid
                //Register the new customer and send login details
                objCust = new CustomerEntity() { CustomerName = userName, CustomerEmail = emailId, CustomerMobile = "" };
                if (objCust.CustomerId < 1)
                {
                    objCust.CustomerId = _objCustomer.Add(objCust);
                }
                //mail to betriggered here
            }
            uint reviewId = 0;
            _userReviewsRepo.SaveUserReviewRatings(overAllrating, ratingQuestionAns, userName, emailId, (uint)objCust.CustomerId,reviewId);

            isSaved = reviewId > 0;
            //return valid status

            return isSaved;

        }
    }   // Class
}   // Namespace
