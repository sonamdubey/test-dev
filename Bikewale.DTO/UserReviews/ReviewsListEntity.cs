namespace Bikewale.Entities.DTO
{
    public class ReviewsList
    {
        public ReviewBase ReviewEntity { get; set; }
        public ReviewRatingBase ReviewRating { get; set; }
        public ReviewTaggedBike TaggedBike { get; set; }
    }
}
