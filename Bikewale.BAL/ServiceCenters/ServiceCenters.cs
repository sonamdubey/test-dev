using Bikewale.Cache.Core;
using Bikewale.Cache.ServiceCenters;
using Bikewale.DAL.ServiceCenters;
using Bikewale.Entities.ServiceCenters;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.ServiceCenters;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;

namespace Bikewale.BAL.ServiceCenters
{
    /// <summary>
    /// Created By : Sajal Gupta on 07/11/2016
    /// Description: BAL layer for fetching service center data.
    /// </summary>
    public class ServiceCenters : IServiceCenters
    {
        private readonly IServiceCentersCacheRepository objSCCR = null;

        public ServiceCenters()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<ICacheManager, MemcacheManager>();
                container.RegisterType<IServiceCentersRepository, ServiceCentersRepository>();
                container.RegisterType<IServiceCentersCacheRepository, ServiceCentersCacheRepository>();

                objSCCR = container.Resolve<IServiceCentersCacheRepository>();
            }
        }

        /// <summary>
        /// Created By : Sajal Gupta on 07/11/2016
        /// Description: BAL layer Function for fetching service center data from cache.
        /// </summary>
        public ServiceCenterData GetServiceCentersByCity(uint cityId, int makeId)
        {
            ServiceCenterData objServiceCenterData = null;
            try
            {
                if (objSCCR != null)
                {
                    objServiceCenterData = objSCCR.GetServiceCentersByCity(cityId, makeId);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ServiceCenters.GetServiceCentersByCity");
                objErr.SendMail();
            }
            return objServiceCenterData;
        }
    }
}
