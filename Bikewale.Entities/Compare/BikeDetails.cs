using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.Compare
{
    public class BikeDetails
    {
        public BikeEntityBase Base { get; set; }
        public BikeSpecification Specification { get; set; }
        public BikeFeature Feature { get; set; }        
    }
}
