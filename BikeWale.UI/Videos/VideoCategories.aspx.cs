using Bikewale.Cache.Core;
using Bikewale.Cache.Videos;
using Bikewale.Controls;
using Bikewale.Entities.Pager;
using Bikewale.Entities.Videos;
using Bikewale.Interfaces.Videos;
using Bikewale.Interfaces.Cache.Core;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bikewale.Notifications;


namespace Bikewale.Videos
{
    /// <summary>
    /// Created By : Lucky Rathore on 19 Feb 2016
    /// </summary>
    public class VideoCategories : System.Web.UI.Page
    {
        protected Repeater rptVideos;
        protected string category = string.Empty;
        protected string maxPage = string.Empty;
        protected LinkPagerControl repeaterPager;
        protected int categoryId = 0;
        protected string make = string.Empty;
        protected string model = string.Empty;
        protected string categoryIdList = string.Empty;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Read Query string
            ParseQueryString();

            if (!IsPostBack)
            {
                BindVideos();
            }
        }   // page load

        /// <summary>
        /// Written By : ashish G. Kamble on 22 Feb 2016
        /// Summary : function to read the query string values.
        /// </summary>
        private void ParseQueryString()
        {
           categoryIdList  = Convert.ToString(Request.QueryString.Get("cid"));            
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
                    }

                    if (objVideosList != null)
                    {
                        if (objVideosList.Videos.Count() > 0)
                        {
                            category = Bikewale.Utility.VideoTitleDescription.VideoHeading(2);
                            rptVideos.DataSource = objVideosList;
                            rptVideos.DataBind();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }   // End of BindVideos

    }   // class
} // namespace