
using Bikewale.Entities.Used;
using System.Collections.Generic;
namespace BikewaleOpr.Interface.Used
{
    /// <summary>
    /// Created by:Sangram Nandkhile on 24 Oct 2016
    /// Desc: Interface for Sell your bike 
    /// </summary>
    public interface ISellBikes
    {
        IEnumerable<SellBikeAd> GetClassifiedPendingInquiries();
        bool SaveEditedInquiry(uint inquiryId, short isApproved, int approvedBy);
    }
}
