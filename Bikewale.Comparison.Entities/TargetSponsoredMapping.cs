using System.Collections.Generic;

namespace Bikewale.Comparison.Entities
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 28-Jul-2017
    /// Summary: Object to hold model versions and respective model version
    /// 
    /// </summary>
    public class TargetSponsoredMapping
    {
        public IEnumerable<BikeModel> SponsoredModelVersion { get; set; }
        public IEnumerable<BikeModelVersionMapping> TargetVersionsMapping { get; set; }
    }

}
