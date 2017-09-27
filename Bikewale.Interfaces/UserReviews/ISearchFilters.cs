using Bikewale.Entities.UserReviews;
using Bikewale.Entities.UserReviews.Search;
namespace Bikewale.Interfaces.UserReviews.Search
{
    /// <summary>
    /// Created By : Sushil Kumar on 7th May 2017
    /// Description : Interface to search user reviews based on categories
    /// Modified by Sajal Gupta on 11 Sep 2017
    /// Descripttion : Added GetUserReviewsList
    /// 
    /// </summary>
    public interface IUserReviewsSearch
    {
        SearchResult GetUserReviewsList(InputFilters inputFilters);
        SearchResult GetUserReviewsList(ReviewDataCombinedFilter inputFilters);
        SearchResult GetUserReviewsList(InputFilters inputFilters, uint skipTopCount);
    }
}