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

namespace Bikewale.Content
{
    //Modified By : Ashwini Todkar on 29 Sept 2014
    //Modified code to retrieve roadtest from api

    /// <summary>
    /// Modified By : Sushil Kumar on 10th Nov 2016
    /// Description : Bind most popular bikes widget for edit cms
    /// </summary>
    public class DefaultRT : System.Web.UI.Page
    {
        protected UpcomingBikesMinNew ctrlUpcomingBikes;
        protected Bikewale.Mobile.Controls.LinkPagerControl ctrlPager;
        protected string nextUrl = string.Empty, prevUrl = string.Empty, makeName = string.Empty, modelName = string.Empty, makeMaskingName = string.Empty, modelMaskingName = string.Empty;
        protected MostPopularBikesMin ctrlPopularBikes;
        private uint makeId, modelId;
        protected int startIndex, endIndex, totalArticles;
        protected RoadTestListing objRoadTests;
        protected IList<ArticleSummary> articlesList;
        protected PopularBikesByBodyStyle ctrlBikesByBodyStyle;
        protected bool isModelTagged;
        
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
            GetRoadTestList();
        }

        /// <summary>
        /// Created By : Sajal Gupta on 30-01-2017
        /// Description : Binded page through common view model.
        /// </summary>
        private void GetRoadTestList()
        {
            try
            {
                objRoadTests = new RoadTestListing();

                if (!objRoadTests.isRedirection && !objRoadTests.pageNotFound)
                {
                    objRoadTests.GetRoadTestList();

                    if (objRoadTests.isContentFound)
                    {
                        objRoadTests.BindLinkPager(ctrlPager);
                        makeId = objRoadTests.MakeId;
                        makeName = objRoadTests.makeName;
                        makeMaskingName = objRoadTests.makeMaskingName;
                        modelId = objRoadTests.ModelId;
                        modelName = objRoadTests.modelName;
                        modelMaskingName = objRoadTests.modelMaskingName;
                        articlesList = objRoadTests.articlesList;
                        startIndex = objRoadTests.startIndex;
                        endIndex = objRoadTests.endIndex;
                        totalArticles = objRoadTests.totalrecords;
                        prevUrl = objRoadTests.prevPageUrl;
                        nextUrl = objRoadTests.nextPageUrl;
                        BindPageWidgets();
                    }
                }
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, "Bikewale.Content.RoadTest.GetRoadTestList");
            }
            finally
            {
                if (objRoadTests.isRedirection)
                {
                    CommonOpn.RedirectPermanent(objRoadTests.redirectUrl);
                }
                else if (objRoadTests.pageNotFound || !objRoadTests.isContentFound)
                {
                    UrlRewrite.Return404();
                }
            }
        }


        /// <summary>
        /// Created by : Aditi Srivastava on 8 Nov 2016
        /// Summary  : Bind upcoming bikes list
        /// Modified by : Sajal Gupta on 27-01-2017
        /// Descriuption  : Added footer lnk to the widget when makeid not present.
        /// </summary>
        private void BindPageWidgets()
        {

            try
            {
                GlobalCityAreaEntity currentCityArea = GlobalCityArea.GetGlobalCityArea();
                isModelTagged = (modelId > 0);
                if (ctrlPopularBikes!=null)
                {
                    ctrlPopularBikes.totalCount = 4;
                    ctrlPopularBikes.CityId = Convert.ToInt32(currentCityArea.CityId);
                    ctrlPopularBikes.cityName = currentCityArea.City;
                    if (makeId > 0)
                    {
                        ctrlPopularBikes.MakeId = (int)makeId;
                        ctrlPopularBikes.makeMasking = objRoadTests.makeMaskingName;
                        ctrlPopularBikes.makeName = objRoadTests.makeName;
                    }
                }
                if (isModelTagged)
                {
                    if (ctrlBikesByBodyStyle != null)
                    {
                        ctrlBikesByBodyStyle.ModelId = modelId;
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
                        if (makeId > 0)
                        {
                            ctrlUpcomingBikes.MakeId = (int)makeId;
                            ctrlUpcomingBikes.makeMaskingName = objRoadTests.makeMaskingName;
                            ctrlUpcomingBikes.makeName = objRoadTests.makeName;
                        }
                    }
                }
              }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Content.RoadTest.BindPageWidgets");
                
            }
        }
    }
}