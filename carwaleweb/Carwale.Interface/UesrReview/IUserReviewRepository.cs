using Carwale.Entity.UserReview;
using System;
using Carwale.Entity.Enum;

namespace Carwale.Interfaces.UesrReview
{
    public interface IUserReviewRepository
    {
        string SaveUserReview(UserReviewDetails reviewDetails);
        void SendUserReviewEmail(int reviewId, UserReviewStatus status);
    }
}
