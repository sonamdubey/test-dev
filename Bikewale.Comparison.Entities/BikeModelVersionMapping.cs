using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Comparison.Entities
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 28-Jul-2017
    /// Summary: Entity for Bike Model Version Mapping
    /// </summary>
    public class BikeModelVersionmapping : BikeModel
    {
        public uint SponsoredModelId { get; set; }
        public uint SponsoredVersionId { get; set; }
        public string ImpressionUrl { get; set; }
    }

}
