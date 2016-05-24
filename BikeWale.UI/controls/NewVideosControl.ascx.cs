using Bikewale.BindViewModels.Controls;
using System;
using System.Web.UI.WebControls;

namespace Bikewale.Controls
{

    /// <summary>
    /// Written By : Sangram Nandkhile on 24 May 2016
    /// Summary : Control to show videos
    /// </summary>
    public class NewVideosControl : System.Web.UI.UserControl
    {
        protected Repeater rptVideos;

        public int TotalRecords { get; set; }
        public int? MakeId { get; set; }
        public int? ModelId { get; set; }
        public int FetchedRecordsCount { get; set; }
        public string MakeMaskingName { get; set; }
        public string ModelMaskingName { get; set; }
        protected string MoreVideoUrl = string.Empty;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindVideos();
            if (String.IsNullOrEmpty(MakeMaskingName) && String.IsNullOrEmpty(ModelMaskingName))
            {
                MoreVideoUrl = string.Format("/bike-videos/");
            }

            else if (!String.IsNullOrEmpty(MakeMaskingName) && String.IsNullOrEmpty(ModelMaskingName))
            {
                MoreVideoUrl = string.Format("/{0}-bikes/videos/", MakeMaskingName);
            }
            else if (!String.IsNullOrEmpty(MakeMaskingName) && !String.IsNullOrEmpty(ModelMaskingName))
            {
                MoreVideoUrl = string.Format("/{0}-bikes/{1}/videos/", MakeMaskingName, ModelMaskingName);
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

        public override void Dispose()
        {
            rptVideos.DataSource = null;
            rptVideos.Dispose();

            base.Dispose();
        }
    }
}