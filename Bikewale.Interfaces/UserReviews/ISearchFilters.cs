using Bikewale.Entities.UserReviews.Search;
namespace Bikewale.Interfaces.UserReviews.Search
{
    /// <summary>
    /// Created By : Sushil Kumar on 7th May 2017
    /// Description : Interface to search user reviews based on categories
    /// </summary>
    public interface IUserReviewsSearch
    {
        SearchResult GetUserReviewsList(InputFilters inputFilters);
        SearchResult GetUserReviewsListDesktop(InputFilters inputFilters);
    }
}