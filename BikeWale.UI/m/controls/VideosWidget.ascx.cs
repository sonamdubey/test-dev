using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bikewale.BindViewModels.Controls;

namespace Bikewale.Mobile.Controls
{
    public class VideosWidget : System.Web.UI.UserControl
    {
        protected Repeater rptVideos;
        protected uint counter = 1;
        public int TotalRecords { get; set; }
        public int? MakeId { get; set; }
        public int? ModelId { get; set; }
        public int FetchedRecordsCount { get; set; }

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindVideos();
        }

        protected void BindVideos()
        {
            BindVideosControl objVideo = new BindVideosControl();
            objVideo.TotalRecords = this.TotalRecords;
            objVideo.MakeId = this.MakeId;
            objVideo.ModelId = this.ModelId;

            objVideo.BindVideos(rptVideos);

            this.FetchedRecordsCount = objVideo.FetchedRecordsCount;
        }
    }
}