﻿namespace Bikewale.Entities.UserReviews
{
    public class ReviewsListEntity
    {
        //private BikeMakeEntityBase objmakeBase = new BikeMakeEntityBase();
        //public BikeMakeEntityBase MakeEntity { get { return objmakeBase; } set { objmakeBase = value; } }

        //private ReviewEntityBase objReviewBase = new ReviewEntityBase();
        public ReviewEntityBase ReviewEntity { get; set; }
        public ReviewRatingEntityBase ReviewRating { get; set; }
        public ReviewTaggedBikeEntity TaggedBike { get; set; }
    }
}
