using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;

namespace Bikewale.Models.UserReviews
{
    /// <summary>
    /// Created by: Vivek Singh Tomar On 12th Aug 2017
    /// </summary>
    public class UserReviewWinnerWidget
    {
        private readonly IUserReviewsCache _userReviewCache = null;
        public UserReviewWinnerWidget(IUserReviewsCache userReviewCache)
        {
            _userReviewCache = userReviewCache;
        }

        /// <summary>
        /// Created by: Vivek Singh Tomar On 12th Aug 2017
        /// Summary: To get winners of user reviews contest
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RecentReviewsWidget> GetData()
        {
            IEnumerable<RecentReviewsWidget> objReviewsWinnersList = null;
            try
            {
                objReviewsWinnersList = _userReviewCache.GetUserReviewsWinners();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.UserReviews.UserReviewWinnerWidget");
            }
            return objReviewsWinnersList;
        }
    }
}