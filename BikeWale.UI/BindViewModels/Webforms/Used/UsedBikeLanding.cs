using Bikewale.BindViewModels.Controls;
using Bikewale.Cache.Core;
using Bikewale.Cache.Location;
using Bikewale.Cache.Used;
using Bikewale.Cache.UsedBikes;
using Bikewale.DAL.Location;
using Bikewale.DAL.Used;
using Bikewale.DAL.UsedBikes;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entities.Used;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.Used;
using Bikewale.Interfaces.UsedBikes;
using Carwale.Notifications;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.BindViewModels.Webforms.Used
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 06 Oct 2016
    /// Summary: Summary for viewmodel used landing
    /// </summary>
    public class UsedBikeLandingPage
    {
        private IUnityContainer container;
        public IEnumerable<UsedBikeMakeEntity> TopMakeList;
        public IEnumerable<UsedBikeMakeEntity> OtherMakeList;
        public IEnumerable<CityEntityBase> Cities = null;
        public IEnumerable<UsedBikeCities> objCitiesWithCount = null;

        /// <summary>
        /// Created by: Sangram Nandkhile on 06 Oct 2016
        /// Summary: Constructor for viewmodel to initialize container and fetch values
        /// </summary>
        public UsedBikeLandingPage(int topCount)
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
                    container.RegisterType<IUsedBikeDetails, UsedBikeDetailsRepository>();
                    container.RegisterType<IUsedBikesRepository, UsedBikesRepository>();
                    container.RegisterType<IUsedBikeDetailsCacheRepository, UsedBikeDetailsCache>();
                    objCitiesCache = container.Resolve<ICity>();
                    objUsedBikes = container.Resolve<IUsedBikesCache>();

                    GetAllMakes(objUsedBikes, topCount);
                    GetAllCities(objCitiesCache);
                    BindCityWidgetWithCount();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.BindViewModels.Webforms.Used.UsedBikeLandingPage.constructor");
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Created By: Sangram Nandkhile 06 Oct, 2016
        /// Description: get top 6 makes and remaining makes
        /// </summary>
        /// <param name="objUsedBikes"></param>
        private void GetAllMakes(IUsedBikesCache objUsedBikes, int topcount)
        {
            try
            {
                var totalList = objUsedBikes.GetUsedBikeMakesWithCount();
                if (totalList != null && totalList.Count() > 0)
                {
                    TopMakeList = totalList.Take(topcount);
                    OtherMakeList = totalList.Skip(topcount);
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
                objCitiesWithCount = objCitiesWithCount.Where(x => x.priority > 0);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Default.BindCityWidgetWithCount");
                objErr.SendMail();
            }
        }
    }
}