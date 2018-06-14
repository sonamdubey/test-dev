using Bikewale.BAL.BikeData;
using Bikewale.BindViewModels.Webforms.EditCMS;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Mobile.Controls;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Web;

namespace Bikewale.Mobile.Content
{
    /// <summary>
    /// Created By : Ashwini Todkar on 21 May 2014
    /// Modified by : Aditi Srivastava on 18 Nov 2016
    /// Summary     : Replaced drop down page numbers with Link pagination
    /// </summary>
    public class RoadTest : System.Web.UI.Page
    {
        protected LinkPagerControl ctrlPager;
        protected int startIndex = 0, endIndex = 0, totalrecords;
        protected string prevPageUrl = String.Empty, nextPageUrl = String.Empty, makeName = string.Empty, modelName = string.Empty, makeMaskingName = string.Empty, modelMaskingName = string.Empty;
        protected uint modelId, makeId;
        HttpRequest page = HttpContext.Current.Request;
        protected UpcomingBikesMin ctrlUpcomingBikes;
        protected PopularBikesMin ctrlPopularBikes;
        private GlobalCityAreaEntity currentCityArea;
        protected PopularBikesByBodyStyle ctrlBikesByBodyStyle;
        protected RoadTestListing objRoadTests;
        protected IList<ArticleSummary> articlesList;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            GetRoadTestList();
        }

        /// <summary>
        /// Created by : Sajal Gupta on 27-01-2017
        /// Description : Binded upcoming and popular bikes widget.
        /// Modified by : Aditi Srivastava on 2 Feb 2017
        /// Summary     : Modified logic for different kinds of wodgets on model tagging
        /// </summary>
        protected void BindWidgets()
        {
            try
            {
                if (ctrlPopularBikes != null)
                {
                    ctrlPopularBikes.totalCount = 9;
                    currentCityArea = GlobalCityArea.GetGlobalCityArea();
                    ctrlPopularBikes.CityId = Convert.ToInt32(currentCityArea.CityId);
                    ctrlPopularBikes.cityName = currentCityArea.City;
                    if (makeId > 0)
                    {
                        ctrlPopularBikes.makeId = Convert.ToInt32(makeId);
                        ctrlPopularBikes.makeName = makeName;
                        ctrlPopularBikes.makeMasking = makeMaskingName;
                    }
                }
                if (modelId > 0)
                {
                    if (ctrlBikesByBodyStyle != null)
                    {
                        ctrlBikesByBodyStyle.ModelId = modelId;
                        ctrlBikesByBodyStyle.topCount = 9;
                        ctrlBikesByBodyStyle.CityId = currentCityArea.CityId;
                    }
                }
                else
                {
                    if (ctrlUpcomingBikes != null)
                    {
                        ctrlUpcomingBikes.sortBy = (int)EnumUpcomingBikesFilter.Default;
                        ctrlUpcomingBikes.pageSize = 9;
                        if (makeId > 0)
                        {
                            ctrlUpcomingBikes.MakeId = Convert.ToInt32(makeId);
                            ctrlUpcomingBikes.makeName = makeName;
                            ctrlUpcomingBikes.makeMaskingName = makeMaskingName;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Mobile.Content.RoadTest.BindWidgets");
            }
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
                        totalrecords = objRoadTests.totalrecords;
                        prevPageUrl = objRoadTests.prevPageUrl;
                        nextPageUrl = objRoadTests.nextPageUrl;
                        BindWidgets();
                    }
                }
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, "Bikewale.Mobile.Content.RoadTest.GetRoadTestList");
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

    }
}