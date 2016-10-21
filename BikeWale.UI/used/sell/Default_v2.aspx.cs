using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.DAL.BikeData;
using Bikewale.DAL.Location;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Location;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;

namespace Bikewale.Used.Sell
{
    public class Default_v2 : System.Web.UI.Page
    {
        protected IEnumerable<Bikewale.Entities.BikeData.BikeMakeEntityBase> objMakeList = null;
        private IBikeMakesCacheRepository<int> _makesRepository;
        protected List<CityEntityBase> objCityList = null;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindMakes();
            BindCities();
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
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.used.sell.default.BindMakes()");
                objErr.SendMail();
            }
        }

        protected void BindCities()
        {
            ICity _city = new CityRepository();
            try
            {
                objCityList = _city.GetAllCities(EnumBikeType.All);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.used.sell.default.BindCities()");
                objErr.SendMail();
            }
        }
    }
}