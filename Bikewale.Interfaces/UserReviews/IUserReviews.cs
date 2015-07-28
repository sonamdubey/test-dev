using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Entities.UserReviews;

namespace Bikewale.Interfaces.UserReviews
{
    public interface IUserReviews
    {
        List<ReviewTaggedBikeEntity> GetMostReviewedBikesList(ushort totalRecords);
        List<ReviewTaggedBikeEntity> GetReviewedBikesList();

        List<ReviewsListEntity> GetMostReadReviews(ushort totalRecords);
        List<ReviewsListEntity> GetMostHelpfulReviews(ushort totalRecords);
        List<ReviewsListEntity> GetMostRecentReviews(ushort totalRecords);
        List<ReviewsListEntity> GetMostRatedReviews(ushort totalRecords);

        ReviewRatingEntity GetBikeRatings(uint modelId);
        List<ReviewEntity> GetBikeReviewsList(uint startIndex, uint endIndex, uint modelId, uint versionId, FilterBy filter, out uint totalReviews);

        ReviewDetailsEntity GetReviewDetails(uint reviewId);

        bool AbuseReview(uint reviewId, string comment, string userId);
        bool UpdateViews(uint reviewId);
        bool UpdateReviewUseful(uint reviewId, bool isHelpful);
    }   // class
}   // namespace
