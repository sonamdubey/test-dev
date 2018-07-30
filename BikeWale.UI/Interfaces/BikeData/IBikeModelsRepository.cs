using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.BikeData.NewLaunched;
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
    /// Modified By:-Subodh Jain 2 Feb 2017
    /// Summary :- Added Get make if video
    /// Modified By:-Subodh Jain 2 Feb 2017
    /// Summary :- Added GetSimilarBikesVideos method
    /// Modified By : Aditi Srivastava on 17 Jan 2017
    /// Description : Edited function to get top 10 bikes and their mopdelcount by bodystyle and cityid.
    /// Modified by :   Sumit Kate on 10 Feb 2017
    /// Description :   Added GetNewLaunchedBikesList 
    /// Modified  By :- subodh Jain 10 Feb 2017
    /// Summary :- BikeInfo Slug details GetBikeInfo
    ///    Modified  By :- subodh Jain 10 march 2017
    /// Summary :- Added GetMostPopularScooters
    /// Modified by : Ashutosh Sharma on 18-Aug-2017
    /// Description : Added GetPopularBikesByBodyStyle
    /// Modified By :   Vishnu Teja Yalakuntla on 09 Sep 2017
    /// Description :   Added GetSimilarBikesUserReviewsWithPrice and GetSimilarBikesUserReviewsWithPriceInCity methods.
    /// Modified By: Snehal Dange on 12 Sep 2017
    /// Description : Added GetAlternativeBikesWithPhotosInCity method
    /// Modified by : Ashutosh Sharma on 26-Sep-2017
    /// Description : Added GetFuturisticVersionMinSpecs
    /// Modified by : Ashutosh Sharma on 03 Oct 2017
    /// Description : Added 'GetMostPopularBikesByMakeWithCityPrice' to get most popular bikes by make wity city price if city is selected.
    /// Modified By:Snehal Dange on 03 Nov 2017
    /// Description: Added GetMileageForModel to get mileage information on model page
    /// 
    /// Modified by :   Sumit Kate on 15 Jan 2018
    /// Description :   Added new method to fetch model image + color photos by model ids
    /// 
    /// Modified By :   Deepak Israni on 14 June 2018
    /// Description :   Added new method GetQuestionAnswerModels to get model ids for which question and answers is active.
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
        BikeModelPageEntity GetModelPage(U modelId, int versionId);
        IEnumerable<NewBikeModelColor> GetModelColor(U modelId);
        IEnumerable<BikeSpecificationEntity> GetModelSpecifications(U modelId);
        IEnumerable<MostPopularBikesBase> GetMostPopularBikesbyMakeCity(uint topCount, uint makeId, uint cityId);
        IEnumerable<BikeUserReviewRating> GetUserReviewSimilarBike(uint modelId, uint topCount);
        /// <summary>
        /// To Get Overall Most Popular Bikes
        /// </summary>
        /// <param name="topCount"></param>
        /// <param name="makeId"></param>
        /// <returns></returns>
        IEnumerable<MostPopularBikesBase> GetMostPopularBikes(int? topCount = null, int? makeId = null);
        /// <summary>
        /// To get Most popular Bikes based on MakeId
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        IEnumerable<MostPopularBikesBase> GetMostPopularBikesByMake(int makeId);
        IEnumerable<MostPopularBikesBase> GetMostPopularBikesByMakeWithCityPrice(int makeId, uint cityId);

        Hashtable GetMaskingNames();
        Hashtable GetOldMaskingNames();

        List<FeaturedBikeEntity> GetFeaturedBikes(uint topRecords);
        IEnumerable<BikeMakeModelEntity> GetAllModels(EnumBikeType requestType);
        List<BikeVersionMinSpecs> GetVersions(U modelId, bool isNew);
        IEnumerable<BikeVersionMinSpecs> GetFuturisticVersions(U modelId);
        BikeModelContent GetRecentModelArticles(U modelId);
        ModelHostImagePath GetModelPhotoInfo(U modelId);

        ReviewDetailsEntity GetDetailsByModel(U modelId, uint cityId);
        ReviewDetailsEntity GetDetailsByVersion(U versionId, uint cityId);
        ReviewDetailsEntity GetDetails(string reviewId, bool isAlreadyViewed);
        IEnumerable<ModelColorImage> GetModelColorPhotos(U modelId);
        EnumBikeBodyStyles GetBikeBodyType(uint modelId);

        IEnumerable<MostPopularBikesBase> GetPopularBikesByBodyStyle(ushort bodyStyleId, uint topCount, uint cityId);
        ICollection<MostPopularBikesBase> GetPopularBikesByModelBodyStyle(int modelId, int topCount, uint cityId);
        GenericBikeInfo GetBikeInfo(uint modelId, uint cityId);
        GenericBikeInfo GetBikeInfo(uint modelId);
        BikeRankingEntity GetBikeRankingByCategory(uint modelId);
        IEnumerable<BikeMakeEntityBase> GetMakeIfVideo();
        IEnumerable<SimilarBikeWithVideo> GetSimilarBikesVideos(uint modelId, uint totalRecords, uint cityid);
        ICollection<BestBikeEntityBase> GetBestBikesByCategory(EnumBikeBodyStyles bodyStyle, uint? cityId = null);
        IEnumerable<BestBikeEntityBase> GetBestBikesByModelInMake(uint modelId);
        ICollection<BestBikeEntityBase> GetBestBikesByModelInMake(uint modelId, uint cityId);
        IEnumerable<NewLaunchedBikeEntityBase> GetNewLaunchedBikesList();
        IEnumerable<NewLaunchedBikeEntityBase> GetNewLaunchedBikesList(uint cityId);
        IEnumerable<MostPopularBikesBase> GetElectricBikes();
        IEnumerable<MostPopularBikesBase> GetElectricBikes(uint cityId);
        IEnumerable<MostPopularBikesBase> GetMostPopularScooters(uint makeId);
        IEnumerable<MostPopularBikesBase> GetMostPopularScooters(uint topCount, uint? cityId);
        IEnumerable<MostPopularBikesBase> GetMostPopularScooters(uint topCount, uint makeId, uint cityId);
        IEnumerable<SimilarBikeUserReview> GetSimilarBikesUserReviewsWithPrice(uint modelId, uint totalRecords);
        IEnumerable<SimilarBikeUserReview> GetSimilarBikesUserReviewsWithPriceInCity(uint modelId, uint cityId, uint totalRecords);

        IEnumerable<SimilarBikesWithPhotos> GetAlternativeBikesWithPhotos(U modelId, ushort totalRecords);
        IEnumerable<SimilarBikesWithPhotos> GetAlternativeBikesWithPhotosInCity(U modelId, ushort totalRecords, uint cityId);
        BikeMileageEntity GetMileageForModel();
        BikeSeriesEntityBase GetSeriesByModelId(uint modelId);
        IEnumerable<ModelIdWithBodyStyle> GetModelIdsForImages();
        ICollection<Entities.BikeData.BikeModelColorImageEntity> GetModelImages(string modelIds);
        IEnumerable<MostPopularBikesBase> GetAdPromotedBike(BikeFilters ObjData);
        IEnumerable<MostPopularBikesBase> GetAdPromotedBikeWithOutCity(BikeFilters ObjData);

        Hashtable GetQuestionAnswerModels();
    }
}
