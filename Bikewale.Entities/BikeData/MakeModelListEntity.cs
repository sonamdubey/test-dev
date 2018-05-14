

using System.Collections.Generic;

namespace Bikewale.Entities.BikeData
{
    public class MakeModelListEntity
    {
        public BikeMakeEntityBase MakeBase { get; set; }
        public IEnumerable<BikeModelEntityBase> ModelBase { get; set; }
    }
}
