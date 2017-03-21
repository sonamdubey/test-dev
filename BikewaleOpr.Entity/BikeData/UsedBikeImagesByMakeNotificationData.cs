
using System.Collections.Generic;
namespace BikewaleOpr.Entity.BikeData
{
    /// <summary>
    /// Created by : Sajal Gupta on 21-03-2017
    /// Description: Entity to hold bike by make and isnotify
    /// </summary>
    public class UsedBikeImagesByMakeNotificationData
    {
        public IEnumerable<UsedModelsByMake> BikesByMake { get; set; }
        public bool IsNotify { get; set; }
    }
}
