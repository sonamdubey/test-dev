using System;
using System.Web;
using Bikewale.Cache.Core;
using Bikewale.Cache.Videos;
using Bikewale.Common;
using Bikewale.DAL.Videos;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Videos;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Videos;
using Bikewale.Mobile.Controls;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
namespace Bikewale.Mobile.Videos
{
    public class Video : System.Web.UI.Page
    {
        protected BikeVideoEntity videoModel;
        protected SimilarVideos ctrlSimilarVideos;
        protected uint videoId = 0;
        protected bool isMakeModelTag = false;
        protected string metaDesc = string.Empty;
        protected string metaKeywords = string.Empty;
        protected string metaTitle = string.Empty;
        protected GenericBikeInfoControl ctrlGenericBikeInfo;
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            ParseQueryString();

            BindVideoDetails();
            BindSimilarVideoControl();
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
        /// Modified  By :- subodh Jain 10 Feb 2017
        /// Summary :- BikeInfo Slug details
        /// </summary>
        private void BindSimilarVideoControl()
        {
            try
            {
                ctrlSimilarVideos.TopCount = 6;
                ctrlSimilarVideos.VideoBasicId = videoId;
                ctrlSimilarVideos.SectionTitle = "Related videos";
                if (ctrlGenericBikeInfo != null)
                {
                    ctrlGenericBikeInfo.ModelId = videoModel.ModelId;
                    ctrlGenericBikeInfo.CityId = GlobalCityArea.GetGlobalCityArea().CityId;
                    ctrlGenericBikeInfo.PageId = BikeInfoTabType.Videos;
                    ctrlGenericBikeInfo.TabCount = 3;
                }
            }
            catch (Exception ex)
            {

                ErrorClass.LogError(ex, "Bikewale.Mobile.Videos.BindSimilarVideoControl");
            }
        }
        /// <summary>
        /// API call to fetch Video details
        /// Modified by :   Sumit Kate on 08 Mar 2016
        /// Description :   Fixed the Object reference exception by adding null check
        /// </summary>
        private void BindVideoDetails()
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IVideosCacheRepository, VideosCacheRepository>()
                             .RegisterType<IVideos, Bikewale.BAL.Videos.Videos>()
                             .RegisterType<ICacheManager, MemcacheManager>()
                             .RegisterType<IVideoRepository, ModelVideoRepository>();

                    var objCache = container.Resolve<IVideosCacheRepository>();
                    videoModel = objCache.GetVideoDetails(videoId);

                    #region Post API call video details
                    if (videoModel != null)
                    {
                        if (videoModel.MakeName != null || videoModel.ModelName != null)
                            isMakeModelTag = true;
                        if (!string.IsNullOrEmpty(videoModel.DisplayDate))
                            videoModel.DisplayDate = FormatDate.GetFormatDate(videoModel.DisplayDate, "dd MMMM yyyy");
                        videoModel.Description = FormatDescription.SanitizeHtml(videoModel.Description);
                    }
                    else
                    {
                        UrlRewrite.Return404();
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
        }
    }
}