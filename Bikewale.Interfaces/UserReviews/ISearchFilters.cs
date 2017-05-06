using Bikewale.Entities.UserReviews.Search;
namespace Bikewale.Interfaces.UserReviews.Search
{
    public interface IUserReviewsSearch
    {
        SearchResult GetUserReviewsList(InputFilters inputFilters);
    }
}