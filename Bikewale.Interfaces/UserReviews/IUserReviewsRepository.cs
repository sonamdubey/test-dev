using Bikewale.Entities.UserReviews;
using System.Collections.Generic;

namespace Bikewale.Interfaces.UserReviews
{
    public interface IUserReviewsRepository
    {
        List<ReviewTaggedBikeEntity> GetMostReviewedBikesList(ushort totalRecords);
        List<ReviewTaggedBikeEntity> GetReviewedBikesList();

        List<ReviewsListEntity> GetMostReadReviews(ushort totalRecords);
        List<ReviewsListEntity> GetMostHelpfulReviews(ushort totalRecords);
        List<ReviewsListEntity> GetMostRecentReviews(ushort totalRecords);
        List<ReviewsListEntity> GetMostRatedReviews(ushort totalRecords);

        ReviewRatingEntity GetBikeRatings(uint modelId);
        ReviewListBase GetBikeReviewsList(uint startIndex, uint endIndex, uint modelId, uint versionId, FilterBy filter);

        ReviewDetailsEntity GetReviewDetails(uint reviewId);

        bool AbuseReview(uint reviewId, string comment, string userId);
        bool UpdateViews(uint reviewId);
        bool UpdateReviewUseful(uint reviewId, bool isHelpful);

        UserReviewsData GetUserReviewsData();
        uint SaveUserReviewRatings(string overAllrating, string ratingQuestionAns, string userName, string emailId, uint customerId, uint makeId, uint modelId, uint sourceId);
        bool SaveUserReviews(uint reviewId, string tipsnAdvices, string comment, string commentTitle, string reviewsQuestionAns);
        UserReviewSummary GetUserReviewSummary(uint reviewId);
    }   // class
}   // namespace
