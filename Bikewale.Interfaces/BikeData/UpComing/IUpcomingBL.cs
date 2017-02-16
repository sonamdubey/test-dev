
using Bikewale.Entities.BikeData;
using System.Collections.Generic;
namespace Bikewale.Interfaces.BikeData.UpComing
{
    /// <summary>
    /// Created By :- Subodh Jain 16 Feb 2017
    /// Summary :-  upcoming bikes interface
    /// </summary>
    public interface IUpcomingBL
    {
        IEnumerable<UpcomingBikeEntity> GetUpComingBike(int? makeId);
    }
}
