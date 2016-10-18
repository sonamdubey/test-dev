
using Bikewale.Entities.Used;
namespace Bikewale.Interfaces.Used
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 13 Oct 2016
    /// Summary: Interface for Sell bikes - BAL
    /// </summary>
    public interface ISellBikes
    {
        SellBikeInquiryResultEntity SaveSellBikeAd(SellBikeAd ad);
        bool UpdateOtherInformation(SellBikeAdOtherInformation adInformation, int inquiryAd, ulong customerId);
        SellBikeAd GetById(int inquiryId, ulong customerId);
        bool VerifyMobile(SellerEntity seller);
        bool IsFakeCustomer(ulong customerId);
    }
}
