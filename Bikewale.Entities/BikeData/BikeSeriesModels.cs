using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Created by : Vivek Singh Tomar on 26th sep 2017
    /// Modified by : Entity to hold models of similar series
    /// </summary>
    [Serializable, DataContract]
    public class BikeSeriesModels
    {
        [DataMember]
        public IEnumerable<NewBikeEntityBase> NewBikes { get; set; }
        [DataMember]
        public IEnumerable<UpcomingBikeEntityBase> UpcomingBikes { get; set; }
    }
}
