using Bikewale.BindViewModels.Webforms;
using Bikewale.Cache.Core;
using Bikewale.Cache.Videos;
using Bikewale.Common;
using Bikewale.controls;
using Bikewale.Entities.Videos;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Videos;
using Microsoft.Practices.Unity;
using System;
using System.Web;

namespace Bikewale.Videos
{
    public class video : System.Web.UI.Page
    {
        //protected VideoDescriptionModel videoModel;
        protected BikeVideoEntity videoModel;
        protected SimilarVideos ctrlSimilarVideos;
        protected uint videoId = 0;

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
            videoId = 20106;
            BindVideoDetails();
            //videoModel = new VideoDescriptionModel(videoId);
        }
        private void BindSimilarVideoControl()
        {
            ctrlSimilarVideos.TopCount = 6;
            ctrlSimilarVideos.sectionTitle = "Related videos";
            ctrlSimilarVideos.BasicId = 20156;
        }
        private void BindVideoDetails()
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IVideosCacheRepository, VideosCacheRepository>()
                             .RegisterType<IVideos, Bikewale.BAL.Videos.Videos>()
                             .RegisterType<ICacheManager, MemcacheManager>();

                    var objCache = container.Resolve<IVideosCacheRepository>();
                    videoModel = objCache.GetVideoDetails(videoId);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }
    }
}