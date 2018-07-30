using Bikewale.Entities.BikeData;
using Bikewale.Entities.BikeData.NewLaunched;
using System.Collections.Generic;
namespace Bikewale.Interfaces.BikeData.NewLaunched
{
    /// <summary>
    /// Created by  :   Sumit Kate on 10 Feb 2017
    /// Description :   NewBikeLaunches Business Layer Interface
    /// Modified by :   Sanskar Gupta on 07 Feb 2018
    /// Description :   Added new function GetNewLaunchedBikesListByMakeAndDays
    /// Modified by :   Rajan Chauhan on 3 Apr 2018
    /// Description :   Added new function GetNewLauchedBikesList
    /// </summary>
    public interface INewBikeLaunchesBL
    {
        IEnumerable<BikesCountByMakeEntityBase> GetMakeList();
        IEnumerable<BikesCountByMakeEntityBase> GetMakeList(uint skipMakeId);
        IEnumerable<BikesCountByYearEntityBase> YearList();
        NewLaunchedBikeResult GetBikes(InputFilter filters);
        NewLaunchedBikesBase GetNewLaunchedBikesList(int startIndex, int endIndex, int? makeId = null);
        IEnumerable<NewLaunchedBikeEntityBase> GetNewLaunchedBikesListByMakeAndDays(InputFilter filters);
    }
}
