using Bikewale.BindViewModels.Webforms.EditCMS;
using Bikewale.Common;
using Bikewale.Controls;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.Location;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Web;

namespace Bikewale.News
{
    /// <summary>
    /// Modified By : Ashwini Todkar on 24 Sept 2014
    /// </summary>
    public class Default : System.Web.UI.Page
    {
        protected UpcomingBikesMinNew ctrlUpcomingBikes;
        protected Bikewale.Mobile.Controls.LinkPagerControl ctrlPager;

        protected string prevUrl = string.Empty, nextUrl = string.Empty;
        private NewsListing objNews = null;
        protected IEnumerable<ArticleSummary> newsArticles = null;
        protected MostPopularBikesMin ctrlPopularBikes;

        private const int _pagerSlotSize = 10;
        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
        }

        /// <summary>
        /// Modified By : Sushil Kumar on 26th July 2016
        /// Description : Added Features,expert reviews and autoexpo categories for multiple categories
        /// Created By : Sushil Kumar on 10th Nov 2016
        /// Description : Fetch most pouplar bikes list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Load(object sender, EventArgs e)
        {
            // Modified By :Lucky Rathore on 12 July 2016.
            Form.Action = Request.RawUrl;
            //code for device detection added by Ashwini Todkar
            // Modified By :Ashish Kamble on 5 Feb 2016
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];

            DeviceDetection dd = new DeviceDetection(originalUrl);
            dd.DetectDevice();

            GlobalCityAreaEntity currentCityArea = GlobalCityArea.GetGlobalCityArea();
                        BindUpcoming();

            GetNewsList();

            ctrlPopularBikes.totalCount = 4;
            ctrlPopularBikes.CityId = Convert.ToInt32(currentCityArea.CityId);
            ctrlPopularBikes.cityName = currentCityArea.City;
        }

        /// <summary>
        /// Created By : Sushil Kumar on 10th Nov 2016
        /// Description : Get news list
        /// </summary>
        private void GetNewsList()
        {

            try
            {
                objNews = new NewsListing();

                if (Request["pn"] != null)
                    objNews.ProcessQueryString(Request.QueryString["pn"]);
                objNews.FetchNewsList(ctrlPager);
                newsArticles = objNews.objNewsList;
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + " : Bikewale.News.NewsListing.GetNewsList");
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar on 10th Nov 2016
        /// Description : Get category name by id
        /// </summary>
        /// <param name="catId"></param>
        /// <returns></returns>
        protected string GetContentCategory(string catId)
        {

            if (objNews != null)
            {
                return objNews.GetContentCategory(catId);
            }
            else return string.Empty;

        }

        /// <summary>
        /// Created by : Aditi Srivastava on 8 Nov 2016
        /// Summary  : Bind upcoming bikes list
        /// </summary>
        private void BindUpcoming()
        {
            ctrlUpcomingBikes.sortBy = (int)EnumUpcomingBikesFilter.Default;
            ctrlUpcomingBikes.pageSize = 9;
            ctrlUpcomingBikes.topCount = 4;
          }

    }//End of Class
}//End of NameSpace