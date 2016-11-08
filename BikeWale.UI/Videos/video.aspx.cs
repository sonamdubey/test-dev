﻿using Bikewale.Cache.Core;
using Bikewale.Cache.Videos;
using Bikewale.Common;
using Bikewale.Controls;
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
        protected string metaDesc = string.Empty;
        protected string metaKeywords = string.Empty;
        protected string metaTitle = string.Empty;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            // Modified By :Lucky Rathore on 12 July 2016.
            Form.Action = Request.RawUrl;
            // Modified By :Ashish Kamble on 5 Feb 2016
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];

            DeviceDetection dd = new DeviceDetection(originalUrl);
            dd.DetectDevice();
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
                metaDesc = videoModel.Description;
                switch (videoModel.SubCatId)
                {
                    case "55":
                        metaTitle = String.Format("Expert Video Review - {0} {1} - BikeWale", videoModel.MakeName, videoModel.ModelName);
                        videoModel.SubCatName = "Expert Reviews";
                        break;
                    case "57":
                        metaTitle = String.Format("First Ride Video Review - {0} {1} - BikeWale", videoModel.MakeName, videoModel.ModelName);
                        break;
                    case "59":
                        metaTitle = String.Format("Bike Launch Video Review - {0} {1} - BikeWale", videoModel.MakeName, videoModel.ModelName);
                        break;
                    case "53":
                        metaTitle = String.Format("Do It Yourself - {0} {1} - BikeWale", videoModel.MakeName, videoModel.ModelName);
                        break;
                    default:
                        if (metaDesc == string.Empty)
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
                             .RegisterType<ICacheManager, MemcacheManager>();

                    var objCache = container.Resolve<IVideosCacheRepository>();
                    videoModel = objCache.GetVideoDetails(videoId);

                    #region Post API call video details

                    if (videoModel != null)
                    {
                        if (videoModel.MakeName != null || videoModel.ModelName != null)
                            isMakeModelTag = true;
                        if (!string.IsNullOrEmpty(videoModel.DisplayDate))
                            videoModel.DisplayDate = FormatDate.GetFormatDate(videoModel.DisplayDate, "MMMM dd, yyyy");
                        videoModel.Description = FormatDescription.SanitizeHtml(videoModel.Description);
                    }
                    else
                    {
                        Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        this.Page.Visible = false;
                    }
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