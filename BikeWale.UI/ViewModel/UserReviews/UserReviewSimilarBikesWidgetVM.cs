using System.Collections.Generic;

namespace Bikewale.Models.UserReviews
{
    /// <summary>
    /// Created By  :   Vishnu Teja Yalakuntla on 04 Sep 2017
    /// Description :   View model for user review similar bikes widget
    /// </summary>
    public class UserReviewSimilarBikesWidgetVM
    {
        public IEnumerable<Bikewale.Entities.SimilarBikeUserReview> SimilarBikes { get; set; }
        public string GlobalCityName { get; set; }
    }
}
