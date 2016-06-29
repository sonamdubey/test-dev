using Bikewale.Entities.UserReviews;
using System.Collections.Generic;

namespace Bikewale.Interfaces.UserReviews
{
    public interface IUserReviewsCache
    {
        IEnumerable<ReviewEntity> GetBikeReviewsList(uint startIndex, uint endIndex, uint modelId, uint versionId, FilterBy filter, out uint totalReviews);
    }
}
