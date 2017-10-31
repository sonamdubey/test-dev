using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.UserReviews
{

    /// <summary>
    /// Created By :Snehal Dange
    /// Description :  Entity to save submitted rating on rating-review page
    /// </summary>
    [Serializable, DataContract]
    public class InputRatingSaveEntity
    {
        [JsonProperty("overAllrating")]
        public string OverAllrating { get; set; }

        [JsonProperty("ratingQuestionAns")]
        public string RatingQuestionAns { get; set; }

        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("emailId")]
        public string EmailId { get; set; }

        [JsonProperty("makeId")]
        public uint MakeId { get; set; }

        [JsonProperty("modelId")]
        public uint ModelId { get; set; }

        [JsonProperty("priceRangeId")]
        public ushort PriceRangeId { get; set; }

        [JsonProperty("reviewId")]
        public uint ReviewId { get; set; }

        [JsonProperty("isDesktop")]
        public bool? IsDesktop { get; set; }

        [JsonProperty("returnUrl")]
        public string ReturnUrl { get; set; }

        [JsonProperty("platformId")]
        public ushort PlatformId { get; set; }

        [JsonProperty("sourceId")]
        public ushort? SourceId { get; set; }

        [JsonProperty("contestSrc")]
        public ushort? ContestSrc { get; set; }

        [JsonProperty("utmzCookieValue")]
        public string UtmzCookieValue { get; set; }
    }
}
