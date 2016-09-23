
namespace Bikewale.Entities.Used
{
    /// <summary>
    /// Created by  :   Sumit Kate on 23 Sep 2016
    /// Description :   Purchase Inquiry Status Codes
    /// </summary>
    public enum PurchaseInquiryStatusCode
    {
        InvalidRequest = 0,
        Success = 1,
        InvalidCustomerInfo = 2,
        MobileNotVerified = 3,
        Unused = 4,
        MaxLimitReached = 5
    }
}
