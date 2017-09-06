using Newtonsoft.Json;

namespace Bikewale.DTO.UserReviews
{
    public class WriteReviewInput
    {
        [JsonProperty("reviewDescription")]
        public string ReviewDescription { get; set; }
        [JsonProperty("reviewTitle")]
        public string ReviewTitle { get; set; }
        [JsonProperty("reviewQuestion")]
        public string ReviewQuestion { get; set; }
        [JsonProperty("reviewTips")]
        public string ReviewTips { get; set; }
        [JsonProperty("emailId")]
        public string EmailId { get; set; }
        [JsonProperty("userName")]
        public string UserName { get; set; }
        [JsonProperty("makeName")]
        public string MakeName { get; set; }
        [JsonProperty("modelName")]
        public string ModelName { get; set; }
        [JsonProperty("reviewId")]
        public uint ReviewId { get; set; }
        [JsonProperty("mileage")]
        public string Mileage { get; set; }
        [JsonProperty("customerId")]
        public ulong CustomerId { get; set; }
        [JsonProperty("isDesktop")]
        bool? IsDesktop { get; set; }
    }
}
