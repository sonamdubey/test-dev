using BikewaleOpr.Entities.UserReviews;
using BikewaleOpr.Entity.UserReviews;
using System.Collections.Generic;

namespace BikewaleOpr.Interface.UserReviews
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 15 Apr 2017
    /// Summary : Interface have methods related to the user reviews.
    /// </summary>
    public interface IUserReviewsRepository
    {
        IEnumerable<ReviewBase> GetReviewsList(ReviewsInputFilters filter);
        IEnumerable<DiscardReasons> GetUserReviewsDiscardReasons();
        void UpdateUserReviewsStatus(uint reviewId, ReviewsStatus reviewStatus, uint moderatorId, ushort disapprovalReasonId, string review, string reviewTitle, string reviewTips);
        UserReviewSummary GetUserReviewSummary(uint reviewId);
    }
}
