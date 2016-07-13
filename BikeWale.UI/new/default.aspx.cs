using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.Controls;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace Bikewale.New
{
    public class Default : System.Web.UI.Page
    {
        protected News_new ctrlNews;
        protected UpcomingBikes_new ctrlUpcomingBikes;
        protected NewLaunchedBikes_new ctrlNewLaunchedBikes;
        protected MostPopularBikes_new ctrlMostPopularBikes;
        protected ExpertReviews ctrlExpertReviews;
        protected VideosControl ctrlVideos;
        protected ComparisonMin ctrlCompareBikes;
        protected NewBikesOnRoadPrice NBOnRoadPrice;
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
            // Modified By :Lucky Rathore on 12 July 2016.
            Form.Action = Request.RawUrl;
            //device detection
            // Modified By :Ashish Kamble on 5 Feb 2016
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];

            DeviceDetection dd = new DeviceDetection(originalUrl);
            dd.DetectDevice();

            //to get Most Popular Bikes
            ctrlMostPopularBikes.totalCount = 6;
            ctrlMostPopularBikes.PQSourceId = (int)PQSourceEnum.Desktop_New_MostPopular;

            //To get Upcoming Bike List Details 
            ctrlNewLaunchedBikes.pageSize = 6;
            ctrlNewLaunchedBikes.PQSourceId = (int)PQSourceEnum.Desktop_New_NewLaunches;

            //To get Upcoming Bike List Details 
            ctrlUpcomingBikes.sortBy = (int)EnumUpcomingBikesFilter.Default;
            ctrlUpcomingBikes.pageSize = 6;

            ctrlNews.TotalRecords = 3;
            ctrlExpertReviews.TotalRecords = 3;
            ctrlVideos.TotalRecords = 3;
            ctrlCompareBikes.TotalRecords = 4;

            NBOnRoadPrice.PQSourceId = (int)PQSourceEnum.Desktop_New_PQ_Widget;

            BindRepeaters();
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
                Trace.Warn(ex.Message);
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "BindRepeaters");
                objErr.SendMail();
            }
        }
    }
}