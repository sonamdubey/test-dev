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
    public class ReviewFilter
    {
        public bool RatingQuestion { get; set; }
        public bool ReviewQuestion { get; set; }
        public bool BasicDetails { get; set; }       
        public uint SanitizedReviewLength { get; set; }
        public bool SantizeHtml { get; set; }
    }
}
