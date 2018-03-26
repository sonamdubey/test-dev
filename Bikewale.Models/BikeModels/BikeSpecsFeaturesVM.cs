using Bikewale.Entities.BikeData;
using System.Linq;

namespace Bikewale.Models.BikeModels
{
    /// <summary>
    /// Created by : Ashutosh Sharma on 19 Mar 2018
    /// </summary>
    public class BikeSpecsFeaturesVM
    {
        public string BikeName { get; set; }
        public bool IsVersionSpecsAvailable { get { return VersionSpecsFeatures != null && ((VersionSpecsFeatures.Specs != null && VersionSpecsFeatures.Specs.Any()) || (VersionSpecsFeatures.Features != null && VersionSpecsFeatures.Features.Any())); } }
        public string ModelName { get; set; }
        public SpecsFeaturesEntity VersionSpecsFeatures { get; set; }
    }
}
