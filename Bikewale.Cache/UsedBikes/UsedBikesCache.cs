using Bikewale.Entities.UsedBikes;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.UsedBikes;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bikewale.Cache.UsedBikes
{
    class UsedBikesCache : IUsedBikesCache
    {
        private readonly ICacheManager _cache;
        private readonly IUsedBikes _objModels;

        /// <summary>
        /// Intitalize the references for the cache and DL
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="objModels"></param>
        public UsedBikesCache(ICacheManager cache, IUsedBikes objModels)
        {
            _cache = cache;
            _objModels = objModels;
        }

        /// <summary>
        /// Written By : Sajal Gupta on 14/09/2016
        /// Description : Get bikes based on model/ make/city .
        /// </summary>
        public IEnumerable<MostRecentBikes> GetUsedBikes(uint makeId, uint modelId, uint cityId, uint totalCount)
        {
            IEnumerable<MostRecentBikes> objUsedBikes = null;
            StringBuilder key = new StringBuilder("");
            key = (modelId == 0) ? key.AppendFormat("BW_MostRecentUsedBikes_Make_{0}", makeId) : key.AppendFormat("BW_MostRecentUsedBikes_Model_{0}", modelId);
            if (cityId != 0)
                key.AppendFormat("_CityId_{0}", cityId);

            try
            {
                objUsedBikes = _cache.GetFromCache<IEnumerable<MostRecentBikes>>(Convert.ToString(key), new TimeSpan(0, 30, 0), () => _objModels.GetPopularUsedBikes(makeId, modelId, cityId, totalCount));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Cache.UsedBikes.GetUsedBikes");
                objErr.SendMail();
            }
            return objUsedBikes;

        }


    }
}
