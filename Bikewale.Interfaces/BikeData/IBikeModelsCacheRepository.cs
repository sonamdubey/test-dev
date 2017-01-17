using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Entities.UserReviews;
using System.Collections.Generic;

namespace Bikewale.Interfaces.BikeData
{
    /// <summary>
    /// Modified By  : Sushil Kumar on 28th June 2016
    /// Description : Added Upcoming bikes method.
    /// Modified By :   Sumit Kate on 01 Jul 2016
    /// Description :   GetMostPopularBikes Method
    /// Modified by :   Sumit Kate on 01 Jul 2016
    /// Description :   Added New Launched Bike List method
    /// Modified by :   Aditi Srivastava on 17th Aug 2016
    /// Description :   Added GetModelPhotos method
    /// Modeified by : Subodh Jain on 22 sep 2016
    /// Description : added GetMostPopularBikesbyMakeCity method
    /// Modeified by : Subodh Jain on 22 sep 2016
    /// Description : added GetUserReviewSimilarBike method
    /// </summary>
    /// <typeparam name="U"></typeparam>
    public interface IBikeModelsCacheRepository<U>
    {
        BikeModelPageEntity GetModelPageDetails(U modelId);
        BikeModelPageEntity GetModelPageDetails(U modelId, int versionId);
        IEnumerable<UpcomingBikeEntity> GetUpcomingBikesList(EnumUpcomingBikesFilter sortBy, int pageSize, int? makeId = null, int? modelId = null, int? curPageNo = null);
        IEnumerable<MostPopularBikesBase> GetMostPopularBikesByMake(int makeId);
        IEnumerable<MostPopularBikesBase> GetMostPopularBikes(int? topCount = null, int? makeId = null);
        NewLaunchedBikesBase GetNewLaunchedBikesList(int startIndex, int endIndex, int? makeId = null);
        NewLaunchedBikesBase GetNewLaunchedBikesListByMake(int startIndex, int endIndex, int? makeId = null);
        BikeDescriptionEntity GetModelSynopsis(U modelId);
        List<ModelImage> GetModelPhotoGallery(U modelId);
        IEnumerable<ModelImage> GetModelPhotos(U modelId);
        IEnumerable<MostPopularBikesBase> GetMostPopularBikesbyMakeCity(uint topCount, uint makeId, uint cityId);
        IEnumerable<NewBikeModelColor> GetModelColor(U modelId);
        IEnumerable<BikeUserReviewRating> GetUserReviewSimilarBike(uint modelId, uint topCount);
    }
}
