using Carwale.Entity.CMS;
using Carwale.Entity.CMS.UserReviews;
using Carwale.Entity.Enum;
using System.Collections.Generic;

namespace Carwale.Interfaces.CMS.UserReviews
{
    public interface IUserReviewsCache 
    {
        UserReviewDetail GetUserReviewDetailById(int reviewId, CMSAppId applicationId);
        List<UserReviewEntity> GetUserReviewsList(int makeId,int modelId, int versionId, int start, int end, int sortCriteria);
        string GetUserReviewedIdsByModel(int modelId);
        List<CarReviewBaseEntity> GetMostReviewedCars();
        List<UserReviewEntity> GetUserReviewsByType(UserReviewsSorting type);
        int ProcessEmailVerfication(bool isEmailVerified, int userId);
    }
}
