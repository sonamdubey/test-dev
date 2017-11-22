using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Bikewale.DTO.UserReviews
{
    /// <summary>
    /// Created by Snehal Dange on 01-09-2017
    /// Description :DTO created as input for rate-bike page
    /// </summary>
    /// <returns></returns>
    public class RateBikeInput
    {
        [Required]
        [JsonProperty("modelId")]
        public uint ModelId { get; set; }

        [Required]
        [JsonProperty("reviewId")]
        public uint ReviewId { get; set; }

        [Required]
        [JsonProperty("customerId")]
        public ulong CustomerId { get; set; }


        [JsonProperty("sourceId")]
        public ushort SourceId { get; set; }

        [JsonProperty("selectedRating")]
        public ushort SelectedRating { get; set; }

        [JsonProperty("isFake")]
        public bool IsFake { get; set; }


        [JsonProperty("returnUrl")]
        public string ReturnUrl { get; set; }

        [JsonProperty("contestsrc")]
        public ushort Contestsrc { get; set; }
    }
}
