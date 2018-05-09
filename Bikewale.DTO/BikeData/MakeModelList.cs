
using Bikewale.Entities.BikeData;
using System.Collections.Generic;

namespace Bikewale.DTO.BikeData
{
    public class MakeModelList
    {
        public BikeMakeEntityBase MakeBase { get; set; }
        public IEnumerable<BikeModelEntityBase> ModelBase { get; set; }
    }
}
