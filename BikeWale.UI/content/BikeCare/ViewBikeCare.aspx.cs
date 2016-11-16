using Bikewale.BindViewModels.Webforms.EditCMS;
using Bikewale.Controls;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Entities.Location;
using Bikewale.Notifications;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bikewale.Content
{
    public class ViewBikeCare : System.Web.UI.Page
    {
        /// <summary>
        /// Created By:-Subodh Jain 12 Nov 2016
        /// Summary :- Detaile Page for TipsAndAdvice
        /// </summary>
        DetailPageBikeCare objDetailBikeCare;

        protected String baseUrl = String.Empty, pageTitle = String.Empty, modelName = String.Empty, modelUrl = String.Empty, pageDescription = String.Empty, pageKeywords = String.Empty;
        protected String data = String.Empty, nextPageUrl = String.Empty, prevPageUrl = String.Empty, author = String.Empty, displayDate = String.Empty, canonicalUrl = String.Empty;
        protected ArticlePageDetails objTipsAndAdvice;
        public StringBuilder bikeTested;
        protected IEnumerable<ModelImage> objImg = null;
        protected UpcomingBikesMinNew ctrlUpcoming;
        protected MostPopularBikesMin ctrlPopularBikes;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            DetailBikeCare();
            BindPageWidgets();
        }
        /// <summary>
        /// Created By:-Subodh Jain 12 Nov 2016
        /// Summary :- Detaile Page for TipsAndAdvice
        /// </summary>
        private void DetailBikeCare()
        {
            objDetailBikeCare = new DetailPageBikeCare();
            try
            {
                objTipsAndAdvice = objDetailBikeCare.objTipsAndAdvice;
                objImg = objDetailBikeCare.objImg;
                pageTitle = objDetailBikeCare.title;
                pageDescription = objDetailBikeCare.description;
                pageKeywords = objDetailBikeCare.keywords;
                displayDate = objDetailBikeCare.displayDate;
                bikeTested = objDetailBikeCare.bikeTested;
                canonicalUrl = objDetailBikeCare.canonicalUrl;
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "ViewBikeCare.DetailBikeCare");
                objErr.SendMail();
            }


        }
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