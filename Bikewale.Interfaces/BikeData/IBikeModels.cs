using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Entities.PhotoGallery;
using Bikewale.Entities.UserReviews;
using System.Collections.Generic;

namespace Bikewale.Interfaces.BikeData
{
    /// <summary>
    /// Created By : Ashish G. Kamble
    /// Summary : Interface for bike models data
    /// Modified By Vivek Gupta on 9-5-2016
    /// Added defimition of BikeModelContent GetRecentModelArticles(U modelId);
    /// Modified by: Aditi Srivastava on 16th Aug,2016
    /// Description: added new method  IEnumerable<ModelImage> GetModelPhotos() 
    /// Modified by: Subodh Jain on 22 sep 2016
    /// Description: added new method GetMostPopularBikesbyMakeCity
    /// Modified by: Subodh Jain on 17 jan 2017
    /// Description: added new method GetUserReviewSimilarBike
    /// Modified by: Sangram Nandkhile on 10 Feb 2017
    /// Description: Added new method GetModelPhotoGalleryWithMainImage, GetModelPageDetailsNew which has extra main model image
    /// <typeparam name="T">Generic type (need to specify type while implementing this interface)</typeparam>
    /// <typeparam name="U">Generic type (need to specify type while implementing this interface)</typeparam>
    public interface IBikeModels<T, U> : IRepository<T, U>
    {
        List<BikeModelEntityBase> GetModelsByType(EnumBikeType requestType, int makeId);
        List<BikeVersionsListEntity> GetVersionsList(U modelId, bool isNew);
        BikeDescriptionEntity GetModelSynopsis(U modelId);

        UpcomingBikeEntity GetUpcomingBikeDetails(U modelId);
        List<UpcomingBikeEntity> GetUpcomingBikesList(UpcomingBikesListInputEntity inputParams, EnumUpcomingBikesFilter sortBy, out int recordCount);
        List<UpcomingBikeEntity> GetUpcomingBikesList(EnumUpcomingBikesFilter sortBy, int pageSize, int? makeId = null, int? modelId = null, int? curPageNo = null);
        NewLaunchedBikesBase GetNewLaunchedBikesList(int startIndex, int endIndex, int? makeid = null);
        //List<NewLaunchedBikeEntity> GetNewLaunchedBikesList(int pageSize, out int recordCount, int? currentPageNo = null);
        BikeModelPageEntity GetModelPageDetails(U modelId);
        BikeModelPageEntity GetModelPageDetailsNew(U modelId);
        //IEnumerable<BikeSpecificationEntity> GetModelSpecifications(U modelId, int versionId);
        IEnumerable<NewBikeModelColor> GetModelColor(U modelId);
        List<ModelImage> GetBikeModelPhotoGallery(U modelId);
        List<ModelImage> GetModelPhotoGalleryWithMainImage(U ModelId);
        BikeModelContent GetRecentModelArticles(U modelId);
        IEnumerable<MostPopularBikesBase> GetMostPopularBikesbyMakeCity(uint topCount, uint makeId, uint cityId);
        IEnumerable<BikeUserReviewRating> GetUserReviewSimilarBike(uint modelId, uint topCount);
        ModelPhotoGalleryEntity GetPhotoGalleryData(U modelId);
    }
}
