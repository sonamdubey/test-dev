using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.DTO
{
    public class ReviewBase
    {
        [JsonProperty("reviewId")]
        public int ReviewId { get; set; }
        [JsonProperty("reviewTitle")]
        public string ReviewTitle { get; set; }
        [JsonProperty("reviewDate")]
        public DateTime ReviewDate { get; set; }
        [JsonProperty("writtenBy")]
        public string WrittenBy { get; set; }
    }
}
