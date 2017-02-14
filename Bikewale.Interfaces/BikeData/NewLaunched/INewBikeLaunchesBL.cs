﻿using Bikewale.Entities.BikeData.NewLaunched;
using System.Collections.Generic;
namespace Bikewale.Interfaces.BikeData.NewLaunched
{
    /// <summary>
    /// Created by  :   Sumit Kate on 10 Feb 2017
    /// Description :   NewBikeLaunches Business Layer Interface
    /// </summary>
    public interface INewBikeLaunchesBL
    {
        IEnumerable<BikesCountByMakeEntityBase> GetMakeList();
        IEnumerable<BikesCountByMakeEntityBase> GetMakeList(uint skipMakeId);
        IEnumerable<BikesCountByYearEntityBase> YearList();
        NewLaunchedBikeResult GetBikes(InputFilter filters);
    }
}
