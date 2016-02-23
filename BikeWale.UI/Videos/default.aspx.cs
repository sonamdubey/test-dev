using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.Videos;
using System;
using System.Web.UI.WebControls;

namespace Bikewale.Videos
{
    /// <summary>
    /// Created By : Sushil Kumar K
    /// Created On : 18th February 2016
    /// Description : To bind all sections of video landing page
    /// </summary>
    public class Default : System.Web.UI.Page
    {

        protected Repeater rptLandingVideos;
        protected Bikewale.Controls.VideoByCategory ctrlFirstRide, ctrlLaunchAlert, ctrlMiscellaneous, ctrlTopMusic, ctrlDoItYourself;
        protected Bikewale.Controls.ExpertReviewVideos ctrlExpertReview;
        protected int ctrlVideosLandingCount = 0;
        protected BikeVideoEntity ctrlVideosLandingFirst = null;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //device detection
            //DeviceDetection dd = new DeviceDetection();
            //dd.DetectDevice();

            BindLandingVideos();

            ctrlFirstRide.CategoryIdList = "57";
            ctrlFirstRide.TotalRecords = 6;
            ctrlFirstRide.SectionTitle = "First Ride";
            ctrlFirstRide.SectionBackgroundClass = "";  

            ctrlExpertReview.CategoryIdList = "47,55";
            ctrlExpertReview.TotalRecords = 2;
            ctrlExpertReview.SectionTitle = "Expert Reviews";
            ctrlFirstRide.SectionBackgroundClass = "bg-white";

            ctrlLaunchAlert.CategoryIdList = "59";
            ctrlLaunchAlert.TotalRecords = 6;
            ctrlLaunchAlert.SectionTitle = "Launch Alert";
            ctrlFirstRide.SectionBackgroundClass = "";

            ctrlMiscellaneous.CategoryIdList = "58";
            ctrlMiscellaneous.TotalRecords = 6;
            ctrlMiscellaneous.SectionTitle = "Miscellaneous";
            ctrlMiscellaneous.SectionBackgroundClass = "";

            ctrlTopMusic.CategoryIdList = "60";
            ctrlTopMusic.TotalRecords = 6;
            ctrlTopMusic.SectionTitle = "PowerDrift Top Music";
            ctrlTopMusic.SectionBackgroundClass = "";

            ctrlDoItYourself.CategoryIdList = "53";
            ctrlDoItYourself.TotalRecords = 6;
            ctrlDoItYourself.SectionTitle = "Do it yourself";
            ctrlDoItYourself.SectionBackgroundClass = "";


        }

        protected void BindLandingVideos()
        {
            BindVideosSectionCatwise objVideo = new BindVideosSectionCatwise();
            objVideo.TotalRecords = 5;
            objVideo.CategoryId = Entities.Videos.EnumVideosCategory.MostPopular;
            objVideo.DoSkip = 1;
            objVideo.FetchVideos();
            ctrlVideosLandingFirst = objVideo.FirstVideoRecord;
            ctrlVideosLandingCount = objVideo.FetchedRecordsCount;
            objVideo.BindVideos(rptLandingVideos);
        }

        public override void Dispose()
        {
            rptLandingVideos.DataSource = null;
            rptLandingVideos.Dispose();

            base.Dispose();
        }

    }
}