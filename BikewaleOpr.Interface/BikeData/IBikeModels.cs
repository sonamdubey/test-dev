
using BikewaleOpr.Entity.BikeData;
using System.Collections.Generic;

namespace BikewaleOpr.Interface.BikeData
{
    /// <summary>
    /// Created by : Sajal Gupta on 09-03-2017
    /// Description: Added GetPendingUsedBikesWithoutModelImage method.
    /// Modified by : Ashutosh Sharma on 17 Apr 2018.
    /// Description : Added GetModelIdIfTopVersion to get model id for a version if the version is top version is among other version of same model.
    /// </summary>
    public interface IBikeModels
    {
        UsedBikeImagesByMakeNotificationData GetPendingUsedBikesWithoutModelImage();
        IEnumerable<BikeModelsByMake> GetModelsWithMissingColorImage();
        void UpdateModelESIndex(string ids, string operation);
        int GetModelIdIfTopVersion(int versionId);
        BikeMakeModelData GetModelDetailsById(uint modelId);
    }
}
