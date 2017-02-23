
using Bikewale.Entities.BikeData;
using System.Collections.Generic;
namespace Bikewale.Interfaces.BikeData.UpComing
{
    /// <summary>
    /// Created By :- Subodh Jain 16 Feb 2017
    /// Summary :-  interface have methods to get data for upcoming bikes
    /// </summary>
    public interface IModelsCache
    {
        IEnumerable<UpcomingBikeEntity> GetUpcomingModels();
    }
}
