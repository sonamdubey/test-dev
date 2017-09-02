using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.UserReviews
{
    /// <summary>
    /// Created by : Snehal Dange on 01-09-2017
    /// Summary    : Dto created for userreviews data
    /// </summary>
   public class UserReviewsData
    {
       
        public IEnumerable<UserReviewQuestionDto> Questions { get; set; }
        public IEnumerable<UserReviewRatingDto> Ratings { get; set; }
        public IEnumerable<UserReviewOverallRatingDto> OverallRating { get; set; }
        public IEnumerable<UserReviewPriceRange> PriceRange { get; set; }
    }
}
