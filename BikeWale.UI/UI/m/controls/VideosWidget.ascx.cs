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
        protected string MoreVideoUrl = string.Empty;
        public string MakeMaskingName { get; set; }
        public string ModelMaskingName { get; set; }

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindVideos();
            if (String.IsNullOrEmpty(MakeMaskingName) && String.IsNullOrEmpty(ModelMaskingName))
            {
                MoreVideoUrl = string.Format("/m/bike-videos/");
            }

            else if (!String.IsNullOrEmpty(MakeMaskingName) && String.IsNullOrEmpty(ModelMaskingName))
            {
                MoreVideoUrl = string.Format("/m/{0}-bikes/videos/", MakeMaskingName);
            }
            else if (!String.IsNullOrEmpty(MakeMaskingName) && !String.IsNullOrEmpty(ModelMaskingName))
            {
                MoreVideoUrl = string.Format("/m/{0}-bikes/{1}/videos/", MakeMaskingName, ModelMaskingName);
            }
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