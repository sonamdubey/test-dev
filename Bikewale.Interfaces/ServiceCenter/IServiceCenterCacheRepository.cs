
using Bikewale.Entities.service;
namespace Bikewale.Interfaces.ServiceCenter
{
    public interface IServiceCenterCacheRepository
    {
        ServiceCenterLocatorList GetServiceCenterList(uint makeid);
    }
}
