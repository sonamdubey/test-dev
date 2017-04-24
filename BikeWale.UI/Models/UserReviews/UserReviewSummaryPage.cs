using Bikewale.Entities;
using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Notifications;
using System;
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
                    objData.PrevPageUrl = Bikewale.Utility.UserReviews.FormatPreviousPageUrl(objData.Summary.PageSource, objData.Summary.Make.MaskingName, objData.Summary.Model.MaskingName);
                    objData.WriteReviewLink = string.Format("/write-a-review/?q={0}", _strEncoded);
                    objData.Summary.Questions = objData.Summary.Questions.Where(x => x.Type == UserReviewQuestionType.Review);
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
            objData.PageMetaTags.CanonicalUrl = string.Format("{0}/user-reviews/review-summary/{1}/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrlForJs, objData.Summary.Model.ModelId);
        }
        #endregion

    }
}