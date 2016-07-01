﻿using Bikewale.Cache.Core;
using Bikewale.Cache.Location;
using Bikewale.DAL.Location;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Location;
using Microsoft.Practices.Unity;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Bikewale.BAL.Location
{
    public class Cities : ICity
    {
        private readonly ICityCacheRepository objCities = null;

        public Cities()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<ICityCacheRepository, CityCacheRepository>()
                    .RegisterType<ICity, CityRepository>()
                    .RegisterType<ICacheManager, MemcacheManager>();
                objCities = container.Resolve<ICityCacheRepository>();
            }
        }

        public List<CityEntityBase> GetPriceQuoteCities(uint modelId) { throw new NotImplementedException(); }
        public List<CityEntityBase> GetAllCities(EnumBikeType requestType) { throw new NotImplementedException(); }
        public List<CityEntityBase> GetCities(string stateId, EnumBikeType requestType) { throw new NotImplementedException(); }
        public Hashtable GetMaskingNames() { throw new NotImplementedException(); }
        public Hashtable GetOldMaskingNames() { throw new NotImplementedException(); }

        /// <summary>
        /// Created by : Vivek Gupta
        /// Date : 24 june 2016
        /// Desc : get dealer cities for dealer locatr
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="stateId"></param>
        /// <returns></returns>
        public DealerStateCities GetDealerStateCities(uint makeId, uint stateId)
        {
            return objCities.GetDealerStateCities(makeId, stateId);
        }
    }
}
