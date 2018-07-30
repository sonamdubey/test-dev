using Bikewale.Entities.UserReviews;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.Models.UserReviews
{
    /// <summary>
    /// 
    /// </summary>
    public class PopularBikesWithExpertReviewsWidgetVM
    {
        public IEnumerable<PopularBikesWithExpertReviews> ExpertReviews { get; set; }
        public bool IsExpertReviewsAvailable { get { return ExpertReviews != null && ExpertReviews.Any(); } }
    }
}
