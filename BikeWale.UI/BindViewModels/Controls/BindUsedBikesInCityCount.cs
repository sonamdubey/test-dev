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
        public IEnumerable<UsedBikesCountInCity> bikesCountCityList { get; set; }
        private IUsedBikeDetailsCacheRepository objCache = null;

        public BindUsedBikesInCityCount()
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IUsedBikeDetailsCacheRepository, UsedBikeDetailsCache>()
                        .RegisterType<IUsedBikeDetails, UsedBikeDetailsRepository>()
                        .RegisterType<ICacheManager, Bikewale.Cache.Core.MemcacheManager>();
                    objCache = container.Resolve<IUsedBikeDetailsCacheRepository>();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BindUsedBikesInCityCount.BindUsedBikesInCityCount");
            }
        }

        /// <summary>
        /// Created by Sajal Gupta on 03-01-2017
        /// Desc : function to bind used bikes in city count list by make
        /// </summary>
        public void BindUsedBikesInCityCountByMake(uint makeId)
        {
            try
            {
                if (makeId > 0)
                {
                    bikesCountCityList = objCache.GetUsedBikeInCityCountByMake(makeId);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("BindUsedBikesInCityCount.BindUsedBikesInCityCountByMake {0}", makeId));
            }
        }

        /// <summary>
        /// Created by Sajal Gupta on 03-01-2017
        /// Desc : function to bind used bikes in city count list by model
        /// </summary>
        public void BindUsedBikesInCityCountByModel(uint modelId)
        {
            try
            {
                if (modelId > 0)
                {
                    bikesCountCityList = objCache.GetUsedBikeInCityCountByModel(modelId);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("BindUsedBikesInCityCount.BindUsedBikesInCityCountByModel {0}", modelId));
            }
        }
    }
}