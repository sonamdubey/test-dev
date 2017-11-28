using Bikewale.Entities.Used;
using System.Collections.Generic;

namespace BikewaleOpr.Interface.Used
{
    /// <summary>
    /// Created By: Aditi Srivastava on 18 Oct 2016
    /// Description: Used Bike Seller repository
    /// Modified by : Aditi Srivastava on 26 May 2017
    /// summary     : Added function GetUsedBikeSellerDetails
    /// </summary>
    public interface ISellerRepository
    {
        UsedBikeSellerBase GetSellerDetails(int inquiryId, bool isDealer);
        IEnumerable<SellBikeAd> GetClassifiedPendingInquiries();
        bool SaveEditedInquiry(uint inquiryId, short isApproved, int approvedBy);
        UsedBikeProfileDetails GetUsedBikeSellerDetails(int inquiryId, bool isDealer); 
    }
}
