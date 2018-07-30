using Bikewale.BindViewModels.Webforms.EditCMS;
using Bikewale.Common;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.Location;
using Bikewale.Mobile.Controls;
using Bikewale.Utility;
using System;
using System.Web;

namespace Bikewale.Mobile.Content
{
    /// <summary>
    /// Created By:- Subodh jain 11 Nov 2016
    /// Summary :- Bike Care Landing page
    /// </summary>
    public class BikeCare : System.Web.UI.Page
    {
        BikeCareModels objBikeCare = null;
        public LinkPagerControl ctrlPager;
        protected uint makeId, modelId, totalArticles;
        public string pgPrevUrl = string.Empty, pgNextUrl = string.Empty, pgTitle = string.Empty, pgDescription = string.Empty, pgKeywords = string.Empty;
        protected CMSContent objArticleList;
        public int startIndex = 0, endIndex = 0;
        protected UpcomingBikesMin ctrlUpcomingBikes;
        protected PopularBikesMin ctrlPopularBikes;
        private GlobalCityAreaEntity currentCityArea;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BikeCareTips();
            BindWidgets();
        }

        /// <summary>
        /// Created by : Sajal Gupta on 27-01-2017
        /// Description : Binded upcoming and popular bikes widget.
        /// </summary>
        protected void BindWidgets()
        {
            try
            {
                ctrlUpcomingBikes.sortBy = (int)EnumUpcomingBikesFilter.Default;
                ctrlUpcomingBikes.pageSize = 9;
                ctrlPopularBikes.totalCount = 9;
                currentCityArea = GlobalCityArea.GetGlobalCityArea();
                ctrlPopularBikes.CityId = Convert.ToInt32(currentCityArea.CityId);
                ctrlPopularBikes.cityName = currentCityArea.City;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "m.BikeCare.BindWidgets");
            }
        }

        /// <summary>
        /// Created By:- Subodh jain 11 Nov 2016
        /// Summary :- Bike Care Landing page Binding
        /// </summary>
        private void BikeCareTips()
        {
            objBikeCare = new BikeCareModels();
            if (objBikeCare != null && !objBikeCare.pageNotFound)
            {
                try
                {

                    objBikeCare.BindLinkPager(ctrlPager);
                    objArticleList = objBikeCare.objArticleList;
                    pgTitle = objBikeCare.title;
                    pgDescription = objBikeCare.description;
                    pgKeywords = objBikeCare.keywords;
                    pgPrevUrl = objBikeCare.prevUrl;
                    pgNextUrl = objBikeCare.nextUrl;
                    totalArticles = objBikeCare.totalRecords;
                    startIndex = objBikeCare.startIndex;
                    endIndex = objBikeCare.endIndex;
                }
                catch (Exception ex)
                {
                    ErrorClass.LogError(ex, "BikeCare.BikeCareTips");
                    
                }
            }
            else
            {
                UrlRewrite.Return404();

            }
        }
    }
}