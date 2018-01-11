
using BikewaleOpr.Entities.BikeData;
using BikewaleOpr.Entity.BikeData;
using System;
using System.Collections.Generic;
namespace BikewaleOpr.Interface.BikeData
{
    /// <summary>
    /// Created By : Sushil Kumar on 26th Oct 2016 
    /// Description : Interface for bike models 
    /// Modified by : Sajal Gupta on 22-12-2016
    /// Description : Added SaveModelUnitSold, GetLastSoldUnitData
    ///  Modified by : Sajal Gupta on 03-03-2017
    /// Description : Added FetchPhotoId, DeleteUsedBikeModelImage, GetUsedBikeModelImageByMake, GetPendingUsedBikesWithoutModelImage
    /// Modified by : Aditi Srivastava on 23 May 2017
    /// Summary     : added function to get model ingo by makeId
    /// Modified By: Ashutosh Sharma on 27-07-2017
    /// Discription: Added UpdateAsSoldInquiry
    /// Modified By: Sanskar Gupta on 2018-01-04
    /// Description: Added GetPromotionBikes
    /// </summary>
    public interface IBikeModelsRepository
    {
        IEnumerable<BikeModelEntityBase> GetModels(uint makeId, string requestType);
        IEnumerable<BikeModelEntityBase> GetModels(uint makeId, ushort requestType);
        void SaveModelUnitSold(string list, DateTime date);
        SoldUnitData GetLastSoldUnitData();
        uint FetchPhotoId(UsedBikeModelImageEntity objModelImageEntity);
        bool DeleteUsedBikeModelImage(uint modelId);
        IEnumerable<UsedBikeModelImageData> GetUsedBikeModelImageByMake(uint makeId);
        UsedBikeImagesNotificationData GetPendingUsedBikesWithoutModelImage();
        IEnumerable<BikeModelMailEntity> GetModelsByMake(uint makeId, string hostUrl, string oldMakeMasking, string newMakeMasking);
        IEnumerable<BikeMakeModelData> GetModelsWithMissingColorImage();
        bool UpdateInquiryAsSold(uint inquiryId);
        IEnumerable<BikeVersionEntityBase> GetVersionsByModel(EnumBikeType requestType, uint modelId);
        bool IsModelMaskingNameExists(uint makeId, string modelMaskingName);
        IEnumerable<BikeMakeModelData> GetPromotionBikes();
    }
}
