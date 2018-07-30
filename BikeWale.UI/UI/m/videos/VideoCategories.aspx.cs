using Bikewale.Cache.Core;
using Bikewale.Cache.Videos;
using Bikewale.DAL.Videos;
using Bikewale.Entities.Videos;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Videos;
using Bikewale.Notifications;
using Bikewale.Utility.StringExtention;
using Microsoft.Practices.Unity;
using System;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI.WebControls;

namespace Bikewale.Mobile.Videos
{
    /// <summary>
    /// Created By : Lucky Rathore on 25 Feb 2016
    /// </summary>
    public class VideoCategories : System.Web.UI.Page
    {
        protected Repeater rptVideos;
        protected int totalRecords = 0;
        protected string make = string.Empty, model = string.Empty, titleName = string.Empty, canonTitle = string.Empty,
            pageHeading = string.Empty, descName = string.Empty;
        protected string categoryIdList = string.Empty;
        protected bool Ad_Bot_320x50 = false;

        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Read Query string
            ParseQueryString();
            BindVideos();
        }   // page load

        /// <summary>
        /// Created By : ashish G. Kamble on 22 Feb 2016
        /// Summary : function to read the query string values.
        /// updated by : Lucky Rathore on 25 Feb. 2016
        /// summary : Functionaly for PageHeading Added.
        /// </summary>
        private void ParseQueryString()
        {
            categoryIdList = Request.QueryString.Get("cid");
            if (!string.IsNullOrEmpty(categoryIdList))
            {
                categoryIdList = categoryIdList.Replace("-", ",");
            }
            titleName = Request.QueryString["title"];
            if (!string.IsNullOrEmpty(titleName))
            {
                pageHeading = string.Format("{0} Video", StringHelper.Capitlization(titleName).Replace('-', ' '));
            }
        }

        /// <summary>
        /// Writtten By : Lucky Rathore
        /// Summary : Function to bind the videos to the videos repeater.
        ///           Initially 6 records are binded.
        /// </summary>
        private void BindVideos()
        {
            BikeVideosListEntity objVideosList = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IVideosCacheRepository, VideosCacheRepository>()
                             .RegisterType<IVideos, Bikewale.BAL.Videos.Videos>()
                             .RegisterType<ICacheManager, MemcacheManager>()
                             .RegisterType<IVideoRepository, ModelVideoRepository>();

                    var objCache = container.Resolve<IVideosCacheRepository>();

                    if (!String.IsNullOrEmpty(categoryIdList))
                    {
                        objVideosList = objCache.GetVideosBySubCategory(categoryIdList, 1, 6);
                        if (objVideosList != null)
                        {
                            if (objVideosList.Videos != null && objVideosList.Videos.Any())
                            {
                                totalRecords = objVideosList.TotalRecords;
                                rptVideos.DataSource = objVideosList.Videos;
                                rptVideos.DataBind();
                            }
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"] + MethodBase.GetCurrentMethod().Name);
                
            }
        }
    }
}