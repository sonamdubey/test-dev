using Bikewale.Entities.BikeData;
using Bikewale.Entities.BikeData.NewLaunched;
using Bikewale.Entities.GenericBikes;
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
    /// Modified by :   Sangram Nandkhile on 30 Jan 2017
    /// Description :   Removed GetModelPhotos method
    /// Modified By : Sushil Kumar on 2nd Jan 2016
    /// Description : Addded new interface input parameter for generic bike info
    /// Modified By : Sushil Kumar on 12 Jan 2017
    /// Description : Addded new method for get bike ranking by model id
    /// Modified By : Aditi Srivastava on 17 Jan 2017
    /// Description : Added function to get top 10 bikes by bodystyle
    /// Mocified by : Sajal Gupta on 02-02-2017
    /// Description : Modified function GetBestBikesByCategory
    /// Mocified by : Sangram Nandkhile on 10 Feb 2017
    /// Description : Modified GetAllPhotos and CreateAllPhotoList
    /// Modified by : Sumit Kate on 10 Feb 2017
    /// Description : Added GetNewLaunchedBikesList     
    /// Modified  By :- subodh Jain 10 Feb 2017
    /// Summary :- BikeInfo Slug details GetBikeInfo
    /// Modified By : Sajal Gupta on 24-02-2017 Added GetModelColorPhotos.
    /// Modified  By :- subodh Jain 10 march 2017
    /// Summary :- added GetMostPopularScooters
    /// Modified by : Ashutosh Sharma on 18-Aug-2017
    /// Description : Added GetPopularBikesByBodyStyle and changed old "GetPopularBikesByBodyStyle" to GetPopularBikesByBodyStyleViaModel
    /// Modified by : Ashutosh Sharma on 18-Aug-2017
    /// Description : Added 'GetMostPopularBikesByMakeWithCityPrice' to get most popular bikes by make wity city price if city is selected.
    /// Modified By:Snehal Dange on 3rd Nov 2017
    /// Description: Added GetMileageDetails to get mileage info for model
    /// Modified by : Sanskar Gupta on 12 Jan 2018
    /// Description : Added IEnumerable<MostPopularBikesBase> GetAdPromotedBike(BikeFilters bikeFilters)
    /// <typeparam name="U"></typeparam>
    /// </summary>
    public interface IBikeModelsCacheRepository<U>
    {
        BikeModelPageEntity GetModelPageDetails(U modelId);
        BikeModelPageEntity GetModelPageDetails(U modelId, int versionId);
        IEnumerable<UpcomingBikeEntity> GetUpcomingBikesList(EnumUpcomingBikesFilter sortBy, int pageSize, int? makeId = null, int? modelId = null, int? curPageNo = null);
        IEnumerable<MostPopularBikesBase> GetMostPopularBikesByMake(int makeId);
        IEnumerable<MostPopularBikesBase> GetMostPopularBikesByMakeWithCityPrice(int makeId, uint cityId);
        IEnumerable<MostPopularBikesBase> GetMostPopularBikes(int? topCount = null, int? makeId = null);
        NewLaunchedBikesBase GetNewLaunchedBikesList(int startIndex, int endIndex, int? makeId = null);
        NewLaunchedBikesBase GetNewLaunchedBikesListByMake(int startIndex, int endIndex, int? makeId = null);
        BikeDescriptionEntity GetModelSynopsis(U modelId);
        IEnumerable<MostPopularBikesBase> GetMostPopularBikesbyMakeCity(uint topCount, uint makeId, uint cityId);
        IEnumerable<NewBikeModelColor> GetModelColor(U modelId);
        IEnumerable<BikeUserReviewRating> GetUserReviewSimilarBike(uint modelId, uint topCount);
        EnumBikeBodyStyles GetBikeBodyType(uint modelId);
        IEnumerable<MostPopularBikesBase> GetPopularBikesByBodyStyle(ushort bodyStyleId, uint topCount, uint cityId);
        ICollection<MostPopularBikesBase> GetMostPopularBikesByModelBodyStyle(int modelId, int topCount, uint cityId);
        GenericBikeInfo GetBikeInfo(uint modelId, uint cityId);
        GenericBikeInfo GetBikeInfo(uint modelId);
        BikeRankingEntity GetBikeRankingByCategory(uint modelId);
        ICollection<BestBikeEntityBase> GetBestBikesByCategory(EnumBikeBodyStyles bodyStyle, uint? cityId = null);
        ICollection<BestBikeEntityBase> GetBestBikesByModelInMake(uint modelId, uint? cityId = null);
        IEnumerable<NewLaunchedBikeEntityBase> GetNewLaunchedBikesList();
        IEnumerable<NewLaunchedBikeEntityBase> GetNewLaunchedBikesList(uint cityId);
        IEnumerable<ModelColorImage> GetModelColorPhotos(U modelId);
        Bikewale.Entities.CMS.Photos.ModelHostImagePath GetModelPhotoInfo(U modelId);
        IEnumerable<MostPopularBikesBase> GetMostPopularScooters(uint topCount, uint? cityId);
        IEnumerable<MostPopularBikesBase> GetMostPopularScooters(uint makeId);
        IEnumerable<MostPopularBikesBase> GetElectricBikes();
        IEnumerable<MostPopularBikesBase> GetElectricBikes(uint cityId);
        IEnumerable<MostPopularBikesBase> GetMostPopularScooters(uint topCount, uint makeId, uint cityId);
        BikeMileageEntity GetMileageDetails();
        BikeSeriesEntityBase GetSeriesByModelId(uint modelId);
        IEnumerable<ModelIdWithBodyStyle> GetModelIdsForImages();
        ICollection<Entities.BikeData.BikeModelColorImageEntity> GetModelImages(string modelIds);
        IEnumerable<MostPopularBikesBase> GetAdPromotedBike(BikeFilters bikeFilters);
        IEnumerable<MostPopularBikesBase> GetAdPromotedBikeWithOutCity(BikeFilters bikeFilters);
    }
}
