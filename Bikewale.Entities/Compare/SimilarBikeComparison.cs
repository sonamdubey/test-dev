using Bikewale.Entities.BikeData;
using System.Collections.Generic;

namespace Bikewale.Entities.Compare
{
    /// <summary>
    /// Created By:Snehal Dange on 26th Oct 2017
    /// Description :  Entity created for similar bike comparison widget on Compare bikes
    /// </summary>
    public class SimilarBikeComparison
    {
        public IEnumerable<NewBikeEntityBase>  ComparedBikes{ get; set; }
        public NewBikeEntityBase SimilarBike { get; set; }
        public  sbyte BikeOrder1 { get; set; }
        public sbyte BikeOrder2 { get; set; }

    }
}
