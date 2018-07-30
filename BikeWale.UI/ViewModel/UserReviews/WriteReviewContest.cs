using Bikewale.Entities.BikeData;
using Bikewale.Entities.UserReviews;
using Bikewale.Models.Make;
using System.Collections.Generic;

namespace Bikewale.Models
{
    /// <summary>
    /// Summary: View model for write Review
    /// Created by: Sangram Nandkhile on 05 June 2017
    /// Modified by: Vivek Singh Tomar On 12th Aug 2017
    /// Summary: Added property to hold list of user reviews contest winners
    /// Modified By :   Vishnu Teja Yalakuntla on 23rd Aug 2017
    /// Description :   Added MakeModelPopupVM for popup
    /// Modified By:  Snehal Dange on 25th Sep 2017
    /// Description : Added MakeId and ModelId
    /// </summary>
    public class WriteReviewContestVM : ModelBase
    {
        public IEnumerable<BikeMakeEntityBase> Makes { get; set; }
        public string QueryString { get; set; }
        public IEnumerable<RecentReviewsWidget> UserReviewsWinners { get; set; }
        public UserReviewPopupVM UserReviewPopup { get; set; }
        public uint? MakeId { get; set; }
        public uint? ModelId { get; set; }
        public string MakeName { get; set; }
        public string ModelName { get; set; }
        public string MakeMaskingName { get; set; }
        public string ModelMaskingName { get; set; }
    }
}
