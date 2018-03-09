
using BikewaleOpr.Entity.BikeData;
using System.Collections.Generic;

namespace BikewaleOpr.Interface.BikeData
{
    /// <summary>
    /// Created by : Sajal Gupta on 09-03-2017
    /// Description: Added GetPendingUsedBikesWithoutModelImage method;
    /// </summary>
    public interface IBikeModels
    {
        UsedBikeImagesByMakeNotificationData GetPendingUsedBikesWithoutModelImage();
        IEnumerable<BikeModelsByMake> GetModelsWithMissingColorImage();
        void UpdateModelESIndex(string ids, string operation);
    }
}
