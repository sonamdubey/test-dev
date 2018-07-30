using Bikewale.DTO.CMS.Articles;
using Bikewale.DTO.Videos;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.Model.v2
{
    /// <summary>
    /// Created By : Sushil Kumar on 6th September 2017
    /// Desc : DTO to store model page contents for editorial and reviews
    /// </summary>
    public class BikeModelContentDTO
    {
        [JsonProperty("reviewDetails")]
        public IEnumerable<UserReviews.UserReviewSummaryDto> ReviewDetails { get; set; }

        [JsonProperty("news")]
        public IEnumerable<CMSArticleSummary> News { get; set; }

        [JsonProperty("expertReviews")]
        public IEnumerable<CMSArticleSummary> ExpertReviews { get; set; }

        [JsonProperty("videos")]
        public IEnumerable<VideoBase> Videos { get; set; }
    }
}
