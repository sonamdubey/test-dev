using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.Videos;
using System.Collections.Generic;

namespace Bikewale.Entities.BikeData.v2
{
    /// <summary>
    /// Created By : Sushil Kumar on 6th September 2017
    /// Desc : Entity to store model page contents for editorial and reviews
    /// </summary>
    public class BikeModelContent
    {
        public IEnumerable<UserReviews.UserReviewSummary> ReviewDetails { get; set; }
        public IEnumerable<ArticleSummary> News { get; set; }
        public IEnumerable<ArticleSummary> ExpertReviews { get; set; }
        public IEnumerable<BikeVideoEntity> Videos { get; set; }
        public IEnumerable<ArticleSummary> TipsAndAdvices { get; set; }
    }
}
