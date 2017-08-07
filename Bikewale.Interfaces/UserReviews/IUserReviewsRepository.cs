using Bikewale.Entities.UserReviews;
using Bikewale.Entities.UserReviews.Search;
using System.Collections;
using System.Collections.Generic;

namespace Bikewale.Interfaces.UserReviews
{
    /// <summary>
    /// Modified by sajal gupta on 13-07-2017
    /// Description : Added SaveUserReviewMileage, GetReviewQuestionValuesByModel, GetRecentReviews
    /// </summary>
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
        uint SaveUserReviewRatings(string overAllrating, string ratingQuestionAns, string userName, string emailId, uint customerId, uint makeId, uint modelId, uint reviewId, string returnUrl, ushort platformId, string utmzCookieValue, ushort? sourceId);
        bool SaveUserReviews(uint reviewId, string tipsnAdvices, string comment, string commentTitle, string reviewsQuestionAns);
        UserReviewSummary GetUserReviewSummary(uint reviewId);
        bool IsUserVerified(uint reviewId, ulong customerId);
        ReviewListBase GetUserReviews();
        SearchResult GetUserReviewsList(string searchQuery);
        UserReviewSummary GetUserReviewSummaryWithRating(uint reviewId);
        BikeReviewsInfo GetBikeReviewsInfo(uint modelId, uint? skipReviewId);
        BikeRatingsReviewsInfo GetBikeRatingsReviewsInfo(uint modelId);
        Hashtable GetUserReviewsIdMapping();

        IEnumerable<UserReviewSummary> GetUserReviewSummaryList(string reviewIdList);

        BikeReviewIdListByCategory GetReviewsIdListByModel(uint modelId);

        bool SaveUserReviewMileage(uint reviewId, string mileage);
        QuestionsRatingValueByModel GetReviewQuestionValuesByModel(uint modelId);

        IEnumerable<RecentReviewsWidget> GetRecentReviews();
    }   // class
}   // namespace
