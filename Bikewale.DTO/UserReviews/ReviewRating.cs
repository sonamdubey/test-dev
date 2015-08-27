using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.DTO
{
    public class ReviewRating : ReviewRatingBase
    {
        public float StyleRating { get; set; }
        public float ComfortRating { get; set; }
        public float PerformanceRating { get; set; }
        public float ValueRating { get; set; }
        public float FuelEconomyRating { get; set; }        
    }
}
