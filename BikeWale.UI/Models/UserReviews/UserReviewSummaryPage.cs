using Bikewale.Entities;
using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Notifications;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Bikewale.Models.UserReviews
{
    /// <summary>
    /// Created by : Aditi Srivastava on 18 Apr 2017
    /// Summary    : Model for user review summary
    /// </summary>
    public class UserReviewSummaryPage
    {
        #region Variables for dependency injection
        private readonly IUserReviews _userReviews = null;
        private uint _reviewId;
        private string _strEncoded;
        #endregion

        #region Public variables
        public StatusCodes status;
        public bool IsDesktop;
        #endregion

        #region Constructor
        public UserReviewSummaryPage(IUserReviews userReviews, uint reviewId, string q)
        {
            _userReviews = userReviews;
            _reviewId = reviewId;
            _strEncoded = q;
            CheckQueryString();
        }
        #endregion

        #region Functions
        /// <summary>
        /// Created by : Aditi Srivastava on 19 Apr 2017
        /// Summary    : Check if query string is empty
        /// </summary>
        private void CheckQueryString()
        {
            if (string.IsNullOrEmpty(_strEncoded))
                status = StatusCodes.ContentNotFound;
        }
        /// <summary>
        /// Created by : Aditi Srivastava on 18 Apr 2017
        /// Summary    : Get data for user review summary
        /// Modified by : Ashutosh Sharma on 28-Aug-2017
        /// Description : Calling BindQuestions method
        /// </summary>
        public UserReviewSummaryVM GetData()
        {
            UserReviewSummaryVM objData = new UserReviewSummaryVM();
            try
            {
                objData.Summary = _userReviews.GetUserReviewSummary(_reviewId);
                if (objData.Summary != null)
                {
                    status = StatusCodes.ContentFound;
                    objData.WriteReviewLink = string.Format("/write-a-review/?q={0}", _strEncoded);
                    BindQuestions(objData);
                    BindPageMetas(objData);
                }
                else
                {
                    status = StatusCodes.ContentNotFound;
                }

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "UserReviewSummaryPage.GetData");
            }
            return objData;
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 19 Apr 2017
        /// Summary    : Bind page metas
        /// </summary>
        private void BindPageMetas(UserReviewSummaryVM objData)
        {
            objData.PageMetaTags.Title = string.Format("Review Summary | {0} {1} - BikeWale", objData.Summary.Make.MakeName, objData.Summary.Model.ModelName);
            objData.PageMetaTags.Description = string.Format("See summary of {0}'s {1} {2} review.", objData.Summary.CustomerName, objData.Summary.Make.MakeName, objData.Summary.Model.ModelName);
            objData.PageMetaTags.CanonicalUrl = string.Format("https://www.bikewale.com/rate-your-bike/{0}/", objData.Summary.Model.ModelId);
        }
        /// <summary>
        /// Created by : Ashutosh Sharma on 24-Aug-2017
        /// Description : Method to bind rating and review questions
        /// </summary>
        /// <param name="objPage"></param>
        private void BindQuestions(UserReviewSummaryVM objPage)
        {
            try
            {
                objPage.RatingQuestions = new Collection<UserReviewQuestion>();
                objPage.ReviewQuestions = new Collection<UserReviewQuestion>();

                if (objPage.Summary != null)
                {
                    foreach (UserReviewQuestion ques in objPage.Summary.Questions)
                    {
                        if (ques.Type == UserReviewQuestionType.Rating)
                        {
                            if (ques.SelectedRatingId != 0)
                                objPage.RatingQuestions.Add(ques);
                        }
                        else
                            objPage.ReviewQuestions.Add(ques);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("UserReviewSummaryPage.BindQuestions() - ReviewId :{0}", _reviewId));
            }
        }
        #endregion

    }
}