using Bikewale.Entities.UserReviews;
using System.Collections.Generic;

namespace Bikewale.Models
{
    /// <summary>
    /// Modified by : Ashutosh Sharma on 24-Aug-2017
    /// Description : Added RatingQuestions and ReviewQuestions
    /// </summary>
    public class UserReviewSummaryVM : ModelBase
    {
        public UserReviewSummary Summary { get; set; }
        public string WriteReviewLink { get; set; }
        public ICollection<UserReviewQuestion> RatingQuestions { get; set; }
        public ICollection<UserReviewQuestion> ReviewQuestions { get; set; }
    }

}
