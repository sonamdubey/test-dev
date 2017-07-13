﻿using Bikewale.Entities.Customer;
using Bikewale.Entities.NewBikeSearch;
using Bikewale.Entities.UserReviews;
using Bikewale.Entities.UserReviews.Search;
using Bikewale.Interfaces.Customer;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Models.UserReviews;
using Bikewale.Notifications;
using Bikewale.Utility;
using Bikewale.Utility.LinqHelpers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

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
                if (filters.PriceRangeId > 0)
                {
                    filterExpression = filterExpression.And(m => m.PriceRangeIds != null && m.PriceRangeIds.Contains(filters.PriceRangeId));
                }
            }
            return filterExpression.Compile();
        }

        /// <summary>
        /// Created By : Sushil Kumar on 16th April 2017
        /// Description : Create function to be executed for user reviews linq filters
        /// </summary>
        /// <param name="overAllrating"></param>
        /// <param name="ratingQuestionAns"></param>
        /// <param name="userName"></param>
        /// <param name="emailId"></param>
        /// <returns></returns>        
        public UserReviewRatingObject SaveUserRatings(string overAllrating, string ratingQuestionAns, string userName, string emailId, uint makeId, uint modelId, uint reviewId, string returnUrl, ushort platformId, ushort? sourceId)
        {

            UserReviewRatingObject objRating = null;
            try
            {
                objRating = new UserReviewRatingObject();

                CustomerEntityBase objCust = null;
                //check for user registration
                objCust = new CustomerEntityBase() { CustomerName = userName, CustomerEmail = emailId };
                objCust = ProcessUserCookie(objCust, ref reviewId);

                objRating.IsFake = _objCustomerRepo.IsFakeCustomer(objCust.CustomerId);

                if (!objRating.IsFake)
                {
                    objRating.ReviewId = _userReviewsRepo.SaveUserReviewRatings(overAllrating, ratingQuestionAns, userName, emailId, (uint)objCust.CustomerId, makeId, modelId, reviewId, returnUrl, platformId, sourceId);
                    objRating.CustomerId = objCust.CustomerId;
                }
                else
                {
                    objRating.ReviewId = reviewId;
                    objRating.CustomerId = objCust.CustomerId;
                }


            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, String.Format("Bikewale.BAL.UserReviews.UserReviews.SaveUserRatings({0},{1})", modelId, emailId));
            }

            return objRating;

        }

        /// <summary>
        /// Created By : Sushil Kumar on 16th April 2017
        /// Description : save user review written by user 
        /// </summary>
        /// <param name="reviewId"></param>
        /// <param name="tipsnAdvices"></param>
        /// <param name="comment"></param>
        /// <param name="commentTitle"></param>
        /// <param name="reviewsQuestionAns"></param>
        /// <returns></returns>
        public bool SaveUserReviews(uint reviewId, string tipsnAdvices, string comment, string commentTitle, string reviewsQuestionAns)
        {
            bool isSuccess = false;
            if (reviewId > 0)
            {
                //checked for Customer login and cookie details
                //if unauthorized request return false
                isSuccess = _userReviewsRepo.SaveUserReviews(reviewId, tipsnAdvices, comment, commentTitle, reviewsQuestionAns);
            }

            return isSuccess;
        }


        /// <summary>
        /// Created By : Sushil Kumar on 16th April 2017
        /// Description : Get user reviews summary for all pages
        /// </summary>
        /// <param name="reviewId"></param>
        /// <returns></returns>
        public UserReviewSummary GetUserReviewSummary(uint reviewId)
        {
            UserReviewSummary objSummary = null;
            UserReviewsData objUserReviewData = null;

            try
            {
                objSummary = _userReviewsRepo.GetUserReviewSummary(reviewId);
                objUserReviewData = _userReviewsCache.GetUserReviewsData();

                if (objSummary != null && objSummary.Questions != null)
                {
                    var objQuestions = new List<UserReviewQuestion>();
                    foreach (var question in objSummary.Questions)
                    {
                        var objQuestion = objUserReviewData.Questions.FirstOrDefault(q => q.Id == question.Id);

                        if (objQuestion != null)
                        {
                            objQuestion.SelectedRatingId = question.SelectedRatingId;
                            if (objQuestion.SelectedRatingId == 0)
                            {
                                objQuestion.Visibility = false;
                                objQuestion.IsRequired = false;
                            }
                            objQuestions.Add(objQuestion);
                        }
                    }

                    objQuestions.FirstOrDefault(x => x.Id == 2).SubQuestionId = 3;

                    objSummary.Questions = objQuestions;

                    objSummary.OverallRating = objUserReviewData.OverallRating.FirstOrDefault(x => x.Id == objSummary.OverallRatingId);
                    objSummary.TipsDescriptionSmall = objSummary.Tips.Truncate(150);
                }
            }
            catch (Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, string.Format("Bikewale.BAL.UserReviews.UserReviews.GetUserReviewSummary({0})", reviewId));
            }

            return objSummary;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 17 Apr 2017
        /// Description :   Process User Cookie
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        private CustomerEntityBase ProcessUserCookie(CustomerEntityBase customer, ref uint reviewId)
        {
            try
            {
                string customerEmail = customer.CustomerEmail, customerName = customer.CustomerName;

                //if tempcurrentuser cookie exists return the buyers basic details
                BWCookies.GetBuyerDetailsFromCookie(ref customer);

                //Is new Customer or email id is changed 
                if ((customer.CustomerId == 0 && !String.IsNullOrEmpty(customer.CustomerEmail)) || customer.CustomerEmail != customerEmail)
                {
                    //perform customer registration with submitted details
                    customer.CustomerName = customerName;
                    customer.CustomerEmail = customerEmail;
                    RegisterCustomer(customer);
                    //customer registration successful
                    if (customer.CustomerId > 0)
                    {
                        //create tempcurrentuser cookie
                        string customerData = String.Format("{0}&{1}&{2}&{3}", customer.CustomerName, customer.CustomerEmail, customer.CustomerMobile, BikewaleSecurity.EncryptUserId(Convert.ToInt64(customer.CustomerId)));
                        BWCookies.SetBuyerDetailsCookie(customerData);
                    }
                    reviewId = 0;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, String.Format("ProcessUserCookie({0})", Newtonsoft.Json.JsonConvert.SerializeObject(customer)));
            }
            return customer;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 17 Apr 2017
        /// Description :   Register Customer
        /// </summary>
        /// <param name="customer"></param>
        private void RegisterCustomer(CustomerEntityBase customer)
        {
            CustomerEntity objCust = null;
            try
            {
                //Check if Customer exists
                objCust = _objCustomer.GetByEmailMobile(customer.CustomerEmail, customer.CustomerMobile);
                if (objCust != null && objCust.CustomerId > 0)
                {
                    //If exists update the mobile number and name
                    _objCustomerRepo.UpdateCustomerMobileNumber(customer.CustomerMobile, customer.CustomerEmail, customer.CustomerName);
                    //set customer id for further use
                    customer.CustomerId = objCust.CustomerId;
                }
                else
                {
                    //Register the new customer
                    objCust = new CustomerEntity() { CustomerName = customer.CustomerName, CustomerEmail = customer.CustomerEmail, CustomerMobile = customer.CustomerMobile, ClientIP = CurrentUser.GetClientIP() };
                    customer.CustomerId = _objCustomer.Add(objCust);
                }

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, String.Format("RegisterBuyer({0})", Newtonsoft.Json.JsonConvert.SerializeObject(customer)));
            }
        }

        /// <summary>
        /// Created By Sajal Gupta on 19-04-2017
        /// Description : Function to save user reviews with server side validations
        /// </summary>
        /// <param name="encodedId"></param>
        /// <param name="tipsnAdvices"></param>
        /// <param name="comment"></param>
        /// <param name="commentTitle"></param>
        /// <param name="reviewsQuestionAns"></param>
        /// <param name="emailId"></param>
        /// <param name="userName"></param>
        /// <param name="makeName"></param>
        /// <param name="modelName"></param>
        /// <param name="reviewDescription"></param>
        /// <param name="reviewTitle"></param>
        /// <returns></returns>
        public WriteReviewPageSubmitResponse SaveUserReviews(string encodedId, string tipsnAdvices, string comment, string commentTitle, string reviewsQuestionAns, string emailId, string userName, string makeName, string modelName, string mileage)
        {            
            WriteReviewPageSubmitResponse objResponse = null;
            try
            {


                if (!string.IsNullOrEmpty(encodedId))
                {
                    uint _reviewId;
                    ulong _customerId;

                    string decodedString = Utils.Utils.DecryptTripleDES(encodedId);
                    NameValueCollection queryCollection = HttpUtility.ParseQueryString(decodedString);

                    uint.TryParse(queryCollection["reviewid"], out _reviewId);
                    ulong.TryParse(queryCollection["customerid"], out _customerId);


                    if (_reviewId > 0 && _customerId > 0 && _userReviewsRepo.IsUserVerified(_reviewId, _customerId))
                    {
                        bool isValid = true;

                        objResponse = new WriteReviewPageSubmitResponse();

                        if (!string.IsNullOrEmpty(comment) && comment.Length < 300 && string.IsNullOrEmpty(commentTitle))
                        {
                            objResponse.ReviewErrorText = "Your review should contain as least 300 characters";
                            objResponse.TitleErrorText = "Please provide a title for your review.";
                            isValid = false;
                        }
                        else if (string.IsNullOrEmpty(comment) && !string.IsNullOrEmpty(commentTitle))
                        {
                            objResponse.ReviewErrorText = "Your review should contain as least 300 characters";
                            isValid = false;
                        }
                        else if (!string.IsNullOrEmpty(comment) && comment.Length > 300 && string.IsNullOrEmpty(commentTitle))
                        {
                            objResponse.TitleErrorText = "Please provide a title for your review.";
                            isValid = false;
                        }

                        if (isValid)
                        {
                            objResponse.IsSuccess = SaveUserReviews(_reviewId, tipsnAdvices, comment, commentTitle, reviewsQuestionAns);

                            if (!string.IsNullOrEmpty(comment))
                                UserReviewsEmails.SendReviewSubmissionEmail(userName, emailId, makeName, modelName);

                            if (mileage != null && mileage.Length > 0)
                                _userReviewsRepo.SaveUserReviewMileage(_reviewId, mileage);
                        }
                    }
                    else
                        objResponse.IsSuccess = false;
                }
                else
                    objResponse.IsSuccess = false;
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "SaveUserReviews");
            }
            return objResponse;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 26 Apr 2017
        /// Description :   Returns the filtered User reviews based on filter values provided
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="modelId"></param>
        /// <param name="versionId"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public ReviewListBase GetUserReviews(uint startIndex, uint endIndex, uint modelId, uint versionId, FilterBy filter)
        {
            ReviewListBase filteredReviews = null;
            bool isAsc;
            try
            {

                filteredReviews = _userReviewsCache.GetUserReviews();
                if (filteredReviews != null && filteredReviews.TotalReviews > 0)
                {
                    filteredReviews.ReviewList = filteredReviews.ReviewList.Sort(ProcessOrderBy(filter, out isAsc), isAsc);
                    filteredReviews.ReviewList = filteredReviews.ReviewList.Where(ProcessInputFilter(modelId, versionId));
                    filteredReviews.TotalReviews = (uint)(filteredReviews.ReviewList != null ? filteredReviews.ReviewList.Count() : 0);
                    if (filteredReviews.TotalReviews > 0 && startIndex > 0 && endIndex > 0 && startIndex < endIndex)
                    {
                        startIndex = startIndex == 1 ? 0 : startIndex;
                        filteredReviews.ReviewList = filteredReviews.ReviewList.Skip((int)startIndex).Take((int)(endIndex - startIndex));
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, String.Format("GetUserReviews({0},{1},{2},{3},{4})", startIndex, endIndex, modelId, versionId, filter));
            }
            return filteredReviews;
        }

        /// <summary>
        /// Created By  :   Sumit Kate on 26 Apr 2017
        /// Description :   Create function to be executed for user reviews linq filters
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        private Func<ReviewEntity, bool> ProcessInputFilter(uint modelId, uint versionId)
        {
            Expression<Func<ReviewEntity, bool>> filterExpression = PredicateBuilder.True<ReviewEntity>();

            if (modelId > 0)
            {
                filterExpression = filterExpression.And(m => m.TaggedBike.ModelEntity.ModelId == modelId);
            }
            if (versionId > 0)
            {
                filterExpression = filterExpression.And(m => m.TaggedBike.VersionEntity.VersionId == versionId);
            }
            return filterExpression.Compile();
        }

        /// <summary>
        /// Created By  :   Sumit Kate on 26 Apr 2017
        /// Summary     :   Process orderby filter
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        private Func<ReviewEntity, float> ProcessOrderBy(FilterBy filter, out bool isAsc)
        {
            isAsc = false;
            switch (filter)
            {
                case FilterBy.MostRecent:
                    return m => (m.ReviewDate.Ticks);
                case FilterBy.MostHelpful:
                    return m => (m.Liked);
                case FilterBy.MostRead:
                    return m => (m.Viewed);
                case FilterBy.MostRated:
                    return m => (m.OverAllRating.OverAllRating);
                default:
                    return m => (m.Liked);
            }
        }
        
    }   // Class
}   // Namespace
