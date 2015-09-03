using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bikewale.Controls;
using Bikewale.controls;

namespace Bikewale
{
    public class Default : System.Web.UI.Page
    {
        protected News_new ctrlNews;
        protected ExpertReviews ctrlExpertReviews;
        protected VideosControl ctrlVideos;
        protected ComparisonMin ctrlCompareBikes;
        protected PopularUsedBikes ctrlPopularUsedBikes;
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlNews.TotalRecords = 3;
            ctrlExpertReviews.TotalRecords = 3;
            ctrlVideos.TotalRecords = 3;
            ctrlCompareBikes.TotalRecords = 4;
            ctrlPopularUsedBikes.TotalRecords = 6;            
        }
    }
}