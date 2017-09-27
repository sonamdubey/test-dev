using Bikewale.Entities.UserReviews.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
