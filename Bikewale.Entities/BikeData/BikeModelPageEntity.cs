using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.BikeData
{
    public class BikeModelPageEntity
    {
        public BikeDescriptionEntity modelDesc { get; set; }
        public BikeModelEntity modelDetails { get; set; }
        public List<BikeVersionsListEntity> modelVersion { get; set; }
        public BikeSpecificationEntity versionSpecs { get; set; }
    }
}
