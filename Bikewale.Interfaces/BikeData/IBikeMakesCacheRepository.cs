using Bikewale.Entities.BikeData;
using System.Collections.Generic;

namespace Bikewale.Interfaces.BikeData
{
    /// <summary>
    /// Created by  :   Sumit Kate on 03 Mar 2016
    /// Description :   Bike Makes Cache Repository
    /// Modified By : Sangram Nandkhile on 20 Jun 2016
    /// Summary :   Added new function GetDiscontinuedBikeModelsByMake
    /// Modified by :   Sumit Kate on 16 Sep 2016
    /// Description :   Added new function GetMakeMaskingResponse
    /// Modified by :   Sangram Nandkhile on 10 Mar 2017
    /// Description :   Added new function GetScooterMakes
    /// Modified by :   Aditi Srivastava on 15 Mar 2017
    /// Summary     :   Added new function GetScooterMakeDescription
    /// </summary>
    public interface IBikeMakesCacheRepository<U>
    {
        IEnumerable<BikeMakeEntityBase> GetMakesByType(EnumBikeType makeType);
        IEnumerable<BikeVersionEntity> GetDiscontinuedBikeModelsByMake(uint makeId);
        BikeDescriptionEntity GetMakeDescription(U makeId);
        BikeMakeEntityBase GetMakeDetails(uint makeId);
        IEnumerable<BikeMakeModelBase> GetAllMakeModels();
        MakeMaskingResponse GetMakeMaskingResponse(string maskingName);
        IEnumerable<BikeMakeEntityBase> GetScooterMakes();
        BikeDescriptionEntity GetScooterMakeDescription(uint makeId);
    }
}
