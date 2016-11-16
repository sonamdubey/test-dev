using Bikewale.BindViewModels.Webforms.EditCMS;
using Bikewale.Controls;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.Location;
using Bikewale.Notifications;
using Bikewale.Utility;
using System;

namespace Bikewale.Content
{
    public class BikeCare : System.Web.UI.Page
    {
        /// <summary>
        /// Created By:- Subodh jain 11 Nov 2016
        /// Summary :- Bike Care Landing page Desktop
        /// </summary> 
        BikeCareModels objBikeCare = null;
        protected UpcomingBikesMinNew ctrlUpcoming;
        protected MostPopularBikesMin ctrlPopularBikes;
        public Bikewale.Mobile.Controls.LinkPagerControl ctrlPager;
        protected uint makeId, modelId, totalArticles;
        public string pgPrevUrl = string.Empty, pgNextUrl = string.Empty, pgTitle = string.Empty, pgDescription = string.Empty, pgKeywords = string.Empty;
        protected CMSContent objArticleList;
        public int startIndex = 0, endIndex = 0;
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BikeCareTips();
            BindPageWidgets();
        }
        /// <summary>
        /// Created By:- Subodh jain 15 Nov 2016
        /// Summary :- Bike Care Landing page Binding
        /// </summary>
        private void BikeCareTips()
        {
            objBikeCare = new BikeCareModels();
            if (objBikeCare != null)
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
                    Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "BikeCare.BikeCareTips");
                    objErr.SendMail();
                }
            }
        }
        /// <summary>
        /// Created By:- Subodh jain 15 Nov 2016
        /// Summary :- Bike Care Landing page Binding for widgets
        /// </summary>
        private void BindPageWidgets()
        {

            try
            {
                GlobalCityAreaEntity currentCityArea = GlobalCityArea.GetGlobalCityArea();

                ctrlPopularBikes.totalCount = 4;
                ctrlPopularBikes.CityId = Convert.ToInt32(currentCityArea.CityId);
                ctrlPopularBikes.cityName = currentCityArea.City;

                ctrlUpcoming.sortBy = (int)EnumUpcomingBikesFilter.Default;
                ctrlUpcoming.pageSize = 9;
                ctrlUpcoming.topCount = 4;

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikeCare.BindPageWidgets");
                objErr.SendMail();
            }
        }

    }
}