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
        protected UpcomingBikesMinNew ctrlUpcoming;
        protected Bikewale.Mobile.Controls.LinkPagerControl ctrlPager;
        protected string nextUrl = string.Empty, prevUrl = string.Empty, makeName = string.Empty, modelName = string.Empty, makeMaskingName = string.Empty, modelMaskingName = string.Empty;
        protected MostPopularBikesMin ctrlPopularBikes;
        string makeId = string.Empty, modelId = string.Empty;
        protected int startIndex, endIndex, totalArticles;
        protected RoadTestListing objRoadTests;
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
                        makeId = objRoadTests.makeId;
                        makeName = objRoadTests.makeName;
                        makeMaskingName = objRoadTests.makeMaskingName;
                        modelId = objRoadTests.modelId;
                        modelName = objRoadTests.modelName;
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
                ErrorClass objErr = new ErrorClass(err, "Bikewale.Mobile.Content.RoadTest.GetRoadTestList");
            }
            finally
            {
                if (objRoadTests.isRedirection)
                {
                    CommonOpn.RedirectPermanent(objRoadTests.redirectUrl);
                }
                else if (objRoadTests.pageNotFound || !objRoadTests.isContentFound)
                {
                    Response.Redirect("/pagenotfound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
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

                ctrlPopularBikes.totalCount = 4;
                ctrlPopularBikes.CityId = Convert.ToInt32(currentCityArea.CityId);
                ctrlPopularBikes.cityName = currentCityArea.City;

                ctrlUpcoming.sortBy = (int)EnumUpcomingBikesFilter.Default;
                ctrlUpcoming.pageSize = 9;
                ctrlUpcoming.topCount = 4;

                int _makeId;
                int.TryParse(makeId, out _makeId);

                if (_makeId > 0)
                {
                    ctrlPopularBikes.MakeId = Convert.ToInt32(makeId);
                    ctrlPopularBikes.makeMasking = makeMaskingName;
                    ctrlPopularBikes.makeName = makeName;

                    ctrlUpcoming.makeName = makeName;
                    ctrlUpcoming.makeMaskingName = makeMaskingName;
                    ctrlUpcoming.MakeId = Convert.ToInt32(makeId);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + "BindPageWidgets");
                objErr.SendMail();
            }
        }
    }
}