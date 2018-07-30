using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Bikewale.DTO.UserReviews
{
    /// <summary>
    /// Created By  :   Vishnu Teja Yalakuntla on 07 Sep 2017
    /// Description :   Input for SaveWriteReviewDetails App API,
    /// </summary>
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
        [Required, JsonProperty("emailId")]
        public string EmailId { get; set; }
        [Required, JsonProperty("userName")]
        public string UserName { get; set; }
        [Required, JsonProperty("makeName")]
        public string MakeName { get; set; }
        [Required, JsonProperty("modelName")]
        public string ModelName { get; set; }
        [Required, JsonProperty("reviewId")]
        public uint ReviewId { get; set; }
        [JsonProperty("mileage")]
        public uint Mileage { get; set; }
        [Required, JsonProperty("customerId")]
        public ulong CustomerId { get; set; }
        [JsonProperty("isDesktop")]
        bool? IsDesktop { get; set; }
    }
}
