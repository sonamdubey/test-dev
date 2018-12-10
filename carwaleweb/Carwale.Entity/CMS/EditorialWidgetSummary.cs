using Carwale.Entity.CMS.Articles;
using System.Collections.Generic;

namespace Carwale.Entity.CMS
{
    public class EditorialWidgetSummary
    {
        public List<ArticleSummary> TopNews { get; set; }
        public List<ArticleSummary> TopReviews { get; set; }
        public List<Video> TopVideos { get; set; } 
    }
}
