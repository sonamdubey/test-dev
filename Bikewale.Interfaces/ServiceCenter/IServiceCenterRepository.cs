
using Bikewale.Entities.service;
namespace Bikewale.Interfaces.ServiceCenter
{
    /// <summary>
    /// Created By:-Subodh jain 7 nov 2016
    /// Summary:- For service center locator 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public interface IServiceCenterRepository<T, U> : IRepository<T, U>
    {
        ServiceCenterLocatorList GetServiceCenterList(uint makeId);
    }
}
