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
        protected string prevPageUrl = String.Empty, nextPageUrl = String.Empty, modelId = string.Empty, makeId = string.Empty, makeName = string.Empty, modelName = string.Empty, makeMaskingName = string.Empty, modelMaskingName = string.Empty;
        private bool _isContentFound = true;
        HttpRequest page = HttpContext.Current.Request;
        protected UpcomingBikesMin ctrlUpcomingBikes;
        protected PopularBikesMin ctrlPopularBikes;
        private GlobalCityAreaEntity currentCityArea;
        protected bool showBodyStyleWidget;
        protected PopularBikesByBodyStyle ctrlBikesByBodyStyle;
        protected EnumBikeBodyStyles bodyStyle;
        protected RoadTestListing objRoadTests;
        private bool pageNotFound;
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
                if (!string.IsNullOrEmpty(makeId) && Convert.ToInt32(makeId) > 0)
                {
                    ctrlPopularBikes.makeId = Convert.ToInt32(makeId);
                    ctrlPopularBikes.makeName = makeName;
                    ctrlPopularBikes.makeMasking = makeMaskingName;
                    ctrlUpcomingBikes.MakeId = Convert.ToInt32(makeId);
                    ctrlUpcomingBikes.makeName = makeName;
                    ctrlUpcomingBikes.makeMaskingName = makeMaskingName;
                }
                else
                {
                    ctrlPopularBikes.IsMakeAgnosticFooterNeeded = true;
                }

                uint intModelId;
                uint.TryParse(modelId, out intModelId);

                if (intModelId > 0)
                {
                    ctrlBikesByBodyStyle.ModelId = intModelId;
                    ctrlBikesByBodyStyle.topCount = 9;
                    ctrlBikesByBodyStyle.CityId = currentCityArea.CityId;
                }

                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>()
                        .RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>()
                        .RegisterType<ICacheManager, MemcacheManager>()
                        .RegisterType<IBikeModelsCacheRepository<int>, BikeModelsCacheRepository<BikeModelEntity, int>>();

                    IBikeModelsCacheRepository<int> modelCache = container.Resolve<IBikeModelsCacheRepository<int>>();
                    if (intModelId > 0)
                    {
                        bodyStyle = modelCache.GetBikeBodyType(intModelId);
                    }
                }

                if (intModelId > 0 && (bodyStyle == EnumBikeBodyStyles.Scooter || bodyStyle == EnumBikeBodyStyles.Cruiser || bodyStyle == EnumBikeBodyStyles.Sports))
                    showBodyStyleWidget = true;

            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "m.RoadTest.BindWidgets");
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
                objRoadTests.BindLinkPager(ctrlPager);
                pageNotFound = objRoadTests.pageNotFound;
                makeId = objRoadTests.makeId;
                makeName = objRoadTests.makeName;
                makeMaskingName = objRoadTests.makeMaskingName;
                modelId = objRoadTests.modelId;
                _isContentFound = objRoadTests.isContentFound;
                modelName = objRoadTests.modelName;
                articlesList = objRoadTests.articlesList;
                startIndex = objRoadTests.startIndex;
                endIndex = objRoadTests.endIndex;
                totalrecords = objRoadTests.totalrecords;
                prevPageUrl = objRoadTests.prevPageUrl;
                nextPageUrl = objRoadTests.nextPageUrl;

                if (!_isContentFound || pageNotFound)
                {
                    Response.Redirect("/m/pagenotfound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }

                BindWidgets();
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "Bikewale.Mobile.Content.RoadTest.GetRoadTestList");
            }
        }

    }
}