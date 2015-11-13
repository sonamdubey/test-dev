using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.BikeData
{
    public class BikeMakeModelEntity
    {
        public BikeMakeEntityBase MakeBase { get; set; }
        public BikeModelEntityBase ModelBase { get; set; }
    }
}
