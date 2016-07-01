using Bikewale.Entities.Location;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Location;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;

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
        public IEnumerable<Entities.Location.CityEntityBase> GetPriceQuoteCities(uint modelId)
        {
            IEnumerable<Entities.Location.CityEntityBase> topBikeComapareBase = null;
            string key = string.Empty;
            try
            {
                key = String.Format("BW_PQCity_{0}", modelId);
                topBikeComapareBase = _cache.GetFromCache<IEnumerable<Entities.Location.CityEntityBase>>(key, new TimeSpan(1, 0, 0), () => _objCity.GetPriceQuoteCities(modelId));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikeCompareCacheRepository.CompareList");
                objErr.SendMail();
            }
            return topBikeComapareBase;
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
                ErrorClass objErr = new ErrorClass(ex, "BikeCompareCacheRepository.GetDealerStateCities");
                objErr.SendMail();
            }
            return objStateCities;
        }
    }
}
