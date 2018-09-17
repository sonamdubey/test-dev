using Bikewale.Entities.UserReviews;
using Bikewale.Models.QuestionAndAnswers;
using System.Collections.Generic;
using System.Linq;
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
        public IEnumerable<UserReviewQuestion> RatingQuestions { get; set; }
        public IEnumerable<UserReviewQuestion> ReviewQuestions { get; set; }
        public UnansweredQuestionsVM UnansweredQuestions { get; set; }
        public bool HasUnansweredQuestions { get { return UnansweredQuestions != null && UnansweredQuestions.Questions != null && UnansweredQuestions.Questions.Any(); } }
        public bool IsMobile { get; set; }
    }

}
