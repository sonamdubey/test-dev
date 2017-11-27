using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Bikewale.Mobile.New
{
	public class Search : System.Web.UI.Page
	{
        protected IEnumerable<BikeMakeEntityBase> rptPopularBrand, rptOtherBrands;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

		protected void Page_Load(object sender, EventArgs e)
		{
            BindRepeaters();
		}

        /// <summary>
        /// Created by  : Sushil Kumar on 04 Mar 2016
        /// Bind the Brands Repeaters
        /// </summary>
        private void BindRepeaters()
        {
            IEnumerable<Entities.BikeData.BikeMakeEntityBase> makes = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeMakesCacheRepository, BikeMakesCacheRepository>()
                             .RegisterType<ICacheManager, MemcacheManager>()
                             .RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>()
                            ;
                    var objCache = container.Resolve<IBikeMakesCacheRepository>();
                    makes = objCache.GetMakesByType(EnumBikeType.New);

                    if (makes != null && makes.Any())
                    {
                        rptPopularBrand = makes.Where(m => m.PopularityIndex > 0);

                        rptOtherBrands = makes.Where(m => m.PopularityIndex == 0);
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
                Bikewale.Notifications.ErrorClass.LogError(ex, "BindRepeaters");
                
            }
        }
	}
}