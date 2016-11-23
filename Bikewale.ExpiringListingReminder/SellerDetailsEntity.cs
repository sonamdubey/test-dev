
namespace Bikewale.ExpiringListingReminder
{
    /// <summary>
    /// Created By Sajal Gupta on 23-11-2016.
    /// Desc : Entity to hold seller data for expiry listing.
    /// </summary>
    public class SellerDetailsEntity
    {
        public string sellerName { get; set; }
        public string makeName { get; set; }
        public string modelName { get; set; }
        public int daysToExpire { get; set; }
        public string number { get; set; }
        public string inquiryId { get; set; }
        public int makeId { get; set; }
        public int modelId { get; set; }
        public int customerId { get; set; }
        public string sellerEmail { get; set; }
    }
}
