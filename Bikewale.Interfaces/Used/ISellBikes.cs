
using Bikewale.Entities.Used;
namespace Bikewale.Interfaces.Used
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 13 Oct 2016
    /// Summary: Interface for Sell bikes - BAL
    /// Modified By : Aditi Srivastava on 27 Oct 2016
    /// Description : Added function to remove bike photos
    /// Modified By : Aditi Srivastava on 10 Nov 2016
    /// Description : Added function to send notification
    /// </summary>
    public interface ISellBikes
    {
        SellBikeInquiryResultEntity SaveSellBikeAd(SellBikeAd ad);
        bool UpdateOtherInformation(SellBikeAdOtherInformation adInformation, int inquiryAd, ulong customerId);
        SellBikeAd GetById(int inquiryId, ulong customerId);
        bool VerifyMobile(SellerEntity seller);
        void SendNotification(SellBikeAd ad);
        bool IsFakeCustomer(ulong customerId);
        bool RemoveBikePhotos(ulong customerId, string profileId, string photoId);
        SellBikeImageUploadResultEntity UploadBikeImage(bool isMain, ulong customerId, string profileId, string fileExtension, string description);
        bool MakeMainImage(uint photoId, ulong customerId, string profileId);
    }
}
