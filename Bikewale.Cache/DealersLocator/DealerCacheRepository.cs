using Bikewale.Entities.DealerLocator;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Dealer;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Cache.DealersLocator
{
    /// <summary>
    /// Created By : Lucky Rathore
    /// Created On : 21 March 2016
    /// </summary>
    public class DealerCacheRepository : IDealerCacheRepository
    {

        private readonly ICacheManager _cache;
        private readonly IDealer _objDealers;

        public DealerCacheRepository(ICacheManager cache, IDealer objDealers)
        {
            _cache = cache;
            _objDealers = objDealers;
        }

        /// <summary>
        /// Created By : Lucky Rathore on 21 March 2016
        /// Description : Cahing of Dealer detail By Make and City
        /// </summary>
        /// <param name="cityId">e.g. 1</param>
        /// <param name="makeId">e.g. 9</param>
        /// <returns></returns>
        public Dealers GetDealerByMakeCity(uint cityId, uint makeId)
        {
            //IEnumerable<Entities.BikeData.BikeMakeEntityBase> makes = null;
            Entities.DealerLocator.Dealers dealers = null;
            string key = String.Format("BW_DealerList_Make_{0}_City_{1}", makeId, cityId);
            try
            {
                dealers = _cache.GetFromCache<Entities.DealerLocator.Dealers>(key, new TimeSpan(1, 0, 0), () => _objDealers.GetDealerByMakeCity(cityId, makeId));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikeMakesCacheRepository.GetMakesByType");
                objErr.SendMail();
            }
            return dealers;
        }
    }
}
