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

namespace Bikewale.Mobile.Used.Sell
{
    public class Default : System.Web.UI.Page
    {
        protected IEnumerable<Bikewale.Entities.BikeData.BikeMakeEntityBase> objMakeList = null;
        private IBikeMakesCacheRepository<int> _makesRepository;
        protected List<CityEntityBase> objCityList = null;
        protected string userEmail = null, userName = null, userId = null;
        
        /// <summary>
        /// Created By : Sajal Gupta on 29/11/2016
        /// Description : Function to be called on page load.
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            BindUserId();
            BindMakes();
            BindCities();
        }

        /// <summary>
        /// Created By : Sajal Gupta on 20/10/2016
        /// Description : Function to bind userId.
        /// </summary>
        protected void BindUserId()
        {
            try
            {
                if (CurrentUser.Id != null)
                {
                    userId = CurrentUser.Id;
                    userEmail = CurrentUser.Email;
                    userName = CurrentUser.Name;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.used.sell.default.BindUserId()");
                objErr.SendMail();
            }
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

        /// <summary>
        /// Created By : Sajal Gupta on 20/10/2016
        /// Description : Function to bind Cities (both registered at and current city).
        /// </summary>
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