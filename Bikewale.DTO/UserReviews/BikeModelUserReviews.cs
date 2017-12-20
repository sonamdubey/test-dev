using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.UserReviews
{
    /// <summary>
    /// Created By : Sushil Kumar on 7th Sep 2017
    /// Description : DTO to save bike reviews info, rating info and other bike details
    /// </summary>
    public class BikeModelUserReviews
    {
        [JsonProperty("make")]
        public Make.MakeBase Make { get; set; }
        [JsonProperty("model")]
        public Model.ModelBase Model { get; set; }
        [JsonProperty("price")]
        public uint Price { get; set; }
        [JsonProperty("originalImagePath")]
        public string OriginalImagePath { get; set; }
        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }
        [JsonProperty("isDiscontinued")]
        public bool IsDiscontinued { get; set; }
        [JsonProperty("isUpcoming")]
        public bool IsUpcoming { get; set; }
        [JsonProperty("review")]
        public BikeReviewsData ReviewDetails { get; set; }
        [JsonProperty("rating")]
        public BikeRatingData RatingDetails { get; set; }
        [JsonProperty("userReviews")]
        public Bikewale.DTO.UserReviews.Search.SearchResult UserReviews { get; set; }
    }
}
