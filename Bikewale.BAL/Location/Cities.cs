using Bikewale.Cache.Core;
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
        public IEnumerable<CityEntityBase> GetAllCities(EnumBikeType requestType) { throw new NotImplementedException(); }
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

        /// <summary>
        /// Created By Subodh Jain 6 oct 2016
        /// Desc Get used bike count in cities
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Entities.Used.UsedBikeCities> GetUsedBikeByCityWithCount()
        {
            return objCities.GetUsedBikeByCityWithCount();
        }
        /// <summary>
        /// Created by : Subodh Jain 29 Dec 2016
        /// Summary: Get Used bikes by make in cities
        /// </summary>
        public IEnumerable<Entities.Used.UsedBikeCities> GetUsedBikeByMakeCityWithCount(uint makeid)
        {
            return objCities.GetUsedBikeByMakeCityWithCount(makeid);
        }

		/// <summary>
		/// Created By  : Pratibha Verma on 17 May 2018
		/// Description : returns all cities where model price is available 
		/// </summary>
		/// <param name="modelId"></param>
		/// <returns></returns>
		public IEnumerable<CityEntityBase> GetModelPriceCities(uint modelId, uint popularCityCount)
		{
			return objCities.GetModelPriceCities(modelId, popularCityCount);
		}
	}
}
