
using Bikewale.Entities.service;
namespace Bikewale.Interfaces.ServiceCenter
{
    public interface IServiceCenterRepository<T, U> : IRepository<T, U>
    {
        ServiceCenterLocatorList GetServiceCenterList(uint makeId);
    }
}
