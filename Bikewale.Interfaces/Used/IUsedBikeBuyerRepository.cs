
namespace Bikewale.Interfaces.Used
{
    /// <summary>
    /// Created by  :   Sumit Kate on 01 Sep 2016
    /// Description :   Used Bike Buyer Repository
    /// </summary>
    public interface IUsedBikeBuyerRepository
    {
        bool IsBuyerEligible(string mobile);
        bool UploadPhotosRequest(string sellInquiryId, string buyerId, byte consumerType, string buyerMessage);
        bool HasShownInterestInUsedBike(bool isDealer, string inquiryId, string customerId);
    }
}
