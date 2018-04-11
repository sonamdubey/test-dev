
using System.Collections.Generic;
namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Created by : Sangram Nandkhile on 27 Dec 2017
    /// Entity to hold dealer pricing for model versions
    /// Modified by : Rajan Chauhan on 10 Apr 2018
    /// Description : Removed explicit specs with minSpecsList
    /// </summary>
    public class BikeVersionWithMinSpec
    {
        public uint VersionId { get; set; }
        public string VersionName { get; set; }
        public long OnRoadPrice { get; set; }
        public IEnumerable<SpecsItem> MinSpecsList { get; set; }
    }
}
