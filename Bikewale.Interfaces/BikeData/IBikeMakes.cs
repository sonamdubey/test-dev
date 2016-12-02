using Bikewale.Entities.BikeData;
using System.Collections;
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
    /// Modified by :   Sumit Kate on 16 Sep 2016
    /// Description :   Added new function GetOldMaskingNames
    /// </summary>
    /// <typeparam name="T">Generic type (need to specify type while implementing this interface)</typeparam>
    /// <typeparam name="U">Generic type (need to specify type while implementing this interface)</typeparam>
    public interface IBikeMakes<T, U> : IRepository<T, U>
    {
        List<BikeMakeEntityBase> GetMakesByType(EnumBikeType requestType);
        BikeDescriptionEntity GetMakeDescription(U makeId);
        BikeMakeEntityBase GetMakeDetails(string makeId);
        BikeMakeEntityBase GetMakeDetails(uint makeId);
        IEnumerable<BikeMakeEntityBase> UpcomingBikeMakes();
        IEnumerable<BikeVersionEntity> GetDiscontinuedBikeModelsByMake(uint makeId);
        IEnumerable<BikeMakeModelBase> GetAllMakeModels();
        Hashtable GetOldMaskingNames();

    }
}