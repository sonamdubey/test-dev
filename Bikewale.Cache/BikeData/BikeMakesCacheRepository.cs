using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;

namespace Bikewale.Cache.BikeData
{
    /// <summary>
    /// Created by  :   Sumit Kate on 03 Mar 2016
    /// Description :   Bike Makes Repository Cache
    /// </summary>
    public class BikeMakesCacheRepository<T, U> : IBikeMakesCacheRepository<U>
    {
        private readonly ICacheManager _cache;
        private readonly IBikeMakes<T, U> _objMakes;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="objMakes"></param>
        public BikeMakesCacheRepository(ICacheManager cache, IBikeMakes<T, U> objMakes)
        {
            _cache = cache;
            _objMakes = objMakes;
        }
        /// <summary>
        /// Created by  :   Sumit Kate on 03 Mar 2016
        /// Summary     :   Gets the Makes list
        /// </summary>
        /// <param name="makeType">Type of make</param>
        /// <returns></returns>
        public IEnumerable<Entities.BikeData.BikeMakeEntityBase> GetMakesByType(Entities.BikeData.EnumBikeType makeType)
        {
            IEnumerable<Entities.BikeData.BikeMakeEntityBase> makes = null;
            string key = String.Format("BW_Makes_{0}", makeType.ToString());
            try
            {
                makes = _cache.GetFromCache<IEnumerable<Entities.BikeData.BikeMakeEntityBase>>(key, new TimeSpan(1, 0, 0), () => _objMakes.GetMakesByType(makeType));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikeMakesCacheRepository.GetMakesByType");
                objErr.SendMail();
            }
            return makes;
        }

        /// <summary>
        /// Created by  :   Sangram Nandkhile on 17 June 2016
        /// Summary     :   Gets the Discontinued Models for make
        /// </summary>
        /// <param name="makeType">Type of make</param>
        /// <returns></returns>
        public IEnumerable<BikeVersionEntity> GetDiscontinuedBikeModelsByMake(uint makeId)
        {
            IEnumerable<BikeVersionEntity> bikes = null;
            string key = String.Format("BW_DiscontinuedBikes_Make_{0}", makeId.ToString());
            try
            {
                bikes = _cache.GetFromCache<IEnumerable<BikeVersionEntity>>(key, new TimeSpan(24, 0, 0), () => _objMakes.GetDiscontinuedBikeModelsByMake(makeId));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikeMakesCacheRepository.GetDiscontinuedBikeModelsByMake");
                objErr.SendMail();
            }
            return bikes;
        }
    }
}
