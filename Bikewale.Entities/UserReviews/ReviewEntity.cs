using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.UserReviews
{
    public class ReviewEntity : ReviewEntityBase
    {
        public string Comments { get; set; }
        public string Pros { get; set; }
        public string Cons { get; set; }
        public ushort Liked { get; set; }
        public ushort Disliked { get; set; }
        public uint Viewed { get; set; }
        private ReviewRatingEntityBase objRating = new ReviewRatingEntityBase();
        public ReviewRatingEntityBase OverAllRating { get { return objRating; } set { objRating = value;} }
        public ReviewTaggedBikeEntity TaggedBike { get; set; }
    }
}
