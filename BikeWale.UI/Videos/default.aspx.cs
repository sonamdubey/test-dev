using System;

namespace Bikewale.Videos
{
    public class Default : System.Web.UI.Page
    {

        protected Bikewale.Controls.Videos ctrlVideosLanding;
        protected Bikewale.Controls.VideoByCategory ctrlFirstRide, ctrlLaunchAlert;
        protected Bikewale.Controls.ExpertReviewVideos ctrlExpertReview;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //device detection
            //DeviceDetection dd = new DeviceDetection();
            //dd.DetectDevice();

            ctrlVideosLanding.CategoryId = Entities.Videos.EnumVideosCategory.FeaturedAndLatest;
            ctrlVideosLanding.TotalRecords = 5;

            ctrlFirstRide.CategoryId = Entities.Videos.EnumVideosCategory.JustLatest;
            ctrlFirstRide.TotalRecords = 6;
            ctrlFirstRide.sectionTitle = "First Ride";

            ctrlLaunchAlert.CategoryId = Entities.Videos.EnumVideosCategory.FeaturedAndLatest;
            ctrlLaunchAlert.TotalRecords = 6;
            ctrlLaunchAlert.sectionTitle = "Launch Alert";

            ctrlExpertReview.CategoryId = Entities.Videos.EnumVideosCategory.FeaturedAndLatest;
            ctrlExpertReview.TotalRecords = 2;
            ctrlExpertReview.sectionTitle = "Exper Reviews";
        }
    }
}