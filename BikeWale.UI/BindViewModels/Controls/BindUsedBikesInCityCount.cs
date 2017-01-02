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

        public BindUsedBikesInCityCount(uint makeId)
        {
            if (makeId > 0)
                BindBikeCountList(makeId);
        }

        public void BindBikeCountList(uint makeId)
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IUsedBikeDetailsCacheRepository, UsedBikeDetailsCache>()
                        .RegisterType<IUsedBikeDetails, UsedBikeDetailsRepository>()
                        .RegisterType<ICacheManager, Bikewale.Cache.Core.MemcacheManager>();
                    var objCache = container.Resolve<IUsedBikeDetailsCacheRepository>();
                    bikesCountCityList = objCache.GetUsedBikeInCityCount(makeId);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("BindUsedBikesInCityCount.BindBikeCountList {0}", makeId));
                objErr.SendMail();
            }
        }
    }
}