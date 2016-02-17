using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.Videos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.Controls
{
    public class Videos : System.Web.UI.UserControl
    {
        protected Repeater rptLandingVideos;

        public int TotalRecords { get; set; }
        //public int? MakeId { get; set; }
        //public int? ModelId { get; set; }
        public int FetchedRecordsCount { get; set; }
        protected BikeVideoEntity FirstVideoRecord { get; set; }

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
            BindVideosLandingControl objVideo = new BindVideosLandingControl();
            objVideo.TotalRecords = this.TotalRecords;
            //objVideo.MakeId = this.MakeId;
            //objVideo.ModelId = this.ModelId;

            objVideo.BindVideos(rptLandingVideos);
            this.FirstVideoRecord = objVideo.FirstVideoRecord;
            this.FetchedRecordsCount = objVideo.FetchedRecordsCount;
        }

        public override void Dispose()
        {
            rptLandingVideos.DataSource = null;
            rptLandingVideos.Dispose();

            base.Dispose();
        }
    }
}