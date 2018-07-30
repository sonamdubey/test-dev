using Bikewale.Entities.BikeData;
using Bikewale.Entities.Compare;
using System.Collections.Generic;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by : Aditi Srivastava on 27 Apr 2017
    /// Summary    : View Model for popular comparisons
    /// </summary>
    public class PopularComparisonsVM
    {
        public IEnumerable<SimilarCompareBikeEntity> CompareBikes { get; set; }
        public CompareSources CompareSource { get; set; }
        public bool IsDataAvailable { get; set; }
    }
}
