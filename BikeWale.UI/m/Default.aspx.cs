using Bikewale.BindViewModels.Webforms;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.HomePage;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Mobile.Controls;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace Bikewale.Mobile
{
    /// <summary>
    /// Created By : Sushil Kumar on 28th Oct 2016
    /// Description : Added new launched,upcoming and poular bikes widget 
    /// </summary>
    public class Default : System.Web.UI.Page
    {
        protected NewNewsWidget ctrlNews;
        protected NewExpertReviewsWidget ctrlExpertReviews;
        protected NewVideosWidget ctrlVideos;
        protected CompareBikesMin ctrlCompareBikes;
        protected MOnRoadPricequote MOnRoadPricequote;
        protected MUpcomingBikes mctrlUpcomingBikes;
        protected MNewLaunchedBikes mctrlNewLaunchedBikes;
        protected MMostPopularBikes mctrlMostPopularBikes;
        protected BestBikes ctrlBestBikes;
        protected UsedBikeModel ctrlusedBikeModel;
        protected UsedBikeInCities ctrlusedBikeInCities;
        protected short reviewTabsCnt = 0;
        //Variable to Assing ACTIVE .css class
        protected bool isExpertReviewActive = false, isNewsActive = false, isVideoActive = false;
        //Varible to Hide or show controlers
        protected bool isExpertReviewZero = true, isNewsZero = true, isVideoZero = true;
        protected Repeater rptPopularBrand, rptOtherBrands;
        protected BindDefaultPage bindHomePage;
        protected HomePageBannerEntity bannerEntity;
        protected string usedBikeLink=string.Empty,usedBikeTitle=string.Empty;
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        /// <summary>
        /// MOdified By : Sushil Kumar on 27th Oct 2016
        /// Description : Fetch 4 comparisions list obj
        /// Modified by :   Sumit Kate on 29 Dec 2016
        /// Description :   Bind the Home Page banner
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            BindDefaultPage bindHomePage = new BindDefaultPage();
            bannerEntity = bindHomePage.HomePage;

            BindBikesWidgets();

            ctrlNews.TotalRecords = 3;
            ctrlNews.ShowWidgetTitle = false;
            ctrlExpertReviews.TotalRecords = 3;
            ctrlExpertReviews.ShowWidgetTitle = false;
            ctrlVideos.TotalRecords = 3;
            ctrlVideos.ShowWidgetTitle = false;
            ctrlCompareBikes.TotalRecords = 4;
            MOnRoadPricequote.PQSourceId = (int)PQSourceEnum.Mobile_HP_PQ_Widget;

            BindBrandsRepeaters();

        }

        /// <summary>
        /// Created By : Sushil Kumar on 28th Oct 2016
        /// Description : Added new launched,upcoming and poular bikes binding 
        /// Modified by:-Subodh Jain 16 March 2017
        /// Summary :- Added used model and city widget
        /// </summary>
        private void BindBikesWidgets()
        {
            try
            {
                //to get Most Popular Bikes
                mctrlMostPopularBikes.totalCount = 9;
                mctrlMostPopularBikes.PQSourceId = (int)PQSourceEnum.Mobile_HP_MostPopular;

                //To get Upcoming Bike List Details 
                mctrlNewLaunchedBikes.pageSize = 9;
                mctrlNewLaunchedBikes.curPageNo = null;
                mctrlNewLaunchedBikes.PQSourceId = (int)PQSourceEnum.Mobile_HP_NewLaunches;

                //To get Upcoming Bike List Details 
                mctrlUpcomingBikes.sortBy = (int)EnumUpcomingBikesFilter.Default;
                mctrlUpcomingBikes.pageSize = 9;
                GlobalCityAreaEntity currentCityArea = GlobalCityArea.GetGlobalCityArea();
                string _cityName = currentCityArea.City;
                CityEntityBase cityDetails = null;
                if (ctrlusedBikeModel != null)
                {
                    if (currentCityArea.CityId > 0)
                    {
                        cityDetails = new CityHelper().GetCityById(currentCityArea.CityId);
                        ctrlusedBikeModel.CityId = currentCityArea.CityId;
                    }
                    usedBikeLink = string.Format("/m/used/bikes-in-{0}/", cityDetails != null ? cityDetails.CityMaskingName : "india");
                    usedBikeTitle = string.Format("Second Hand Bikes in {0}", currentCityArea.CityId > 0 ? _cityName : "India");
                    ctrlusedBikeModel.IsLandingPage = true;
                    ctrlusedBikeModel.WidgetTitle = usedBikeTitle;
                    ctrlusedBikeModel.WidgetHref = usedBikeLink;
                    ctrlusedBikeModel.TopCount = 9;
                }
                if (ctrlusedBikeInCities != null)
                {
                    ctrlusedBikeInCities.IsLandingPage = true;
                    ctrlusedBikeInCities.WidgetHref = usedBikeLink;
                    ctrlusedBikeInCities.WidgetTitle = usedBikeTitle;
                }
                
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "BindBikesWidgets");
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Created by  : Sushil Kumar on 04 Mar 2016
        /// Bind the Brands Repeaters
        /// Modified  : Allow only six popular brands at first fold
        /// </summary>
        private void BindBrandsRepeaters()
        {
            IEnumerable<Entities.BikeData.BikeMakeEntityBase> makes = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeMakesCacheRepository<int>, BikeMakesCacheRepository<BikeMakeEntity, int>>()
                             .RegisterType<ICacheManager, MemcacheManager>()
                             .RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>()
                            ;
                    var objCache = container.Resolve<IBikeMakesCacheRepository<int>>();
                    makes = objCache.GetMakesByType(EnumBikeType.New);
                    if (makes != null && makes.Count() > 0)
                    {
                        rptPopularBrand.DataSource = makes.Take(6);
                        rptPopularBrand.DataBind();

                        rptOtherBrands.DataSource = makes.Skip(6).OrderBy(m => m.MakeName);
                        rptOtherBrands.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "BindRepeaters");
                objErr.SendMail();
            }
        }
    }
}