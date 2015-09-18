using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.BikeData
{
    public class BikeModelPageEntity
    {
        public BikeDescriptionEntity ModelDesc { get; set; }
        public BikeModelEntity ModelDetails { get; set; }
        public List<BikeVersionMinSpecs> ModelVersions { get; set; }
        public BikeSpecificationEntity ModelVersionSpecs { get; set; }
        public IEnumerable<BikeModelColor> ModelColors { get; set; }
        public UpcomingBikeEntity UpcomingBike { get; set; }
    }
}
