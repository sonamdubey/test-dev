using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Bikewale.DTO.UserReviews
{
    /// <summary>
    /// Created By : Sushil Kumar on 9th Nov 2017
    /// Description : Dto to get user input for option review paramters to save
    /// </summary>
    public class InputParamterRatingSave
    {
        [JsonProperty("customerId")]
        public uint CustomerId { get; set; }

        [Required, JsonProperty("reviewId")]
        public uint ReviewId { get; set; }

        [JsonProperty("mileage")]
        public ushort Mileage { get; set; }

        [Required, JsonProperty("encodedId")]
        public string EncodedCustomerAndReviewId { get; set; }

        [JsonProperty("qamapping")]
        public string QuestionAnswerMapping { get; set; }
    }
}
