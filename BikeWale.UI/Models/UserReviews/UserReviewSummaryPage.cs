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

        public bool IsMobile { get; internal set; }
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

                    objData.RatingQuestions = objData.Summary.Questions.Where(c => c.Type == UserReviewQuestionType.Rating);
                    objData.ReviewQuestions = objData.Summary.Questions.Where(c => c.Type == UserReviewQuestionType.Review);

                    BindPageMetas(objData);
                }
                else
                {
                    status = StatusCodes.ContentNotFound;
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "UserReviewSummaryPage.GetData");
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
        
        #endregion

    }
}
