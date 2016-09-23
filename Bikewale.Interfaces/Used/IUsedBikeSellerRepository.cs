using Bikewale.Entities.Used;
using System;

namespace Bikewale.Interfaces.Used
{
    /// <summary>
    /// Created by  :   Sumit Kate on 01 Sep 2016
    /// Description :   Used Bike Seller Repository
    /// Modified by :   Sumit Kate on 23 Sep 2016
    /// Description :   Added source id parameter to SaveCustomerInquiry method
    /// </summary>
    public interface IUsedBikeSellerRepository
    {
        UsedBikeSellerBase GetSellerDetails(string inquiryId, bool isDealer);
        int SaveCustomerInquiry(string inquiryId, ulong customerId, UInt16 sourceId);
        ClassifiedInquiryDetailsMin GetInquiryDetails(string inquiryId);
    }
}
