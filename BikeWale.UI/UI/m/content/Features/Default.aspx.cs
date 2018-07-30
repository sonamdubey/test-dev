using Bikewale.BindViewModels.Webforms.EditCMS;
using Bikewale.Common;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.Location;
using Bikewale.Mobile.Controls;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Web;

namespace Bikewale.Mobile.Content
{
    /// <summary>
    /// Created By : Ashwini Todkar on 20 May 2014
    /// Modified By : Ashwini Todkar on 30 Sept 2014
    /// Created By : Sushil Kumar on 28th July 2016
    /// Description : Removed commented code realted to get features old content 
    /// Modified by : Aditi Srivastava on 18 Nov 2016
    /// Summary     : Replaced drop down page numbers with Link pagination
    /// Modified By : Sajal Gupta on 30-01-2017
    /// Description : Binded through commonm view model.
    /// </summary>
    public class Features : System.Web.UI.Page
    {
        protected LinkPagerControl ctrlPager;
        protected int curPageNo = 1;
        protected string prevPageUrl = String.Empty, nextPageUrl = String.Empty;
        protected int startIndex = 0, endIndex = 0;
        protected uint totalArticles;
        HttpRequest page = HttpContext.Current.Request;
        protected int totalrecords;
        protected UpcomingBikesMin ctrlUpcomingBikes;
        protected PopularBikesMin ctrlPopularBikes;
        private GlobalCityAreaEntity currentCityArea;
        protected FeaturesListing objFeatures;
        protected IList<ArticleSummary> articlesList;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            GetFeaturesList();
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
                ErrorClass.LogError(ex, "Bikewale.Mobile.Content.Features.BindWidgets");
            }
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
                        totalrecords = objFeatures.totalrecords;
                        BindWidgets();
                    }
                }
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, "Bikewale.Mobile.Content.Features.GetFeaturesList");
            }
            finally
            {
                if (objFeatures.IsPageNotFound || !objFeatures.isContentFound)
                {
                    UrlRewrite.Return404();
                }
            }
        }

    }
}