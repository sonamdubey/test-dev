
using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Notifications;
using Bikewale.Utility;
using System;
namespace Bikewale.Models
{
    /// <summary>
    /// Created by  :   Sumit Kate on 03 Apr 2017
    /// Description :   UserReviewsList Widget Model
    /// </summary>
    public class UserReviewsListWidget
    {
        private readonly IUserReviewsCache _userReviewCache = null;
        public FilterBy Filter { get; set; }
        public uint ModelId { get; set; }
        public int TopCount { get; set; }
        public uint VersionId { get; set; }

        public UserReviewsListWidget(IUserReviewsCache userReviewCache)
        {
            _userReviewCache = userReviewCache;
        }

        public ReviewListBase GetData()
        {
            ReviewListBase reviews = null;
            try
            {
                int stratIndex, endIndex;
                int PageNo = 1;
                Paging.GetStartEndIndex(TopCount, PageNo, out stratIndex, out endIndex);
                reviews = _userReviewCache.GetBikeReviewsList((uint)stratIndex, (uint)endIndex, ModelId, VersionId, Filter);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "UserReviewsListWidget.GetData()");
            }
            return reviews;
        }
    }
}