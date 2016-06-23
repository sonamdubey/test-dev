using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bikewale.Controls;
using Bikewale.Controls;
using Bikewale.Common;
using Bikewale.Entities.PriceQuote;
using Microsoft.Practices.Unity;
using Bikewale.Cache.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Cache.Core;
using Bikewale.Entities.BikeData;
using Bikewale.DAL.BikeData;

namespace Bikewale
{
    public class Default : System.Web.UI.Page
    {
        protected News_new ctrlNews;
        protected ExpertReviews ctrlExpertReviews;
        protected VideosControl ctrlVideos;
        protected ComparisonMin ctrlCompareBikes;
        protected PopularUsedBikes ctrlPopularUsedBikes;
        protected OnRoadPricequote ctrlOnRoadPriceQuote;
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
            DeviceDetection dd = new DeviceDetection();
            dd.DetectDevice();

            ctrlNews.TotalRecords = 3;
            ctrlExpertReviews.TotalRecords = 3;
            ctrlVideos.TotalRecords = 3;
            ctrlCompareBikes.TotalRecords = 4;
            ctrlPopularUsedBikes.TotalRecords = 6;
            ctrlOnRoadPriceQuote.PQSourceId = (int)PQSourceEnum.Desktop_HP_PQ_Widget;

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