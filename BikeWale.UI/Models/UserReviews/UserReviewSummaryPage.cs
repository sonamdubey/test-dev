using Bikewale.Interfaces.UserReviews;
using Bikewale.Notifications;
using System;
using System.Collections.Specialized;
using System.Web;

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
        private string strEncoded;
        #endregion

        #region Constructor
        public UserReviewSummaryPage(IUserReviews userReviews, uint reviewId)
        {
            _userReviews = userReviews;
            _reviewId = reviewId;
            ProcessQueryString();
        }
        #endregion

        #region Functions
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
                objData.WriteReviewLink = string.Format("/write-a-review/?q={0}", strEncoded);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "UserReviewSummaryPage.GetData");
            }
            return objData;
        }
        /// <summary>
        /// Created by : Aditi Srivastava on 18 Apr 2017
        /// Summary    : process query string
        /// </summary>
        private void ProcessQueryString()
        {
            if (HttpContext.Current.Request.QueryString != null && !String.IsNullOrEmpty(HttpContext.Current.Request.QueryString["q"]))
            strEncoded = HttpContext.Current.Request.QueryString["q"];
        }
        #endregion

    }
}