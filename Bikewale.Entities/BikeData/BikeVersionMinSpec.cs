
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Created by : Sangram Nandkhile on 27 Dec 2017
    /// Entity to hold dealer pricing for model versions
    /// Modified by : Rajan Chauhan on 10 Apr 2018
    /// Description : Removed explicit specs with minSpecsList
    /// </summary>
    [Serializable, DataContract]
    public class BikeVersionWithMinSpec
    {
        [DataMember]
        public uint VersionId { get; set; }
        [DataMember]
        public string VersionName { get; set; }
        [DataMember]
        public long OnRoadPrice { get; set; }
        public IEnumerable<SpecsItem> MinSpecsList { get; set; }
    }
}
