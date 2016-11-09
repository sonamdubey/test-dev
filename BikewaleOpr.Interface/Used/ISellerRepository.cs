using Bikewale.Entities.Used;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.Interface.Used
{
    /// <summary>
    /// Created By: Aditi Srivastava on 18 Oct 2016
    /// Description: Used Bike Seller repository
    /// </summary>
    public interface ISellerRepository
    {
        UsedBikeSellerBase GetSellerDetails(int inquiryId, bool isDealer);
        IEnumerable<SellBikeAd> GetClassifiedPendingInquiries();
        bool SaveEditedInquiry(uint inquiryId, short isApproved, int approvedBy);
    }
}
