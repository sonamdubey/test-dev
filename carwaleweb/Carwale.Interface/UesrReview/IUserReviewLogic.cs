using Carwale.Entity.Enum;
using Carwale.DTOs.UserReviews;
using Carwale.Entity.UserReview;

namespace Carwale.Interfaces.UesrReview
{
    public interface IUserReviewLogic
    {
        string SaveCustomerReview(UserReviewBody reviewBody);
		    UserReviewPageDetails GetWriteReviewPageDetails(int reviewId);
        UserReveiwLandingPageDetails GetLandingPageDetails(Platform platformId);
        int GetVersion(string cookie, int modelId);
	}
}
