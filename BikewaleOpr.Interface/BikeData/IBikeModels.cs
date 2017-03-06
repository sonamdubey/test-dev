
using BikewaleOpr.Entities.BikeData;
using BikewaleOpr.Entity.BikeData;
using System;
using System.Collections.Generic;
namespace BikewaleOpr.Interface.BikeData
{
    /// <summary>
    /// Created By : Sushil Kumar on 26th Oct 2016 
    /// Description : Interface for bike models 
    /// Modified by : Sajal Gupta on 22-12-2016
    /// Description : Added SaveModelUnitSold, GetLastSoldUnitData
    /// </summary>
    public interface IBikeModels
    {
        IEnumerable<BikeModelEntityBase> GetModels(uint makeId, string requestType);
        void SaveModelUnitSold(string list, DateTime date);
        SoldUnitData GetLastSoldUnitData();
        List<string> GetPendingUsedBikesWithoutModelImage();
    }
}
