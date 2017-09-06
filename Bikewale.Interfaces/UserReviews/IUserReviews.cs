using Bikewale.DTO.UserReviews;
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
        Entities.UserReviews.UserReviewsData GetUserReviewsData();
        UserReviewSummary GetUserReviewSummary(uint reviewId);
        IEnumerable<UserReviewQuestion> GetUserReviewQuestions(UserReviewsInputEntity inputParams);
        IEnumerable<UserReviewQuestion> GetUserReviewQuestions(UserReviewsInputEntity inputParams, Entities.UserReviews.UserReviewsData objUserReviewQuestions);
        UserReviewRatingObject SaveUserRatings(string overAllrating, string ratingQuestionAns, string userName, string emailId, uint makeId, uint modelId, uint reviewId,string returnUrl,ushort platformId, string utmzCookieValue, ushort? sourceId);
        bool SaveUserReviews(uint reviewId, string tipsnAdvices, string comment, string commentTitle, string reviewsQuestionAns);
        WriteReviewPageSubmitResponse SaveUserReviews(string encodedId, string tipsnAdvices, string comment, string commentTitle, string reviewsQuestionAns, string emailId, string userName, string makeName, string modelName, string mileage);
        ReviewListBase GetUserReviews(uint startIndex, uint endIndex, uint modelId, uint versionId, FilterBy filter);

        UserReviewRatingData GetRateBikeData(RateBikeInput objRateBike);

    }
}
