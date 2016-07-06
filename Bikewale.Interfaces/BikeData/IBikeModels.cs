using Bikewale.Entities.BikeData;
using System.Collections.Generic;

namespace Bikewale.Interfaces.BikeData
{
    /// <summary>
    /// Created By : Ashish G. Kamble
    /// Summary : Interface for bike models data
    /// Modified By Vivek Gupta on 9-5-2016
    /// Added defimition of BikeModelContent GetRecentModelArticles(U modelId);
    /// </summary>
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
        NewLaunchedBikesBase GetNewLaunchedBikesList(int startIndex, int endIndex);
        //List<NewLaunchedBikeEntity> GetNewLaunchedBikesList(int pageSize, out int recordCount, int? currentPageNo = null);
        BikeModelPageEntity GetModelPageDetails(U modelId);
        List<Entities.CMS.Photos.ModelImage> GetBikeModelPhotoGallery(U modelId);
        BikeModelContent GetRecentModelArticles(U modelId);
    }
}
