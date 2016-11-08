using Bikewale.Entities.ServiceCenters;

namespace Bikewale.Interfaces.ServiceCenters
{
    /// <summary>
    /// Created By : Sajal Gupta on 07/11/2016
    /// Description: Interface for BAL layer Function for fetching service center data from cache.
    /// </summary>
    public interface IServiceCenters
    {
        ServiceCenterData GetServiceCentersByCity(uint cityId, int makeId);
    }
}
