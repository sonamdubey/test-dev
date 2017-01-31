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
        protected NewsListing objNews = null;
        protected IEnumerable<ArticleSummary> newsArticles = null;
        protected MostPopularBikesMin ctrlPopularBikes;
        protected string startIndex, endIndex, totalArticles;
        private GlobalCityAreaEntity currentCityArea;
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
            GetNewsList();
            currentCityArea = GlobalCityArea.GetGlobalCityArea();
            BindUpcoming();
            BindPopularBikes();

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
                objNews.FetchNewsList(ctrlPager);
                newsArticles = objNews.objNewsList;
                startIndex = Bikewale.Utility.Format.FormatPrice(objNews.StartIndex.ToString());
                endIndex = Format.FormatPrice(objNews.EndIndex.ToString());
                totalArticles = Format.FormatPrice(objNews.TotalArticles.ToString());
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + " : Bikewale.News.NewsListing.GetNewsList");
                objErr.SendMail();
            }
            finally
            {
                if (objNews != null && objNews.IsPageNotFound)
                {
                    Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
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
            ctrlUpcomingBikes.MakeId = (int?)objNews.MakeId;
            ctrlUpcomingBikes.ModelId = (int?)objNews.ModelId;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 02 jan 2017
        /// Description :   Bind Popular Bikes Widget
        /// Modified by :   Sajal Gupta on 27 jan 2017
        /// Description :   Added footer link to the popular bikes widget.
        /// </summary>
        private void BindPopularBikes()
        {
            ctrlPopularBikes.totalCount = 4;
            ctrlPopularBikes.CityId = (int)currentCityArea.CityId;
            ctrlPopularBikes.cityName = currentCityArea.City;
            ctrlPopularBikes.MakeId = (int)objNews.MakeId;
            if (objNews != null && objNews.MakeId > 0)
            {
                ctrlPopularBikes.makeMasking = objNews.objMake.MaskingName;
            }
            ctrlUpcomingBikes.ModelId = (int?)objNews.ModelId;
        }

    }//End of Class
}//End of NameSpace