using Bikewale.Entities.Videos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Models.Videos
{
    public class ScooterVideosVM : ModelBase
    {
        public IEnumerable<BikeVideoEntity> VideosList { get; set; }
    }
}
