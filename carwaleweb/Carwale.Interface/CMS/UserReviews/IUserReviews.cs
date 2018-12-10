using Carwale.DTOs.CMS.UserReviews;
using Carwale.DTOs.CMS;
using Carwale.Entity.CMS;
using Carwale.Entity.CMS.UserReviews;
using Carwale.Entity.Enum;
using System.Collections.Generic;
using System;
using Carwale.Entity.UserReviews;

namespace Carwale.Interfaces.CMS.UserReviews
{
    public interface IUserReviews
    {
        bool CheckVersionReview(string versionId, string email, string customerId, string modelId);
        string SaveDetails(UserReviewDetail userReviewDetail, string userRating);
        List<UserReviewDTO> GetUserReviewsByType(UserReviewsSorting type);
        UserReviewDetailDesktopDto GetUserReviewsDetails(int id);
        List<CarReviewBaseEntity> GetMostReviewedCars();
        Tuple<int, bool> SubmitRating(RatingDetails ratingDetails);
    }
}
