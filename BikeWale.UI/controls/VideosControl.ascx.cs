using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bikewale.BindViewModels.Controls;

namespace Bikewale.Controls
{    
    public class VideosControl : System.Web.UI.UserControl
    {
        protected Repeater rptVideos;

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
            BindVideosControl.TotalRecords = this.TotalRecords;
            BindVideosControl.MakeId = this.MakeId;
            BindVideosControl.ModelId = this.ModelId;

            BindVideosControl.BindVideos(rptVideos);

            this.FetchedRecordsCount = BindVideosControl.FetchedRecordsCount;
        }
    }
}