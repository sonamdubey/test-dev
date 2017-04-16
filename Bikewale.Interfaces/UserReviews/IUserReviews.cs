using Bikewale.Entities.UserReviews;
using System.Collections.Generic;

namespace Bikewale.Interfaces.UserReviews
{
    public interface IUserReviews
    {
        UserReviewsData GetUserReviewsData();
        IEnumerable<UserReviewQuestion> GetUserReviewQuestions(UserReviewsInputEntity inputParams);
        IEnumerable<UserReviewQuestion> GetUserReviewQuestions(UserReviewsInputEntity inputParams, UserReviewsData objUserReviewQuestions);
        bool SaveUserRatings(string overAllrating, string ratingQuestionAns, string userName, string emailId);
    }
}
