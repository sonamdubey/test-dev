using Bikewale.Cache.Core;
using Bikewale.Cache.Location;
using Bikewale.Common;
using Bikewale.DAL.Location;
using Bikewale.Entities.Used;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Location;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;

namespace Bikewale.BindViewModels.Controls
{
    /// <summary>
    /// Created by : Subodh Jain 6 oct 2016
    /// Summary: Bind view model for binding  used bikes in a city with count.
    /// </summary>
    public class BindUsedBikesCityWithCount
    {
        /// <summary>
        /// Created By: Subodh Jain 6 oct 2016
        /// Desc:-Bind view model for binding  used bikes in a city with count
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UsedBikeCities> GetUsedBikeByCityWithCount()
        {
            IEnumerable<UsedBikeCities> objBikeCity = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<ICityCacheRepository, CityCacheRepository>()
                    .RegisterType<ICity, CityRepository>()
                    .RegisterType<ICacheManager, MemcacheManager>();
                    var objCities = container.Resolve<ICityCacheRepository>();
                    objBikeCity = objCities.GetUsedBikeByCityWithCount();
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BindUsedBikesCityWithCount.getusedbikebycitywithcount");
                
            }
            return objBikeCity;
        }

    }
}