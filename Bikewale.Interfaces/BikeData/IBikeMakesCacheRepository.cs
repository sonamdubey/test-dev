using Bikewale.Entities.BikeData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Interfaces.BikeData
{
    /// <summary>
    /// Created by  :   Sumit Kate on 03 Mar 2016
    /// Description :   Bike Makes Cache Repository
    /// </summary>
    public interface IBikeMakesCacheRepository<U>
    {
        IEnumerable<BikeMakeEntityBase> GetMakesByType(EnumBikeType makeType);
    }
}
