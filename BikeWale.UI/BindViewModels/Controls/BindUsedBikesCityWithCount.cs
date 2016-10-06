using Bikewale.Cache.Core;
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
    /// Created by : Subodh Jain 6 oct 2016
    /// Summary: Bind view model for binding  used bikes in a city with count.
    /// </summary>
    public class BindUsedBikesCityWithCount
    {
        /// <summary>
        /// Created By: Subodh Jain 6 oct 2016
        /// Desc:-Bind view model for binding  used bikes in a city with count
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UsedBikeCities> GetUsedBikeByCityWithCount()
        {
            IEnumerable<UsedBikeCities> objBikeCity = null;
            try
            {



                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IUsedBikeDetailsCacheRepository, UsedBikeDetailsCache>()
                        .RegisterType<IUsedBikeDetails, UsedBikeDetailsRepository>()
                        .RegisterType<ICacheManager, MemcacheManager>();
                    var objCache = container.Resolve<IUsedBikeDetailsCacheRepository>();
                    objBikeCity = objCache.GetUsedBikeByCityWithCount();


                }

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BindUsedBikesCityWithCount.getusedbikebycitywithcount");
                objErr.SendMail();
            }
            return objBikeCity;
        }

    }
}