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
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using Bikewale.Notifications;

namespace Bikewale.BindViewModels.Webforms.Used
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 06 Oct 2016
    /// Summary: Summary for viewmodel used landing
    /// </summary>
    public class UsedBikeLandingPage
    {
        public IEnumerable<UsedBikeMakeEntity> TopMakeList { get; set; }
        public IEnumerable<UsedBikeMakeEntity> OtherMakeList { get; set; }
        public IEnumerable<CityEntityBase> Cities { get; set; }
        public IEnumerable<UsedBikeCities> objCitiesWithCount { get; set; }

        /// <summary>
        /// Created by: Sangram Nandkhile on 06 Oct 2016
        /// Summary: Constructor for viewmodel to initialize container and fetch values
        /// modified by:-subodh jain
        /// summary :- passed topcount as parameter count of number of icon of make 
        /// </summary>
        public UsedBikeLandingPage(int topCount)
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
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
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BindViewModels.Webforms.Used.UsedBikeLandingPage.constructor");
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
                ChangeCityLinks(totalList);
                if (totalList != null && totalList.Any())
                {
                    TopMakeList = totalList.Take(topcount);
                    OtherMakeList = totalList.Skip(topcount);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : UsedBikeLandingPage.GetAllMakes");
            }
        }
        /// <summary>
        /// Created By: Sangram Nandkhile on 23 Jan, 2017
        /// Description: Create links depending on city cookie exist or not
        /// </summary>
        private void ChangeCityLinks(IEnumerable<UsedBikeMakeEntity> totalList)
        {
            GlobalCityAreaEntity cityArea = GlobalCityArea.GetGlobalCityArea();
            if (cityArea.CityId > 0)
            {
                BindUsedBikesByMakeCity objBikeCity = new BindUsedBikesByMakeCity();
                foreach (var make in totalList)
                {
                    IEnumerable<UsedBikeCities> UsedBikeCityCountList = objBikeCity.GetUsedBikeByMakeCityWithCount((uint)make.MakeId);
                    var cityCount = UsedBikeCityCountList.FirstOrDefault(p => p.CityId == cityArea.CityId);
                    if (cityCount != null && cityCount.BikesCount > 0)
                    {
                        make.Link = string.Format("/used/{0}-bikes-in-{1}/", make.MaskingName, cityCount.CityMaskingName);
                    }
                    else
                    {
                        make.Link = string.Format("/used/browse-{0}-bikes-in-cities/", make.MaskingName);
                    }
                }
            }
            else
            {
                foreach (var make in totalList)
                {
                    make.Link = string.Format("/used/browse-{0}-bikes-in-cities/", make.MaskingName);
                }

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
                ErrorClass.LogError(ex, "Exception : UsedBikeLandingPage.GetAllCities - used-Default");
            }
        }


    }
}