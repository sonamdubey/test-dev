using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Created by :Snehal Dange on 3rd Nov 2017
    /// Description: Mileage details according to bodystyle
    /// </summary>
    [Serializable]
    public class MileageInfoByBodyStyle
    {
        public uint BodyStyleId { get; set; }
        public float AvgBodyStyleMileageByUserReviews { get; set; }
        public float AvgMileageByARAI { get; set; }
        public ushort TotalBikesInBodyStyle { get; set; }

    }
}
