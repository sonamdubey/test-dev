using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Mobile.Controls;
using Bikewale.Mobile.Controls;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace Bikewale.Mobile
{
    public class Default : System.Web.UI.Page
    {
        protected NewsWidget ctrlNews;
        protected ExpertReviewsWidget ctrlExpertReviews;
        protected VideosWidget ctrlVideos;
        protected CompareBikesMin ctrlCompareBikes;
        protected MOnRoadPricequote MOnRoadPricequote;
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
            ctrlNews.TotalRecords = 3;
            ctrlExpertReviews.TotalRecords = 3;
            ctrlVideos.TotalRecords = 3;
            ctrlCompareBikes.TotalRecords = 1;
            MOnRoadPricequote.PQSourceId = (int)PQSourceEnum.Mobile_HP_PQ_Widget;

            BindBrandsRepeaters();
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