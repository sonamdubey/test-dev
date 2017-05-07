using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.DTO
{
    public class ReviewRatingBase
    {
        [JsonProperty("overAllRating")]
        public float OverAllRating { get; set; }
    }
}
