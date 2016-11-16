using Bikewale.Entities.Customer;
namespace Bikewale.Entities.Used
{
    /// <summary>
    /// Created by  :   Sumit Kate on 23 Sep 2016
    /// Description :   Purchase Inquiry Result Entity
    /// </summary>
    public class PurchaseInquiryResultEntity
    {
        public PurchaseInquiryStatusEntity InquiryStatus { get; set; }
        public CustomerEntityBase Seller { get; set; }
        public string SellerAddress { get; set; }
    }
}
