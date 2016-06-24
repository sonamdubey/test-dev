using Bikewale.Entities.BikeData;
using System.Collections.Generic;

namespace Bikewale.Interfaces.BikeData
{
    /// <summary>
    /// Created by  :   Sumit Kate on 03 Mar 2016
    /// Description :   Bike Makes Cache Repository
    /// Modified By : Sangram Nandkhile on 20 Jun 2016
    /// Summary :   Added new function GetDiscontinuedBikeModelsByMake
    /// </summary>
    public interface IBikeMakesCacheRepository<U>
    {
        IEnumerable<BikeMakeEntityBase> GetMakesByType(EnumBikeType makeType);
        IEnumerable<BikeVersionEntity> GetDiscontinuedBikeModelsByMake(uint makeId);
    }
}
