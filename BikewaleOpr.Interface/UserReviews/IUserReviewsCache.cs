using BikewaleOpr.Entity.UserReviews;
using System.Collections.Generic;

namespace BikewaleOpr.Interface.UserReviews
{
    /// <summary>
    /// Created BY : Ashish G. kamble on 18 Apr 2017
    /// Summary : Interface methods related to user reviews cache
    /// </summary>
    public interface IUserReviewsCache
    {
        IEnumerable<DiscardReasons> GetUserReviewsDiscardReasons();
    }
}
