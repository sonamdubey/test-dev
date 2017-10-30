using Bikewale.Entities.BikeData;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bikewale.Entities.Compare
{
    /// <summary>
    /// Created By:Snehal Dange on 26th Oct 2017
    /// Description :  Entity created for similar bike comparison widget on Compare bikes
    /// </summary>
    [Serializable, DataContract]
    public class SimilarBikeComparisonWrapper
    {
        [DataMember]
        public IEnumerable<SimilarBikeComparisonData> SimilarBikes { get; set; }
        [DataMember]
        public IEnumerable<BasicBikeEntityBase> BikeList { get; set; }        
    }
}
