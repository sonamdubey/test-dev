
using Bikewale.Entities.Used;
using System.Web;
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
        SellBikeImageUploadResultEntity UploadBikeImage(bool isMain, ulong customerId, string profileId, string description, HttpFileCollection imageFile);
    }
}
