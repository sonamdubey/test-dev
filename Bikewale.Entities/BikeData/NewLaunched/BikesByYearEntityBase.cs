using System.Collections.Generic;
namespace Bikewale.Entities.BikeData.NewLaunched
{
    /// <summary>
    /// Created by  :   Sumit Kate on 10 Feb 2017
    /// Description :   Bikes By Year Entity Base
    /// Modified by :   Sumit Kate on 15 Feb 2017
    /// Description :   Added Bike list
    /// </summary>
    public class BikesCountByYearEntityBase
    {
        public int Year { get; set; }
        public int BikeCount { get; set; }
        public IEnumerable<string> Bikes { get; set; }
    }
}
