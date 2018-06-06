using Bikewale.BindViewModels.Webforms.EditCMS;
using Bikewale.Common;
using Bikewale.Controls;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.Location;
using Bikewale.Utility;
using System;
using System.Collections.Generic;

namespace Bikewale.News
{
    /// <summary>
    /// Modified By : Ashwini Todkar on 24 Sept 2014
    /// </summary>
    public class Default : System.Web.UI.Page
    {
        protected UpcomingBikesMinNew ctrlUpcomingBikes;
        protected Bikewale.Mobile.Controls.LinkPagerControl ctrlPager;
        protected PopularBikesByBodyStyle ctrlBikesByBodyStyle;
        protected string prevUrl = string.Empty, nextUrl = string.Empty;
        protected NewsListing objNews = null;
        protected IEnumerable<ArticleSummary> newsArticles = null;
        protected MostPopularBikesMin ctrlPopularBikes;
        protected string startIndex, endIndex, totalArticles;
        private GlobalCityAreaEntity currentCityArea;
        private const int _pagerSlotSize = 10;
        protected bool isModelTagged;
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
            BindPageWidgets();
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
                ErrorClass.LogError(ex, "Bikewale.News.NewsListing.GetNewsList");
            }
            finally
            {
                if (objNews != null && objNews.IsPageNotFound)
                {
                    UrlRewrite.Return404();
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
        /// Created By : Aditi Srivastava on 2 Feb 2017
        /// Summary    : Bind page widgets(popular/upcoming)
        /// </summary>
        private void BindPageWidgets()
        {
            try
            {
                isModelTagged = (objNews != null && objNews.ModelId > 0);
                if (ctrlPopularBikes != null)
                {
                    ctrlPopularBikes.totalCount = 4;
                    ctrlPopularBikes.CityId = (int)currentCityArea.CityId;
                    ctrlPopularBikes.cityName = currentCityArea.City;
                    if (objNews != null && objNews.objMake != null && objNews.MakeId > 0)
                    {
                        ctrlPopularBikes.MakeId = (int)objNews.MakeId;
                        ctrlPopularBikes.makeMasking = objNews.objMake.MaskingName;
                        ctrlPopularBikes.makeName = objNews.objMake.MakeName;
                    }
                }
                if (isModelTagged)
                {
                    if (ctrlBikesByBodyStyle != null)
                    {
                        ctrlBikesByBodyStyle.ModelId = objNews.ModelId;
                        ctrlBikesByBodyStyle.topCount = 4;
                        ctrlBikesByBodyStyle.CityId = currentCityArea.CityId;
                    }
                }
                else
                {
                    if (ctrlUpcomingBikes != null)
                    {
                        ctrlUpcomingBikes.sortBy = (int)EnumUpcomingBikesFilter.Default;
                        ctrlUpcomingBikes.pageSize = 9;
                        ctrlUpcomingBikes.topCount = 4;
                        if (objNews != null && objNews.objMake != null && objNews.MakeId > 0)
                        {
                            ctrlUpcomingBikes.MakeId = (int?)objNews.MakeId;
                            ctrlUpcomingBikes.makeMaskingName = objNews.objMake.MaskingName;
                            ctrlUpcomingBikes.makeName = objNews.objMake.MakeName;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.News.NewsListing.BindPageWidgets");
            }
        }
    }//End of Class
}//End of NameSpace