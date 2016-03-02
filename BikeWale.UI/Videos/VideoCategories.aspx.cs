﻿using Bikewale.Cache.Core;
using Bikewale.Cache.Videos;
using Bikewale.Controls;
using Bikewale.Entities.Videos;
using Bikewale.Interfaces.Videos;
using Bikewale.Interfaces.Cache.Core;
using Microsoft.Practices.Unity;
using System;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Bikewale.Utility.StringExtention;
using Bikewale.Common;
using System.Reflection;


namespace Bikewale.Videos
{
    /// <summary>
    /// Created By : Lucky Rathore on 19 Feb 2016
    /// </summary>
    public class VideoCategories : System.Web.UI.Page
    {
        protected Repeater rptVideos;
        protected LinkPagerControl repeaterPager;
        protected int totalRecords = 0;
        protected string make = string.Empty, model = string.Empty, titleName = string.Empty, canonTitle= string.Empty, pageHeading = string.Empty, descName = string.Empty;
        protected string categoryIdList = string.Empty;

        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            DeviceDetection dd = new DeviceDetection();
            dd.DetectDevice();
            // Read Query string
            ParseQueryString();
            BindVideos();
        }   // page load

        /// <summary>
        /// Written By : ashish G. Kamble on 22 Feb 2016
        /// Summary : function to read the query string values.
        /// </summary>
        private void ParseQueryString()
        {
            categoryIdList  = Request.QueryString.Get("cid");
            categoryIdList = categoryIdList.Replace("-", ",");
            titleName = Request.QueryString["title"];
            canonTitle = titleName.ToLower();
            if (!string.IsNullOrEmpty(titleName))
                {                    
                    //capitalize title
                    titleName = StringHelper.Capitlization(titleName);
                    titleName = titleName.Replace('-', ' ');
                    pageHeading = string.Format("{0} Video", titleName); 
                    titleName = string.Format("{0} Video Review - BikeWale", titleName);
                }
                descName = string.Format("{0} - Watch BikeWale's Expert's Take on New Bike and Scooter Launches - Features, performance, price, fuel economy, handling and more",
                titleName);
            
            
            //title = "Bike Videos, Expert Video Reviews with Road Test & Bike Comparison -   BikeWale";
            //desc = "Check latest bike and scooter videos, " + descText; 
        }

        /// <summary>
        /// Writtten By : Lucky Rathore
        /// Summary : Function to bind the videos to the videos repeater.
        ///           Initially 9 records are binded.
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
                             .RegisterType<ICacheManager, MemcacheManager>();

                    var objCache = container.Resolve<IVideosCacheRepository>();

                    if (!String.IsNullOrEmpty(categoryIdList))
                    {
                        objVideosList = objCache.GetVideosBySubCategory(categoryIdList, 1, 9);
                        if (objVideosList != null)
                        {
                            if (objVideosList.Videos!=null && objVideosList.Videos.Count() > 0)
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
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + MethodBase.GetCurrentMethod().Name);
                objErr.SendMail();
            }
        }   // End of BindVideos

    }   // class
} // namespace