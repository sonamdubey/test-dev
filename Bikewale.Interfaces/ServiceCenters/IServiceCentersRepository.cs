using Bikewale.Entities.ServiceCenters;

namespace Bikewale.Interfaces.ServiceCenters
{
    /// <summary>
    /// Created By : Sajal Gupta on 07/11/2016
    /// Description: Interface for Function for fetching service center data.
    /// </summary>
    public interface IServiceCentersRepository
    {
        ServiceCenterData GetServiceCentersByCity(uint cityId, int makeId);
    }
}
