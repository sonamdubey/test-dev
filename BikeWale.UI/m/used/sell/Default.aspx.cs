using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;

namespace Bikewale.Mobile.Used.Sell
{
    public class Default : System.Web.UI.Page
    {
        protected IEnumerable<Bikewale.Entities.BikeData.BikeMakeEntityBase> objMakeList = null;
        private IBikeMakesCacheRepository<int> _makesRepository;

        /// <summary>
        /// Created By : Sajal Gupta on 29/11/2016
        /// Description : Function to be called on page load.
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            BindMakes();
        }

        /// <summary>
        /// Created By : Sajal Gupta on 20/10/2016
        /// Description : Function to bind makes.
        /// </summary>
        protected void BindMakes()
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeMakesCacheRepository<int>, BikeMakesCacheRepository<BikeMakeEntity, int>>()
                             .RegisterType<ICacheManager, MemcacheManager>()
                             .RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>()
                            ;
                    _makesRepository = container.Resolve<IBikeMakesCacheRepository<int>>();
                    objMakeList = _makesRepository.GetMakesByType(EnumBikeType.Used);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.m.used.sell.default.BindMakes()");
                objErr.SendMail();
            }
        }
    }
}