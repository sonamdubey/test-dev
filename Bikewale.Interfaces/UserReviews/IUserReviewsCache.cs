using Bikewale.Entities.UserReviews;
using Bikewale.Entities.UserReviews.Search;
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
    /// Description : Added GetReviewQuestionValuesByModel
    /// </summary>
    public interface IUserReviewsCache
    {
        ReviewListBase GetBikeReviewsList(uint startIndex, uint endIndex, uint modelId, uint versionId, FilterBy filter);
        UserReviewsData GetUserReviewsData();
        ReviewListBase GetUserReviews();       
        UserReviewSummary GetUserReviewSummaryWithRating(uint reviewId);
        BikeRatingsReviewsInfo GetBikeRatingsReviewsInfo(uint modelId);
        BikeReviewsInfo GetBikeReviewsInfo(uint modelId, uint? skipReviewId);
        Hashtable GetUserReviewsIdMapping();
        IEnumerable<UserReviewSummary> GetUserReviewSummaryList(IEnumerable<uint> reviewIdList);
        BikeReviewIdListByCategory GetReviewsIdListByModel(uint modelId);
        SearchResult GetUserReviewsList(InputFilters inputFilters, string searchQuery);
        QuestionsRatingValueByModel GetReviewQuestionValuesByModel(uint modelId);

    }
}
