using Bikewale.BindViewModels.Controls;
using Bikewale.BindViewModels.Webforms.Videos;
using Bikewale.Entities.Videos;
using Bikewale.Mobile.Controls;
using Bikewale.Notifications;
using System;
using System.Web.UI.WebControls;

namespace Bikewale.Mobile.Videos
{
    /// <summary>
    /// Created By : Sushil Kumar K
    /// Created On : 25th February 2016
    /// Description : To bind all sections of video landing page
    /// Modified By : Vivek Gupta on 01-03-2015, added First look, power drift blockbuster , powerdrift specials and removed do it yourself
    /// </summary>
    public class Default : System.Web.UI.Page
    {

        protected Repeater rptLandingVideos;
        protected VideosByCategory ctrlFirstRide, ctrlLaunchAlert, ctrlMiscellaneous, ctrlTopMusic
                                  , ctrlFirstLook, ctrlPowerDriftBlockBuster, ctrlMotorSports, ctrlPowerDriftSpecials;
        protected ExpertReviewVideos ctrlExpertReview;
        protected int ctrlVideosLandingCount = 0;
        protected BikeVideoEntity ctrlVideosLandingFirst = null;
        protected BindViewModelVideoDefault objVideo;
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindLandingVideos();
            BindMakewidget();
            ctrlExpertReview.CategoryIdList = "55";
            ctrlExpertReview.TotalRecords = 2;
            ctrlExpertReview.SectionTitle = "Expert Reviews";

            ctrlFirstRide.CategoryIdList = "57";
            ctrlFirstRide.TotalRecords = 6;
            ctrlFirstRide.SectionTitle = "First Ride Impressions";

            ctrlLaunchAlert.CategoryIdList = "59";
            ctrlLaunchAlert.TotalRecords = 6;
            ctrlLaunchAlert.SectionTitle = "Launch Alert";

            ctrlFirstLook.CategoryIdList = "61";
            ctrlFirstLook.TotalRecords = 6;
            ctrlFirstLook.SectionTitle = "First Look";

            ctrlPowerDriftBlockBuster.CategoryIdList = "62";
            ctrlPowerDriftBlockBuster.TotalRecords = 6;
            ctrlPowerDriftBlockBuster.SectionTitle = "PowerDrift Blockbuster";

            ctrlMotorSports.CategoryIdList = "51";
            ctrlMotorSports.TotalRecords = 6;
            ctrlMotorSports.SectionTitle = "Motorsports";

            ctrlPowerDriftSpecials.CategoryIdList = "63";
            ctrlPowerDriftSpecials.TotalRecords = 6;
            ctrlPowerDriftSpecials.SectionTitle = "PowerDrift Specials";

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
            objVideo.CategoryId = Entities.Videos.EnumVideosCategory.FeaturedAndLatest;//changed from popular to featured and latest
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
                    objVideo.TopCount = 6;
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