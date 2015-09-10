using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bikewale.Mobile.Controls;
using Bikewale.Mobile.controls;
using Bikewale.Common;

namespace Bikewale.Mobile
{
    public class Default : System.Web.UI.Page
    {
        protected NewsWidget ctrlNews;
        protected ExpertReviewsWidget ctrlExpertReviews;
        protected VideosWidget ctrlVideos;
        protected CompareBikesMin ctrlCompareBikes;
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {   
            //device detection
            DeviceDetection dd = new DeviceDetection(Request.ServerVariables["HTTP_X_REWRITE_URL"].ToString());
            dd.DetectDevice();

            ctrlNews.TotalRecords = 3;
            ctrlExpertReviews.TotalRecords = 3;
            ctrlVideos.TotalRecords = 3;
            ctrlCompareBikes.TotalRecords = 1;
        }
    }
}