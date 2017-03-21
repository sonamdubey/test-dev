
using System.Collections.Generic;
namespace BikewaleOpr.Entity.BikeData
{
    public class UsedBikeImagesByMakeNotificationData
    {
        public IEnumerable<UsedModelsByMake> BikesByMake { get; set; }
        public bool IsNotify { get; set; }
    }
}
