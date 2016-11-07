
using Bikewale.Entities.service;
using Bikewale.Interfaces.ServiceCenter;
namespace Bikewale.BAL.ServiceCenter
{
    public class ServiceCenter<T, U> : IServiceCenter where T : ServiceCenterLocatorList, new()
    {
        private readonly IServiceCenterCacheRepository _objServiceCenter = null;
        public ServiceCenter(IServiceCenterCacheRepository ObjServiceCenter)
        {
            _objServiceCenter = ObjServiceCenter;


        }
        public ServiceCenterLocatorList GetServiceCenterList(uint makeid)
        {
            return _objServiceCenter.GetServiceCenterList(makeid);
        }
    }
}
