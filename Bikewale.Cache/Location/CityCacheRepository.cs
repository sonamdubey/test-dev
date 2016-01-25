using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Location;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                key = String.Format("BW_PQCity_{0}",modelId);
                topBikeComapareBase = _cache.GetFromCache<IEnumerable<Entities.Location.CityEntityBase>>(key, new TimeSpan(1, 0, 0), () => _objCity.GetPriceQuoteCities(modelId));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikeCompareCacheRepository.CompareList");
                objErr.SendMail();
            }
            return topBikeComapareBase;
        }
    }
}
