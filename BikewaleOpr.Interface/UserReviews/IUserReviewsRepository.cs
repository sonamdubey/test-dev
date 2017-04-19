using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BikewaleOpr.Entity.UserReviews;

namespace BikewaleOpr.Interface.UserReviews
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 15 Apr 2017
    /// Summary : Interface have methods related to the user reviews.
    /// </summary>
    public interface IUserReviewsRepository
    {        
        IEnumerable<ReviewBase> GetReviewsList(ReviewsInputFilters filter);
        void GetReviewDetails(uint id);
        IEnumerable<DiscardReasons> GetUserReviewsDiscardReasons();
        void UpdateUserReviewsStatus(uint reviewId, ReviewsStatus reviewStatus, uint moderatorId, ushort disapprovalReasonId);
    }
}
