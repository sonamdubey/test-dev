using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.Entity.BikePricing
{
    /// <summary>
    /// Created by: Ashutosh Sharma on 31-07-2017
    /// Discription : Entity to fetch last updated for a particular city and bike version.
    /// </summary>
    public class PriceLastUpdateEntity
    {
        public int CityId { get; set; }
        public int BikeVersionId { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
