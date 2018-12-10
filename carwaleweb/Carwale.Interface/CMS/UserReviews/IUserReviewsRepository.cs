using Carwale.Entity.CMS;
using Carwale.Entity.CMS.UserReviews;
using Carwale.Entity.Enum;
using Carwale.Entity.UserReviews;
using System;
using System.Collections.Generic;

namespace Carwale.Interfaces.CMS.UserReviews
{
    public interface IUserReviewsRepository
    {
        List<UserReviewEntity> GetUserReviewsList(int makeId, int modelId, int versionId, int start, int end, int sortCriteria);
        UserReviewDetail GetUserReviewDetailById(int reviewId, CMSAppId applicationId);
        string GetUserReviewedIdsByModel(int modelId);
        List<CarReviewBaseEntity> GetMostReviewedCars();
        List<UserReviewEntity> GetUserReviewsByType(UserReviewsSorting type);
        bool CheckVersionReview(string versionId, string email, string customerId, string modelId);
        string SaveDetails(UserReviewDetail userReviewDetail);
        int ProcessEmailVerfication(bool isEmailVerified, int userId);
        UserReviewCustomerInfo GetUserReviewCustomerById(int userId);
        int SaveRating(RatingDetails ratingDetails);
        Tuple<int, bool> UpdateRating(RatingDetails ratingDetails);
        int SaveCustomerDetails(string name, string email);
        Tuple<int, int> InvalidateUserReviewReplica(int replicaId);
        UserReviewReplicaEntity GetLatestUserReviewReplicaByReviewId(int reviewId);
        int DeleteUserReview(int reviewId);
        bool IsVerifiedReviewReplica(int replicaId);
        int UpdateReviewWithLatestReplica(UserReviewReplicaEntity replicaDetails);
        bool IsActiveReview(int reviewId);
        int GetReviewId(int customerId, int versionId);

    }
}
