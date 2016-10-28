
using Bikewale.Entities.Used;
using System;
namespace Bikewale.Interfaces.Used
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 13 Oct 2016
    /// Summary: Interface for Sell bikes - DAL
    /// </summary>
    public interface ISellBikesRepository<T, U> : IRepository<T, U>
    {
        new int Add(T ad);
        T GetById(U inquiryId, UInt64 customerId);
        bool UpdateOtherInformation(SellBikeAdOtherInformation otherInfo, U inquiryId, UInt64 customerId);
        bool IsFakeCustomer(ulong customerId);
        string SaveBikePhotos(bool isMain, bool isDealer, U inquiryId, string originalImageName, string description);
        string UploadImageToCommonDatabase(string photoId, string imageName, ImageCategories imgC, string directoryPath);
    }
}
