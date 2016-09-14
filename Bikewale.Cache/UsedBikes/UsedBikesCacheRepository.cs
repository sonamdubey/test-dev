﻿using Bikewale.Entities.UsedBikes;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.UsedBikes;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;

namespace Bikewale.Cache.UsedBikes
{
    public class UsedBikesCacheRepository : IUsedBikesCacheRepository
    {
        private readonly ICacheManager _cache;
        private readonly IUsedBikesRepository _objModels;

        /// <summary>
        /// Intitalize the references for the cache and DL
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="objModels"></param>
        public UsedBikesCacheRepository(ICacheManager cache, IUsedBikesRepository objModels)
        {
            _cache = cache;
            _objModels = objModels;
        }

        /// <summary>
        /// Written By : Vivek Gupta on 21st june 2016
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="topCount"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public IEnumerable<MostRecentBikes> GetMostRecentUsedBikes(uint makeId, uint topCount, int? cityId)
        {
            IEnumerable<MostRecentBikes> objUsedBikes = null;
            string key = cityId == null ? String.Format("BW_MostRecentUsedBikes_Make_{0}", makeId) : String.Format("BW_MostRecentUsedBikes_Make_{0}_City_{1}", makeId, cityId);
            try
            {
                objUsedBikes = _cache.GetFromCache<IEnumerable<MostRecentBikes>>(key, new TimeSpan(0, 30, 0), () => _objModels.GetMostRecentUsedBikes(makeId, topCount, cityId));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Cache.UsedBikes.GetMostRecentUsedBikes");
                objErr.SendMail();
            }
            return objUsedBikes;
        }

    }
}
