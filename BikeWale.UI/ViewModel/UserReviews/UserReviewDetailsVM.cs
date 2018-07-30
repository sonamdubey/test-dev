using Bikewale.Entities.UserReviews;
using Bikewale.Models.UserReviews;
using System.Collections.Generic;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by Sajal Gupta on 05-05-2017
    /// decription: View model for user review details page
    /// </summary>
    public class UserReviewDetailsVM : ModelBase
    {
        public UserReviewSummary UserReviewDetailsObj { get; set; }
        public ICollection<UserReviewQuestion> RatingQuestions { get; set; }
        public ICollection<UserReviewQuestion> ReviewQuestions { get; set; }
        public uint RatingQuestionCount { get; set; }
        public BikeInfoVM GenericBikeWidgetData { get; set; }
        public RecentExpertReviewsVM ExpertReviews { get; set; }
        public UserReviewSimilarBikesWidgetVM SimilarBikesWidget { get; set; }
        public uint ReviewId { get; set; }
        public UserReviewsSearchVM UserReviews { get; set; }
        public string PageUrl { get; set; }
        public string ReviewAge { get; set; }
    }
}
