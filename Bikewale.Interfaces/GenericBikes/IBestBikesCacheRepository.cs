using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.NewBikeSearch;
using System.Collections.Generic;

namespace Bikewale.Interfaces.GenericBikes
{
    /// <summary>
    /// Created by  :   Sumit Kate on 26 Dec 2016
    /// Description :   Interface for Best Bikes Cache Repository
    /// </summary>
    public interface IBestBikesCacheRepository
    {
        IEnumerable<BestBikeEntityBase> BestBikesByType(EnumBikeBodyStyles bodyStyle, FilterInput filterInputs, InputBaseEntity input);
    }
}
