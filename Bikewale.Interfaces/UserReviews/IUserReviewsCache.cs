using Bikewale.Entities.UserReviews;

namespace Bikewale.Interfaces.UserReviews
{
    /// <summary>
    /// Modified by :   Sumit Kate on 26 Apr 2017
    /// Description :   Add GetUserReviews to store old user reviews
    /// Modified by Sajal Gupta on 05-05-2017
    /// Description : Added GetUserReviewSummaryWithRating
    /// </summary>
    public interface IUserReviewsCache
    {
        ReviewListBase GetBikeReviewsList(uint startIndex, uint endIndex, uint modelId, uint versionId, FilterBy filter);
        UserReviewsData GetUserReviewsData();
        ReviewListBase GetUserReviews();
        UserReviewSummary GetUserReviewSummaryWithRating(uint reviewId);
    }
}
