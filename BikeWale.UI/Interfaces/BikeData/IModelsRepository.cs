
using Bikewale.Entities.BikeData;
using System.Collections.Generic;
namespace Bikewale.Interfaces.BikeData.UpComing
{
    /// <summary>
    /// Created By :- Subodh Jain 16 Feb 2017
    /// Summary :-  interface have methods to get data for upcoming bikes
    /// </summary>
    public interface IModelsRepository
    {
        /// <summary>
        /// Function to get the all upcoming models
        /// </summary>
        /// <returns></returns>
        IEnumerable<UpcomingBikeEntity> GetUpcomingModels();
    }
}
