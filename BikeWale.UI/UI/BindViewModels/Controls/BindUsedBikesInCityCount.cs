using Bikewale.Cache.Used;
using Bikewale.Common;
using Bikewale.DAL.Used;
using Bikewale.Entities.Used;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Used;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;

namespace Bikewale.BindViewModels.Controls
{
    /// <summary>
    /// Created by Sajal Gupta on 30-12-2016
    /// Desc : View model to bind used bikes in city count list
    /// </summary>
    public class BindUsedBikesInCityCount
    {
        public IEnumerable<UsedBikesCountInCity> BikesCountCityList { get; set; }
        private readonly IUsedBikeDetailsCacheRepository _objCache = null;

        public BindUsedBikesInCityCount()
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IUsedBikeDetailsCacheRepository, UsedBikeDetailsCache>()
                        .RegisterType<IUsedBikeDetails, UsedBikeDetailsRepository>()
                        .RegisterType<ICacheManager, Bikewale.Cache.Core.MemcacheManager>();
                    _objCache = container.Resolve<IUsedBikeDetailsCacheRepository>();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BindUsedBikesInCityCount.BindUsedBikesInCityCount");
            }
        }

        /// <summary>
        /// Created by Sajal Gupta on 03-01-2017
        /// Desc : function to bind used bikes in city count list by make
        /// </summary>
        public void BindUsedBikesInCityCountByMake(uint makeId, ushort topCount)
        {
            try
            {
                if (makeId > 0)
                {
                    BikesCountCityList = _objCache.GetUsedBikeInCityCountByMake(makeId, topCount);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BindUsedBikesInCityCount.BindUsedBikesInCityCountByMake {0} {1}", makeId, topCount));
            }
        }

        /// <summary>
        /// Created by Sajal Gupta on 03-01-2017
        /// Desc : function to bind used bikes in city count list by model
        /// </summary>
        public void BindUsedBikesInCityCountByModel(uint modelId, ushort topCount)
        {
            try
            {
                if (modelId > 0)
                {
                    BikesCountCityList = _objCache.GetUsedBikeInCityCountByModel(modelId, topCount);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BindUsedBikesInCityCount.BindUsedBikesInCityCountByModel {0} {1}", modelId, topCount));
            }
        }
    }
}