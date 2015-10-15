using Bikewale.Entities.BikeData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Interfaces.BikeData
{
    /// <summary>
    /// Created By : Ashish G. Kamble
    /// Summary : Interface for bike models data
    /// </summary>
    /// <typeparam name="T">Generic type (need to specify type while implementing this interface)</typeparam>
    /// <typeparam name="U">Generic type (need to specify type while implementing this interface)</typeparam>
    public interface IBikeModels<T, U> : IRepository<T, U>
    {
        List<BikeModelEntityBase> GetModelsByType(EnumBikeType requestType, int makeId);
        List<BikeVersionsListEntity> GetVersionsList(U modelId , bool isNew);
        BikeDescriptionEntity GetModelSynopsis(U modelId);

        UpcomingBikeEntity GetUpcomingBikeDetails(U modelId);
        List<UpcomingBikeEntity> GetUpcomingBikesList(UpcomingBikesListInputEntity inputParams, EnumUpcomingBikesFilter sortBy, out int recordCount);
        List<NewLaunchedBikeEntity> GetNewLaunchedBikesList(int startIndex, int endIndex, out int recordCount);
        BikeModelPageEntity GetModelPageDetails(U modelId);
    }
}
