﻿using Bikewale.Entities.BikeData;
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
    /// <typeparam name="U"></typeparam>
    /// </summary>
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
        IEnumerable<MostPopularBikesBase> GetMostPopularBikesbyMakeCity(uint topCount, uint makeId, uint cityId);
        IEnumerable<NewBikeModelColor> GetModelColor(U modelId);
        IEnumerable<BikeUserReviewRating> GetUserReviewSimilarBike(uint modelId, uint topCount);
        EnumBikeBodyStyles GetBikeBodyType(uint modelId);
        ICollection<MostPopularBikesBase> GetPopularBikesByBodyStyle(int modelId, int topCount, uint cityId);
        GenericBikeInfo GetBikeInfo(uint modelId, uint cityId);
        GenericBikeInfo GetBikeInfo(uint modelId);
        BikeRankingEntity GetBikeRankingByCategory(uint modelId);
        ICollection<BestBikeEntityBase> GetBestBikesByCategory(EnumBikeBodyStyles bodyStyle, uint? cityId = null);
        IEnumerable<NewLaunchedBikeEntityBase> GetNewLaunchedBikesList();
        IEnumerable<NewLaunchedBikeEntityBase> GetNewLaunchedBikesList(uint cityId);
        IEnumerable<ModelColorImage> GetModelColorPhotos(U modelId);
        BikeModelPageEntity GetMostPopularScooters(uint topCount);
        BikeModelPageEntity GetMostPopularScooters(uint topCount, uint makeId);
        BikeModelPageEntity GetMostPopularScooters(uint topCount, uint makeId, uint cityId);
    }
}
