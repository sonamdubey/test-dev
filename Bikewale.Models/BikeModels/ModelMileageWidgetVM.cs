using Bikewale.Entities.BikeData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Models.BikeModels
{
    /// <summary>
    /// Created by:Snehal Dange on 3rd Nov 2017
    /// Description: View Model for mileage widget on model page
    /// </summary>
    public class ModelMileageWidgetVM
    {
        public IEnumerable<BikeWithMileageInfo> SimilarBikeList { get; set; }
        public BikeWithMileageInfo MileageInfo { get; set; }
        public float AvgBodyStyleMileageByUserReviews { get; set; }
        public string WidgetHeading { get; set; }
    }
}
