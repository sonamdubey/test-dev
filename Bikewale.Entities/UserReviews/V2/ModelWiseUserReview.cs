using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.UserReviews.V2
{
    /// <summary>
    /// Created By :Snehal Dange on 13th Sep 2017
    /// Description: Created for Reviews tab on Compare Details page
    /// </summary>
     public class ModelWiseUserReview
    {

        public UInt16 RatingCount { get; set; }
        public float ReviewCount { get; set; }
        public float ReviewRate { get; set; }
        public string Mileage { get; set; }
        public IEnumerable<QuestionRatingsValueEntity> Questions { get; set; }
        public UserReviewSummary UserReviews { get; set; }


    }
}
