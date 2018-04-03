
using BikewaleOpr.Entity.BikeData;
using System.Collections.Generic;

namespace BikewaleOpr.Interface.BikeData
{
    /// <summary>
    /// Created by : Sajal Gupta on 09-03-2017
    /// Description: Added GetPendingUsedBikesWithoutModelImage method.
    /// Modified by : Ashutosh Sharma on 02 Apr 2018.
    /// Description : Added UpdateMinSpecsInESIndex to update verson min specs in elastic search index.
    /// </summary>
    public interface IBikeModels
    {
        UsedBikeImagesByMakeNotificationData GetPendingUsedBikesWithoutModelImage();
        IEnumerable<BikeModelsByMake> GetModelsWithMissingColorImage();
        void UpdateModelESIndex(string ids, string operation);
        void UpdateMinSpecsInESIndex(int versionId, IEnumerable<SpecsItem> specItemList);
    }
}
