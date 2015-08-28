using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.DTO
{
    public class ReviewsList
    {
        public ReviewBase ReviewEntity { get; set; }
        public ReviewRatingBase ReviewRating { get; set; }
        public ReviewTaggedBike TaggedBike { get; set; }
    }
}
