using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.ElasticSearch.Entities
{
    /// <summary>
    /// Created By : Deepak Israni on 19 Feb 2018
    /// Description : Entity to store version information.
    /// Modified by: Dhruv Joshi
    /// Dated: 20th Feb 2018
    /// Description: Added flag for version status
    /// Modified by: Dhruv Joshi
    /// Dated: 21st Feb 2018
    /// Description: Added minspecs as individual properties instead of an entity
    /// </summary>
    public class VersionEntity
    {

        public uint VersionId { get; set; }
        public string VersionName { get; set; }
        public uint Mileage { get; set; }
        public uint KerbWeight { get; set; }
        public double Power { get; set; }
        public double Displacement { get; set; }
        public IEnumerable<PriceEntity> PriceList { get; set; }
        public BikeStatus VersionStatus { get; set; }

    }
}
