using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Created by : Ashutosh Sharma on 15 Mar 2018.
    /// Description : Entity to specs & features data.
    /// </summary>
    public class SpecsFeatures
    {
        /// <summary>
        /// Features doesn't have sub categories.
        /// </summary>
        public IEnumerable<SpecsFeaturesItem> Features { get; set; }
        public IEnumerable<SpecsFeaturesCategory> Specs { get; set; }
    }
}
