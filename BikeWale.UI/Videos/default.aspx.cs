using Bikewale.BindViewModels.Controls;
using Bikewale.BindViewModels.Webforms.Videos;
using Bikewale.Common;
using Bikewale.Entities.Videos;
using System;
using System.Web.UI.WebControls;

namespace Bikewale.Videos
{
    /// <summary>
    /// Created By : Sushil Kumar K
    /// Created On : 18th February 2016
    /// Description : To bind all sections of video landing page
    /// Modified By : Sushil Kumar on 1stFeb 2016
    /// Description : Added new categories PowerDrift BlockBuster, PowerDrift Specials,First Look and removed categories widget DoItYourself.
    /// </summary>
    public class Default : System.Web.UI.Page
    {

        protected Repeater rptLandingVideos;
        protected Bikewale.Controls.VideoByCategory ctrlFirstRide, ctrlFirstLook, ctrlPDBlockbuster, ctrlPDSpecials, ctrlMiscellaneous, ctrlMotorSports, ctrlTopMusic, ctrlLaunchAlert;
        protected Bikewale.Controls.ExpertReviewVideos ctrlExpertReview;
        protected int ctrlVideosLandingCount = 0;
        protected BikeVideoEntity ctrlVideosLandingFirst = null;
        protected BindViewModelVideoDefault objVideo;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Modified By :Lucky Rathore on 12 July 2016.
            Form.Action = Request.RawUrl;
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];

            DeviceDetection dd = new DeviceDetection(originalUrl);
            dd.DetectDevice();

            BindLandingVideos();
            BindMakewidget();
            ctrlExpertReview.CategoryIdList = "55";
            ctrlExpertReview.TotalRecords = 2;
            ctrlExpertReview.SectionTitle = "Expert Reviews";
            ctrlFirstRide.SectionBackgroundClass = "bg-white";

            ctrlFirstRide.CategoryIdList = "57";
            ctrlFirstRide.TotalRecords = 6;
            ctrlFirstRide.SectionTitle = "First Ride Impressions";

            ctrlLaunchAlert.CategoryIdList = "59";
            ctrlLaunchAlert.TotalRecords = 6;
            ctrlLaunchAlert.SectionTitle = "Launch Alert";
            ctrlFirstRide.SectionBackgroundClass = "";

            ctrlFirstLook.CategoryIdList = "61";
            ctrlFirstLook.TotalRecords = 6;
            ctrlFirstLook.SectionTitle = "First Look";

            ctrlPDBlockbuster.CategoryIdList = "62";
            ctrlPDBlockbuster.TotalRecords = 6;
            ctrlPDBlockbuster.SectionTitle = "PowerDrift Blockbuster";

            ctrlMotorSports.CategoryIdList = "51";
            ctrlMotorSports.TotalRecords = 6;
            ctrlMotorSports.SectionTitle = "MotorSports";

            ctrlPDSpecials.CategoryIdList = "63";
            ctrlPDSpecials.TotalRecords = 6;
            ctrlPDSpecials.SectionTitle = "PowerDrift Specials";

            ctrlTopMusic.CategoryIdList = "60";
            ctrlTopMusic.TotalRecords = 6;
            ctrlTopMusic.SectionTitle = "PowerDrift Top Music";

            ctrlMiscellaneous.CategoryIdList = "58";
            ctrlMiscellaneous.TotalRecords = 6;
            ctrlMiscellaneous.SectionTitle = "Miscellaneous";


        }

        protected void BindLandingVideos()
        {
            BindVideosSectionCatwise objVideo = new BindVideosSectionCatwise();
            objVideo.TotalRecords = 5;
            objVideo.CategoryId = Entities.Videos.EnumVideosCategory.FeaturedAndLatest;
            objVideo.DoSkip = 1;
            objVideo.FetchVideos();
            ctrlVideosLandingFirst = objVideo.FirstVideoRecord;
            ctrlVideosLandingCount = objVideo.FetchedRecordsCount;
            objVideo.BindVideos(rptLandingVideos);
        }
        /// <summary>
        /// Modified By :- Subodh Jain on 17 Jan 2017
        /// Summary :- get makedetails if videos is present
        /// </summary>

        private void BindMakewidget()
        {
            try
            {
                objVideo = new BindViewModelVideoDefault();
                if (objVideo != null)
                {
                    objVideo.TopCount = 10;
                    objVideo.GetMakeIfVideo();
                }
            }
            catch (Exception ex)
            {

                ErrorClass.LogError(ex, "Bikewale.Videos.BindMakewidget");
            }

        }
        public override void Dispose()
        {
            rptLandingVideos.DataSource = null;
            rptLandingVideos.Dispose();

            base.Dispose();
        }

    }
}