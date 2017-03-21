using System.Collections.Generic;

namespace BikewaleOpr.Entity.BikeData
{
    public class UsedBikeImagesNotificationData
    {
        public IEnumerable<UsedBikeImagesModel> Bikes { get; set; }
        public bool IsNotify { get; set; }
    }
}
