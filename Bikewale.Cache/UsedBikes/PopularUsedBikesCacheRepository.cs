﻿using Bikewale.Entities.UsedBikes;
using Bikewale.Interfaces.UsedBikes;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Bikewale.Interfaces.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Cache.Core;
using Bikewale.DAL.BikeData;

namespace Bikewale.Cache.UsedBikes
{
    public class PopularUsedBikesCacheRepository : IPopularUsedBikesCacheRepository
    {
        private readonly ICacheManager _cache;
        private readonly IUsedBikes _objModels;

        /// <summary>
        /// Intitalize the references for the cache and BL
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="objModels"></param>
        public PopularUsedBikesCacheRepository(ICacheManager cache, IUsedBikes objModels)
        {
            _cache = cache;
            _objModels = objModels;
        }
        /// <summary>
        /// Written By : Sangram Nandkhile on 06 Nov 2015
        /// </summary>
        /// <param name="topCount"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public IEnumerable<PopularUsedBikesEntity> GetPopularUsedBikes(uint topCount, int? cityId)
        {
            IEnumerable<PopularUsedBikesEntity> objModelPage = null;
            string key = cityId == null? "BW_PopularUsedBikes" : "BW_PopularUsedBikes_" + cityId;
            try
            {
                objModelPage = _cache.GetFromCache<IEnumerable<PopularUsedBikesEntity>>(key, new TimeSpan(0,30, 0), () => _objModels.GetPopularUsedBikes(topCount, cityId));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, MethodBase.GetCurrentMethod().DeclaringType.Name+ " -  " + System.Reflection.MethodInfo.GetCurrentMethod().Name);
                objErr.SendMail();
            }
            return objModelPage;
        }
    }
}
