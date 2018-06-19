

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bikewale.Entities.BikeData
{
    [Serializable, DataContract]
    public class MakeModelListEntity
    {
        [DataMember]
        public BikeMakeEntityBase MakeBase { get; set; }
        [DataMember]
        public IEnumerable<BikeModelEntityBase> ModelBase { get; set; }
    }
}
