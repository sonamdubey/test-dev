
using Bikewale.Entities.service;
namespace Bikewale.Interfaces.ServiceCenter
{
    public interface IServiceCenter
    {
        ServiceCenterLocatorList GetServiceCenterList(uint makeId);
    }
}
