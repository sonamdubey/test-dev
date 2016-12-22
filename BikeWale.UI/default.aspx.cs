using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.Controls;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace Bikewale
{
    /// <summary>
    /// Created By : Sushil Kumar on 28th Oct 2016
    /// Description : Added new launched,upcoming and poular bikes widget 
    /// </summary>
    public class Default : System.Web.UI.Page
    {
        protected News_Widget ctrlNews;
        protected NewExpertReviews ctrlExpertReviews;
        protected NewVideosControl ctrlVideos;
        protected ComparisonMin ctrlCompareBikes;
        protected PopularUsedBikes ctrlPopularUsedBikes;
        protected OnRoadPricequote ctrlOnRoadPriceQuote;

        protected UpcomingBikes_new ctrlUpcomingBikes;
        protected NewLaunchedBikes_new ctrlNewLaunchedBikes;
        protected MostPopularBikes_new ctrlMostPopularBikes;

        protected BestBikes ctrlBestBikes;

        protected short reviewTabsCnt = 0;
        //Variable to Assing ACTIVE .css class
        protected bool isExpertReviewActive = false, isNewsActive = false, isVideoActive = false;
        //Varible to Hide or show controlers
        protected bool isExpertReviewZero = true, isNewsZero = true, isVideoZero = true;
        protected Repeater rptPopularBrand, rptOtherBrands;
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //device detection

            DeviceDetection dd = new DeviceDetection("/");
            dd.DetectDevice();

            BindBikesWidgets();

            ctrlNews.TotalRecords = 3;
            ctrlNews.ShowWidgetTitle = false;

            ctrlExpertReviews.TotalRecords = 3;
            ctrlExpertReviews.ShowWidgetTitle = false;

            ctrlVideos.TotalRecords = 3;
            ctrlVideos.ShowWidgetTitle = false;
            ctrlCompareBikes.TotalRecords = 4;
            GlobalCityAreaEntity currentCityArea = GlobalCityArea.GetGlobalCityArea();
            ctrlPopularUsedBikes.header = String.Format("Popular used bikes in {0}", !String.IsNullOrEmpty(currentCityArea.City) ? currentCityArea.City : "India");
            ctrlPopularUsedBikes.TotalRecords = 6;
            ctrlOnRoadPriceQuote.PQSourceId = (int)PQSourceEnum.Desktop_HP_PQ_Widget;

            BindRepeaters();
        }

        /// <summary>
        /// Created By : Sushil Kumar on 28th Oct 2016
        /// Description : Added new launched,upcoming and poular bikes binding 
        /// </summary>
        private void BindBikesWidgets()
        {
            try
            {
                //to get Most Popular Bikes
                ctrlMostPopularBikes.totalCount = 9;
                ctrlMostPopularBikes.PQSourceId = (int)PQSourceEnum.Desktop_HP_MostPopular;

                //To get Upcoming Bike List Details 
                ctrlNewLaunchedBikes.pageSize = 9;
                ctrlNewLaunchedBikes.PQSourceId = (int)PQSourceEnum.Desktop_HP_NewLaunches;

                //To get Upcoming Bike List Details 
                ctrlUpcomingBikes.sortBy = (int)EnumUpcomingBikesFilter.Default;
                ctrlUpcomingBikes.pageSize = 9;
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "BindBikesWidgets");
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 04 Mar 2016
        /// Bind the Repeaters
        /// </summary>
        private void BindRepeaters()
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
                        rptPopularBrand.DataSource = makes.Where(m => m.PopularityIndex > 0);
                        rptPopularBrand.DataBind();

                        rptOtherBrands.DataSource = makes.Where(m => m.PopularityIndex == 0);
                        rptOtherBrands.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "BindRepeaters");
                objErr.SendMail();
            }
        }
    }
}