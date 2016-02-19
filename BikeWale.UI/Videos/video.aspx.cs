using Bikewale.BindViewModels.Webforms;
using Bikewale.Common;
using Bikewale.controls;
using System;

namespace Bikewale.Videos
{
    public class video : System.Web.UI.Page
    {
        protected VideoDescriptionModel videoModel;
        protected SimilarVideos ctrlSimilarVideos;
        protected int videoId = 0;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //device detection
            DeviceDetection dd = new DeviceDetection(Request.ServerVariables["HTTP_X_REWRITE_URL"]);
            dd.DetectDevice();
            // Read id from query string
            videoId = 18838;
            videoModel = new VideoDescriptionModel(videoId);
        }
        private void BindSimilarVideoControl()
        {
            ctrlSimilarVideos.TopCount = 6;
            ctrlSimilarVideos.BasicId = 20156;
        }
    }
}