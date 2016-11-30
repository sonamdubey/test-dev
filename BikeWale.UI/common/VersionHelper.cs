
using Bikewale.BAL.BikeData;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;
namespace Bikewale.common
{
    /// <summary>
    /// Created By: Sangram Nandkhile on 29 Nov 2016
    /// Desc: Common Function to return Version related data
    /// This class will enable us to reduce code redundancy 
    /// </summary>
    public class VersionHelper
    {
        public BikeVersionEntity GetVersionDetailsById(uint versionId)
        {
            BikeVersionEntity bikeVersionEntity = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeVersionCacheRepository<BikeVersionEntity, uint>, BikeVersionsCacheRepository<BikeVersionEntity, uint>>()
                        .RegisterType<IBikeVersions<BikeVersionEntity, uint>, BikeVersions<BikeVersionEntity, uint>>()
                              .RegisterType<ICacheManager, MemcacheManager>();
                    var objCache = container.Resolve<IBikeVersionCacheRepository<BikeVersionEntity, uint>>();
                    bikeVersionEntity = objCache.GetById(versionId);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("VersionHelper.GetVersionDetailsById() : VersionId:-{0} ", versionId));
                objErr.SendMail();
            }
            return bikeVersionEntity;
        }
    }
}