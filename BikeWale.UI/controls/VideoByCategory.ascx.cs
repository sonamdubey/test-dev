using Bikewale.BindViewModels.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bikewale.Entities.Videos;

namespace Bikewale.Controls
{
    /// <summary>
    /// Created By : Lucky Rathore
    /// Created On : 18 Feb 2016
    /// Description : For Expert Review Video Controles.
    /// </summary>
    public class VideoByCategory : System.Web.UI.UserControl
    {
        protected Repeater rptVideosByCat;
        public uint TotalRecords { get; set; }
        public EnumVideosCategory CategoryId { get; set; }
        public string viewMoreURL { get; set; }        
        public int FetchedRecordsCount { get; set; }
        public string SectionTitle { get; set; }
        public string SectionBackgroundClass { get; set; }
        public BikeVideoEntity FirstVideoRecord { get; set; }

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindVideosLandingControl objVideo = new BindVideosLandingControl();
            objVideo.TotalRecords = this.TotalRecords;
            objVideo.CategoryId = this.CategoryId;
            objVideo.FetchVideos();
            this.FetchedRecordsCount = objVideo.FetchedRecordsCount;
            this.FirstVideoRecord = objVideo.FirstVideoRecord;
            objVideo.BindVideos(rptVideosByCat);
            
        }

        public override void Dispose()
        {
            rptVideosByCat.DataSource = null;
            rptVideosByCat.Dispose();
            base.Dispose();
        }
    }
}