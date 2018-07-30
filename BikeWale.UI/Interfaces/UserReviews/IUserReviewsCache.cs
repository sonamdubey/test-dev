using Bikewale.Entities.UserReviews;
using System.Collections;
using System.Collections.Generic;

namespace Bikewale.Interfaces.UserReviews
{
    /// <summary>
    /// Modified by :   Sumit Kate on 26 Apr 2017
    /// Description :   Add GetUserReviews to store old user reviews
    /// Modified by Sajal Gupta on 05-05-2017
    /// Description : Added GetUserReviewSummaryWithRating, GetUserReviewsIdMapping
    /// Modified By : Sushil Kumar on 7th May 2017
    /// Description : Added methods to get bike reviews by search query and bike reviews and ratings info
    /// Modified by sajal gupta on 14-07-2017
    /// Description : Added GetReviewQuestionValuesByModel,
    /// Modified by sajal gupta on 02-08-2017
    /// Description : Added GetRecentReviews, GetTopRatedBikes
    /// Modified By : Sushil Kumar on 11th Oct 2017
    /// Description : To cache popular bikes with expert reviews count and by cityid
    /// Modified By: Snehal Dange on 20th Nov 2017
    /// Description: To cache popular bikes with most recent and most helpful reviews;
    /// </summary>
    public interface IUserReviewsCache
    {
        ReviewListBase GetBikeReviewsList(uint startIndex, uint endIndex, uint modelId, uint versionId, FilterBy filter);
        UserReviewsData GetUserReviewsData();
        ReviewListBase GetUserReviews();
        UserReviewSummary GetUserReviewSummaryWithRating(uint reviewId);
        BikeRatingsReviewsInfo GetBikeRatingsReviewsInfo(uint modelId);
        BikeReviewsInfo GetBikeReviewsInfo(uint modelId);
        Hashtable GetUserReviewsIdMapping();
        IEnumerable<UserReviewSummary> GetUserReviewSummaryList(IEnumerable<uint> reviewIdList);
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
    }
}
