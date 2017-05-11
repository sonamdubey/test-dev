using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BikewaleOpr.Entity.UserReviews;

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
