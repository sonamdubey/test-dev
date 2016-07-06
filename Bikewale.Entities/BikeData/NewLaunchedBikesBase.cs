using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Created by  :   Sumit Kate on 01 Jul 2016
    /// Description :   New Launched Bikes Wrapper
    /// </summary>
    [Serializable, DataContract]
    public class NewLaunchedBikesBase
    {
        [DataMember]
        public IEnumerable<NewLaunchedBikeEntity> Models { get; set; }
        [DataMember]
        public int RecordCount { get; set; }
    }
}
