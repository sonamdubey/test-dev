using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.Videos;
using System;
using System.Web.UI.WebControls;

namespace Bikewale.Controls
{
    /// <summary>
    /// Created By : Sushil Kumar K
    /// Created On : 19th February 2016
    /// Description : Bind Expert Review Videos Control Repeater  
    /// </summary>
    public class ExpertReviewVideos : System.Web.UI.UserControl
    {
        protected Repeater rptCategoryVideos;
        public EnumVideosCategory CategoryId { get; set; }
        public uint TotalRecords { get; set; }
        public string SectionTitle { get; set; }
        public int FetchedRecordsCount { get; set; }
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
            
            BindVideosSectionCatwise objVideo = new BindVideosSectionCatwise();
            objVideo.TotalRecords = this.TotalRecords;
            objVideo.CategoryId = this.CategoryId;
            objVideo.FetchVideos();
            this.FetchedRecordsCount = objVideo.FetchedRecordsCount;
            this.FirstVideoRecord = objVideo.FirstVideoRecord;
            objVideo.BindVideos(rptCategoryVideos);
        }
        public override void Dispose()
        {
            rptCategoryVideos.DataSource = null;
            rptCategoryVideos.Dispose();

            base.Dispose();
        }

    }
}