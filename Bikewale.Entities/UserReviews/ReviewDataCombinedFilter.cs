using Bikewale.Entities.UserReviews.Search;

namespace Bikewale.Entities.UserReviews
{
    /// <summary>
    /// created by sajal gupta on 21-09-2017
    /// Entity to filter user reviews 
    /// </summary>
    public class ReviewDataCombinedFilter
    {
        public ReviewFilter ReviewFilter { get; set; }
        public InputFilters InputFilter { get; set; }
    }
}
