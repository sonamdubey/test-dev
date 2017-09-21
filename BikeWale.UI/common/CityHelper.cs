﻿
using Bikewale.Cache.Core;
using Bikewale.Cache.Location;
using Bikewale.DAL.Location;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Location;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Bikewale.Common
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 03 Feb 2017
    /// Summary: Common helper to fetch city details by CityId
    /// </summary>
    public class CityHelper
    {
        private readonly ICityCacheRepository cityCacheRepository = null;
        public CityHelper()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<ICity, CityRepository>();
                container.RegisterType<ICacheManager, MemcacheManager>();
                container.RegisterType<ICityCacheRepository, CityCacheRepository>();
                cityCacheRepository = container.Resolve<ICityCacheRepository>();
            }
        }
        /// <summary>
        /// Created by: Sangram Nandkhile on 03 Feb 2017
        /// Summary: Common helper to fetch city details by Id
        /// </summary>
        /// <param name="cityId">CityId</param>
        /// <returns></returns>
        public CityEntityBase GetCityById(uint cityId)
        {
            IEnumerable<CityEntityBase> objCityList = null;
            CityEntityBase selectedCity = null;
            if (cityId > 0)
            {
                try
                {
                    objCityList = cityCacheRepository.GetAllCities(EnumBikeType.All);
                    selectedCity = objCityList.FirstOrDefault(c => c.CityId == cityId);
                }
                catch (Exception ex)
                {
                    ErrorClass objErr = new ErrorClass(ex, String.Format("CityHelper.GetCityById(): cityId: {0}", cityId));
                }
            }
            return selectedCity;
        }
    }
}