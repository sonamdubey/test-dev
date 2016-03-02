using Bikewale.Cache.Core;
using Bikewale.Cache.Videos;
using Bikewale.Common;
using Bikewale.Entities.Videos;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Videos;
using Bikewale.Utility.StringExtention;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.Videos
{
    public partial class VideoMakeModel : System.Web.UI.Page
    {
        protected Repeater rptVideos;
        protected int totalRecords = 0;
        protected string makeName = string.Empty, modelName = string.Empty, titleName = string.Empty, canonTitle = string.Empty, pageHeading = string.Empty, descName = string.Empty;
        protected bool isModel=false;
        protected uint makeId;
        protected uint? modelId;


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
            if (!String.IsNullOrEmpty(Request.QueryString.Get("id"))) makeId = Convert.ToUInt16(Request.QueryString.Get("id"));
            isModel = true;
            makeId = 1;
            modelId = 99;
            pageHeading = "By sangram";
            //canonTitle = titleName.ToLower();
            //if (!string.IsNullOrEmpty(titleName))
            //{
            //    //capitalize title
            //    titleName = StringHelper.Capitlization(titleName);
            //    titleName = titleName.Replace('-', ' ');
            //    pageHeading = string.Format("{0} Video", titleName);
            //    titleName = string.Format("{0} Video Review - BikeWale", titleName);
            //}
            //descName = string.Format("{0} - Watch BikeWale's Expert's Take on New Bike and Scooter Launches - Features, performance, price, fuel economy, handling and more",
            //titleName);
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
            IEnumerable<BikeVideoEntity> objVideosList = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IVideosCacheRepository, VideosCacheRepository>()
                             .RegisterType<IVideos, Bikewale.BAL.Videos.Videos>()
                             .RegisterType<ICacheManager, MemcacheManager>();

                    var objCache = container.Resolve<IVideosCacheRepository>();

                    if (modelId.HasValue)
                    {
                        objVideosList = objCache.GetVideosByMakeModel(1, 9, makeId, modelId);
                    }
                    else 
                    {
                        objVideosList = objCache.GetVideosByMakeModel(1, 9, makeId);
                    }
                    if (objVideosList != null && objVideosList.Count() > 0)
                    {
                        rptVideos.DataSource = objVideosList;
                        rptVideos.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + "BindVideos()");
                objErr.SendMail();
            }
        }
    }
}