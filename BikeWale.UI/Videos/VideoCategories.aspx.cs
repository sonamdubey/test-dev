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
using System.Text.RegularExpressions;


namespace Bikewale.Videos
{
    /// <summary>
    /// Created By : Lucky Rathore on 19 Feb 2016
    /// </summary>
    public class VideoCategories : System.Web.UI.Page
    {
        protected Repeater rptVideos;
        
        protected int maxPage = 0;
        protected LinkPagerControl repeaterPager;
        protected int categoryId = 0;
        protected string make = string.Empty, model = string.Empty, titleName = string.Empty, category = string.Empty, descName = string.Empty;
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
            titleName = string.Empty;
           categoryIdList  = Convert.ToString(Request.QueryString.Get("cid"));            
            if (!string.IsNullOrEmpty(Request.QueryString["cid"]))
            {
                categoryId = Convert.ToUInt16(Request.QueryString["cid"]);
            }
            if (!string.IsNullOrEmpty(Request.QueryString["title"]))
                {
                    titleName = Request.QueryString["title"];
                    titleName.Replace("-", " ");
                    var regCapitalize = Regex.Replace(titleName, @"\b(\w)", m => m.Value.ToUpper());
                    titleName = Regex.Replace(regCapitalize, @"(\s(of|in|by|and)|\'[st])\b", m => m.Value.ToLower(), RegexOptions.IgnoreCase);
                    titleName = string.Format("{0}  Review - BikeWale", titleName);
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
                    }

                    if (objVideosList != null)
                    {
                        if (objVideosList.Videos.Count() > 0)
                        {
                            category = Bikewale.Utility.VideoTitleDescription.VideoHeading(2);
                            rptVideos.DataSource = objVideosList.Videos;
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