using BikewaleOpr.Entities.UserReviews;
using BikewaleOpr.Entity.UserReviews;
using System.Collections.Generic;

namespace BikewaleOpr.Interface.UserReviews
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 15 Apr 2017
    /// Summary : Interface have methods related to the user reviews.
    /// Modified by Sajal Gupta on 16-06-2017. Added GetRatingsList;
    /// Modified by Sajal Gupta on 19-06-2017 . Added GetUserReviewDetails, UpdateUserReviewRatingsStatus
    /// </summary>
    public interface IUserReviewsRepository
    {
        IEnumerable<ReviewBase> GetReviewsList(ReviewsInputFilters filter);

        IEnumerable<DiscardReasons> GetUserReviewsDiscardReasons();

        uint UpdateUserReviewsStatus(uint reviewId, ReviewsStatus reviewStatus, uint moderatorId, ushort disapprovalReasonId, string review, string reviewTitle, string reviewTips, bool iShortListed);

        UserReviewSummary GetUserReviewSummary(uint reviewId);

        ReviewBase GetUserReviewWithEmailIdReviewId(uint reviewId, string emailId);

        IEnumerable<ReviewBase> GetRatingsList();

        IEnumerable<BikeRatingApproveEntity> GetUserReviewDetails(string reviewIds);

        bool UpdateUserReviewRatingsStatus(string reviewIds, ReviewsStatus reviewStatus, uint moderatorId, ushort disapprovalReasonId);

        bool SaveUserReviewWinner(uint reviewId, uint moderatorId);
    }
}
