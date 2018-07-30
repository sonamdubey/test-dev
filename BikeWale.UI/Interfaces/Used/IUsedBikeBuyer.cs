
using Bikewale.Entities.Customer;
using Bikewale.Entities.Used;
namespace Bikewale.Interfaces.Used
{
    /// <summary>
    /// Created by  :   Sumit Kate on 01 Sep 2016
    /// Description :   Used Bike Buyer Business Layer Interface
    /// </summary>
    public interface IUsedBikeBuyer
    {
        bool UploadPhotosRequest(PhotoRequest request);
        BikeInterestDetails ShowInterestInBike(CustomerEntityBase buyer, string profileId, bool isDealer);
        PurchaseInquiryResultEntity SubmitPurchaseInquiry(CustomerEntityBase buyer, string profileId, string pageUrl, ushort sourceId);
    }
}
