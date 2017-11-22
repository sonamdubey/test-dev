
namespace BikewaleOpr.Entity.UserReviews
{
    /// <summary>
    /// Written By : Ashish G. Kamble on 19 Apr 2017
    /// Summary : Class to get the required properties to update the user review info
    /// Modified by sajal gupta on 17-05-2017
    /// Description : Added IsShortListed
    /// Modified By:Snehal Dange on 21st Nov 2017
    /// Description: Added MakeId
    /// </summary>
    public class UpdateReviewsInputEntity
    {
        public uint ReviewId { get; set; }
        public ReviewsStatus ReviewStatus { get; set; }
        public uint ModeratorId { get; set; }
        public ushort DisapprovalReasonId { get; set; }
        public string Review { get; set; }
        public string ReviewTitle { get; set; }
        public string ReviewTips { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string BikeName { get; set; }
        public string MakeMaskingName { get; set; }
        public string ModelMaskingName { get; set; }
        public uint ModelId { get; set; }
        public bool IsShortListed { get; set; }
        public uint MakeId { get; set; }
    }
}
