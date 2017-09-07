using Bikewale.Entities.UserReviews;
using Bikewale.Models.UserReviews;
using System.Collections.Generic;

namespace Bikewale.Interfaces.UserReviews
{
    /// <summary>
    /// Modified by :   Sumit Kate on 26 Apr 2017
    /// Description :   Added GetUserReviews
    /// Modified by :   Snehal Dange on 01 Sep 2017
    /// Description :   Added GetRateBikeData
    /// </summary>
    public interface IUserReviews
    {
        UserReviewsData GetUserReviewsData();
        UserReviewSummary GetUserReviewSummary(uint reviewId);
        IEnumerable<UserReviewQuestion> GetUserReviewQuestions(UserReviewsInputEntity inputParams);
        IEnumerable<UserReviewQuestion> GetUserReviewQuestions(UserReviewsInputEntity inputParams, UserReviewsData objUserReviewQuestions);
        UserReviewRatingObject SaveUserRatings(string overAllrating, string ratingQuestionAns, string userName, string emailId, uint makeId, uint modelId, uint reviewId,string returnUrl,ushort platformId, string utmzCookieValue, ushort? sourceId);       
        WriteReviewPageSubmitResponse SaveUserReviews(ReviewSubmitData objReviewData);
        ReviewListBase GetUserReviews(uint startIndex, uint endIndex, uint modelId, uint versionId, FilterBy filter);

        UserReviewRatingData GetRateBikeData(uint modelId, uint reviewId, ulong customerId, ushort sourceId, uint selectedRating, bool isFake, string returnUrl, int contestsrc);

    }
}
