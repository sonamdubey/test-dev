using Bikewale.Entities.BikeData;
namespace Bikewale.Models
{
    public class UserReviewRatingVM : ModelBase
    {
        /// <summary>
        /// Modified by : Aditi Srivastava on 30 May 2017
        /// Summary     : Added sourceId parameter
        /// Modified by : Aditi Srivastava on 5 July 2017
        /// Summary     : Added contest src for user review contest page
        /// </summary>
        public BikeModelEntity objModelEntity { get; set; }
        public string OverAllRatingText { get; set; }
        public string RatingQuestion { get; set; }
        public string ErrorMessage { get; set; }
        public uint PriceRangeId { get; set; }
        public string ReviewsOverAllrating { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public uint ReviewId { get; set; }
        public bool IsFake { get; set; }
        public uint SelectedRating { get; set; }
        public string ReturnUrl { get; set; }
        public ushort SourceId { get; set; }
        public int ContestSrc { get; set; }
        public string UtmzCookieValue { get; set; }
    }
}
