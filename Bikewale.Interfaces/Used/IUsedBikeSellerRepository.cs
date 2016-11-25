﻿using Bikewale.Entities.Used;
using System;

namespace Bikewale.Interfaces.Used
{
    /// <summary>
    /// Created by  :   Sumit Kate on 01 Sep 2016
    /// Description :   Used Bike Seller Repository
    /// Modified by :   Sumit Kate on 23 Sep 2016
    /// Description :   Added source id parameter to SaveCustomerInquiry method
    /// Modified By  :  Aditi Srivastava on 27 Oct 2016
    /// Description :   Added function to delete used bikes photos
    /// </summary>
    public interface IUsedBikeSellerRepository
    {
        UsedBikeSellerBase GetSellerDetails(string inquiryId, bool isDealer);
        int SaveCustomerInquiry(string inquiryId, ulong customerId, UInt16 sourceId, out bool isNew);
        ClassifiedInquiryDetailsMin GetInquiryDetails(string inquiryId);
        bool RemoveBikePhotos(int inquiryId, string photoId);
        bool RepostSellBikeAd(int inquiryId, ulong customerId);
    }
}
