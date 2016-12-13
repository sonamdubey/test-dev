using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Created BY : Sangram Nandkhile on 05 Dec 2016
    /// Desc: Entity to hold Overview, Features and specs for each version
    /// </summary>
    [Serializable, DataContract]
    public class TransposeModelSpecEntity
    {
        [DataMember]
        public uint BikeVersionId { get; set; }
        [DataMember]
        public Overview objOverview { get; set; }
        [DataMember]
        public Features objFeatures { get; set; }
        [DataMember]
        public Specifications objSpecs { get; set; }

    }
}
