using Bikewale.Entities.Videos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Models.Videos
{
    /// <summary>
    /// Created by :  Ashutosh Sharma on 17-Aug-2017
    /// Description : ViewModel for scooter videos.
    /// </summary>
    public class ScooterVideosVM : ModelBase
    {
        public IEnumerable<BikeVideoEntity> VideosList { get; set; }
        public uint CityId { get; set; }

    }
}
