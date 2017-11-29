using Bikewale.Entities.UserReviews;
using System.Collections;
using System.Collections.Generic;

namespace Bikewale.Interfaces.UserReviews
{
    /// <summary>
    /// Modified by sajal gupta on 13-07-2017
    /// Description : Added SaveUserReviewMileage, GetReviewQuestionValuesByModel, GetRecentReviews, SaveUserReviews
    /// Modified By : Sushil Kumar on 11th Oct 2017
    /// Description : To cache popular bikes with expert reviews count and by cityid
    /// Modified By : Snehal Dange on 21st Nov 2017
    /// Description : Added GetBikesWithReviewsByMake() to get popular bikes with most recent and most helpful reviews
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
        uint SaveUserReviewRatings(InputRatingSaveEntity inputSaveEntity, uint customerId, uint reviewId);
        bool SaveUserReviews(uint reviewId, string tipsnAdvices, string comment, string commentTitle, string reviewsQuestionAns, uint mileage);
        UserReviewSummary GetUserReviewSummary(uint reviewId);
        bool IsUserVerified(uint reviewId, ulong customerId);
        ReviewListBase GetUserReviews();
        UserReviewSummary GetUserReviewSummaryWithRating(uint reviewId);
        BikeReviewsInfo GetBikeReviewsInfo(uint modelId, uint? skipReviewId);
        BikeRatingsReviewsInfo GetBikeRatingsReviewsInfo(uint modelId);
        Hashtable GetUserReviewsIdMapping();

        IEnumerable<UserReviewSummary> GetUserReviewSummaryList(string reviewIdList);

        BikeReviewIdListByCategory GetReviewsIdListByModel(uint modelId);
        QuestionsRatingValueByModel GetReviewQuestionValuesByModel(uint modelId);

        IEnumerable<RecentReviewsWidget> GetRecentReviews();
        IEnumerable<RecentReviewsWidget> GetUserReviewsWinners();
        IEnumerable<TopRatedBikes> GetTopRatedBikes(uint topCount);
        IEnumerable<TopRatedBikes> GetTopRatedBikes(uint topCount, uint cityId);
        IEnumerable<PopularBikesWithExpertReviews> GetPopularBikesWithExpertReviews(ushort topCount);
        IEnumerable<PopularBikesWithExpertReviews> GetPopularBikesWithExpertReviewsByCity(ushort topCount, uint cityId);
        IEnumerable<PopularBikesWithUserReviews> GetPopularBikesWithUserReviewsByMake(uint makeId);
        IEnumerable<BikesWithReviewByMake> GetBikesWithReviewsByMake(uint makeId);
    }   // class
}   // namespace
