using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Entities.CMS.Photos;

namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Modified BY : Ashish G. Kamble on 8 Oct 2015
    /// </summary>
    [Serializable]
    public class BikeModelPageEntity
    {        
        public BikeDescriptionEntity ModelDesc { get; set; }
        
        public BikeModelEntity ModelDetails { get; set; }

        public List<BikeVersionMinSpecs> ModelVersions { get; set; }

        public BikeSpecificationEntity ModelVersionSpecs { get; set; }

        public IEnumerable<NewBikeModelColor> ModelColors { get; set; }

        public UpcomingBikeEntity UpcomingBike { get; set; }

        public List<ModelImage> Photos { get; set; }

        public Overview objOverview { get; set; }

        public Features objFeatures { get; set; }

        public Specifications objSpecs { get; set; } 
    }
}
