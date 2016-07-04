using Bikewale.Entities.BikeData;
using System.Collections.Generic;

namespace Bikewale.Interfaces.BikeData
{
    /// <summary>
    /// Created By : Ashish G. Kamble
    /// Summary : Interface for bike makes data.
    /// Modified By :   Sumit Kate on 16 Nov 2015
    /// Summary :   Added new function UpcomingBikeMakes
    /// Modified By : Sangram Nandkhile on 20 Jun 2016
    /// Summary :   Added new function GetDiscontinuedBikeModelsByMake
    /// </summary>
    /// <typeparam name="T">Generic type (need to specify type while implementing this interface)</typeparam>
    /// <typeparam name="U">Generic type (need to specify type while implementing this interface)</typeparam>
    public interface IBikeMakes<T, U> : IRepository<T, U>
    {
        List<BikeMakeEntityBase> GetMakesByType(EnumBikeType requestType);
        List<BikeModelsListEntity> GetModelsList(U makeId);
        BikeDescriptionEntity GetMakeDescription(U makeId);
        BikeMakeEntityBase GetMakeDetails(string makeId);
        IEnumerable<BikeMakeEntityBase> UpcomingBikeMakes();
        IEnumerable<BikeVersionEntity> GetDiscontinuedBikeModelsByMake(uint makeId);
    }
}