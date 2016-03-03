using Bikewale.BindViewModels.Webforms;
using Bikewale.Cache.Core;
using Bikewale.Cache.Videos;
using Bikewale.Common;
using Bikewale.m.controls;
using Bikewale.Entities.Videos;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Videos;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Web;
namespace Bikewale.m.videos
{
    public class video : System.Web.UI.Page
    {
        protected BikeVideoEntity videoModel;
        protected SimilarVideos ctrlSimilarVideos;
        protected uint videoId = 0;
        protected bool isMakeModelTag = false;
        protected string metaDesc = string.Empty;
        protected string metaKeywords = string.Empty;
        protected string metaTitle = string.Empty;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            ParseQueryString();
            BindSimilarVideoControl();
            BindVideoDetails();
            CreateDescriptionTag();
        }
        /// <summary>
        /// Create a meta descriptin tag using switch case
        /// </summary>
        private void CreateDescriptionTag()
        {
            if (isMakeModelTag)
            {
                metaKeywords = String.Format("{0},{1}, {0} {1}", videoModel.MakeName, videoModel.ModelName);
                string yeh = EnumVideosCategory.DoitYourself.ToString();
                switch (videoModel.SubCatId)
                {
                    case "55":
                        metaDesc = String.Format("{0} {1} Video Review-Watch BikeWale Expert's Take on {0} {1}-Features, performance, price, fuel economy, handling and more.", videoModel.MakeName, videoModel.ModelName);
                        metaTitle = String.Format("Expert Video Review-{0} {1} - BikeWale", videoModel.MakeName, videoModel.ModelName);
                        videoModel.SubCatName = "Expert Reviews";
                        break;
                    case "57":
                        metaDesc = String.Format("First Ride Video Review of {0} {1}-Watch BikeWale Expert's Take on the First Ride of {0} {1}-Features, performance, price, fuel economy, handling and more.", videoModel.MakeName, videoModel.ModelName);
                        metaTitle = String.Format("First Ride Video Review-{0} {1} - BikeWale", videoModel.MakeName, videoModel.ModelName);
                        break;
                    case "59":
                        metaDesc = String.Format("Launch Video of {0} {1}-{0} {1} bike launched. Watch BikeWale's Expert's take on its Launch-Features, performance, price, fuel economy, handling and more.", videoModel.MakeName, videoModel.ModelName);
                        metaTitle = String.Format("Bike Launch Video Review-{0} {1} - BikeWale", videoModel.MakeName, videoModel.ModelName);
                        break;
                    case "53":
                        metaDesc = String.Format("Do It Yourself tips for {0} {1}.  Watch Do It Yourself tips for {0} {1} from BikeWale's Experts.", videoModel.MakeName, videoModel.ModelName);
                        metaTitle = String.Format("Do It Yourself-{0} {1} - BikeWale", videoModel.MakeName, videoModel.ModelName);
                        break;
                    default:
                        metaDesc = "Check latest bike and scooter videos, watch BikeWale expert's take on latest bikes and scooters-features, performance, price, fuel economy, handling and more.";
                        metaTitle = String.Format("{0} - BikeWale", videoModel.VideoTitle);
                        break;
                }
            }
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
            ctrlSimilarVideos.SectionTitle = "Related videos";
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

                    #region Post API call video details

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

                    #endregion
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