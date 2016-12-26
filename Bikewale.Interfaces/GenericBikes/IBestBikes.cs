using Bikewale.Entities.GenericBikes;
using System.Collections.Generic;

namespace Bikewale.Interfaces.GenericBikes
{
    /// <summary>
    /// Created By : Sushil Kumar on 22nd Dec 2016
    /// Description : Interface to get best bikes by type
    /// </summary>
    public interface IBestBikes
    {
        IEnumerable<BestBikeEntityBase> BestBikesByType(EnumBikeBodyStyles bodyStyle);
    }
}
