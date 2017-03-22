using System.Collections.Generic;

namespace BikewaleOpr.Entity.BikeData
{
    /// <summary>
    /// Created by : Sajal Gupta on 21-03-2017
    /// descripton : Entity to hold bikes whose used model images have not been uploaded and last notication date
    /// </summary>
    public class UsedBikeImagesNotificationData
    {
        public IEnumerable<UsedBikeImagesModel> Bikes { get; set; }
        public bool IsNotify { get; set; }
    }
}
