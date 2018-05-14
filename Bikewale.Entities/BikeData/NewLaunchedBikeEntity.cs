using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Modified By : Rajan Chauhan on 3 Apr 2018
    /// Description : Added VersionId, MinSpecsList and removed Specs
    /// </summary>
    [Serializable, DataContract]
    public class NewLaunchedBikeEntity : BikeModelEntity
    {
        [DataMember]
        public uint BikeLaunchId { get; set; }
        [DataMember]
        public int VersionId { get; set; }
        [DataMember]
        public DateTime LaunchDate { get; set; }
        [DataMember]
        public ulong BasicId { get; set; }

        public IEnumerable<SpecsItem> MinSpecsList { get; set; }
    }
}