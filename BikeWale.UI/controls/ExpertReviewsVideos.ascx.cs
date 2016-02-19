using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.Videos;
using System;
using System.Web.UI.WebControls;

namespace Bikewale.Controls
{
    public class ExpertReviewVideos : System.Web.UI.UserControl
    {
        protected Repeater rptCategoryVideos;
        public EnumVideosCategory CategoryId { get; set; }
        public uint TotalRecords { get; set; }
        public string SectionTitle { get; set; }
        protected int FetchedRecordsCount { get; set; }
        public string SectionBackgroundClass { get; set; }
        protected BikeVideoEntity FirstVideoRecord { get; set; }

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindVideosByCategory();
        }

        public void BindVideosByCategory()
        {
            
            BindVideosLandingControl objVideo = new BindVideosLandingControl();
            objVideo.TotalRecords = this.TotalRecords;
            objVideo.CategoryId = this.CategoryId;
            objVideo.BindVideos(rptCategoryVideos);
            this.FetchedRecordsCount = objVideo.FetchedRecordsCount;
            this.FirstVideoRecord = objVideo.FirstVideoRecord;
        }
        public override void Dispose()
        {
            rptCategoryVideos.DataSource = null;
            rptCategoryVideos.Dispose();

            base.Dispose();
        }

    }
}