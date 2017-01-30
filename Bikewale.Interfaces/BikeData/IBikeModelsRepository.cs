using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.UserReviews;
using System.Collections;
using System.Collections.Generic;

namespace Bikewale.Interfaces.BikeData
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 24 Sept 2014
    /// Summary : Interface for DAL to get bikedata 
    /// Modified By Vivek Gupta on 9-5-2016
    /// Added defimition of BikeModelContent GetRecentModelArticles(U modelId);
    /// Modified By: Aditi Srivastava on 17th Aug, 2016
    /// Description: Added GetModelPhotos(int modelId) for getting imageurl and original path;
    /// Modified by : Subodh Jain 22 sep 2016
    /// Description by : Added GetMostPopularBikesbyMakeCity method
    /// Modified By : Sushil Kumar on 5th Jan 2016
    /// Description : To get similar bikes with photos count
    /// Modified By :- Subodh Jain 17 Jan 2017
    /// Description :- Added GetUserReviewSimilarBike,GetDetailsByModel,GetDetailsByVersion,GetDetails
    /// Modified By : Sushil Kumar on 2nd Jan 2016
    /// Description : Addded new interface input parameter for generic bike info
    /// Modified By : Sushil Kumar on 12 Jan 2017
    /// Description : Addded new method for get bike ranking by model id
    /// Modified By : Aditi Srivastava on 17 Jan 2017
    /// Description : Added function to get top 10 bikes by bodystyle
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public interface IBikeModelsRepository<T, U> : IRepository<T, U>
    {
        List<BikeModelEntityBase> GetModelsByType(EnumBikeType requestType, int makeId);
        List<BikeVersionsListEntity> GetVersionsList(U modelId, bool isNew);
        BikeDescriptionEntity GetModelSynopsis(U modelId);
        UpcomingBikeEntity GetUpcomingBikeDetails(U modelId);
        List<UpcomingBikeEntity> GetUpcomingBikesList(UpcomingBikesListInputEntity inputParams, EnumUpcomingBikesFilter sortBy, out int recordCount);
        NewLaunchedBikesBase GetNewLaunchedBikesList(int startIndex, int endIndex);
        NewLaunchedBikesBase GetNewLaunchedBikesListByMake(int startIndex, int endIndex, int? makeid = null);
        BikeModelPageEntity GetModelPage(U modelId);
        IEnumerable<NewBikeModelColor> GetModelColor(U modelId);
        BikeSpecificationEntity MVSpecsFeatures(int versionId);
        IEnumerable<BikeSpecificationEntity> GetModelSpecifications(U modelId);
        IEnumerable<MostPopularBikesBase> GetMostPopularBikesbyMakeCity(uint topCount, uint makeId, uint cityId);
        IEnumerable<BikeUserReviewRating> GetUserReviewSimilarBike(uint modelId, uint topCount);
        /// <summary>
        /// To Get Overall Most Popular Bikes
        /// </summary>
        /// <param name="topCount"></param>
        /// <param name="makeId"></param>
        /// <returns></returns>
        List<MostPopularBikesBase> GetMostPopularBikes(int? topCount = null, int? makeId = null);
        /// <summary>
        /// To get Most popular Bikes based on MakeId
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        List<MostPopularBikesBase> GetMostPopularBikesByMake(int makeId);

        Hashtable GetMaskingNames();
        Hashtable GetOldMaskingNames();

        List<FeaturedBikeEntity> GetFeaturedBikes(uint topRecords);
        IEnumerable<BikeMakeModelEntity> GetAllModels(EnumBikeType requestType);
        List<BikeVersionMinSpecs> GetVersionMinSpecs(U modelId, bool isNew);
        BikeModelContent GetRecentModelArticles(U modelId);
        ModelPhotos GetModelPhotoInfo(U modelId);
        IEnumerable<SimilarBikesWithPhotos> GetAlternativeBikesWithPhotos(U modelId, ushort totalRecords);
        ReviewDetailsEntity GetDetailsByModel(U modelId, uint cityId);
        ReviewDetailsEntity GetDetailsByVersion(U versionId, uint cityId);
        ReviewDetailsEntity GetDetails(string reviewId, bool isAlreadyViewed);
        EnumBikeBodyStyles GetBikeBodyType(uint modelId);
        ICollection<MostPopularBikesBase> GetPopularBikesByBodyStyle(int bodyStyleId, int topCount, uint cityId);
        GenericBikeInfo GetGenericBikeInfo(uint modelId);
        BikeRankingEntity GetBikeRankingByCategory(uint modelId);
        ICollection<BestBikeEntityBase> GetBestBikesByCategory(EnumBikeBodyStyles bodyStyle);
    }
}
