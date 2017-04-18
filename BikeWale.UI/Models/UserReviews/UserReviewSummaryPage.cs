using Bikewale.Interfaces.UserReviews;
using Bikewale.Notifications;
using System;

namespace Bikewale.Models.UserReviews
{
    public class UserReviewSummaryPage
    {
        #region Variables for dependency injection
        private readonly IUserReviews _userReviews = null;
        private uint _reviewId;
        #endregion

        #region Constructor
        public UserReviewSummaryPage(IUserReviews userReviews, uint reviewId)
        {
            _userReviews = userReviews;
            _reviewId = reviewId;
        }
        #endregion

        #region Functions
        public UserReviewSummaryVM GetData()
        {
            UserReviewSummaryVM objData = new UserReviewSummaryVM();
            try
            {
                objData.Summary = _userReviews.GetUserReviewSummary(_reviewId);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "UserReviewSummaryPage.GetData");
            }
            return objData;
        }
        #endregion

    }
}