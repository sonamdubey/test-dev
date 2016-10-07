using Bikewale.BindViewModels.Controls;
using Bikewale.Cache.Core;
using Bikewale.Cache.Location;
using Bikewale.Cache.UsedBikes;
using Bikewale.DAL.Location;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entities.Used;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.UsedBikes;
using Carwale.Notifications;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace Bikewale.BindViewModels.Webforms.Used
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 06 Oct 2016
    /// </summary>
    public class UsedBikeLandingPage
    {
        IUnityContainer container;
        public IEnumerable<UsedBikeMakeEntity> TopMakeList;
        public IEnumerable<UsedBikeMakeEntity> OtherMakeList;
        public IEnumerable<CityEntityBase> Cities = null;
        public IEnumerable<UsedBikeCities> objCitiesWithCount = null;

        public UsedBikeLandingPage()
        {
            try
            {
                using (container = new UnityContainer())
                {
                    ICity objCitiesCache = null;
                    IUsedBikesCache objUsedBikes = null;
                    container.RegisterType<IUsedBikes, Bikewale.BAL.UsedBikes.UsedBikes>();
                    container.RegisterType<ICacheManager, MemcacheManager>();
                    container.RegisterType<IUsedBikesCache, UsedBikesCache>();
                    container.RegisterType<ICityCacheRepository, CityCacheRepository>();
                    container.RegisterType<ICity, CityRepository>();

                    GetAllMakes(objUsedBikes);
                    GetAllCities(objCitiesCache);
                    BindCityWidgetWithCount();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + "Bikewale.BindViewModels.Webforms.Used.UsedBikeLandingPage.constructor");
                objErr.SendMail();
            }
        }
        /// <summary>
        /// Created By: Sangram Nandkhile 06 Oct, 2016
        /// Description: get top 6 makes and remaining makes
        /// </summary>
        /// <param name="objUsedBikes"></param>
        private void GetAllMakes(IUsedBikesCache objUsedBikes)
        {
            try
            {
                objUsedBikes = container.Resolve<IUsedBikesCache>();
                var totalList = objUsedBikes.GetUsedBikeMakesWithCount();
                if (totalList != null && totalList.Count() > 0)
                {
                    TopMakeList = totalList.Take(6);
                    OtherMakeList = totalList.Skip(6);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : UsedBikeLandingPage.GetAllMakes");
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Created By: Aditi Srivastava on 10 Oct, 2016
        /// Description: Get all cities for used bikes
        /// </summary>
        /// <param name="objCitiesCache"></param>
        private void GetAllCities(ICity objCitiesCache)
        {
            objCitiesCache = container.Resolve<ICity>();
            try
            {
                Cities = objCitiesCache.GetAllCities(EnumBikeType.Used);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : UsedBikeLandingPage.GetAllCities - used-Default");
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Created By: Subodh Jain on 06 Oct, 2016
        /// Description: Get top cities with used bike count
        /// </summary>
        /// <param name="objCitiesCache"></param>
        private void BindCityWidgetWithCount()
        {
            try
            {
                BindUsedBikesCityWithCount objBikeCity = new BindUsedBikesCityWithCount();
                objCitiesWithCount = objBikeCity.GetUsedBikeByCityWithCount();
                objCitiesWithCount = objCitiesWithCount.Take(6);

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Default.BindCities");
                objErr.SendMail();
            }
        }
    }
}