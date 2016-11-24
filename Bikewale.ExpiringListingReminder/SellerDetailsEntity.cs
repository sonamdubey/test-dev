
namespace Bikewale.ExpiringListingReminder
{
    /// <summary>
    /// Created By Sajal Gupta on 23-11-2016.
    /// Desc : Entity to hold seller data for expiry listing.
    /// </summary>
    public class SellerDetailsEntity
    {
        public int inquiryId { get; set; }
        public string makeName { get; set; }
        public string modelName { get; set; }
        public string sellerName { get; set; }
        public string sellerMobileNumber { get; set; }
        public string sellerEmail { get; set; }
    }
}
