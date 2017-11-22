using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Created by:Snehal Dange on 3rd Nov 2017
    /// Description: For mileage details for a bike
    /// </summary>
    [Serializable]
    public class BikeWithMileageInfo: BasicBikeEntityBase
    {
        public ushort Rank { get; set; }
        public float Percentile { get; set; }
        public float MileageByUserReviews { get; set; }
        public uint BodyStyleId { get; set; }
        public float ARAIMileage { get; set; }
    }
}
