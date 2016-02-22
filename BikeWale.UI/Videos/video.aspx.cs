using Bikewale.BindViewModels.Webforms;
using Bikewale.Cache.Core;
using Bikewale.Cache.Videos;
using Bikewale.Common;
using Bikewale.controls;
using Bikewale.Entities.Videos;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Videos;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Web;

namespace Bikewale.Videos
{
    /// <summary>
    /// Created By : Sangram Nandkhile
    /// Created On : 19 Feb 2016
    /// Description : For Similar Video control
    /// </summary>
    public class video : System.Web.UI.Page
    {
        protected BikeVideoEntity videoModel;
        protected SimilarVideos ctrlSimilarVideos;
        protected uint videoId = 0;
        protected bool isMakeModelTag = false;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DeviceDetection dd = new DeviceDetection(Request.ServerVariables["HTTP_X_REWRITE_URL"]);
            dd.DetectDevice();
            ParseQueryString();
            BindSimilarVideoControl();
            BindVideoDetails();
        }

        /// <summary>
        /// Read video Id from query string
        /// </summary>
        private void ParseQueryString()
        {
            string videoBasicId = Request.QueryString["id"];
            if (!string.IsNullOrEmpty(videoBasicId))
            {
                UInt32.TryParse(videoBasicId, out videoId);
            }
        }
        /// <summary>
        ///  Addition param for Similar Video controller
        /// </summary>
        private void BindSimilarVideoControl()
        {
            ctrlSimilarVideos.TopCount = 6;
            ctrlSimilarVideos.VideoBasicId = videoId;
            ctrlSimilarVideos.sectionTitle = "Related videos";
            //ctrlSimilarVideos.BasicId = 20156;
        }
        /// <summary>
        /// API call to fetch Video details
        /// </summary>
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
                    if (videoModel == null)
                    {
                        Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        this.Page.Visible = false;
                    }
                    if (videoModel.MakeName != null || videoModel.ModelName != null)
                        isMakeModelTag = true;
                    if (!string.IsNullOrEmpty(videoModel.DisplayDate))
                        videoModel.DisplayDate = FormatDate.GetFormatDate(videoModel.DisplayDate, "MMMM dd, yyyy");
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