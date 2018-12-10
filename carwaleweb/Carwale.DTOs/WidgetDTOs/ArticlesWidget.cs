using Carwale.Entity.CMS.Articles;
using Carwale.Entity.CMS.UserReviews;
using System.Collections.Generic;

namespace Carwale.DTOs.WidgetDTOs
{
    public class ArticlesWidget
    {
        public List<ArticleSummary> Articles;
        public List<UserReviewEntity> UserReviewList;
        public string LandingUrl;
        public string Heading;
    }
}
