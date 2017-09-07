using Bikewale.DTO.Customer;
using Bikewale.DTO.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.UserReviews
{
    /// <summary>
    /// Created by Snehal Dange on 01-09-2017
    /// Description: DTO created as output for rate-bike page 
    /// </summary>
    public class RateBikeDetails
    {
        public ModelDetails BikeDetails { get; set; }

        //   public UserReviewsData UserReviewInfo { get; set; }
        [JsonProperty("customerName")]
        public string CustomerName { get; set; }

        [JsonProperty("customerEmail")]
        public string CustomerEmail { get; set; }

        [JsonProperty("overAllRatingText")]
        public string OverAllRatingText { get; set; }

        [JsonProperty("ratingQuestion")]
        public string RatingQuestion { get; set; }

        [JsonProperty("reviewsOverAllrating")]
        public string ReviewsOverAllrating { get; set; }

        [JsonProperty("priceRangeId")]
        public uint PriceRangeId { get; set; }

        [JsonProperty("reviewId")]
        public uint ReviewId { get; set; }

        [JsonProperty("isFake")]
        public bool IsFake { get; set; }

        [JsonProperty("selectedRating")]
        public uint SelectedRating { get; set; }

        [JsonProperty("returnUrl")]
        public string ReturnUrl { get; set; }

        [JsonProperty("sourceId")]
        public ushort SourceId { get; set; }

        [JsonProperty("contestSrc")]
        public int ContestSrc { get; set; }

    }
}
