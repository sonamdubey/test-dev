using Bikewale.Entities.Compare;
using System.Collections.Generic;

namespace Bikewale.Models.Compare
{
    /// <summary>
    /// Created By :Snehal Dange on 25th Oct 2017
    /// Description :View model for similar bikes comparison in compare bike page
    /// </summary>
    public class SimilarBikesComparisionVM
    {
        public IEnumerable<SimilarBikeComparisonWidget> SimilarBikeComparison { get; set; }
        public string ModelComparisionText { get; set; }

    }
}
