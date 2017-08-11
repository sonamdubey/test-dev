


using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Notifications;
using System.Collections.Generic;

namespace Bikewale.Models
{
    public class UserReviewSearchWidget
    {
        private readonly IUserReviewsCache _userReviewsCache = null;
        public UserReviewSearchWidget(IUserReviewsCache userReviewsCache)
        {
            _userReviewsCache = userReviewsCache;
        }

        public IEnumerable<RecentReviewsWidget> GetData()
        {
            IEnumerable<RecentReviewsWidget> objList = null ;
            try
            {
                objList= _userReviewsCache.GetRecentReviews();
            }
            catch (System.Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, "UserReviewSearchWidget.GetData()");
            }
             
            return objList;
        }

    }
}