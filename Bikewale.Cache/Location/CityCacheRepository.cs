using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entities.Used;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Location;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.Cache.Location
{
    /// <summary>
    /// City Cache Repository
    /// Created by  :   Sumit Kate on 25 Jan 2016    
    /// </summary>
    public class CityCacheRepository : ICityCacheRepository
    {
        private readonly ICity _objCity = null;
        private readonly ICacheManager _cache = null;

        /// <summary>
        /// Constructor to initialize the member variables
        /// </summary>
        /// <param name="objCity"></param>
        /// <param name="cache"></param>
        public CityCacheRepository(ICity objCity, ICacheManager cache)
        {
            _objCity = objCity;
            _cache = cache;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public IEnumerable<CityEntityBase> GetPriceQuoteCities(uint modelId)
        {
            IEnumerable<CityEntityBase> topBikeComapareBase = null;
            string key = string.Empty;
            try
            {
                key = String.Format("BW_PQCity_{0}", modelId);
                topBikeComapareBase = _cache.GetFromCache<IEnumerable<CityEntityBase>>(key, new TimeSpan(1, 0, 0), () => _objCity.GetPriceQuoteCities(modelId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "CityCacheRepository.GetPriceQuoteCities");
                
            }
            return topBikeComapareBase;
        }

        /// <summary>
        /// Created by : Sangram Nandkhile on 13 Sep 2016
        /// Summary: Cache layer for caching cities
        /// </summary>
        /// <param name="requestType"></param>
        /// <returns></returns>
        public IEnumerable<CityEntityBase> GetAllCities(EnumBikeType requestType)
        {
            IEnumerable<CityEntityBase> topBikeComapareBase = null;
            string key = string.Empty;
            try
            {
                key = string.Format("BW_AllCities_{0}", requestType);
                topBikeComapareBase = _cache.GetFromCache<IEnumerable<Entities.Location.CityEntityBase>>(key, new TimeSpan(12, 0, 0), () => _objCity.GetAllCities(requestType));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "CityCacheRepository.GetAllCities");
                
            }
            return topBikeComapareBase;
        }

        /// <summary>
        /// Created by : Sangram Nandkhile on 13 Sep 2016
        /// Summary: Fetch city details from city masking name
        /// </summary>
        /// <param name="cityMasking"></param>
        /// <returns></returns>
        public CityEntityBase GetCityDetails(string cityMasking)
        {
            CityEntityBase city = null;
            string key = string.Empty;
            try
            {
                IEnumerable<CityEntityBase> cityList = GetAllCities(EnumBikeType.All);
                city = (from c in cityList
                        where c.CityMaskingName == cityMasking
                        select c).FirstOrDefault();
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "CityCacheRepository.GetCityDetails");
                
            }
            return city;
        }

        /// <summary>
        /// Created By : Vivek Gupta
        /// Date : 24 june 2016
        /// Desc : get dealer cities for dealer locator
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="stateId"></param>
        /// <returns></returns>
        public DealerStateCities GetDealerStateCities(uint makeId, uint stateId)
        {
            DealerStateCities objStateCities = null;
            string key = string.Empty;
            try
            {
                key = String.Format("BW_CitywiseDealersCnt_Make_{0}_State_{1}", makeId, stateId);
                objStateCities = _cache.GetFromCache<DealerStateCities>(key, new TimeSpan(1, 0, 0), () => _objCity.GetDealerStateCities(makeId, stateId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "CityCacheRepository.GetDealerStateCities");
                
            }
            return objStateCities;
        }
        /// <summary>
        /// Created by Subodh jain 60 oct 2016
        /// Describtion To get Top 6 cities order by poplarity and remaining by alphabetic order
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UsedBikeCities> GetUsedBikeByCityWithCount()
        {
            IEnumerable<UsedBikeCities> objUsedBikesCity = null;
            string key = "BW_UsedBikeInCityWithCount";
            try
            {
                objUsedBikesCity = _cache.GetFromCache<IEnumerable<UsedBikeCities>>(key, new TimeSpan(1, 0, 0), () => _objCity.GetUsedBikeByCityWithCount());
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Cache.CityCacheRepository.GetUsedBikeByCityWithCount");
                
            }
            return objUsedBikesCity;
        }
        /// <summary>
        /// Created by : Subodh Jain 29 Dec 2016
        /// Summary: Get Used bikes by make in cities
        /// </summary>
        public IEnumerable<UsedBikeCities> GetUsedBikeByMakeCityWithCount(uint makeid)
        {
            IEnumerable<UsedBikeCities> objUsedBikesCity = null;
            string key = string.Format("BW_UsedBikeInMakeCityWithCount_{0}", makeid);
            try
            {
                objUsedBikesCity = _cache.GetFromCache<IEnumerable<UsedBikeCities>>(key, new TimeSpan(1, 0, 0), () => _objCity.GetUsedBikeByMakeCityWithCount(makeid));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("CityCacheRepository.GetUsedBikeByMakeCityWithCount_{0}", makeid));
                
            }
            return objUsedBikesCity;
        }

		/// <summary>
		/// Created by  : Pratibha Verma on 17 May 2018
		/// Description : returns all cities where model price is available
		/// </summary>
		/// <param name="modelId"></param>
		/// <returns></returns>
		public IEnumerable<CityEntityBase> GetModelPriceCities(uint modelId, uint popularCityCount)
		{
			IEnumerable<CityEntityBase> objCityList = null;
			try
			{
				string key = string.Format("BW_AllCities_{0}", modelId);
				objCityList = _cache.GetFromCache<IEnumerable<CityEntityBase>>(key, new TimeSpan(12, 0, 0), () => _objCity.GetModelPriceCities(modelId, popularCityCount));
			}
			catch (Exception ex)
			{
				ErrorClass.LogError(ex, "Bikewale.Cache.Location.CityCacheRepository.GetModelPriceCities");
			}
			return objCityList;
		}

	}
}
