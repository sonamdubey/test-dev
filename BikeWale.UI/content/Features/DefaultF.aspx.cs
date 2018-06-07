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
using System.Web.UI.WebControls;

namespace Bikewale.Content
{
    /// <summary>
    /// Modified By : Ashwini Todkar on 25 Sept 2014
    /// Retrieved features from carwale web api
    /// Modified By : Sushil Kumar on 10th Nov 2016
    /// Description : Bind most popular bikes widget for edit cms
    /// Modified By : Sajal Gupta on 30-01-2017
    /// Description : Binded through commonm view model.
    /// </summary>
    public class DefaultF : System.Web.UI.Page
    {
        protected Repeater rptFeatures;
        protected Bikewale.Mobile.Controls.LinkPagerControl ctrlPager;
        protected string prevPageUrl = string.Empty, nextPageUrl = string.Empty;
        protected MostPopularBikesMin ctrlPopularBikes;
        protected UpcomingBikesMinNew ctrlUpcomingBikes;
        protected int startIndex, endIndex, totalArticles;
        protected FeaturesListing objFeatures;
        protected IList<ArticleSummary> articlesList;

        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
        }


        /// <summary>
        /// Modified By : Sushil Kumar on 10th Nov 2016
        /// Description : Bind most popular bikes widget for edit cms
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

            GetFeaturesList();

        }

        /// <summary>
        /// Created By : Sajal Gupta on 30-01-2017
        /// Description : Binded page through common view model.
        /// </summary>
        private void GetFeaturesList()
        {
            try
            {
                objFeatures = new FeaturesListing();

                if (!objFeatures.IsPageNotFound)
                {
                    objFeatures.GetFeaturesList();

                    if (objFeatures.isContentFound)
                    {
                        objFeatures.BindLinkPager(ctrlPager);
                        prevPageUrl = objFeatures.prevPageUrl;
                        nextPageUrl = objFeatures.nextPageUrl;
                        articlesList = objFeatures.articlesList;
                        startIndex = objFeatures.startIndex;
                        endIndex = objFeatures.endIndex;
                        totalArticles = objFeatures.totalrecords;
                        BindPageWidgets();
                    }
                }
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, "Bikewale.Content.Features.GetFeaturesList");
            }
            finally
            {
                if (objFeatures.IsPageNotFound || !objFeatures.isContentFound)
                {
                    UrlRewrite.Return404();
                }
            }
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 8 Nov 2016
        /// Summary  : Bind upcoming bikes list
        /// Modified By : Sajal Gupta on 27-01-2017
        /// Description : Added footer link to the popular bikes widget
        /// </summary>
        private void BindPageWidgets()
        {
            try
            {
                GlobalCityAreaEntity currentCityArea = GlobalCityArea.GetGlobalCityArea();
                ctrlPopularBikes.totalCount = 4;
                ctrlPopularBikes.CityId = Convert.ToInt32(currentCityArea.CityId);
                ctrlPopularBikes.cityName = currentCityArea.City;

                ctrlUpcomingBikes.sortBy = (int)EnumUpcomingBikesFilter.Default;
                ctrlUpcomingBikes.pageSize = 9;
                ctrlUpcomingBikes.topCount = 4;
               
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Content.Features.BindPageWidgets");
                
            }
        }
    }
}