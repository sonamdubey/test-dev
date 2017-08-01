using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.Entity.UserReviews
{
    /// <summary>
    /// Created By : Ashish G. Kamble
    /// Summary : Class to use as input filters to the user review
    /// Modifiied by Sajal Gupta on 01-08-20173
    /// Descripitopn Added SearchEmailId, SearchReviewId
    /// </summary>
    public class ReviewsInputFilters
    {
        public uint MakeId { get; set; }
        public uint ModelId { get; set; }
        public ReviewsStatus ReviewStatus { get; set; }
        public DateTime ReviewDate { get; set; }
        public string SearchEmailId { get; set; }
        public uint SearchReviewId { get; set; }
    }
}
