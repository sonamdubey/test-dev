using Bikewale.Entities.UserReviews;

namespace Bikewale.Interfaces.UserReviews
{
    public interface IUserReviewsCache
    {
        ReviewListBase GetBikeReviewsList(uint startIndex, uint endIndex, uint modelId, uint versionId, FilterBy filter);
    }
}
