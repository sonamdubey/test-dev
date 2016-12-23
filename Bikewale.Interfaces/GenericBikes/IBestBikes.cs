using Bikewale.Entities.GenericBikes;
using System.Collections.Generic;

namespace Bikewale.Interfaces.GenericBikes
{
    public interface IBestBikes
    {
        IEnumerable<BestBikeEntityBase> BestBikesByType(EnumBikeBodyStyles bodyStyle);
    }
}
