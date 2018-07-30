using Bikewale.BindViewModels.Webforms.EditCMS;
using Bikewale.Common;
using Bikewale.Controls;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.Location;
using Bikewale.Utility;
using System;

namespace Bikewale.Content
{
    /// <summary>
    /// Created By:- Subodh jain 11 Nov 2016
    /// Summary :- Bike Care Landing page Desktop
    /// </summary> 
    public class BikeCare : System.Web.UI.Page
    {
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
            // Modified By :Lucky Rathore on 12 July 2016.
            Form.Action = Request.RawUrl;
            //code for device detection added by Ashwini Todkar
            // Modified By :Ashish Kamble on 5 Feb 2016
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];
            DeviceDetection dd = new DeviceDetection(originalUrl);
            dd.DetectDevice();
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
                    ErrorClass.LogError(ex, "BikeCare.BikeCareTips");
                    
                }
            }
        }

        /// <summary>
        /// Created By:- Subodh jain 15 Nov 2016
        /// Summary :- Bike Care Landing page Binding for widgets
        /// Modified BY : Sajal Gupta
        /// Description : Added footer to the widget.
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
                ErrorClass.LogError(ex, "BikeCare.BindPageWidgets");
                
            }
        }

    }
}