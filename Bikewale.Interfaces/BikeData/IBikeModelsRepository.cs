using Bikewale.Entities.BikeData;
using System.Collections;
using System.Collections.Generic;

namespace Bikewale.Interfaces.BikeData
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 24 Sept 2014
    /// Summary : Interface for DAL to get bikedata 
    /// Modified By Vivek Gupta on 9-5-2016
    /// Added defimition of BikeModelContent GetRecentModelArticles(U modelId);
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
        List<NewLaunchedBikeEntity> GetNewLaunchedBikesList(int startIndex, int endIndex, out int recordCount);

        BikeModelPageEntity GetModelPage(U modelId);
        BikeSpecificationEntity MVSpecsFeatures(int versionId);

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
    }
}
