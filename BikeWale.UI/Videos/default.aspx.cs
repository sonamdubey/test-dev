using System;

namespace Bikewale.Videos
{
    public class Default : System.Web.UI.Page
    {

        protected Bikewale.Controls.Videos ctrlVideosLanding;
        protected Bikewale.Controls.VideoByCategory ctrlFirstRide, ctrlLaunchAlert, ctrlMiscellaneous, ctrlTopMusic, ctrlDoItYourself;
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

            ctrlVideosLanding.CategoryId = Entities.Videos.EnumVideosCategory.MostPopular;
            ctrlVideosLanding.TotalRecords = 5;
            ctrlVideosLanding.DoSkip = 1;

            ctrlFirstRide.CategoryId = Entities.Videos.EnumVideosCategory.FirstRide;
            ctrlFirstRide.TotalRecords = 6;
            ctrlFirstRide.SectionTitle = "First Ride";
            ctrlFirstRide.SectionBackgroundClass = "";  

            ctrlExpertReview.CategoryId = Entities.Videos.EnumVideosCategory.ExpertReviews;
            ctrlExpertReview.TotalRecords = 2;
            ctrlExpertReview.SectionTitle = "Expert Reviews";
            ctrlFirstRide.SectionBackgroundClass = "bg-white";

            ctrlLaunchAlert.CategoryId = Entities.Videos.EnumVideosCategory.JustLatest;
            ctrlLaunchAlert.TotalRecords = 6;
            ctrlLaunchAlert.SectionTitle = "Launch Alert";
            ctrlFirstRide.SectionBackgroundClass = "";             

            ctrlMiscellaneous.CategoryId = Entities.Videos.EnumVideosCategory.FirstRide;
            ctrlMiscellaneous.TotalRecords = 6;
            ctrlMiscellaneous.SectionTitle = "Miscellaneous";
            ctrlMiscellaneous.SectionBackgroundClass = "";

            ctrlTopMusic.CategoryId = Entities.Videos.EnumVideosCategory.FirstRide;
            ctrlTopMusic.TotalRecords = 6;
            ctrlTopMusic.SectionTitle = "PowerDrift Top Music ";
            ctrlTopMusic.SectionBackgroundClass = "bg-white";

            ctrlDoItYourself.CategoryId = Entities.Videos.EnumVideosCategory.FirstRide;
            ctrlDoItYourself.TotalRecords = 6;
            ctrlDoItYourself.SectionTitle = "Do it yourself";
            ctrlDoItYourself.SectionBackgroundClass = "";


        }
    }
}