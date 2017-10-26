using Bikewale.Entities.BikeData;
using System.Collections.Generic;

namespace Bikewale.Entities.Compare
{
    /// <summary>
    /// Created By:Snehal Dange on 26th Oct 2017
    /// Description :  Entity created for similar bike comparison widget on Compare bikes
    /// </summary>
    public class SimilarBikeComparisonWrapper
    {
        public IEnumerable<SimilarBikeComparisonData> SimilarBikes { get; set; }
        public IEnumerable<BasicBikeEntityBase> BikeList { get; set; }        
    }
}
