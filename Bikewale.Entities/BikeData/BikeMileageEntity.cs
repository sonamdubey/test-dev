using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.BikeData
{

    /// <summary>
    /// Created By : Snehal Dange on 3rd Nov 2017
    /// Description: Mileage entity wrapper which includes mileage details by Bodystyle and individual bikes mileage info
    /// </summary>
    public class BikeMileageEntity
    {
        public IEnumerable<BikeWithMileageInfo> Bikes { get; set; }
        public IEnumerable<MileageInfoByBodyStyle> BodyStyleMileage { get; set; }

    }
}
