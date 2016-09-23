using Bikewale.Entities.Used;

namespace Bikewale.Interfaces.Used
{
    /// <summary>
    /// Created by  :   Sumit Kate on 01 Sep 2016
    /// Description :   Used Bike Seller Repository
    /// </summary>
    public interface IUsedBikeSellerRepository
    {
        UsedBikeSellerBase GetSellerDetails(string inquiryId, bool isDealer);
        string SaveCustomerInquiry(string inquiryId, string customerId);
        ClassifiedInquiryDetailsMin GetInquiryDetails(string inquiryId);
    }
}
