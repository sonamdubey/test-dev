using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.UserReviews
{
    public class ReviewRatingEntity : ReviewRatingEntityBase
    {
        public float StyleRating { get; set; }
        public float ComfortRating { get; set; }
        public float PerformanceRating { get; set; }
        public float ValueRating { get; set; }
        public float FuelEconomyRating { get; set; }        
    }
}
