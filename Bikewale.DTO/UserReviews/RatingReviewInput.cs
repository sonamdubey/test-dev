﻿using Newtonsoft.Json;

namespace Bikewale.DTO.UserReviews
{
    /// <summary>
    /// Created by: Snehal Dange on 01-09-2017
    /// Description: DTO created as input for Review screen input
    /// </summary>
    public class RatingReviewInput
    {
        [JsonProperty("overAllrating")]
        public string OverAllrating { get; set; }

        [JsonProperty("customerId")]
        public ulong CustomerId { get; set; }

        [JsonProperty("reviewId")]
        public uint ReviewId { get; set; }

        [JsonProperty("isFake")]
        public bool IsFake { get; set; }

        [JsonProperty("makeId")]
        public uint MakeId { get; set; }

        [JsonProperty("modelId")]
        public uint ModelId { get; set; }

        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("emailId")]
        public string EmailId { get; set; }

        [JsonProperty("priceRangeId")]
        public ushort PriceRangeId { get; set; }

        [JsonProperty("returnUrl")]
        public string ReturnUrl { get; set; }

        [JsonProperty("sourceId")]
        public ushort? SourceId { get; set; }

        [JsonProperty("contestSrc")]
        public ushort? ContestSrc { get; set; }
       

    }
}
