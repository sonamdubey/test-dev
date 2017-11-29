using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;

namespace Bikewale.Cache.BikeData
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 16 Feb 2017
    /// Summary : Function to get the data from cache and store data in cache
    /// </summary>
    public class ModelsCache : IModelsCache
    {
        private readonly ICacheManager _cache;
        private readonly IModelsRepository _modelsRepo = null;

        public ModelsCache(ICacheManager cache, IModelsRepository modelsRepo)
        {
            _cache = cache;
            _modelsRepo = modelsRepo;
        }

        public IEnumerable<UpcomingBikeEntity> GetUpcomingModels()
        {
            IEnumerable<UpcomingBikeEntity> objBikes = null;
            try
            {
                objBikes = _cache.GetFromCache<IEnumerable<UpcomingBikeEntity>>("BW_UpcomingModels", new TimeSpan(1, 0, 0), () => _modelsRepo.GetUpcomingModels());
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Cache.BikeData.GetUpcomingModels");
            }

            return objBikes;
        }
    }
}
