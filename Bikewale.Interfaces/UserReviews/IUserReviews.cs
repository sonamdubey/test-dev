using Bikewale.Entities.UserReviews;
using System.Collections.Generic;

namespace Bikewale.Interfaces.UserReviews
{
    public interface IUserReviews
    {
        UserReviewsData GetUserReviewsData();
        IEnumerable<UserReviewQuestion> GetUserReviewQuestions(UserReviewsInputEntity inputParams);
        IEnumerable<UserReviewQuestion> GetUserReviewQuestions(UserReviewsInputEntity inputParams, UserReviewsData objUserReviewQuestions);
        uint SaveUserRatings(string overAllrating, string ratingQuestionAns, string userName, string emailId, uint makeId, uint modelId);
        bool SaveUserReviews(uint reviewId, string tipsnAdvices, string comment, string commentTitle, string reviewsQuestionAns);

    }
}
