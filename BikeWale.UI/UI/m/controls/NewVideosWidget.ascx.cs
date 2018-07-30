using Bikewale.BindViewModels.Controls;
using System;
using System.Web.UI.WebControls;

namespace Bikewale.Mobile.Controls
{
    public class NewVideosWidget : System.Web.UI.UserControl
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
        public string MakeName { get; set; }
        public string ModelName { get; set; }
        public string linkTitle { get; set; }
        private bool _showWidget = true;
        public bool ShowWidgetTitle { get { return _showWidget; } set { _showWidget = value; } }

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindVideos();
            if (String.IsNullOrEmpty(MakeMaskingName) && String.IsNullOrEmpty(ModelMaskingName))
            {
                MoreVideoUrl = string.Format("/m/videos/");
                linkTitle = "Bikes Videos";
            }

            else if (!String.IsNullOrEmpty(MakeMaskingName) && String.IsNullOrEmpty(ModelMaskingName))
            {
                MoreVideoUrl = string.Format("/m/{0}-bikes/videos/", MakeMaskingName);
                linkTitle = string.Format("{0} Videos", MakeName);
            }
            else if (!String.IsNullOrEmpty(MakeMaskingName) && !String.IsNullOrEmpty(ModelMaskingName))
            {
                MoreVideoUrl = string.Format("/m/{0}-bikes/{1}/videos/", MakeMaskingName, ModelMaskingName);
                linkTitle = string.Format("{0} {1} Videos", MakeName, ModelName);
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