
using Bikewale.Entities.BikeData;
using System.Collections.Generic;
namespace Bikewale.Models
{

    /// Created By :- Subodh Jain 09 May 2017
    /// Summary :-View model for Comparison with bodystyle
    /// </summary>
    public class ComparisonWidgetVM
    {

        public IEnumerable<SimilarCompareBikeEntity> topBikeCompares { get; set; }
        public IEnumerable<SimilarCompareBikeEntity> topBikeComparesScooters { get; set; }
        public IEnumerable<SimilarCompareBikeEntity> topBikeComparesCruisers { get; set; }
        public IEnumerable<SimilarCompareBikeEntity> topBikeComparesSports { get; set; }

    }
}
