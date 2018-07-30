
using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bikewale.DTO.UserReviews
{
    /// <summary>
    /// Modified by: Snehal Dange on 8th Dec 2017
    /// Description: Added  model overall parameters:TotalRatings , TotalReviews ,OverAllModelRating
    /// </summary>
    public class UserReviewSummaryDto
    {
        [JsonProperty("overallRating"), DataMember]
        public UserReviewOverallRatingDto OverallRating { get; set; }

        [JsonProperty("make"), DataMember]
        public MakeBase Make { get; set; }

        [JsonProperty("model"), DataMember]
        public ModelBase Model { get; set; }

        [JsonProperty("originalImgPath"), DataMember]
        public string OriginalImgPath { get; set; }

        [JsonProperty("originalImagePath"), DataMember]
        public string OriginalImagePath { get; set; }

        [JsonProperty("hostUrl"), DataMember]
        public string HostUrl { get; set; }

        [JsonProperty("description"), DataMember]
        public string Description { get; set; }

        [JsonProperty("sanitizedDescription"), DataMember]
        public string SanitizedDescription { get; set; }

        [JsonProperty("shortDescription"), DataMember]
        public string ShortDescription { get { return Description.TruncateHtml(270); } }

        [JsonProperty("title"), DataMember]
        public string Title { get; set; }

        [JsonProperty("tips"), DataMember]
        public string Tips { get; set; }

        [JsonProperty("overallRatingId"), DataMember]
        public ushort OverallRatingId { get; set; }

        [JsonProperty("questions"), DataMember]
        public IEnumerable<UserReviewQuestionDto> Questions { get; set; }

        [JsonProperty("customerId"), DataMember]
        public string CustomerId { get; set; }

        [JsonProperty("customerName"), DataMember]
        public string CustomerName { get; set; }

        [JsonProperty("customerEmail"), DataMember]
        public string CustomerEmail { get; set; }

        [JsonProperty("upVotes"), DataMember]
        public uint UpVotes { get; set; }

        [JsonProperty("downVotes"), DataMember]
        public uint DownVotes { get; set; }

        [JsonProperty("views"), DataMember]
        public uint Views { get; set; }

        [JsonProperty("reviewId"), DataMember]
        public uint ReviewId { get; set; }

        [JsonProperty("reviewAge"), DataMember]
        public string ReviewAge { get; set; }

        [JsonProperty("isRatingQuestion"), DataMember]
        public bool IsRatingQuestion { get; set; }

        [JsonProperty("ratingQuestionsCount"), DataMember]
        public ushort RatingQuestionsCount { get; set; }

        [JsonProperty("reviewUrl"), DataMember]
        public string ReviewUrl { get; set; }

        [JsonProperty("thanksHeading"), DataMember]
        public string ThanksHeading { get { return "Tell us more about your experience"; } }

        [JsonProperty("thanksText"), DataMember]
        public string ThanksText { get { return "Write a detailed review and help fellow riders make a decision. Share your experience with the world."; } }

        [JsonProperty("totalRatings"), DataMember]
        public uint TotalRatings { get; set; }

        [JsonProperty("totalReviews"), DataMember]
        public uint TotalReviews { get; set; }

        [JsonProperty("overAllModelRating"), DataMember]
        public float OverAllModelRating { get; set; }
    }
}
