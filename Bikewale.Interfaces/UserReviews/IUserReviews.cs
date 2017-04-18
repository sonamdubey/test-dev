﻿using Bikewale.Entities.UserReviews;
using System.Collections.Generic;

namespace Bikewale.Interfaces.UserReviews
{
    public interface IUserReviews
    {
        UserReviewsData GetUserReviewsData();
        UserReviewSummary GetUserReviewSummary(uint reviewId);
        IEnumerable<UserReviewQuestion> GetUserReviewQuestions(UserReviewsInputEntity inputParams);
        IEnumerable<UserReviewQuestion> GetUserReviewQuestions(UserReviewsInputEntity inputParams, UserReviewsData objUserReviewQuestions);
        UserReviewRatingObject SaveUserRatings(string overAllrating, string ratingQuestionAns, string userName, string emailId, uint makeId, uint modelId, uint sourceId, uint reviewId);
        bool SaveUserReviews(uint reviewId, string tipsnAdvices, string comment, string commentTitle, string reviewsQuestionAns);
        bool SaveUserReviews(string encodedId, string tipsnAdvices, string comment, string commentTitle, string reviewsQuestionAns, string emailId, string userNamem, string makeName, string modelName);


    }
}
