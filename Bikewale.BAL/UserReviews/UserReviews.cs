
using Bikewale.DTO.UserReviews;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Customer;
using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Customer;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Models.UserReviews;
using Bikewale.Notifications;
using Bikewale.Utility;
using Bikewale.Utility.LinqHelpers;
using System;
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
        private readonly IBikeMaskingCacheRepository<BikeModelEntity, int> _objBikeModel = null;
        // container.RegisterType<IUserReviewsCache, Bikewale.Cache.UserReviews.UserReviewsCacheRepository>();

        public UserReviews(IUserReviewsCache userReviewsCache, IUserReviewsRepository userReviewsRepo, ICustomer<CustomerEntity, UInt32> objCustomer,
            ICustomerRepository<CustomerEntity, UInt32> objCustomerRepo, IBikeMaskingCacheRepository<BikeModelEntity, int> objBikeModel)
        {
            _userReviewsCache = userReviewsCache;
            _userReviewsRepo = userReviewsRepo;
            _objCustomer = objCustomer;
            _objCustomerRepo = objCustomerRepo;
            _objBikeModel = objBikeModel;

        }

        /// <summary>
        /// Created By : Sushil Kumar on 16th April 2017
        /// Description : To get all user reviews questions,ratings,overall ratings and price range data
        /// </summary>
        /// <returns></returns>
        public Entities.UserReviews.UserReviewsData GetUserReviewsData()
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
                Entities.UserReviews.UserReviewsData objUserReviewData = _userReviewsCache.GetUserReviewsData();
                objQuestions = GetUserReviewQuestions(inputParams, objUserReviewData);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.UserReviews.UserReviews.GetUserReviewQuestions(inputParams) ");
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
        public IEnumerable<UserReviewQuestion> GetUserReviewQuestions(UserReviewsInputEntity inputParams, Entities.UserReviews.UserReviewsData objUserReviewQuestions)
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
                ErrorClass.LogError(ex, "Bikewale.BAL.UserReviews.UserReviews.GetUserReviewQuestions(inputParams,objUserReviewQuestions)");
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
        /// Modified by :Snehal Dange on 7th September 2017
        /// Description :  Added InputRatingSaveEntity
        /// </summary>
        /// <param name="overAllrating"></param>
        /// <param name="ratingQuestionAns"></param>
        /// <param name="userName"></param>
        /// <param name="emailId"></param>
        /// <returns></returns>        
        public UserReviewRatingObject SaveUserRatings(InputRatingSaveEntity objInputRating)
        {
            UserReviewRatingObject objRating = null;
            try
            {
                objRating = new UserReviewRatingObject();
                CustomerEntityBase objCust = null;
                var reviewId = objInputRating.ReviewId;
                //check for user registration
                objCust = new CustomerEntityBase() { CustomerName = objInputRating.UserName, CustomerEmail = objInputRating.EmailId };
                objCust = ProcessUserCookie(objCust, ref reviewId);

                objRating.IsFake = _objCustomerRepo.IsFakeCustomer(objCust.CustomerId);

                if (!objRating.IsFake)
                {
                    objRating.ReviewId = _userReviewsRepo.SaveUserReviewRatings(objInputRating, (uint)objCust.CustomerId, reviewId);
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
                ErrorClass.LogError(ex, String.Format("Bikewale.BAL.UserReviews.UserReviews.SaveUserRatings({0},{1})", objInputRating.ModelId, objInputRating.EmailId));
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
        private bool SaveUserReviews(uint reviewId, string tipsnAdvices, string comment, string commentTitle, string reviewsQuestionAns, uint mileage)
        {
            bool isSuccess = false;
            if (reviewId > 0)
            {
                //checked for Customer login and cookie details
                //if unauthorized request return false
                isSuccess = _userReviewsRepo.SaveUserReviews(reviewId, tipsnAdvices, comment, commentTitle, reviewsQuestionAns, mileage);
            }

            return isSuccess;
        }


        /// <summary>
        /// Created By : Sushil Kumar on 16th April 2017
        /// Description : Get user reviews summary for all pages
        /// Modified by : Ashutosh Sharma on 24-Aug-2017
        /// Description : Added lines to get SelectedRatingText and MinHeading
        /// </summary>
        /// <param name="reviewId"></param>
        /// <returns></returns>
        public UserReviewSummary GetUserReviewSummary(uint reviewId)
        {
            UserReviewSummary objSummary = null;
            Entities.UserReviews.UserReviewsData objUserReviewData = null;

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
                            objQuestion.SelectedRatingText = question.SelectedRatingText;
                            objQuestion.MinHeading = question.MinHeading;
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

                ErrorClass.LogError(ex, string.Format("Bikewale.BAL.UserReviews.UserReviews.GetUserReviewSummary({0})", reviewId));
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
                ErrorClass.LogError(ex, String.Format("ProcessUserCookie({0})", Newtonsoft.Json.JsonConvert.SerializeObject(customer)));
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
                ErrorClass.LogError(ex, String.Format("RegisterBuyer({0})", Newtonsoft.Json.JsonConvert.SerializeObject(customer)));
            }
        }

        /// <summary>
        /// Created By Sajal Gupta on 19-04-2017
        /// Description : Function to save user reviews with server side validations
        /// Modified by Sajal Gupta on 13-07*-2017
        /// Descriptiopn : Added milaeage field.
        /// Modified By :   Vishnu Teja Yalakuntla on 07 Sep 2017
        /// Description :   Removed EncodedId
        /// Modified By : Sanskar Gupta on 09-01-2018
        /// Description : Bug fix (scenario where a user could have entered less than 300 characters or inject JS via the description field.)
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
        public WriteReviewPageSubmitResponse SaveUserReviews(ReviewSubmitData objReviewData)
        {
            WriteReviewPageSubmitResponse objResponse = null;
            try
            {
                if (objReviewData != null && objReviewData.ReviewId > 0 && objReviewData.CustomerId > 0 && _userReviewsRepo.IsUserVerified(objReviewData.ReviewId, objReviewData.CustomerId))
                {

                    objResponse = new WriteReviewPageSubmitResponse();

                    string jsRemovedReview = StringHtmlHelpers.removeMaliciousCode(objReviewData.ReviewDescription);
                    string trimmedReview = null;
                    if (jsRemovedReview != null)
                        trimmedReview = (StringHtmlHelpers.StripHtml(jsRemovedReview)).Trim();

                    if (string.IsNullOrEmpty(trimmedReview) || trimmedReview.Length < 300)
                    {
                        //Invalid Review_Description
                        objResponse.IsSuccess = false;
                        objResponse.ReviewErrorText = "Your review should contain at least 300 characters.";
                    }

                    objReviewData.ReviewDescription = trimmedReview;

                    objResponse.IsSuccess = SaveUserReviews(objReviewData.ReviewId, objReviewData.ReviewTips, objReviewData.ReviewDescription, objReviewData.ReviewTitle, objReviewData.ReviewQuestion, Convert.ToUInt32(objReviewData.Mileage));
                   


                    if (!string.IsNullOrEmpty(objReviewData.ReviewDescription))
                        UserReviewsEmails.SendReviewSubmissionEmail(objReviewData.UserName, objReviewData.EmailId, objReviewData.MakeName, objReviewData.ModelName);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "SaveUserReviews");
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
                ErrorClass.LogError(ex, String.Format("GetUserReviews({0},{1},{2},{3},{4})", startIndex, endIndex, modelId, versionId, filter));
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


        /// <summary>
        /// Created By  :   Snehal Dange on 1st Sep 2017
        /// Summary     :   Get User Rating
        /// </summary>
        private void GetUserRatings(UserReviewRatingData objUserReviewRatingData)
        {

            try
            {
                Entities.UserReviews.UserReviewsData objReviewData = null;
                objReviewData = GetUserReviewsData();
                if (objReviewData != null)
                {
                    if (objUserReviewRatingData.ReviewId == 0)
                    {

                        if (objReviewData.Questions != null)
                        {

                            UserReviewsInputEntity filter = new UserReviewsInputEntity()
                            {
                                Type = Entities.UserReviews.UserReviewQuestionType.Rating
                            };
                            var objQuestions = GetUserReviewQuestions(filter, objReviewData);
                            if (objQuestions != null)
                            {
                                objQuestions.FirstOrDefault(x => x.Id == 2).SubQuestionId = 3;
                                objQuestions.FirstOrDefault(x => x.Id == 3).Visibility = false;
                                objQuestions.FirstOrDefault(x => x.Id == 3).IsRequired = false;
                                objUserReviewRatingData.Questions = objQuestions;
                            }

                        }
                        if (objReviewData.OverallRating != null)
                        {
                            objUserReviewRatingData.OverallRating = objReviewData.OverallRating;
                        }


                    }
                    else
                    {
                        UserReviewSummary objUserReviewDataReview = GetUserReviewSummary(objUserReviewRatingData.ReviewId);

                        if (objReviewData.OverallRating != null)
                        {
                            objUserReviewRatingData.OverallRating = objReviewData.OverallRating;
                        }

                        if (objUserReviewDataReview != null)
                        {
                            objUserReviewRatingData.ReviewsOverAllrating = objUserReviewDataReview.OverallRatingId.ToString();
                            objUserReviewRatingData.Questions = objUserReviewDataReview.Questions;
                            objUserReviewRatingData.CustomerEmail = objUserReviewDataReview.CustomerEmail;
                            objUserReviewRatingData.CustomerName = objUserReviewDataReview.CustomerName;
                        }
                    }

                    var objLastPrice = objReviewData.PriceRange.Last();
                    if (objUserReviewRatingData.ObjModelEntity != null && objUserReviewRatingData.ObjModelEntity.MinPrice >= objLastPrice.MaxPrice)
                    {
                        objUserReviewRatingData.PriceRangeId = (ushort)objLastPrice.RangeId;
                    }
                    else
                    {
                        var objFirstPriceRange = objReviewData.PriceRange.First(x => x.MinPrice <= objUserReviewRatingData.ObjModelEntity.MinPrice && x.MaxPrice >= objUserReviewRatingData.ObjModelEntity.MinPrice);
                        if (objFirstPriceRange != null)
                        {
                            objUserReviewRatingData.PriceRangeId = (ushort)objFirstPriceRange.RangeId;
                        }

                    }
                }
            }

            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Bikewale.BAL.UserReviews.GetUserRatings({0})", objUserReviewRatingData.ReviewId));
            }

        }

        /// <summary>
        /// Created By  :   Snehal Dange on 1st Sep 2017
        /// Summary     :   Get Rate Bike data for aoi services
        /// </summary>
        public UserReviewRatingData GetRateBikeData(RateBikeInput objRateBike)
        {
            UserReviewRatingData objUserReviewRatingData = new UserReviewRatingData();
            try
            {
                objUserReviewRatingData.ObjModelEntity = _objBikeModel.GetById((int)objRateBike.ModelId);
                objUserReviewRatingData.ReviewId = objRateBike.ReviewId;
                objUserReviewRatingData.IsFake = objRateBike.IsFake;
                GetUserRatings(objUserReviewRatingData);

                objUserReviewRatingData.SelectedRating = objRateBike.SelectedRating;
                if (objUserReviewRatingData != null && objUserReviewRatingData.ObjModelEntity != null)
                {
                    objUserReviewRatingData.SourceId = objRateBike.SourceId;
                    objUserReviewRatingData.ReturnUrl = objRateBike.ReturnUrl;
                    objUserReviewRatingData.ContestSrc = objRateBike.Contestsrc;

                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Bikewale.BAL.UserReviews.GetRateBikeData({0},{1},{2},{3},{4},{5},{6})", objRateBike.ModelId, objRateBike.ReviewId, objRateBike.CustomerId, objRateBike.SourceId, objRateBike.SelectedRating, objRateBike.ReturnUrl, objRateBike.Contestsrc));
            }
            return objUserReviewRatingData;
        }

        /// <summary>
        /// Created by sajal gupta on 30-10-2017
        /// descriptiion : function to get rating app screen data
        /// </summary>
        /// <param name="priceRangeId"></param>
        /// <returns></returns>
        public IEnumerable<UserReviewQuestion> GetRatingAppScreenData(ushort priceRangeId)
        {
            IEnumerable<UserReviewQuestion> questions = null;
            try
            {
                UserReviewsData objUserReviewData = _userReviewsCache.GetUserReviewsData();
                if (objUserReviewData != null && objUserReviewData.Questions != null)
                {
                    UserReviewsInputEntity filter = new UserReviewsInputEntity()
                    {
                        Type = Entities.UserReviews.UserReviewQuestionType.Review,
                        PriceRangeId = priceRangeId
                    };
                    questions = GetUserReviewQuestions(filter, objUserReviewData);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Bikewale.BAL.UserReviews.GetRatingAppScreenData({0})", priceRangeId));
            }
            return questions;
        }


    }   // Class
}   // Namespace
