using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Created by  :   Sumit Kate on 13 Sep 2016
    /// Description :   Bike Make and make's model list
    /// </summary>
    [Serializable, DataContract]
    public class BikeMakeModelBase
    {
        [DataMember]
        public BikeMakeEntityBase Make { get; set; }
        [DataMember]
        public IEnumerable<BikeModelEntityBase> Models { get; set; }
    }

    [Serializable, DataContract]
    public class BikeModelMake : BikeModelEntityBase
    {
        [DataMember]
        public int MakeId { get; set; }
    }
}
