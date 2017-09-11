﻿using Bikewale.DTO.UserReviews;
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
        UserReviewRatingObject SaveUserRatings(InputRatingSaveEntity objInputRating);
        WriteReviewPageSubmitResponse SaveUserReviews(ReviewSubmitData objReviewData);
        ReviewListBase GetUserReviews(uint startIndex, uint endIndex, uint modelId, uint versionId, FilterBy filter);

        UserReviewRatingData GetRateBikeData(RateBikeInput objRateBike);

    

    }
}
