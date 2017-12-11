using System.Collections.Generic;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Entities.PhotoGallery;
using Bikewale.Entities.UserReviews;

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
    /// Modified by : Sajal Gupta - added CreateAllPhotoList on 27-02-2017
    /// Modified By :- Subodh jain 14 march 2017
    /// Summary :- Added GetMostPopularScooters
    /// Modified by :   Sumit Kate on 24 Mar 2017
    /// Description :   IEnumerable<MostPopularBikesBase> GetMostPopularBikes(EnumBikeType requestType, uint topCount, uint makeId, uint cityId)
    /// Modified by : Vivek Singh Tomar on 5th Oct 2017
    /// Summary : Added GetModelColorPhotos(U modelId)
    /// Modified By :Snehal Dange on 3rd Nov 2017
    /// Description: Added GetMileageDetails() to get mileage info for model
    /// Modified By : Vivek Singh Tomar on 28th Nov 2017
    /// Description : Added GetSeriesByModelId to get series details for given model id
    /// <typeparam name="T">Generic type (need to specify type while implementing this interface)</typeparam>
    /// <typeparam name="U">Generic type (need to specify type while implementing this interface)</typeparam>
    public interface IBikeModels<T, U> : IRepository<T, U>
    {
        List<BikeModelEntityBase> GetModelsByType(EnumBikeType requestType, int makeId);
        List<BikeVersionsListEntity> GetVersionsList(U modelId, bool isNew);
        BikeDescriptionEntity GetModelSynopsis(U modelId);

        UpcomingBikeEntity GetUpcomingBikeDetails(U modelId);
        List<UpcomingBikeEntity> GetUpcomingBikesList(UpcomingBikesListInputEntity inputParams, EnumUpcomingBikesFilter sortBy, out int recordCount);
        IEnumerable<UpcomingBikeEntity> GetUpcomingBikesList(EnumUpcomingBikesFilter sortBy, int pageSize, int? makeId = null, int? modelId = null, int? curPageNo = null);
        NewLaunchedBikesBase GetNewLaunchedBikesList(int startIndex, int endIndex, int? makeid = null);
        BikeModelPageEntity GetModelPageDetails(U modelId);
        IEnumerable<NewBikeModelColor> GetModelColor(U modelId);
        IEnumerable<ModelImage> GetBikeModelPhotoGallery(U modelId);
        IEnumerable<ModelImage> GetModelPhotoGalleryWithMainImage(U modelId);
        BikeModelContent GetRecentModelArticles(U modelId);
        Bikewale.Entities.BikeData.v2.BikeModelContent GetRecentModelArticlesv2(U modelId);
        IEnumerable<MostPopularBikesBase> GetMostPopularBikesbyMakeCity(uint topCount, uint makeId, uint cityId);
        IEnumerable<BikeUserReviewRating> GetUserReviewSimilarBike(uint modelId, uint topCount);
        ModelPhotoGalleryEntity GetPhotoGalleryData(U modelId);
        IEnumerable<ColorImageBaseEntity> CreateAllPhotoList(U modelId);
        BikeModelPageEntity GetModelPageDetails(U modelId, int versionId);
        IEnumerable<MostPopularBikesBase> GetMostPopularScooters(uint makeId);
        IEnumerable<MostPopularBikesBase> GetMostPopularScooters(uint topCount, uint? cityId);
    
        IEnumerable<MostPopularBikesBase> GetMostPopularBikes(EnumBikeType requestType, uint topCount, uint makeId, uint cityId);
        IEnumerable<ModelColorImage> GetModelColorPhotos(U modelId);
        BikeMileageEntity GetMileageDetails(uint modelId);
        BikeSeriesEntityBase GetSeriesByModelId(uint modelId);
    }
}
